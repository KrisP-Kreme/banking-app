using Microsoft.EntityFrameworkCore;
using BankingApplication.Models;

namespace BankingApplication.Data;

public class BankingApplicationContext : DbContext
{
    public BankingApplicationContext(DbContextOptions<BankingApplicationContext> options) : base(options)
    { }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<BillPay> BillPays { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Login> Logins { get; set; }
    public DbSet<Payee> Payees { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}