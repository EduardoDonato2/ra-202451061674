using CryptoTracker.Api.Config;
using CryptoTracker.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Vincula as configurações do appsettings.json com a classe MongoDbSettings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// 2. Registra o repositório na Injeção de Dependência
builder.Services.AddSingleton<ICarteiraRepository, CarteiraRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();