using CryptoTracker.Api.Models;

namespace CryptoTracker.Api.Services;

public interface ICarteiraService
{
    Task<List<CarteiraCripto>> ObterTodasAsync();
    Task<CarteiraCripto?> ObterPorIdAsync(string id);
    Task<CarteiraCripto> CriarAsync(CarteiraCripto carteira);
    Task AtualizarAsync(string id, CarteiraCripto carteiraAtualizada);
    Task RemoverAsync(string id);
}