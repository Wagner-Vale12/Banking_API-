using BankingApiVerity.Data;
using BankingApiVerity.Dtos;
using BankingApiVerity.Models;
using BankingApiVerity.Repositories;

namespace BankingApiVerity.Services;

public class AccountService : IAccountService
{
    private readonly AppDbContext _context;
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public AccountService(
        AppDbContext context,
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository)
    {
        _context = context;
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }

    public Task<List<Account>> GetAllAsync()
    {
        return _accountRepository.GetAllAsync();
    }

    public async Task<Account> CreateAsync(CreateAccountRequest request)
    {
        if (request.InitialBalance < 0)
            throw new InvalidOperationException("Saldo inicial não pode ser negativo.");

        var account = new Account
        {
            CustomerName = request.CustomerName,
            Document = request.Document,
            Balance = request.InitialBalance
        };

        await _accountRepository.AddAsync(account);
        await _context.SaveChangesAsync();

        return account;
    }

    public async Task<Account> DepositAsync(Guid accountId, DepositRequest request)
    {
        if (request.Amount <= 0)
            throw new InvalidOperationException("Valor precisa ser maior que zero.");

        if (await _transactionRepository.ExistsByIdempotencyKeyAsync(request.IdempotencyKey))
            throw new InvalidOperationException("Operação duplicada detectada.");

        var account = await _accountRepository.GetByIdAsync(accountId)
            ?? throw new KeyNotFoundException("Conta não encontrada.");

        account.Balance += request.Amount;

        await _transactionRepository.AddAsync(new BankTransaction
        {
            AccountId = account.Id,
            Type = "CREDIT",
            Amount = request.Amount,
            Description = request.Description,
            IdempotencyKey = request.IdempotencyKey
        });

        await _context.SaveChangesAsync();
        return account;
    }

    public async Task TransferAsync(TransferRequest request)
    {
        if (request.Amount <= 0)
            throw new InvalidOperationException("Valor precisa ser maior que zero.");

        if (await _transactionRepository.ExistsByIdempotencyKeyAsync(request.IdempotencyKey))
            throw new InvalidOperationException("Transferência duplicada detectada.");

        var from = await _accountRepository.GetByIdAsync(request.FromAccountId)
            ?? throw new KeyNotFoundException("Conta origem não encontrada.");

        var to = await _accountRepository.GetByIdAsync(request.ToAccountId)
            ?? throw new KeyNotFoundException("Conta destino não encontrada.");

        if (from.Balance < request.Amount)
            throw new InvalidOperationException("Saldo insuficiente.");

        // Em banco relacional real, aqui usaríamos transaction com BeginTransactionAsync.
        // No InMemory provider não existe transação real, mas o raciocínio está no código.
        from.Balance -= request.Amount;
        to.Balance += request.Amount;

        await _transactionRepository.AddAsync(new BankTransaction
        {
            AccountId = from.Id,
            Type = "TRANSFER_OUT",
            Amount = request.Amount,
            Description = $"Transferência para {to.CustomerName}",
            IdempotencyKey = request.IdempotencyKey
        });

        await _transactionRepository.AddAsync(new BankTransaction
        {
            AccountId = to.Id,
            Type = "TRANSFER_IN",
            Amount = request.Amount,
            Description = $"Transferência recebida de {from.CustomerName}",
            IdempotencyKey = request.IdempotencyKey + "-IN"
        });

        await _context.SaveChangesAsync();
    }

    public Task<List<BankTransaction>> GetStatementAsync(Guid accountId)
    {
        return _transactionRepository.GetByAccountIdAsync(accountId);
    }
}
