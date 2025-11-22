using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CalculadoraEmocional.Api.Data;
using CalculadoraEmocional.Api.HealthChecks;
using CalculadoraEmocional.Api.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


var apiKey = builder.Configuration["ApiKey"];


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Calculadora Emocional API",
        Version = "v1",
        Description = "API .NET para cálculo de bem-estar e risco de burnout em empresas híbridas (projeto acadêmico - Futuro do Trabalho).",
        Contact = new OpenApiContact
        {
            Name = "Equipe GS .NET",
            Email = "contato@empresa.com"
        }
    });

  
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Informe a API Key no header 'X-Api-Key'. Ex: workingsafe-adm (ou use o botão Authorize no Swagger).",
        Name = "X-Api-Key",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });


    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddHealthChecks()
    .AddCheck<DbHealthCheck>("azure_sql");


builder.Services.AddDbContext<CalculadoraEmocionalContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CalculadoraEmocionalConnection"),
        sqlOptions =>
        {
            sqlOptions.CommandTimeout(60);        
            sqlOptions.EnableRetryOnFailure(3);   
        }));


builder.Services.AddScoped<CalculadoraEmocionalService>();

var app = builder.Build();


var logger = app.Logger;

app.Use(async (context, next) =>
{
    var correlationId = context.Request.Headers["X-Correlation-Id"].FirstOrDefault();

    if (string.IsNullOrWhiteSpace(correlationId))
        correlationId = Guid.NewGuid().ToString();

    context.Response.Headers["X-Correlation-Id"] = correlationId;

    using (logger.BeginScope(new Dictionary<string, object>
    {
        ["CorrelationId"] = correlationId
    }))
    {
        logger.LogInformation(
            "Request iniciado: {Method} {Path} CorrelationId={CorrelationId}",
            context.Request.Method,
            context.Request.Path,
            correlationId);

        await next();

        logger.LogInformation(
            "Request finalizado: {Method} {Path} Status={StatusCode} CorrelationId={CorrelationId}",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            correlationId);
    }
});

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? string.Empty;

    if (path.StartsWith("/health") || path.StartsWith("/swagger"))
    {
        await next();
        return;
    }

    if (path.StartsWith("/api"))
    {
        if (!context.Request.Headers.TryGetValue("X-Api-Key", out var chaveEnviada) ||
            string.IsNullOrWhiteSpace(apiKey) ||
            !string.Equals(chaveEnviada, apiKey, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API Key inválida ou ausente.");
            return;
        }
    }

    await next();
});


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Calculadora Emocional API v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.MapHealthChecks("/health/details", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        var resposta = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                nome = e.Key,
                status = e.Value.Status.ToString(),
                descricao = e.Value.Description,
                duracaoMs = e.Value.Duration.TotalMilliseconds
            })
        };

        var json = JsonSerializer.Serialize(resposta, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await context.Response.WriteAsync(json);
    }
});

app.Run();
