namespace BankingApiVerity.Dtos;

public record CreateAccountRequest(string CustomerName, string Document, decimal InitialBalance);
