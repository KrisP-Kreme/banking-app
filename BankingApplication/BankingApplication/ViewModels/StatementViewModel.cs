using BankingApplication.Models;

namespace BankingApplication.ViewModels;

public class StatementViewModel
{
    public int AccountNumber { get; set; }
    public Account Account { get; set; }
    public List<Transaction> Transactions { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}

