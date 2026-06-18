using BankingApiVerity.Models;

namespace BankingApiVerity.Data;

public static class SeedData
{
    public static void Populate(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (context.Accounts.Any()) return;

        context.Accounts.AddRange(
            new Account { CustomerName = "Wagner", Document = "11111111111", Balance = 1000 },
            new Account { CustomerName = "Cliente Carrefour", Document = "22222222222", Balance = 500 }
        );

        context.SaveChanges();
    }
}
