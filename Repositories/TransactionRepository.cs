using BankingApiVerity.Data;
using BankingApiVerity.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApiVerity.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<bool> ExistsByIdempotencyKeyAsync(string idempotencyKey)
    {
        return _context.Transactions.AnyAsync(x => x.IdempotencyKey == idempotencyKey);
    }

    public async Task AddAsync(BankTransaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
    }

    public Task<List<BankTransaction>> GetByAccountIdAsync(Guid accountId)
    {
        return _context.Transactions
            .Where(x => x.AccountId == accountId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}
