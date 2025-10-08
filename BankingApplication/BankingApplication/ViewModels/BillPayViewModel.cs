using System.Collections.Generic;
using BankingApplication.Models;

namespace BankingApplication.ViewModels;
public class BillPayViewModel
{
    public BillPay BillPay { get; set; }
    public List<Payee> Payees { get; set; }
}
