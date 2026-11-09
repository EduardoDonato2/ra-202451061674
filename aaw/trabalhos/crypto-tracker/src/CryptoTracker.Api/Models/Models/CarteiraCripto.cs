using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CryptoTracker.API.Models;

public class carteiraCripto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectID)]
    public string? Id { get; set;}

    [BsonElement("nomeCarteira")]
    public string NomeCarteira { get; set; } = string.Empty;

    [BsonElement("usuarioId")]
    public string UsuarioId { get; set; } = string.Empty;

    [BsonElement("saldoDisponivelUsd")]
    public decimal SaldoDisponivelUsd { get; set; }

    [BsonElement("perfilRisco")]
    public string PerfilRisco { get; set; } = "Conservador";

    [BsonElement("dataCriacao")]
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}