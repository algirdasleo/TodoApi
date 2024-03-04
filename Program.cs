using TodoApi.Service;
using TodoApi.Interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole(); 
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<ITodoService, TodoService>(); // Dependency injection, naudojamas Singleton, kad per visa application runtime butu naudojamas tas pats ITodoService objektas

builder.Services.AddSwaggerGen(c =>     // Naudojamas Swagger, kad butu galima testuoti API
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) 
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();               
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1"));
}

app.UseAuthorization();

app.MapControllers();

app.Run();
