using CryptoTracker.Api.Config;
using CryptoTracker.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CryptoTracker.Api.Repositories;

public class CarteiraRepository : ICarteiraRepository
{
    private readonly IMongoCollection<CarteiraCripto> _carteirasCollection;

    public CarteiraRepository(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        
        _carteirasCollection = mongoDatabase.GetCollection<CarteiraCripto>("Carteiras");
    }

    public async Task<List<CarteiraCripto>> ObterTodasAsync() =>
        await _carteirasCollection.Find(_ => true).ToListAsync();

    public async Task<CarteiraCripto?> ObterPorIdAsync(string id) =>
        await _carteirasCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CriarAsync(CarteiraCripto carteira) =>
        await _carteirasCollection.InsertOneAsync(carteira);

    public async Task AtualizarAsync(string id, CarteiraCripto carteiraAtualizada) =>
        await _carteirasCollection.ReplaceOneAsync(x => x.Id == id, carteiraAtualizada);

    public async Task RemoverAsync(string id) =>
        await _carteirasCollection.DeleteOneAsync(x => x.Id == id);
}