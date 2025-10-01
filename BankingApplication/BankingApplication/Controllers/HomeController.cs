using BankingApplication.Data;
using BankingApplication.Models;
using McbaExample.Utilities;
using McbaExample.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using SimpleHashing.Net;

namespace McbaExample.Controllers;

public class HomeController : Controller    
{
    private readonly BankingApplicationContext _context;

    // Simulate being "logged in" as Matthew Bolger by hard-coding the CustomerID.
    private string SessionKey_CustomerID = $"{nameof(HomeController)}_CustomerID";

    public HomeController(BankingApplicationContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Lazy loading.
        //var customer = await _context.Customers.FindAsync(_customerID);
        var customerID = HttpContext.Session.GetInt32(SessionKey_CustomerID);

        if (customerID == null)
        {
            return RedirectToAction(nameof(Login));
        }

        // Eager loading.
        var customer = await _context.Customers.Include(x => x.Accounts).
            FirstOrDefaultAsync(x => x.CustomerID == customerID);

        return View(customer);
    }

    public async Task<IActionResult> Deposit(int accountNumber)
    {
        return View(
            new DepositViewModel
            {
                AccountNumber = accountNumber,
                Account = await _context.Accounts.FindAsync(accountNumber)
            });
    }

    [HttpPost]
    public async Task<IActionResult> Deposit(DepositViewModel viewModel)
    {
        viewModel.Account = await _context.Accounts.FindAsync(viewModel.AccountNumber);

        // Note this code could be moved out of the controller, e.g., into the model, business objects, facade,
        // validators, etc...
        if (viewModel.Account == null)
        {
            ModelState.AddModelError("", "Account not found.");
            return View(viewModel);
        }

        if (viewModel.Amount <= 0)
        {
            ModelState.AddModelError(nameof(viewModel.Amount), "Amount must be positive.");
            return View(viewModel);
        }
        if (viewModel.Amount.HasMoreThanTwoDecimalPlaces())
        {
            ModelState.AddModelError(nameof(viewModel.Amount), "Amount cannot have more than 2 decimal places.");
            return View(viewModel);
        }

        // Note this code could be moved out of the controller, e.g., into the model or repository (design pattern).
        viewModel.Account.Balance += viewModel.Amount;
        _context.Transactions.Add(
            new Transaction
            {
                TransactionType = TransactionType.D,
                Amount = (decimal)viewModel.Amount,
                TransactionTimeUtc = DateTime.UtcNow,
                AccountNumber = viewModel.AccountNumber
            });

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy() => View();

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (viewModel.LoginID == null)
        {
            ModelState.AddModelError(nameof(viewModel.LoginID), "Please enter a Login ID");
            return View(viewModel);
        }

        if (viewModel.Password == null)
        {
            ModelState.AddModelError(nameof(viewModel.LoginID), "Please enter a Password");
            return View(viewModel);
        }

        var login = await _context.Logins.FirstOrDefaultAsync(x => x.LoginID == viewModel.LoginID);

        if (login == null)
        {
            ModelState.AddModelError("", "Incorrect Login ID or Password, try again");
        }

        bool passwordMatches = new SimpleHash().Verify(viewModel.Password, login.PasswordHash);

        if (!passwordMatches)
        {
            ModelState.AddModelError("", "Invalid Login ID or Password, try again");
            return View(viewModel);
        }


        HttpContext.Session.SetInt32(SessionKey_CustomerID, login.CustomerID);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove(SessionKey_CustomerID);
        return RedirectToAction(nameof(Login));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
