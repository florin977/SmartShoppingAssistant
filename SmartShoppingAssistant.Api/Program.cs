using GenerativeAI.Microsoft;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using System.Text;
using System.Text.Json.Serialization;

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
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// JWT authentication setup
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();