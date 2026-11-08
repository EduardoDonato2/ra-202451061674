# Proposta — Trabalho Semestral · Arquitetura de Aplicações Web 2026.2

- **Aluno:** Eduardo Donato Batista Mendes · **Curso/Turma:** Ciência da Computação
- **Repositório:** https://github.com/EduardoDonato2/ra-202451061674.git

## 1. Domínio e problema
O CryptoTracker é um sistema de simulação de carteiras e gestão de risco de criptoativos. A aplicação permite que investidores organizem posições em ativos digitais, executem compras/vendas simuladas sem riscos financeiros, monitorem o patrimônio total e recebam alertas automáticos de risco por alta concentração de ativos.

## 2. Entidades
- **CarteiraCripto**: `id`, `nomeCarteira`, `usuarioId`, `saldoDisponivelUsd`, `perfilRisco`, `dataCriacao`.
- **TransacaoAtivo**: `id`, `carteiraId`, `ticker`, `tipoOperacao`, `quantidade`, `precoUnitarioUsd`, `dataHora`.
- **Relacionamento**: 1-N por Referência (`carteiraId` armazenado na transação). 
  *Justificativa:* Como a quantidade de transações por carteira cresce continuamente ao longo do tempo, a referência evita ultrapassar o limite de tamanho de documento do MongoDB (16 MB) e simplifica relatórios temporais.

## 3. Endpoints previstos
- `GET /api/carteiras` — Listar carteiras do usuário
- `GET /api/carteiras/{id}` — Detalhes da carteira e saldo
- `POST /api/carteiras` — Criar nova carteira
- `PUT /api/carteiras/{id}` — Atualizar dados da carteira
- `DELETE /api/carteiras/{id}` — Remover carteira (Perfil Admin)
- `GET /api/transacoes/carteira/{carteiraId}` — Listar histórico de transações
- `POST /api/transacoes` — Registrar ordem de compra/venda simulada

## 4. Regra de negócio
Ao incluir uma transação (`POST /api/transacoes`), a camada de serviço valida se há `saldoDisponivelUsd` suficiente (em compras), calcula o valor da ordem, atualiza o saldo da carteira e recalcula a distribuição da carteira. Se a exposição a um único ativo ultrapassar 40% do total investido, o perfil de risco da carteira é alterado automaticamente para "Alta Concentração / Agressivo".

## 5. Casos de erro
- `HTTP 400 (Bad Request)`: Saldo insuficiente para realizar a compra ou valores negativos em quantidade/preço.
- `HTTP 404 (Not Found)`: Tentativa de transação vinculada a uma carteira inexistente.

## 6. Stack escolhida
- **Backend:** .NET 10 (C# / Web API)
- **Banco:** MongoDB (Container Docker)
- **Frontend:** HTML5 + JavaScript ES6 (Fetch API)

## 7. Segurança e testes
- Autenticação via JWT com controle de acesso RBAC (`admin` e `usuario`).
- Testes unitários com xUnit cobrindo 2 cenários de sucesso e 2 de erro na camada de serviço.