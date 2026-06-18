using BankingApiVerity.Models;

namespace BankingApiVerity.Repositories;

public interface ITransactionRepository
{
    Task<bool> ExistsByIdempotencyKeyAsync(string idempotencyKey);
    Task AddAsync(BankTransaction transaction);
    Task<List<BankTransaction>> GetByAccountIdAsync(Guid accountId);
}
