using CryptoTracker.Api.Models;
using CryptoTracker.Api.Repositories;

namespace CryptoTracker.Api.Services;

public class CarteiraService : ICarteiraService
{
    private readonly ICarteiraRepository _carteiraRepository;

    public CarteiraService(ICarteiraRepository carteiraRepository)
    {
        _carteiraRepository = carteiraRepository;
    }

    public async Task<List<CarteiraCripto>> ObterTodasAsync() =>
        await _carteiraRepository.ObterTodasAsync();

    public async Task<CarteiraCripto?> ObterPorIdAsync(string id) =>
        await _carteiraRepository.ObterPorIdAsync(id);

    public async Task<CarteiraCripto> CriarAsync(CarteiraCripto carteira)
    {
        // Regra de Negócio Básica
        if (string.IsNullOrWhiteSpace(carteira.NomeCarteira))
            throw new ArgumentException("O nome da carteira é obrigatório.");

        carteira.DataCriacao = DateTime.UtcNow;
        
        await _carteiraRepository.CriarAsync(carteira);
        return carteira;
    }

    public async Task AtualizarAsync(string id, CarteiraCripto carteiraAtualizada)
    {
        var carteiraExistente = await _carteiraRepository.ObterPorIdAsync(id);
        if (carteiraExistente is null)
            throw new KeyNotFoundException("Carteira não encontrada.");

        carteiraAtualizada.Id = carteiraExistente.Id;
        carteiraAtualizada.DataCriacao = carteiraExistente.DataCriacao; // Impede alteração da data

        await _carteiraRepository.AtualizarAsync(id, carteiraAtualizada);
    }

    public async Task RemoverAsync(string id)
    {
        var carteiraExistente = await _carteiraRepository.ObterPorIdAsync(id);
        if (carteiraExistente is null)
            throw new KeyNotFoundException("Carteira não encontrada.");

        await _carteiraRepository.RemoverAsync(id);
    }
}