using BankingApplication.Data;
using BankingApplication.Models;
using BankingApplication.Models;
using Newtonsoft.Json;

namespace BankingApplication.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<BankingApplicationContext>();

        // Look for customers.
        if(context.Customers.Any())
            return; // DB has already been seeded.

        const string Url = "https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";

        using var client = new HttpClient();
        var json = client.GetStringAsync(Url).Result;

        var customers = JsonConvert.DeserializeObject<Customer[]>(json, new JsonSerializerSettings
        {
            DateFormatString = "dd/MM/yyyy hh:mm:ss tt"
        });

        foreach (var c in customers)
        {
            context.Customers.Add(
                new Customer
                {
                    CustomerID = c.CustomerID,
                    Name = c.Name,
                    TFN = c.TFN,
                    Address = c.Address,
                    City = c.City,
                    State = c.State,
                    Postcode = c.Postcode,
                    Mobile = c.Mobile
                });

            context.Logins.Add(
                new Login
                {
                    LoginID = c.Login.LoginID,
                    CustomerID = c.CustomerID,
                    PasswordHash = c.Login.PasswordHash
                });

            foreach (var a in c.Accounts)
            {
                decimal balance = 0;

                if (a.Transactions != null && a.Transactions.Count != 0)
                {
                    foreach(var t in a.Transactions)
                    {
                        balance += t.Amount;
                    }
                }

                var account = new Account
                {
                    AccountNumber = a.AccountNumber,
                    AccountType = a.AccountType,
                    CustomerID = a.CustomerID,
                    Balance = balance
                };

                context.Accounts.Add(account);

                foreach (var t in a.Transactions)
                {
                    context.Transactions.Add(new Transaction
                    {
                        Account = account,
                        TransactionType = t.TransactionType,
                        Amount = t.Amount,
                        Comment = t.Comment,
                        TransactionTimeUtc = t.TransactionTimeUtc
                    });
                }
            }


        }
            
        context.SaveChanges();
    }
}
