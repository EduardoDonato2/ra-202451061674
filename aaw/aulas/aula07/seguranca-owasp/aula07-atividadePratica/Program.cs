using System.Text;
using ApiVazada.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ══════════════════════════════════════════════════════════════════════
//  FALHA 3 (não corrija ainda!) — Segredos no código e erro exposto
//  Parte A: a connection string e a chave de assinatura do JWT estão
//  escritas aqui, em texto puro, dentro de um arquivo versionado no Git.
// ══════════════════════════════════════════════════════════════════════
var connectionString = builder.Configuration["DatabaseConfig"];
var jwtSecret = builder.Configuration["JwtSecret"];

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// ══════════════════════════════════════════════════════════════════════
//  FALHA 3 (não corrija ainda!) — Parte B
//  A página de exceção detalhada está ligada SEMPRE, inclusive fora do
//  ambiente de desenvolvimento. Qualquer erro devolve stack trace,
//  caminho de arquivos do servidor, versões de pacotes e trechos do
//  código-fonte ao cliente. Experimente: GET /api/produtos/quebrar
// ══════════════════════════════════════════════════════════════════════
// Comentamos a linha abaixo para garantir que a stack trace NUNCA é mostrada
// app.UseDeveloperExceptionPage();
// Interceta os erros e devolve uma mensagem genérica, ocultando a stack trace
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync("Ocorreu um erro interno. Detalhes ocultos por seguranca.");
    });
});

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
