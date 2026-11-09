import os
import subprocess
import requests
import google.generativeai as genai
from datetime import datetime

# 1. Carrega as variáveis de ambiente
GEMINI_API_KEY = os.environ.get("GEMINI_API_KEY")
NOTION_TOKEN = os.environ.get("NOTION_TOKEN")
NOTION_DATABASE_ID = os.environ.get("NOTION_DATABASE_ID")

genai.configure(api_key=GEMINI_API_KEY)

# 2. Coleta o código alterado nas últimas 24 horas
try:
    git_diff = subprocess.check_output(['git', 'log', '--since="24 hours"', '-p'], text=True)
    if not git_diff.strip():
        print("Nenhuma alteração detectada nas últimas 24 horas. Encerrando o processo.")
        exit(0)
except Exception as e:
    print(f"Erro ao extrair alterações do git: {e}")
    exit(1)

# 3. Invoca o Gemini com a sua Skill do Professor Thalles
prompt = f"""
Aja com a habilidade 'resumo-diario-trabalho-thalles'.
Com base no log de código abaixo (desenvolvido hoje em .NET e MongoDB para a disciplina de Arquitetura de Aplicações Web), execute as seguintes tarefas:
1. Resuma o que foi desenvolvido no dia.
2. Gere as mensagens de commit adequadas seguindo a Regra D3.
3. Explique as principais linhas de código escritas.
4. Simule a arguição técnica do professor, focando em perguntas de leitura de código, decisão de arquitetura, previsão de falhas e testes de modificação.

Log de alterações (Git Diff):
{git_diff[:15000]}
"""

model = genai.GenerativeModel('gemini-3.8-flash')
response = model.generate_content(prompt)
resumo = response.text

# 4. Formata e envia para a API do Notion
headers = {
    "Authorization": f"Bearer {NOTION_TOKEN}",
    "Content-Type": "application/json",
    "Notion-Version": "2022-06-28"
}

blocos_notion = []
# O Notion aceita no máximo 2000 caracteres por parágrafo, então dividimos o texto
for i in range(0, len(resumo), 2000):
    blocos_notion.append({
        "object": "block",
        "type": "paragraph",
        "paragraph": {
            "rich_text": [{"text": {"content": resumo[i:i+2000]}}]
        }
    })

data = {
    "parent": {"page_id": NOTION_DATABASE_ID},
    "properties": {
        "title": [
            {
                "text": {
                    "content": f"Resumo de Código - {datetime.now().strftime('%d/%m/%Y')}"
                }
            }
        ]
    },
    "children": blocos_notion
}

notion_url = "https://api.notion.com/v1/pages"
resposta_notion = requests.post(notion_url, headers=headers, json=data)

if resposta_notion.status_code == 200:
    print("Resumo processado pela Skill e enviado para o Notion com sucesso!")
else:
    print(f"Erro ao enviar para o Notion: {resposta_notion.text}")
