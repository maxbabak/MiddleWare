using Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<RequestTimingMiddleware>();
app.UseMiddleware<CopyrightMiddleware>();

app.MapControllers();
app.Run();