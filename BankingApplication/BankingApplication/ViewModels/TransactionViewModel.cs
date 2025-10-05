using BankingApplication.Models;

namespace BankingApplication.ViewModels;

public class TransactionViewModel
{
    public int AccountNumber { get; set; }
    public int? DestinationAccountNumber { get; set; }
    public Account Account { get; set; }
    public decimal Amount { get; set; }
    public string Comment { get; set; }
}
