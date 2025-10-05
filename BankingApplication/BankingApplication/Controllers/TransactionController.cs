using BankingApplication.Data;
using BankingApplication.Models;
using BankingApplication.Utilities;
using BankingApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication.Controllers;

public class TransactionController : Controller
{
    private readonly BankingApplicationContext _context;

    // get the current session's user
    private int? CustomerID => HttpContext.Session.GetInt32($"{nameof(HomeController)}_CustomerID");

    public TransactionController(BankingApplicationContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Lazy loading.
        // The Customer.Accounts property will be lazy loaded upon demand.
        //var customer = await _context.Customers.FindAsync(CustomerID);

        // OR
        // Eager loading. (because we're using viewmodel, so we don't link to the model directly)
        var customer = await _context.Customers.Include(x => x.Accounts).
            FirstOrDefaultAsync(x => x.CustomerID == CustomerID);

        return View(customer);
    }

    public async Task<IActionResult> Deposit(int accountNumber)
    {
        return View(
            new TransactionViewModel
            {
                AccountNumber = accountNumber,
                Account = await _context.Accounts.FindAsync(accountNumber)
            });
    }

    [HttpPost]
    public async Task<IActionResult> Deposit(TransactionViewModel viewModel)
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
                Comment = viewModel.Comment,
                TransactionTimeUtc = DateTime.UtcNow,
                AccountNumber = viewModel.AccountNumber
            });

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Withdraw(int accountNumber)
    {
        return View(
            new TransactionViewModel
            {
                AccountNumber = accountNumber,
                Account = await _context.Accounts.FindAsync(accountNumber)
            });
    }

    [HttpPost]
    public async Task<IActionResult> Withdraw(TransactionViewModel viewModel)
    {
        // this is the submitted information
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
        AccountType accType = viewModel.Account.AccountType;
        double currBalance = (double)viewModel.Account.Balance;
        int numTransactions = await _context.Transactions.CountAsync(tc => (tc.TransactionType == TransactionType.W || (tc.TransactionType == TransactionType.T && tc.DestinationAccountNumber != null)) && tc.AccountNumber == viewModel.Account.AccountNumber);
        
        if (accType == AccountType.C)   // if account is checking
        {
            if (numTransactions >= 2)    // more than 2 transactions already
            {
                if ((decimal)(currBalance + 500) - viewModel.Amount < (decimal)0.01)
                {
                    ModelState.AddModelError(nameof(viewModel.Amount), "Amount is more than your balance");
                    return View(viewModel); // return early, balance doesn't get updated
                }

            } 
            else
            {
                if ((decimal)(currBalance + 500) - viewModel.Amount < 0)
                {
                    ModelState.AddModelError(nameof(viewModel.Amount), "Amount is more than your balance");
                    return View(viewModel);
                }
            }
            
        } else if (accType == AccountType.S) // if savings account, we dont need to account for the 500 overdraft
        {
            if (numTransactions >= 2)
            {
                if ((decimal)currBalance - viewModel.Amount < (decimal)0.01)
                {
                    ModelState.AddModelError(nameof(viewModel.Amount), "Amount is more than your balance");
                    return View(viewModel);
                }
            }
            else
            {
                if ((decimal)currBalance - viewModel.Amount < 0)
                {
                    ModelState.AddModelError(nameof(viewModel.Amount), "Amount is more than your balance");
                    return View(viewModel);
                }
            }
            
        }

        if (numTransactions >= 2)
        {
            viewModel.Account.Balance -= (decimal)0.01;
            _context.Transactions.Add(
            new Transaction
            {
                TransactionType = TransactionType.S,
                Amount = (decimal)0.01,
                TransactionTimeUtc = DateTime.UtcNow,
                AccountNumber = viewModel.AccountNumber

            });
        }

        // if no instance of failure
        viewModel.Account.Balance -= viewModel.Amount;
        _context.Transactions.Add(
            new Transaction
            {
                TransactionType = TransactionType.W,
                Amount = (decimal)viewModel.Amount,
                Comment = viewModel.Comment,
                TransactionTimeUtc = DateTime.UtcNow,
                AccountNumber = viewModel.AccountNumber
            });

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Transfer(int accountNumber)
    {
        return View(
            new TransactionViewModel
            {
                AccountNumber = accountNumber,
                Account = await _context.Accounts.FindAsync(accountNumber)
            });
    }

    //Transfer requires destination account number
    //vm takes in the dest acc number too if we ask for it.
    // TODO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    [HttpPost]
    public async Task<IActionResult> Transfer(TransactionViewModel viewModel)
    {
        // this is the submitted information
        viewModel.Account = await _context.Accounts.FindAsync(viewModel.AccountNumber);

        // Note this code could be moved out of the controller, e.g., into the model, business objects, facade,
        // validators, etc...
        if (viewModel.Account == null)
        {
            ModelState.AddModelError("", "Account not found.");
            return View(viewModel);
        }

        // TODO: if account is not in the list of accounts in db
        var accNums = await _context.Accounts.Where(a => a.AccountNumber != viewModel.AccountNumber).Select(a => a.AccountNumber).ToListAsync();
        bool accExist = false;
        bool ownAcc = false;
        foreach (var accNum in accNums)
        {
            if (accNum == viewModel.DestinationAccountNumber)
            {
                accExist = true;
            }
            if (accNum == viewModel.AccountNumber)
            {
                ownAcc = true;
            }
        }
        if (viewModel.DestinationAccountNumber == null || accExist == false)
        {
            ModelState.AddModelError("", "Account not found.");
            return View(viewModel);
        }
        // if account is own account
        if (ownAcc == true)
        {
            ModelState.AddModelError("", "Can't transfer to your own account.");
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

        AccountType accType = viewModel.Account.AccountType;
        double currBalance = (double)viewModel.Account.Balance;
        int numTransactions = await _context.Transactions.CountAsync(tc => (tc.TransactionType == TransactionType.W || (tc.TransactionType == TransactionType.T && tc.DestinationAccountNumber != null)) && tc.AccountNumber == viewModel.Account.AccountNumber);

        if (accType == AccountType.C)   // if account is checking
        {
            if (numTransactions >= 2)    // more than 2 transactions already
            {
                if ((decimal)(currBalance + 500) - viewModel.Amount < (decimal)0.05)
                {
                    ModelState.AddModelError(nameof(viewModel.Amount), "Amount is more than your balance");
                    return View(viewModel); // return early, balance doesn't get updated
                }

            }
            else
            {
                if ((decimal)(currBalance + 500) - viewModel.Amount < 0)
                {
                    ModelState.AddModelError(nameof(viewModel.Amount), "Amount is more than your balance");
                    return View(viewModel);
                }
            }

        }
        else if (accType == AccountType.S) // if savings account, we dont need to account for the 500 overdraft
        {
            if (numTransactions >= 2)
            {
                if ((decimal)currBalance - viewModel.Amount < (decimal)0.05)
                {
                    ModelState.AddModelError(nameof(viewModel.Amount), "Amount is more than your balance");
                    return View(viewModel);
                }
            }
            else
            {
                if ((decimal)currBalance - viewModel.Amount < 0)
                {
                    ModelState.AddModelError(nameof(viewModel.Amount), "Amount is more than your balance");
                    return View(viewModel);
                }
            }

        }

        // service charge for >2 transactions
        if (numTransactions >= 2)
        {
            viewModel.Account.Balance -= (decimal)0.05;
            _context.Transactions.Add(
            new Transaction
            {
                TransactionType = TransactionType.S,
                Amount = (decimal)0.05,
                TransactionTimeUtc = DateTime.UtcNow,
                AccountNumber = viewModel.AccountNumber

            });
        }

        // deduct from current account
        viewModel.Account.Balance -= viewModel.Amount;
        _context.Transactions.Add(
            new Transaction
            {
                TransactionType = TransactionType.T,
                Amount = (decimal)viewModel.Amount,
                Comment = viewModel.Comment,
                TransactionTimeUtc = DateTime.UtcNow,
                AccountNumber = viewModel.AccountNumber,
                DestinationAccountNumber = viewModel.DestinationAccountNumber
            });

        // add to account we're transferring to
        Account destAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == viewModel.DestinationAccountNumber);
        destAccount.Balance += viewModel.Amount;

        _context.Transactions.Add(
            new Transaction
            {
                TransactionType = TransactionType.T,
                Amount = (decimal)viewModel.Amount,
                TransactionTimeUtc = DateTime.UtcNow,
                AccountNumber = destAccount.AccountNumber
            });

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
