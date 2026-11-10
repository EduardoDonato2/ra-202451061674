using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CryptoTracker.Api.Models;

public class TransacaoAtivo
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("carteiraId")]
    public string CarteiraId { get; set; } = string.Empty;

    [BsonElement("ticker")]
    public string Ticker { get; set; } = string.Empty; // Ex: BTC, ETH, SOL

    [BsonElement("tipoOperacao")]
    public string TipoOperacao { get; set; } = "COMPRA"; // COMPRA ou VENDA

    [BsonElement("quantidade")]
    public decimal Quantidade { get; set; }

    [BsonElement("precoUnitarioUsd")]
    public decimal PrecoUnitarioUsd { get; set; }

    [BsonElement("dataHora")]
    public DateTime DataHora { get; set; } = DateTime.UtcNow;
}