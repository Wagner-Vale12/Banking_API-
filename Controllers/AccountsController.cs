using BankingApiVerity.Dtos;
using BankingApiVerity.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankingApiVerity.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var accounts = await _accountService.GetAllAsync();
        return Ok(accounts);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAccountRequest request)
    {
        var account = await _accountService.CreateAsync(request);
        return Created($"/api/accounts/{account.Id}", account);
    }

    [HttpPost("{accountId:guid}/deposit")]
    public async Task<IActionResult> Deposit(Guid accountId, DepositRequest request)
    {
        var account = await _accountService.DepositAsync(accountId, request);
        return Ok(account);
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer(TransferRequest request)
    {
        await _accountService.TransferAsync(request);
        return NoContent();
    }

    [HttpGet("{accountId:guid}/statement")]
    public async Task<IActionResult> GetStatement(Guid accountId)
    {
        var statement = await _accountService.GetStatementAsync(accountId);
        return Ok(statement);
    }
}
