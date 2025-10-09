using BankingApplication.Data;
using BankingApplication.Models;
using BankingApplication.Utilities;
using BankingApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication.Controllers;

public class BillPayController : Controller
{
    private readonly BankingApplicationContext _context;

    // get the current session's user
    private int? CustomerID => HttpContext.Session.GetInt32($"{nameof(HomeController)}_CustomerID");

    public BillPayController(BankingApplicationContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Create(int accountNumber)
    {
        var viewModel = new BillPayViewModel
        {
            BillPay = new BillPay
            {
                AccountNumber = accountNumber
            },
            Payees = _context.Payees.ToList()
        };

        return View(viewModel);

    }

    [HttpPost]
    public IActionResult Create(BillPayViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            viewModel.BillPay.ScheduleTimeUtc = DateTime.SpecifyKind(viewModel.BillPay.ScheduleTimeUtc, DateTimeKind.Utc);
            viewModel.Payees = _context.Payees.ToList();
            return View(viewModel);
        }

        _context.BillPays.Add(viewModel.BillPay);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Index()
    {
        var customer = await _context.Customers.Include(x => x.Accounts).
            ThenInclude(x => x.BillPays).
            ThenInclude(x => x.Payee).
            FirstOrDefaultAsync(x => x.CustomerID == CustomerID);

        return View(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var billPay = await _context.BillPays.FindAsync(id);

        _context.BillPays.Remove(billPay);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Retry(int id)
    {
        var billPay = await _context.BillPays.FindAsync(id);
        if (billPay == null) return NotFound();

        billPay.Status = BillPayStatus.Pending;
        billPay.ScheduleTimeUtc = DateTime.UtcNow.AddMinutes(1);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

}
