using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleHashing.Net;

namespace BankingApplication.Controllers
{
    public class CustomerController : Controller
    {
        private readonly BankingApplicationContext _context;
        private int? CustomerID => HttpContext.Session.GetInt32($"{nameof(HomeController)}_CustomerID");

        public CustomerController(BankingApplicationContext context)
        {
            _context = context;
        }

        // view profile
        public async Task<IActionResult> Index()
        {
            if (CustomerID == null)
                return RedirectToAction("Login", "Home");

            var customer = await _context.Customers
                .Include(c => c.Login)
                .FirstOrDefaultAsync(c => c.CustomerID == CustomerID);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // get to the specific page of the user that we want to edit
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            if (CustomerID == null)
                return RedirectToAction("Login", "Home");

            var customer = await _context.Customers.FindAsync(CustomerID);
            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // parse the parameter of updated payee information
        [HttpPost]
        public async Task<IActionResult> Edit(Customer model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customer = await _context.Customers.FindAsync(CustomerID);
            if (customer == null)
                return NotFound();

            // Update fields
            customer.Name = model.Name;
            customer.TFN = model.TFN;
            customer.Address = model.Address;
            customer.City = model.City;
            customer.State = model.State;
            customer.Postcode = model.Postcode;
            customer.Mobile = model.Mobile; 

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // specifically for password changing
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            if (CustomerID == null)
                return RedirectToAction("Login", "Home");

            // get the current customer
            var customer = await _context.Customers.Include(c => c.Login)
                .FirstOrDefaultAsync(c => c.CustomerID == CustomerID);

            if (customer == null)
                return NotFound();

            return View();
        }

        // change password and hashing it
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            if (CustomerID == null)
                return RedirectToAction("Login", "Home");

            var customer = await _context.Customers.Include(c => c.Login)
                .FirstOrDefaultAsync(c => c.CustomerID == CustomerID);

            if (customer == null)
                return NotFound();

            var hashHelper = new SimpleHash();

            // verify old password
            bool passwordMatches = hashHelper.Verify(oldPassword, customer.Login.PasswordHash);
            if (!passwordMatches)
            {
                ModelState.AddModelError("", "Old password is incorrect.");
                return View();
            }

            // check new passwords match
            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("", "New passwords do not match.");
                return View();
            }

            // hash and save
            customer.Login.PasswordHash = hashHelper.Compute(newPassword);
            await _context.SaveChangesAsync();

            ViewBag.Message = "Password changed successfully.";
            return View();
        }

    }
}
