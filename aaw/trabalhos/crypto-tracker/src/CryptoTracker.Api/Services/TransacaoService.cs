using CryptoTracker.Api.Models;
using CryptoTracker.Api.Repositories;

namespace CryptoTracker.Api.Services;

public class TransacaoService : ITransacaoService
{
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly ICarteiraRepository _carteiraRepository;

    public TransacaoService(
        ITransacaoRepository transacaoRepository,
        ICarteiraRepository carteiraRepository)
    {
        _transacaoRepository = transacaoRepository;
        _carteiraRepository = carteiraRepository;
    }

    public async Task<List<TransacaoAtivo>> ObterTodasAsync() =>
        await _transacaoRepository.ObterTodasAsync();

    public async Task<List<TransacaoAtivo>> ObterPorCarteiraIdAsync(string carteiraId) =>
        await _transacaoRepository.ObterPorCarteiraIdAsync(carteiraId);

    public async Task<TransacaoAtivo?> ObterPorIdAsync(string id) =>
        await _transacaoRepository.ObterPorIdAsync(id);

    public async Task<TransacaoAtivo> RegistrarTransacaoAsync(TransacaoAtivo transacao)
    {
        // 1. Verifica se a carteira existe
        var carteira = await _carteiraRepository.ObterPorIdAsync(transacao.CarteiraId);
        if (carteira is null)
            throw new KeyNotFoundException("Carteira não encontrada.");

        // 2. Validações básicas de valores
        if (string.IsNullOrWhiteSpace(transacao.Ticker))
            throw new ArgumentException("O ticker do ativo (ex: BTC, ETH) é obrigatório.");

        if (transacao.Quantidade <= 0 || transacao.PrecoUnitarioUsd <= 0)
            throw new ArgumentException("Quantidade e preço unitário devem ser maiores que zero.");

        var tipo = transacao.TipoOperacao.Trim().ToUpper();
        if (tipo != "COMPRA" && tipo != "VENDA")
            throw new ArgumentException("O tipo de operação deve ser 'COMPRA' ou 'VENDA'.");

        transacao.TipoOperacao = tipo;
        transacao.Ticker = transacao.Ticker.Trim().ToUpper();
        transacao.DataHora = DateTime.UtcNow;

        decimal valorTotalOperacao = transacao.Quantidade * transacao.PrecoUnitarioUsd;

        // 3. Regra de Negócio: Atualização de Saldo da Carteira
        if (tipo == "COMPRA")
        {
            if (carteira.SaldoDisponivelUsd < valorTotalOperacao)
                throw new ArgumentException($"Saldo insuficiente. Necessário: ${valorTotalOperacao:F2}, Disponível: ${carteira.SaldoDisponivelUsd:F2}.");

            carteira.SaldoDisponivelUsd -= valorTotalOperacao;
        }
        else if (tipo == "VENDA")
        {
            // Verifica se o utilizador possui quantidade suficiente do ativo para vender
            var historicoCarteira = await _transacaoRepository.ObterPorCarteiraIdAsync(carteira.Id!);
            var saldoAtivo = historicoCarteira
                .Where(t => t.Ticker == transacao.Ticker)
                .Sum(t => t.TipoOperacao == "COMPRA" ? t.Quantidade : -t.Quantidade);

            if (saldoAtivo < transacao.Quantidade)
                throw new ArgumentException($"Quantidade insuficiente de {transacao.Ticker} para venda. Disponível: {saldoAtivo}.");

            carteira.SaldoDisponivelUsd += valorTotalOperacao;
        }

        // 4. Persiste a atualização do saldo e grava a transação
        await _carteiraRepository.AtualizarAsync(carteira.Id!, carteira);
        await _transacaoRepository.CriarAsync(transacao);

        return transacao;
    }

    public async Task RemoverAsync(string id)
    {
        var existente = await _transacaoRepository.ObterPorIdAsync(id);
        if (existente is null)
            throw new KeyNotFoundException("Transação não encontrada.");

        await _transacaoRepository.RemoverAsync(id);
    }
}