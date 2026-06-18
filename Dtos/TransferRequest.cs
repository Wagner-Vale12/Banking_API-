namespace BankingApiVerity.Dtos;

public record TransferRequest(Guid FromAccountId, Guid ToAccountId, decimal Amount, string IdempotencyKey);
