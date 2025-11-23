# Calculadora Emocional (.NET 8)

## 🌐 Acesso à API Publicada (Render)
A API está publicada e disponível para uso:

👉 **Swagger Online:**  
https://calculadoraemocional.onrender.com/swagger/

---

## 📘 Resumo do Projeto
A **Calculadora Emocional** é uma API desenvolvida em **ASP.NET Core (.NET 8)** para registrar check-ins emocionais, calcular índices de bem-estar e risco de burnout, e fornecer endpoints REST versionados (v1 e v2 e v3).  
O sistema utiliza **Azure SQL**, autenticação via **API Key**, observabilidade (Health, Logging e Tracing) e testes automatizados com xUnit.

---

## 🎯 Tema da Aplicação
**Workingsafe — Monitoramento de Bem-Estar, Produtividade e Burnout em Ambientes Híbridos.**

---

## 🧩 Funcionalidades
- Registro de check-ins com humor, foco, pausas, horas trabalhadas, observações e tags  
- Cálculo automático de:
  - Índice de bem-estar  
  - Risco de burnout + classificação  
- Integração com Azure SQL  
- Paginação e HATEOAS  
- Versionamento v1/v2/v3  
- Autenticação via API Key  
- Observabilidade (health/details, correlation ID, logs estruturados)

---

## 🔐 Autenticação
Insira a chave no Swagger (Authorize) ou no header:

```
X-Api-Key: workingsafe-adm
```

---

## 📡 Versionamento

### **v1**
- POST check-in  
- GET índices (com paginação e HATEOAS)

### **v2**
- PUT atualizar check-in  
- POST check-in  
- GET índices  

### **v3*
- DELETE Remover check-in
---

## 🗄️ Persistência
- Banco: **Azure SQL**
- ORM: **Entity Framework Core**  
- Suporte a Migrations

---

## 🧪 Testes Automatizados
- Projeto: `CalculadoraEmocional.Tests`  
- Framework: **xUnit**  
- Provider: **InMemoryDatabase**  
- Testes de cálculos, regras e validações  

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
 ├── Dockerfile
 └── appsettings.json

CalculadoraEmocional.Tests/
README.md
```

---

## ▶️ Como Executar Localmente

### Restaurar dependências
```
dotnet restore
```

### Compilar
```
dotnet build
```

### Executar
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

## 📝 Resumo Final
- API .NET 8 completa e funcional  
- Versionamento profissional v1/v2  
- Azure SQL + EF Core Migrations  
- API Key + observabilidade avançada  
- Testes automatizados  
- Deploy concluído no Render  

Swagger online:  
https://calculadoraemocional.onrender.com/swagger/
