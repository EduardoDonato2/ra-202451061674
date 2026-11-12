using CryptoTracker.Api.Models;

namespace CryptoTracker.Api.Services;

public interface ITransacaoService
{
    Task<List<TransacaoAtivo>> ObterTodasAsync();
    Task<List<TransacaoAtivo>> ObterPorCarteiraIdAsync(string carteiraId);
    Task<TransacaoAtivo?> ObterPorIdAsync(string id);
    Task<TransacaoAtivo> RegistrarTransacaoAsync(TransacaoAtivo transacao);
    Task RemoverAsync(string id);
}