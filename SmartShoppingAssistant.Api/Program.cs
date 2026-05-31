using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.IdentityModel.Tokens;
using OpenAI;
using SmartShoppingAssistant.BusinessLogic.Agents;
using SmartShoppingAssistant.BusinessLogic.Agents.Interfaces;
using SmartShoppingAssistant.BusinessLogic.AutoMapperProfiles;
using SmartShoppingAssistant.BusinessLogic.Services;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess;
using SmartShoppingAssistant.DataAccess.Repository;
using SmartShoppingAssistant.DataAccess.Repository.Interfaces;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add controllers with JSON options to handle enum serialization as strings
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var ConnectionString = builder.Configuration.GetConnectionString("SmartShoppingAssistantDb");

builder.Services.AddDbContext<SmartShoppingAssistantDbContext>(options =>
    options.UseSqlServer(ConnectionString));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<IPromotionService, PromotionService>();

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();


// OpenAI API
var openAiApiKey = builder.Configuration["OpenAI:ApiKey"];
var openAiModel = builder.Configuration["OpenAI:Model"] ?? "gpt-4o";

builder.Services.AddSingleton<IChatClient>(
        new OpenAIClient(openAiApiKey)
        .GetChatClient(openAiModel)
        .AsIChatClient()
        .AsBuilder()
        .UseFunctionInvocation()
        .Build()
);


// LM Studio API
/*
var lmStudioClient = new OpenAIClient(
    new ApiKeyCredential("lm-studio"),
    new OpenAIClientOptions
    {
        Endpoint = new Uri("http://localhost:1234/v1")
    }
);

var openAiChatClient = lmStudioClient.GetChatClient("local-model");

builder.Services.AddSingleton<IChatClient>(sp =>
{
    return openAiChatClient
        .AsIChatClient()
        .AsBuilder()
        .UseFunctionInvocation()
        .Build();
});
*/

// Google API
/*
builder.Services.AddSingleton<IChatClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    string apiKey = config["GoogleAIConfig:GoogleApiKey"] ?? throw new Exception("API Key missing!");
    string modelId = config["GoogleAIConfig:ModelId"] ?? "gemini-1.5-flash";

    IChatClient googleClient = new GenerativeAIChatClient(apiKey, modelId);

    return googleClient
        .AsBuilder()
        .UseFunctionInvocation()
        .Build();
});
*/

builder.Services.AddScoped<IPromotionCheckerAgent, PromotionCheckerAgent>();
builder.Services.AddScoped<ISuggestionComposerAgent, SuggestionComposerAgent>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddCors(options =>
{
    // LOCAL DEVELOPMENT ONLY
    // This policy dynamically echoes any origin to bypass the browser's 
    // restriction against wildcards + credentials. Security risk if used in production!
    options.AddPolicy("LocalDevCors",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder
                .WithOrigins("https://localhost:5173")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });

    //  PRODUCTION READY
    // Strictly limits requests to the specified frontend URL.
    options.AddPolicy("ProductionCors",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder
                .WithOrigins("https://www.real-website-domain.com")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

// AutoMapper
// Ensure that BusinessLogic loads, no need to include all the profiles
// besides this one here since they are in the same assembly
builder.Services.AddAutoMapper(cfg => { }, typeof(ProductProfile).Assembly);

// JWT Authentication setup
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Check for token in cookies
                if (context.Request.Cookies.ContainsKey("jwtToken"))
                {
                    context.Token = context.Request.Cookies["jwtToken"];
                }
                return Task.CompletedTask;
            }
        };
    });

// Forwarded Headers setup to correctly identify client IPs and protocol when behind a reverse proxy (like Nginx or Cloudflare)
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

// Rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var clientIp = httpContext.Connection.RemoteIpAddress?.ToString();

        string partitionKey;
        if (!string.IsNullOrWhiteSpace(userId))
        {
            partitionKey = $"User_{userId}";
        }
        else if (!string.IsNullOrWhiteSpace(clientIp))
        {
            partitionKey = $"IP_{clientIp}";
        }
        else
        {
            partitionKey = "Unknown";
        }

        return RateLimitPartition.GetTokenBucketLimiter(partitionKey, _ => new TokenBucketRateLimiterOptions
        {
            TokenLimit = 100, // Max 100 requests burst
            TokensPerPeriod = 20, // Refill 20 tokens every period
            ReplenishmentPeriod = TimeSpan.FromMinutes(1), // Refill every minute
            AutoReplenishment = true
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
       options.SwaggerEndpoint("/openapi/v1.json", "SmartShoppingAssistant API v1"));
}

app.UseForwardedHeaders();

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseCors("LocalDevCors");
}
else
{
    app.UseCors("ProductionCors");
}

// JWT authentication setup
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();