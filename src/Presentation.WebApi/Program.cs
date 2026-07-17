// Presentation.WebApi/Program.cs
using Core.Application;
using Infrastructure.Persistence;
using Presentation.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Inyección limpia y estructurada usando métodos de extensión
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline de Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();