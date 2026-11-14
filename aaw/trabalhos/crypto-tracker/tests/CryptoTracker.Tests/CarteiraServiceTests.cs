using CryptoTracker.Api.Models;
using CryptoTracker.Api.Repositories;
using CryptoTracker.Api.Services;
using Moq;
using Xunit;

namespace CryptoTracker.Tests;

public class CarteiraServiceTests
{
    private readonly Mock<ICarteiraRepository> _carteiraRepoMock;
    private readonly CarteiraService _carteiraService;

    public CarteiraServiceTests()
    {
        _carteiraRepoMock = new Mock<ICarteiraRepository>();
        _carteiraService = new CarteiraService(_carteiraRepoMock.Object);
    }

    [Fact]
    public async Task CriarAsync_DeveCriarCarteira_QuandoDadosForemValidos()
    {
        // Arrange
        var novaCarteira = new CarteiraCripto
        {
            NomeCarteira = "Carteira Principal",
            SaldoDisponivelUsd = 1000m
        };

        // Act
        var resultado = await _carteiraService.CriarAsync(novaCarteira);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Carteira Principal", resultado.NomeCarteira);
        _carteiraRepoMock.Verify(r => r.CriarAsync(It.IsAny<CarteiraCripto>()), Times.Once);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecao_QuandoNomeForVazio()
    {
        // Arrange
        var carteiraInvalida = new CarteiraCripto
        {
            NomeCarteira = "   ",
            SaldoDisponivelUsd = 500m
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _carteiraService.CriarAsync(carteiraInvalida));
        _carteiraRepoMock.Verify(r => r.CriarAsync(It.IsAny<CarteiraCripto>()), Times.Never);
    }

    [Fact]
    public async Task AtualizarAsync_DeveLancarKeyNotFound_QuandoCarteiraNaoExistir()
    {
        // Arrange
        _carteiraRepoMock.Setup(r => r.ObterPorIdAsync("id-inexistente"))
            .ReturnsAsync((CarteiraCripto?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _carteiraService.AtualizarAsync("id-inexistente", new CarteiraCripto { NomeCarteira = "Teste" }));
    }
}