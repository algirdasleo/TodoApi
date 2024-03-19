using TodoApi.Service;
using TodoApi.Interfaces;
using TodoApi.Helpers;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<ITodoService, TodoService>(); 
var connectionString = builder.Configuration.GetConnectionString("TodoDb");
builder.Services.AddSingleton<SQLConnectionFactory>(provider => new SQLConnectionFactory(connectionString));
builder.Services.AddScoped<ITodoDataService, TodoDataService>();

builder.Services.AddSwaggerGen(c =>                                        
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

app.MapControllers();

app.Run();
