using CryptoTracker.Api.Config;
using CryptoTracker.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CryptoTracker.Api.Repositories;

public class TransacaoRepository : ITransacaoRepository
{
    private readonly IMongoCollection<TransacaoAtivo> _transacoesCollection;

    public TransacaoRepository(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        
        _transacoesCollection = mongoDatabase.GetCollection<TransacaoAtivo>("Transacoes");
    }

    public async Task<List<TransacaoAtivo>> ObterTodasAsync() =>
        await _transacoesCollection.Find(_ => true).ToListAsync();

    public async Task<List<TransacaoAtivo>> ObterPorCarteiraIdAsync(string carteiraId) =>
        await _transacoesCollection.Find(x => x.CarteiraId == carteiraId).ToListAsync();

    public async Task<TransacaoAtivo?> ObterPorIdAsync(string id) =>
        await _transacoesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CriarAsync(TransacaoAtivo transacao) =>
        await _transacoesCollection.InsertOneAsync(transacao);

    public async Task RemoverAsync(string id) =>
        await _transacoesCollection.DeleteOneAsync(x => x.Id == id);
}