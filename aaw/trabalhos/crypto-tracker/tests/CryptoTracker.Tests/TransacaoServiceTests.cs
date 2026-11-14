using CryptoTracker.Api.Models;
using CryptoTracker.Api.Repositories;
using CryptoTracker.Api.Services;
using Moq;
using Xunit;

namespace CryptoTracker.Tests;

public class TransacaoServiceTests
{
    private readonly Mock<ITransacaoRepository> _transacaoRepoMock;
    private readonly Mock<ICarteiraRepository> _carteiraRepoMock;
    private readonly TransacaoService _transacaoService;

    public TransacaoServiceTests()
    {
        _transacaoRepoMock = new Mock<ITransacaoRepository>();
        _carteiraRepoMock = new Mock<ICarteiraRepository>();
        _transacaoService = new TransacaoService(_transacaoRepoMock.Object, _carteiraRepoMock.Object);
    }

    [Fact]
    public async Task RegistrarTransacaoAsync_CompraValida_DeveDeduzirSaldoDaCarteira()
    {
        // Arrange
        var carteira = new CarteiraCripto
        {
            Id = "carteira-1",
            NomeCarteira = "Hold BTC",
            SaldoDisponivelUsd = 5000m
        };

        _carteiraRepoMock.Setup(r => r.ObterPorIdAsync("carteira-1")).ReturnsAsync(carteira);

        var compra = new TransacaoAtivo
        {
            CarteiraId = "carteira-1",
            Ticker = "btc",
            TipoOperacao = "COMPRA",
            Quantidade = 0.05m,
            PrecoUnitarioUsd = 60000m // Total = $3.000
        };

        // Act
        var resultado = await _transacaoService.RegistrarTransacaoAsync(compra);

        // Assert
        Assert.Equal("BTC", resultado.Ticker);
        Assert.Equal(2000m, carteira.SaldoDisponivelUsd); // 5000 - 3000 = 2000
        _carteiraRepoMock.Verify(r => r.AtualizarAsync("carteira-1", carteira), Times.Once);
        _transacaoRepoMock.Verify(r => r.CriarAsync(It.IsAny<TransacaoAtivo>()), Times.Once);
    }

    [Fact]
    public async Task RegistrarTransacaoAsync_CompraSemSaldo_DeveLancarArgumentException()
    {
        // Arrange
        var carteira = new CarteiraCripto
        {
            Id = "carteira-1",
            SaldoDisponivelUsd = 100m
        };

        _carteiraRepoMock.Setup(r => r.ObterPorIdAsync("carteira-1")).ReturnsAsync(carteira);

        var compraCara = new TransacaoAtivo
        {
            CarteiraId = "carteira-1",
            Ticker = "ETH",
            TipoOperacao = "COMPRA",
            Quantidade = 1m,
            PrecoUnitarioUsd = 2500m
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _transacaoService.RegistrarTransacaoAsync(compraCara));
        _transacaoRepoMock.Verify(r => r.CriarAsync(It.IsAny<TransacaoAtivo>()), Times.Never);
    }

    [Fact]
    public async Task RegistrarTransacaoAsync_VendaSemAtivoSuficiente_DeveLancarArgumentException()
    {
        // Arrange
        var carteira = new CarteiraCripto { Id = "carteira-1", SaldoDisponivelUsd = 1000m };
        _carteiraRepoMock.Setup(r => r.ObterPorIdAsync("carteira-1")).ReturnsAsync(carteira);

        // Histórico mostra apenas 0.2 BTC comprados anteriormente
        _transacaoRepoMock.Setup(r => r.ObterPorCarteiraIdAsync("carteira-1"))
            .ReturnsAsync(new List<TransacaoAtivo>
            {
                new() { CarteiraId = "carteira-1", Ticker = "BTC", TipoOperacao = "COMPRA", Quantidade = 0.2m }
            });

        var vendaExagerada = new TransacaoAtivo
        {
            CarteiraId = "carteira-1",
            Ticker = "BTC",
            TipoOperacao = "VENDA",
            Quantidade = 0.5m, // Tenta vender mais do que tem
            PrecoUnitarioUsd = 60000m
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _transacaoService.RegistrarTransacaoAsync(vendaExagerada));
    }

    [Fact]
    public async Task RegistrarTransacaoAsync_VendaValida_DeveCreditarSaldoNaCarteira()
    {
        // Arrange
        var carteira = new CarteiraCripto { Id = "carteira-1", SaldoDisponivelUsd = 1000m };
        _carteiraRepoMock.Setup(r => r.ObterPorIdAsync("carteira-1")).ReturnsAsync(carteira);

        _transacaoRepoMock.Setup(r => r.ObterPorCarteiraIdAsync("carteira-1"))
            .ReturnsAsync(new List<TransacaoAtivo>
            {
                new() { CarteiraId = "carteira-1", Ticker = "SOL", TipoOperacao = "COMPRA", Quantidade = 10m }
            });

        var vendaValida = new TransacaoAtivo
        {
            CarteiraId = "carteira-1",
            Ticker = "SOL",
            TipoOperacao = "VENDA",
            Quantidade = 4m,
            PrecoUnitarioUsd = 150m // Total = $600
        };

        // Act
        await _transacaoService.RegistrarTransacaoAsync(vendaValida);

        // Assert
        Assert.Equal(1600m, carteira.SaldoDisponivelUsd); // 1000 + 600 = 1600
        _carteiraRepoMock.Verify(r => r.AtualizarAsync("carteira-1", carteira), Times.Once);
    }
}