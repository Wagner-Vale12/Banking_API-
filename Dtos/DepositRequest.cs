namespace BankingApiVerity.Dtos;

public record DepositRequest(decimal Amount, string Description, string IdempotencyKey);
