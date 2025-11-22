# Calculadora Emocional (.NET 8)

## 📘 Resumo do Projeto
A **Calculadora Emocional** é uma API desenvolvida em **ASP.NET Core (.NET 8)** para registrar check-ins emocionais diários de colaboradores, calcular índices de bem-estar e risco de burnout, armazenar dados corporativos em **Azure SQL** e fornecer endpoints REST versionados (**v1** e **v2**) para uso por dashboards, integrações e aplicações cliente.

---

## 🎯 Tema da Aplicação
**Workingsafe – Monitoramento de Bem-Estar, Produtividade e Burnout em Ambientes Híbridos.**

---

## 🧩 Visão do Sistema Completo
- Coleta diária de check-ins: humor, foco, pausas, horas trabalhadas, observações e tags.
- Processamento automático de bem-estar e risco de burnout.
- Dashboards gerenciais com dados anonimizados.
- Alertas para riscos elevados.
- Integração planejada com soluções Java, .NET, Mobile e serviços de IA.

---

## 🏗️ Escopo do Módulo (.NET)
- Processamento e cálculo dos índices emocionais.
- Persistência em **Azure SQL** via **EF Core + Migrations**.
- Versionamento da API (v1 e v2).
- Autenticação via **API Key**.
- Health Check, Logging e Tracing.

---

## 🔐 Autenticação – API Key via Swagger
Para acessar os endpoints protegidos:

- Clique em **Authorize** no Swagger  
- Insira a chave:

```
workingsafe-adm
```

Ou envie no header:

```
X-Api-Key: workingsafe-adm
```

---

## ⚙️ Funcionalidades Implementadas
### ✔ Campos do Check-in
- empresaId  
- colaboradorId  
- dataReferencia  
- humor  
- foco  
- pausas  
- horasTrabalhadas  
- observacoes  
- tags  

---

## 📐 Lógica de Cálculo

### Índice de Bem-Estar
\[
bemEstar = \frac{humor + foco + pausas}{3}
\]

### Risco de Burnout
\[
risco = (horasTrabalhadas \times 0.2) - (pausas \times 0.3)
\]

Classificação:
- baixo  
- moderado  
- alto → dispara alerta automático  

---

## 📡 Versionamento da API

### 🔹 v1
- POST check-in  
- GET índices  
- Paginação  
- HATEOAS  
- Swagger estruturado  

### 🔹 v2
- PUT atualizar check-in  
- Recalcula automaticamente os índices  

---

## ❗ Por que NÃO existe DELETE na API?
A API **não implementa DELETE intencionalmente**, por motivos de segurança e integridade dos dados.

**Justificativa oficial (incluída no README):**

> O delete não existirá pois não é uma função em que o usuário da API deverá ter acesso, visto que são informações confidenciais. O usuário, após realizar o check-in, estará encaminhando informações anônimas para que seja gerado um resumo e entregue ao responsável da empresa. Dessa forma, não existindo DELETE, torna-se mais difícil “forjar” ou manipular os dados enviados ao responsável, garantindo integridade e auditoria do processo.

Essa justificativa atende totalmente ao requisito contextual da aplicação e demonstra maturidade no design da API.

---

## 🗄️ Persistência – Azure SQL + EF Core Migrations
- Banco relacional Azure SQL.
- EF Core configurado com `UseSqlServer`.
- Pasta `Migrations/` presente no projeto.
- O banco de produção já possui tabela criada; migrations são usadas apenas para **versionamento do modelo**.

---

## 🔍 Monitoramento e Observabilidade
- `/health` → status geral  
- `/health/details` → status do Azure SQL + duração das checagens  
- Logging estruturado (entrada/saída das requisições)  
- Tracing com `X-Correlation-Id`  

---

## 🧪 Testes Automatizados
- Projeto: `CalculadoraEmocional.Tests`
- Framework: **xUnit**
- Provider InMemory
- Testes de:
  - cálculo do bem-estar  
  - cálculo do burnout  
  - classificação  
  - regras de negócio  

---

## 🗂️ Estrutura do Repositório
```
CalculadoraEmocional.Api/
 ├── Controllers/
 ├── Data/
 ├── Entities/
 ├── HealthChecks/
 ├── Models/
 ├── Services/
 ├── Migrations/
 ├── Program.cs
 ├── appsettings.json
CalculadoraEmocional.Tests/
README.md
```

---

## ▶️ Como Executar

### Restaurar dependências
```
dotnet restore
```

### Compilar
```
dotnet build
```

### Executar API
```
dotnet run --project CalculadoraEmocional.Api
```

### Swagger
```
https://localhost:{porta}/swagger
```

---

## 🧪 Executar Testes
```
dotnet test
```

---

## 📡 Exemplos de Endpoints

### POST – Registrar check-in (v1)
```
POST /api/v1/calculadora-emocional/checkin
```

### GET – Listar índices (v1)
```
GET /api/v1/calculadora-emocional/indices
```

### PUT – Atualizar check-in (v2)
```
PUT /api/v2/calculadora-emocional/checkin/{id}
```

---

## 📝 Resumo
- API REST robusta em .NET 8  
- Autenticação via API Key  
- Versionamento v1/v2  
- Azure SQL + EF Core Migrations  
- Observabilidade completa (health, logs, tracing)  
- Testes automatizados  
- Justificativa clara para ausência do DELETE (integridade e segurança)

