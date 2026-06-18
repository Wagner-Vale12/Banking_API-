using BankingApiVerity.Data;
using BankingApiVerity.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApiVerity.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;

    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Account>> GetAllAsync()
    {
        return _context.Accounts.OrderBy(x => x.CustomerName).ToListAsync();
    }

    public Task<Account?> GetByIdAsync(Guid id)
    {
        return _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
    }
}
