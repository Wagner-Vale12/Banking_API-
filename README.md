# BankingApiVerity - Projeto em .NET Backend

API simples simulando contexto de banco digital: contas, depósito, transferência e extrato.

## Objetivo

Entender manutenção de endpoints em uma API .NET Backend com:

- Controllers
- Services
- Repositories
- Dependency Injection
- Entity Framework Core
- LINQ
- Tratamento de erros com Middleware
- Conceitos de transação financeira
- Idempotência para evitar operação duplicada

## Como rodar

```bash
dotnet restore
dotnet run
```

Depois acesse o Swagger:

```text
http://localhost:5000/swagger
```

ou veja a URL exibida no terminal.

## Endpoints

### Listar contas

```http
GET /api/accounts
```

### Criar conta

```http
POST /api/accounts
```

Body:

```json
{
  "customerName": "Wagner",
  "document": "12345678900",
  "initialBalance": 1000
}
```

### Depositar

```http
POST /api/accounts/{accountId}/deposit
```

Body:

```json
{
  "amount": 100,
  "description": "Depósito inicial",
  "idempotencyKey": "deposito-001"
}
```

### Transferir

```http
POST /api/accounts/transfer
```

Body:

```json
{
  "fromAccountId": "GUID_DA_CONTA_ORIGEM",
  "toAccountId": "GUID_DA_CONTA_DESTINO",
  "amount": 100,
  "idempotencyKey": "transferencia-001"
}
```

### Extrato

```http
GET /api/accounts/{accountId}/statement
```

## Como explicar em entrevista

> Esse projeto simula uma API financeira em .NET. A camada Controller recebe a requisição, chama o Service, que concentra a regra de negócio, e o Repository encapsula o acesso ao banco via Entity Framework. A injeção de dependência é configurada no Program.cs. Em operações financeiras, como transferência, é importante garantir consistência de dados, evitar duplicidade usando idempotency key e tratar erros de forma centralizada.

### Program.cs

Onde registramos dependências:

```csharp
builder.Services.AddScoped<IAccountService, AccountService>();
```

### Controller

Recebe a requisição HTTP e chama o Service.

### Service

Contém a regra de negócio: validar saldo, impedir duplicidade e movimentar valores.

### Repository

Acessa o banco usando Entity Framework.

### DbContext

Representa a sessão com o banco de dados.

### LINQ

Usado para consultar dados:

```csharp
_context.Transactions
    .Where(x => x.AccountId == accountId)
    .OrderByDescending(x => x.CreatedAt)
    .ToListAsync();
```

## Observação sobre Transaction

O projeto usa banco em memória. Em SQL Server/PostgreSQL real, a transferência deveria usar transação:

```csharp
await using var transaction = await _context.Database.BeginTransactionAsync();
try
{
    // debita origem
    // credita destino
    // salva transações
    await _context.SaveChangesAsync();
    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}
```
