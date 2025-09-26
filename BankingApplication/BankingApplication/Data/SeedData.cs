using BankingApplication.Data;
using BankingApplication.Models;
using McbaExample.Models;

namespace McbaExample.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<BankingApplicationContext>();

        // Look for customers.
        if(context.Customers.Any())
            return; // DB has already been seeded.

        context.Customers.AddRange(
            new Customer
            {
                CustomerID = 2100,
                Name = "Matthew Bolger",
                Address = "123 Fake Street",
                City = "Melbourne",
                Postcode = 3000
            },
            new Customer
            {
                CustomerID = 2200,
                Name = "Rodney Cocker",
                Address = "456 Real Road",
                City = "Melbourne",
                Postcode = 3005
            },
            new Customer
            {
                CustomerID = 2300,
                Name = "Shekhar Kalra"
            });

        context.Logins.AddRange(
            new Login
            {
                LoginID = "12345678",
                CustomerID = 2100,
                PasswordHash =
                    "Rfc2898DeriveBytes$50000$MrW2CQoJvjPMlynGLkGFrg==$x8iV0TiDbEXndl0Fg8V3Rw91j5f5nztWK1zu7eQa0EE="
            },
            new Login
            {
                LoginID = "38074569",
                CustomerID = 2200,
                PasswordHash =
                    "Rfc2898DeriveBytes$50000$fB5lteA+LLB0mKVz9EBA7A==$Tx0nXJ8aJjBU/mS2ssFIMs3m7vaiyitRmBRvBAYWauw="
            },
            new Login
            {
                LoginID = "17963428",
                CustomerID = 2300,
                PasswordHash =
                    "Rfc2898DeriveBytes$50000$jDBijGZNWLh+0MOXnp68Yw==$4bQ9SJGtRQJolToCjFTPsVzRtH8QQUpEsioJ6Y3ewN4="
            });

        context.Accounts.AddRange(
            new Account
            {
                AccountNumber = 4100,
                AccountType = AccountType.Saving,
                CustomerID = 2100,
                Balance = 100
            },
            new Account
            {
                AccountNumber = 4101,
                AccountType = AccountType.Checking,
                CustomerID = 2100,
                Balance = 900
            },
            new Account
            {
                AccountNumber = 4200,
                AccountType = AccountType.Saving,
                CustomerID = 2200,
                Balance = 500.95m
            },
            new Account
            {
                AccountNumber = 4300,
                AccountType = AccountType.Checking,
                CustomerID = 2300,
                Balance = 1250.50m
            });
            
        const string format = "dd/MM/yyyy hh:mm:ss tt";

        context.Transactions.AddRange(
            new Transaction
            {
                TransactionType = TransactionType.D,
                AccountNumber = 4100,
                Amount = 100,
                Comment = "Opening balance",
                TransactionTimeUtc = DateTime.ParseExact("02/01/2025 08:00:00 PM", format, null)
            },
            new Transaction
            {
                TransactionType = TransactionType.D,
                AccountNumber = 4101,
                Amount = 600,
                Comment = "First deposit",
                TransactionTimeUtc = DateTime.ParseExact("02/01/2025 08:30:00 PM", format, null)
            },
            new Transaction
            {
                TransactionType = TransactionType.D,
                AccountNumber = 4101,
                Amount = 300,
                Comment = "Second deposit",
                TransactionTimeUtc = DateTime.ParseExact("02/01/2025 08:45:00 PM", format, null)
            },
            new Transaction
            {
                TransactionType = TransactionType.D,
                AccountNumber = 4200,
                Amount = 500,
                Comment = "Deposited $500",
                TransactionTimeUtc = DateTime.ParseExact("02/01/2025 09:00:00 PM", format, null)
            },
            new Transaction
            {
                TransactionType = TransactionType.D,
                AccountNumber = 4200,
                Amount = (double)0.95m,
                Comment = "Deposited $0.95",
                TransactionTimeUtc = DateTime.ParseExact("02/01/2025 09:15:00 PM", format, null)
            },
            new Transaction
            {
                TransactionType = TransactionType.D,
                AccountNumber = 4300,
                Amount = (double)1250.50m,
                Comment = null,
                TransactionTimeUtc = DateTime.ParseExact("02/01/2025 10:00:00 PM", format, null)
            });

        context.SaveChanges();
    }
}
