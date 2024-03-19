using TodoApi.Service;
using TodoApi.Interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<ITodoService, TodoService>(); 
builder.Services.AddScoped<ITodoDataService, TodoDataService>(provider =>   // kadangi yra naudojamas SQLite, reikia perduoti IConfiguration objekta, kad butu galima gauti connection string is appsettings.json failo
{                                                                           // ir del to negalima naudot automatisko dependency injection, todel reikia naudoti provider objekta
    var logger = provider.GetRequiredService<ILogger<TodoDataService>>(); 
    var connectionString = builder.Configuration.GetConnectionString("TodoDb"); // gaunamas connection string is appsettings.json failo
    return new TodoDataService(logger, connectionString); 
});

builder.Services.AddSwaggerGen(c =>                                         // Naudojamas Swagger, kad butu galima testuoti API
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
