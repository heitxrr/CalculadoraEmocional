using CalculadoraEmocional.Api.Data;
using CalculadoraEmocional.Api.Models;
using CalculadoraEmocional.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CalculadoraEmocional.Tests
{
    public class CalculadoraEmocionalServiceTests
    {
        private CalculadoraEmocionalService CriarServiceInMemory()
        {
            var options = new DbContextOptionsBuilder<CalculadoraEmocionalContext>()
                .UseInMemoryDatabase(databaseName: "CalculadoraEmocionalTestsDb")
                .Options;

            var context = new CalculadoraEmocionalContext(options);
            return new CalculadoraEmocionalService(context);
        }

        [Fact]
        public async Task CalcularERegistrarAsync_DeveCalcularIndiceERiscoCorretamente()
        {
            // Arrange
            var service = CriarServiceInMemory();

            var request = new CheckinRequest
            {
                EmpresaId = 1,
                ColaboradorId = 10,
                DataReferencia = new DateOnly(2025, 11, 16),
                Humor = 3,
                Foco = 4,
                Pausas = 2,
                HorasTrabalhadas = 9
            };

            // Fórmulas esperadas:
            // Índice de Bem-Estar = (humor + foco + pausas) / 3
            var indiceEsperado = (request.Humor + request.Foco + request.Pausas) / 3.0;

            // Risco de burnout = (horas trabalhadas × 0.2) − (pausas × 0.3)
            var riscoEsperado = (request.HorasTrabalhadas * 0.2) - (request.Pausas * 0.3);

            // Act
            var resultado = await service.CalcularERegistrarAsync(request);

            // Assert
            Assert.Equal(indiceEsperado, resultado.IndiceBemEstar, precision: 3);
            Assert.Equal(riscoEsperado, resultado.RiscoBurnout, precision: 3);
            Assert.Equal(request.EmpresaId, resultado.EmpresaId);
            Assert.Equal(request.ColaboradorId, resultado.ColaboradorId);
            Assert.False(double.IsNaN(resultado.IndiceBemEstar));
            Assert.False(double.IsNaN(resultado.RiscoBurnout));
        }
    }
}
