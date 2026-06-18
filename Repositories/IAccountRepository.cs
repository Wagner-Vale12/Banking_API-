using BankingApiVerity.Models;

namespace BankingApiVerity.Repositories;

public interface IAccountRepository
{
    Task<List<Account>> GetAllAsync();
    Task<Account?> GetByIdAsync(Guid id);
    Task AddAsync(Account account);
}
