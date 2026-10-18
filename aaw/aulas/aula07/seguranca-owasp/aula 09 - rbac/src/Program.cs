using System.Text;
using BibliotecaApi.Autorizacao;
using BibliotecaApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Acervo em memória (compartilhado por toda a aplicação).
builder.Services.AddSingleton<BibliotecaStore>();

// ── Autenticação JWT — exatamente como ficou na Aula 08 ─────────────
var jwt = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
        ClockSkew = TimeSpan.Zero
    };
});

// ── Autorização: o lugar CENTRAL das regras de acesso ───────────────
builder.Services.AddAuthorization(options =>
{
    // ABAC (Falha 3)
    options.AddPolicy("PodeRenovarEmprestimo",
        p => p.Requirements.Add(new DonoDoEmprestimoRequirement()));

    // RBAC (Falha 1): a matriz traduzida para policies nomeadas
    options.AddPolicy("GerenciarAcervo", p => p.RequireRole("Bibliotecario", "Admin"));
    options.AddPolicy("RemoverLivro",    p => p.RequireRole("Admin"));

    // Falha 2 entra aqui (próximo passo)

        // Falha 2 (causa): endpoint sem atributo exige login
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    // Falha 2 (sintoma): relatórios são só para Admin
    options.AddPolicy("VerRelatorios", p => p.RequireRole("Admin"));
});

builder.Services.AddSingleton<IAuthorizationHandler, DonoDoEmprestimoHandler>();

// ── PBAC (Falha 4): políticas fora do código, recarregadas a quente ─
builder.Configuration.AddJsonFile("politicas.json", optional: false, reloadOnChange: true);
builder.Services.Configure<PoliticasOptions>(builder.Configuration.GetSection("Politicas"));
builder.Services.AddSingleton<MotorDePoliticas>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
