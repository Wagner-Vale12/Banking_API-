namespace BankingApiVerity.Models;

public class BankTransaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AccountId { get; set; }
    public string Type { get; set; } = string.Empty; // DEBIT, CREDIT, TRANSFER_OUT, TRANSFER_IN
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string IdempotencyKey { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
