using System.Collections.Generic;
using CalculadoraEmocional.Api.Data;
using CalculadoraEmocional.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Health Checks
builder.Services.AddHealthChecks();

// DbContext - banco local SQLite
builder.Services.AddDbContext<CalculadoraEmocionalContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CalculadoraEmocionalConnection")));

// Nosso serviço da Calculadora Emocional
builder.Services.AddScoped<CalculadoraEmocionalService>();

var app = builder.Build();

// Cria o banco/tabelas se não existirem
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CalculadoraEmocionalContext>();
    db.Database.EnsureCreated();
}

var logger = app.Logger;

app.Use(async (context, next) =>
{
    // Tenta pegar um CorrelationId vindo do cliente
    var correlationId = context.Request.Headers["X-Correlation-Id"].FirstOrDefault();

    // Se não vier, gera um novo
    if (string.IsNullOrWhiteSpace(correlationId))
        correlationId = Guid.NewGuid().ToString();

    // Devolve o CorrelationId no header da resposta
    context.Response.Headers["X-Correlation-Id"] = correlationId;

    // Adiciona o CorrelationId ao escopo de logging
    using (logger.BeginScope(new Dictionary<string, object>
    {
        ["CorrelationId"] = correlationId
    }))
    {
        logger.LogInformation("Request iniciado: {Method} {Path} CorrelationId={CorrelationId}",
            context.Request.Method,
            context.Request.Path,
            correlationId);

        await next();

        logger.LogInformation("Request finalizado: {Method} {Path} Status={StatusCode} CorrelationId={CorrelationId}",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            correlationId);
    }
});


// Swagger SEMPRE habilitado
app.UseSwagger();
app.UseSwaggerUI();

// HTTPS redirection
app.UseHttpsRedirection();

app.UseAuthorization();

// Controllers
app.MapControllers();

// Endpoint de Health Check
app.MapHealthChecks("/health");

app.Run();
