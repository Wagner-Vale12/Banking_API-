using BankingApiVerity.Dtos;
using BankingApiVerity.Models;

namespace BankingApiVerity.Services;

public interface IAccountService
{
    Task<List<Account>> GetAllAsync();
    Task<Account> CreateAsync(CreateAccountRequest request);
    Task<Account> DepositAsync(Guid accountId, DepositRequest request);
    Task TransferAsync(TransferRequest request);
    Task<List<BankTransaction>> GetStatementAsync(Guid accountId);
}
