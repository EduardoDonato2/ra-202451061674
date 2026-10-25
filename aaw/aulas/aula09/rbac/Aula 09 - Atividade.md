# HANDOUT — AULA 09

## Projete a Matriz de Acesso

*SisAcad — sistema acadêmico fictício — Arquitetura de Aplicações Web*

## 🎯 MISSÃO

Vocês são o time de segurança contratado para definir QUEM pode fazer O QUÊ no SisAcad, o novo sistema acadêmico da faculdade. Os papéis já existem: Aluno, Professor, Coordenador e Secretaria.

- Preencham a matriz abaixo com ✓ (pode) ou ✗ (não pode) para cada papel × ação
- Justifiquem por escrito os 3 casos que geraram mais discussão na dupla
- Respondam à pergunta-armadilha da Parte 03 — ela esconde o limite do modelo

*⏱️ Tempo: 25 minutos  |  👥 Formato: em duplas  |  Regra de ouro: na dúvida, NEGUE o acesso.*

> **Nomes:** ____________________   **Turma:** ____________________   **Data:** ___ / ___ / ______

## PARTE 01 — A matriz papel × permissão

Marquem ✓ (pode) ou ✗ (não pode) em cada célula:

| **Ação** | **Aluno** | **Professor** | **Coordenador** | **Secretaria** |
| --- | --- | --- | --- | --- |
| 1. Ver as próprias notas | ✓ | ✓ | ✓ | ✓ |
| 2. Ver notas de toda a turma | x | ✓ | ✓ | ✓ |
| 3. Lançar nota | x | ✓ | x | x |
| 4. Alterar nota já lançada | x | ✓ | x | x |
| 5. Matricular aluno em disciplina | x | x | ✓ | x |
| 6. Trancar matrícula | x | x | ✓ | x |
| 7. Criar nova disciplina | x | x | ✓ | x |
| 8. Ver dados pessoais de aluno | ✓ | x | ✓ | ✓ |
| 9. Emitir histórico escolar | ✓ | x | x | ✓ |
| 10. Aprovar colação de grau | x | x | ✓ | x |

*Dica: pensem em consequências — quem responde perante o MEC se uma nota for alterada indevidamente? E perante a LGPD se dados pessoais vazarem?*

## PARTE 02 — Os casos polêmicos

Quais 3 células geraram mais discussão na dupla? Anotem a célula (ação + papel) e a justificativa da decisão final:

1. Caso 1: Celula 5; Matricular Aluno / O coordenador deve ter acesso para matricular o alunos, pois ele consegue ver quais materias estão disponiveis para cursar.

2. Caso 2: Celula 8; Ver dados pessoais / O professo é o unico que não tem acesso pois ele nao necessita da informação dos dados do aluno.

3. Caso 3: Celula 10; Aprovar Colação / Apenas o coordenador pode aprovar a colação pois ele tem acesso as notas dos alunos e seus desempenhos

## PARTE 03 — A pergunta-armadilha

Vocês marcaram que Professor pode “lançar nota”. Mas… de QUALQUER turma? O professor de Cálculo pode lançar nota na turma de Arquitetura de Aplicações Web?

1. O papel “Professor” sozinho é suficiente para decidir esse acesso? Por quê? Não pois precisaria de acesso restrito para lancar nota a sua materia, caso nao tiver outras restricoes ele consegue alterar notas de outras materias

2. Que informação ALÉM do papel o sistema precisa conhecer para decidir corretamente? IDPRofessor - IDMateria - IDturma, pois ele ira conseguir acessar a sua turma e sua materia especifica

3. Onde vocês já viram esse mesmo problema nesta disciplina? (dica: Aula 07…)
No problema MASS ASSIGMENT, alteração de role 

## DESAFIO

1. Chegou um pedido da coordenação: criar o papel “Monitor” — um aluno que ajuda o professor em uma disciplina. Que permissões vocês dariam ao Monitor? Que risco novo esse papel cria?

APenas ver notas de toda turma, permitindo apenas uma funcao diminui o risco de ocorrer algum erro. O risco de criacao deste role seria alterar dados, vazar dados.
