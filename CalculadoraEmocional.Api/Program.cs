using CalculadoraEmocional.Api.Data;
using CalculadoraEmocional.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
