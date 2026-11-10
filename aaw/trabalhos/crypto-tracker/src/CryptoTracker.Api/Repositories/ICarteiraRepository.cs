using CryptoTracker.Api.Models;

namespace CryptoTracker.Api.Repositories;

public interface ICarteiraRepository
{
    Task<List<CarteiraCripto>> ObterTodasAsync();
    Task<CarteiraCripto?> ObterPorIdAsync(string id);
    Task CriarAsync(CarteiraCripto carteira);
    Task AtualizarAsync(string id, CarteiraCripto carteiraAtualizada);
    Task RemoverAsync(string id);
}