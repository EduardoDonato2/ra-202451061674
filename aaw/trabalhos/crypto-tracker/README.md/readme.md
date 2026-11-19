# CryptoTracker API

- **Aluno:** Eduardo Donato Batista Mendes
- **Curso / Turma:** Ciência da Computação — Arquitetura de Aplicações Web (2026.2)

## Descrição do Projeto e Domínio

O **CryptoTracker** é uma aplicação web completa voltada ao gerenciamento de carteiras de criptoativos e registro automatizado de operações de compra e venda. O sistema controla o saldo disponível em USD de cada carteira e valida a custódia de ativos antes de autorizar vendas, integrando uma API RESTful em **.NET 10** com persistência NoSQL em **MongoDB**, segurança via **JWT** e uma interface web com consumo assíncrono via `fetch`.

---

## Arquitetura e Tecnologias

- **Backend:** C# / .NET 10 Web API (camadas `Controller` → `Service` → `Repository` com Injeção de Dependência)
- **Banco de Dados:** MongoDB (via Docker Compose)
- **Frontend:** HTML5, CSS3 e JavaScript Assíncrono (`fetch`) sem recarregamento de página
- **Segurança:** Autenticação JWT (HMAC-SHA256) e Autorização baseada em perfis (RBAC)
- **Documentação:** OpenAPI / Swagger
- **Testes Automatizados:** xUnit + Moq

---

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (com WSL2 ativo)

---

## Variáveis de Ambiente

Configure as variáveis abaixo no ambiente ou via `appsettings.json` local (nunca suba segredos reais no repositório):

| Variável | Descrição | Exemplo |
| :--- | :--- | :--- |
| `MongoDbSettings__ConnectionString` | String de conexão com o servidor MongoDB | `mongodb://localhost:27017` |
| `MongoDbSettings__DatabaseName` | Nome do banco de dados NoSQL | `CryptoTrackerDb` |
| `JwtSettings__SecretKey` | Chave secreta (mín. 32 caracteres) para assinatura HMAC-SHA256 | `SuaChaveSecretaSuperSeguraCom32Chars!` |
| `JwtSettings__Issuer` | Emissor do token JWT | `CryptoTrackerApi` |
| `JwtSettings__Audience` | Público-alvo do token JWT | `CryptoTrackerUsers` |

---

## Como Executar Localmente

### 1. Subir o Banco de Dados (MongoDB)
Na raiz do projeto (`crypto-tracker`), inicie o container do MongoDB:
```bash
docker compose up -d

## Uso de IA

- **Ferramentas usadas:** Gemini (Google)
- **Onde ajudou:** Apoio na estruturação inicial das camadas em .NET 10, configuração da autenticação JWT, escrita dos testes unitários com xUnit/Moq e montagem da interface web assíncrona.
- **O que eu revisei/reescrevi: ** Escrevi o todas interfaces, autenticacoes, regras de negocios, docker, etc, porem o codigo todo foi tirando duvida com a IA, testando, perguntando oque podia melhorar e porque a opcao era melhor etc.