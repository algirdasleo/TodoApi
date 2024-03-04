using TodoApi.Service;
using TodoApi.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<ITodoService, TodoService>(); // dependency injection - prideda ITodoService ir TodoService i DI containeri

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
app.Run();
