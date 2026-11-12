using CryptoTracker.Api.Models;

namespace CryptoTracker.Api.Repositories;

public interface ITransacaoRepository
{
    Task<List<TransacaoAtivo>> ObterTodasAsync();
    Task<List<TransacaoAtivo>> ObterPorCarteiraIdAsync(string carteiraId);
    Task<TransacaoAtivo?> ObterPorIdAsync(string id);
    Task CriarAsync(TransacaoAtivo transacao);
    Task RemoverAsync(string id);
}