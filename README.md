**Calculadora Emocional (.NET) — README para Entrega Acadêmica**

**Resumo**
Este repositório contém o módulo backend implementado em ASP.NET Core (.NET 8) para a "Calculadora Emocional": uma API que recebe check-ins diários de colaboradores, calcula índices de bem‑estar e risco de burnout, persiste registros e fornece endpoints REST para consumo por dashboards, integrações e aplicações cliente.

**Tema Geral**
O Futuro do Trabalho — Monitoramento de Bem‑Estar, Produtividade e Burnout em Empresas Híbridas.

**Visão do Sistema Completo**
- Check‑ins diários de colaboradores (humor, foco, pausas, horas trabalhadas).
- Análise avançada (camadas futuras com IA para recomendações).
- Dashboards gerenciais com dados anonimizados.
- Notificações e alertas automáticos para riscos elevados.
- Armazenamento inteligente e integração com múltiplas tecnologias (Java, .NET, Mobile, IA, Oracle, MongoDB).

**Responsabilidade deste Módulo (.NET)**
- Cálculo dos índices de bem‑estar e risco de burnout.
- Persistência dos registros.
- Endpoints REST (v1) e documentação via Swagger.
- Observabilidade: health, logging e tracing.

**Principais Funcionalidades Implementadas**
- Registro de Check‑in (endpoint POST): recebe `humor`, `foco`, `pausas`, `horasTrabalhadas`, `empresaId`, `colaboradorId`, `dataReferencia`.
- Cálculo do Índice de Bem‑Estar:

	$\displaystyle bemEstar = \frac{humor + foco + pausas}{3}$

- Cálculo do Risco de Burnout:

	$\displaystyle risco = (horasTrabalhadas \times 0.2) - (pausas \times 0.3)$

- Classificação do risco em `baixo`, `moderado` e `alto`. Disparo de alerta quando `alto`.
- Geração de recomendações automáticas conforme o nível de risco.
- Listagem de índices com paginação, contagem total e filtro por `colaboradorId`.
- HATEOAS: respostas contendo links úteis (`self`, `listar índices`, `novo check-in`).

**Persistência**
- EF Core com provedor SQLite (arquivo local `calculadora_emocional.db`).
- Entidades principais: `Checkin` e `IndiceEmocional`.
- Criação automática do banco em tempo de execução: `EnsureCreated()`.

**Observabilidade**
- Health check (endpoint `/health` que retorna `Healthy`).
- Logging estruturado para operações relevantes (entrada/saída, risco alto, paginação).
- Tracing com `X-Correlation-Id` propagado entre logs e resposta.

**Versionamento e Documentação**
- API organizada como `v1`. O projeto está preparado para evoluir para `v2` com alterações compatíveis.
- Documentação automática via Swagger (acessível quando a aplicação estiver em execução).

**Testes**
- Projeto de testes `CalculadoraEmocional.Tests` (xUnit).
- Utiliza provider InMemory para validar a lógica de cálculo e regras.

**Estrutura do Repositório (resumida)**
- `CalculadoraEmocional.Api/Controllers/`
- `CalculadoraEmocional.Api/Services/`
- `CalculadoraEmocional.Api/Models/` (DTOs)
- `CalculadoraEmocional.Api/Entities/`
- `CalculadoraEmocional.Api/Data/` (EF Core Context)
- `Program.cs`, `appsettings.json`, `calculadora_emocional.db`, `README.md`

**Como executar (passo a passo)**
Abra PowerShell na raiz do repositório e execute:

```powershell
dotnet restore
dotnet build
dotnet run --project CalculadoraEmocional.Api
```

Após subir a API, a documentação Swagger normalmente estará em `https://localhost:{porta}/swagger`.

Para executar os testes unitários:

```powershell
dotnet test
```

**Exemplos de Endpoints**
- Registrar check‑in (POST):

	POST /api/v1/calculadora-emocional/checkin

	Corpo (JSON):

	```json
	{
		"empresaId": 1,
		"colaboradorId": 42,
		"dataReferencia": "2025-11-17",
		"humor": 4,
		"foco": 3,
		"pausas": 2,
		"horasTrabalhadas": 8.0
	}
	```

- Listar índices (GET):

	GET /api/v1/calculadora-emocional/indices?colaboradorId=42

As respostas incluem links HATEOAS quando aplicável.

**Resumo Executivo (para apresentação)**
- API RESTful em .NET 8 com lógica de cálculo e persistência.
- Banco SQLite via EF Core e criação automática do esquema.
- Observabilidade (health, logs, tracing) e testes automatizados.
- Projeto modular, documentado e pronto para integração com outras camadas.

**Observações / Próximos Passos Recomendados**
- Adotar migrações EF Core para ambiente de produção em vez de `EnsureCreated()`.
- Implementar pipeline de anonimização para dashboards.
- Integrar com serviços de notificação e camada de IA para recomendações personalizadas.

---
_Documento preparado para entrega acadêmica. Se desejar, adapto o conteúdo para um slide de apresentação ou resumo técnico._
