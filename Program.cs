using BankingApiVerity.Data;
using BankingApiVerity.Middlewares;
using BankingApiVerity.Repositories;
using BankingApiVerity.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Banco em memória para estudo. Em produção seria SQL Server/PostgreSQL.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("BankingDb"));

// Dependency Injection
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

SeedData.Populate(app);

app.Run();
