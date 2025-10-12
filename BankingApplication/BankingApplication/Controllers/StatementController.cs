using Microsoft.AspNetCore.Mvc;
using BankingApplication.Data;
using BankingApplication.Models;
using BankingApplication.Utilities;
using BankingApplication.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication.Controllers;

public class StatementController : Controller
{
    private readonly BankingApplicationContext _context;
    private int? CustomerID => HttpContext.Session.GetInt32($"{nameof(HomeController)}_CustomerID");
    public StatementController(BankingApplicationContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        if (CustomerID == null)
            return RedirectToAction("Login", "Home");

        var customer = await _context.Customers.Include(x => x.Accounts).
            FirstOrDefaultAsync(x => x.CustomerID == CustomerID);

        return View(customer);
    }

    public async Task<IActionResult> Statement(int accountNumber, int page = 1)
    {
        if (CustomerID == null)
            return RedirectToAction("Login", "Home");

        const int pageSize = 4;

        var account = await _context.Accounts.FindAsync(accountNumber);
        if (account == null)
            return NotFound();

        // query to get the list of transactions in the order of most recently added
        var transactionsQuery = _context.Transactions
            .Where(t => t.AccountNumber == accountNumber)
            .OrderByDescending(t => t.TransactionTimeUtc);

        int totalTransactions = await transactionsQuery.CountAsync();

        // pagination
        var transactions = await transactionsQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var viewModel = new StatementViewModel
        {
            AccountNumber = accountNumber,
            Account = account,
            Transactions = transactions,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(totalTransactions / (double)pageSize)
        };

        return View(viewModel);
    }

}
