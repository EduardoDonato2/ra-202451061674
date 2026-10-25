# HANDOUT — AULA 10

## Decifrando o Contrato

*Lendo uma especificação OpenAPI — Arquitetura de Aplicações Web*

## 🎯 MISSÃO

Vocês receberam o contrato OpenAPI de uma API que NUNCA viram — e nada mais: sem código, sem wiki, sem ninguém para perguntar. Provem que o contrato basta:

- Parte 1 — Interroguem a especificação YAML do Card 1 e respondam às perguntas do Card 2
- Parte 2 — Comparem com a “documentação em prosa” do Card 3 e registrem o que só a spec responde

*⏱️ Tempo: 30 minutos  |  👥 Formato: em duplas  |  Só vale usar o que está impresso neste handout.*

> **Nomes:**Eduardo Donato  **Turma:** ____________________   **Data:** 24 / 11 / 2004

## CARD 01 — O contrato — especificação OpenAPI (YAML)

Este é TODO o material que o time parceiro enviou. Leiam com calma — cada linha responde a uma pergunta do Card 2.

```text
openapi: 3.1.0
info:
  title: API Biblioteca
  version: 1.0.0
servers:
  - url: https://api.biblioteca.newton.br
paths:
  /api/v1/livros:
    get:
      summary: Lista os livros (paginado)
      parameters:
        - name: page
          in: query
          schema: { type: integer, default: 1 }
      responses:
        "200": { description: Lista de livros }
    post:
      summary: Cadastra um livro
      security:
        - bearerAuth: []
      requestBody:
        required: true
        content:
          application/json:
            schema: { $ref: "#/components/schemas/NovoLivro" }
      responses:
        "201": { description: Livro criado }
        "400": { description: Campos inválidos (ProblemDetails) }
        "401": { description: Token ausente ou inválido }
components:
  securitySchemes:
    bearerAuth:
      type: http
      scheme: bearer
      bearerFormat: JWT
  schemas:
    NovoLivro:
      type: object
      required: [titulo, autor]
      properties:
        titulo: { type: string, maxLength: 120 }
        autor:  { type: string }
        ano:    { type: integer, minimum: 1450 }
```

## CARD 02 — Interrogatório do contrato

Respondam usando SOMENTE o Card 1:

1. Que endpoints/operações existem e o que cada um faz?

GET /api/v1/livros: Lista os livros cadastrados na biblioteca de forma paginada.

POST /api/v1/livros: Realiza o cadastro de um novo livro no sistema.

2. Quais campos são OBRIGATÓRIOS para cadastrar um livro? Que restrições os campos têm?

titulo e autor


Restricoes: titulo: deve ser uma string (texto) com tamanho máximo de 120 caracteres (maxLength: 120).

autor: deve ser uma string (texto).

ano: é um campo opcional, deve ser um número inteiro (integer) e o valor mínimo permitido é 1450 (minimum: 1450).

3. Em que situações a API devolve 400? E 401? Qual operação exige token — e de que tipo?

400 (Bad Request): Devolvido quando os dados enviados no corpo da requisição descumprirem as regras do schema NovoLivro (ex.: campo obrigatório ausente, título com mais de 120 caracteres ou ano menor que 1450). O erro segue o padrão ProblemDetails.

401 (Unauthorized): Devolvido quando a requisição não enviar o cabeçalho de autorização ou enviar um token inválido/expirado.

Operação que exige token: Apenas a rota de cadastro POST /api/v1/livros.

Tipo do token: Esquema Bearer utilizando formato JWT.

4. Montem a requisição POST completa no papel (método, URL, headers, body JSON válido):

HTTP
POST https://api.biblioteca.newton.br/api/v1/livros HTTP/1.1
Host: api.biblioteca.newton.br
Content-Type: application/json
Authorization: Bearer <SEU_TOKEN_JWT_AQUI>

{
  "titulo": "Engenharia de Software",
  "autor": "Ian Sommerville",
  "ano": 2011
}w

## CARD 03 — A mesma API, documentada “em prosa”

Versão que o parceiro tinha antes de adotar OpenAPI:

*“Para cadastrar um livro, envie os dados do livro para o endpoint de livros. É preciso estar autenticado. Os campos incluem título, autor e ano. Em caso de erro, a API retorna uma mensagem apropriada. Também é possível listar os livros cadastrados.”*

1. Listem 3 informações que vocês usaram no Card 2 e que esta prosa NÃO responde com precisão:

Caminho e URL exatos da API: A prosa menciona apenas "endpoint de livros", enquanto o YAML define o servidor ([https://api.biblioteca.newton.br](https://api.biblioteca.newton.br)) e o path completo (/api/v1/livros).

Obrigatoriedade e validações dos campos: A prosa fala apenas quais campos existem, mas não explicita que titulo e autor são obrigatórios, nem informa as restrições (máximo de 120 caracteres para o título e ano mínimo de 1450).

Mecanismo de autenticação e códigos HTTP: A prosa só diz que "é preciso estar autenticado" e "retorna mensagem apropriada", omitindo o formato do cabeçalho (Authorization: Bearer), o tipo de token (JWT) e os códigos de status de retorno (201, 400 com ProblemDetails e 401).

## DESAFIO

1. Mesmo com a spec em mãos, que pergunta a dupla AINDA precisaria fazer ao time da API? (Dica: o contrato diz o que a API promete — não diz tudo sobre como ela se comporta.)

Obtenção do Token: Qual é a rota e o fluxo para autenticação/login para obter o token JWT inicial, e qual o tempo de expiração desse token?

Regras de Negócio e Duplicidade: O que acontece se tentarmos cadastrar um livro com o mesmo título e autor já existente na base? A API aceita duplicados ou retorna erro?

Detalhes da Paginação: Qual é o tamanho padrão da página no GET /api/v1/livros e é possível passar outros parâmetros como limit ou size?
