using BankingApplication.Models;

namespace McbaExample.ViewModels;

public class DepositViewModel
{
    public int AccountNumber { get; set; }
    public Account Account { get; set; }
    public decimal Amount { get; set; }
}
