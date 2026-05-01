using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.BusinessLogic.AutoMapperProfiles;
using SmartShoppingAssistant.BusinessLogic.Services;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var ConnectionString = builder.Configuration.GetConnectionString("ExperimentDatabase");

builder.Services.AddDbContext<SmartShoppingAssistantDbContext>(options =>
    options.UseSqlServer(ConnectionString));

builder.Services.AddScoped<IRepository<Product>, BaseRepository<Product>>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IRepository<Category>, BaseRepository<Category>>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// AutoMapper
// Ensure that BusinessLogic loads, no need to include all the profiles
// besides this one here since they are in the same assembly
builder.Services.AddAutoMapper(cfg => { }, typeof(ProductProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();