using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using AdminPortal.Models;
using Newtonsoft.Json;

namespace AdminPortal.Controllers;

public class PayeesController : Controller
{
    private readonly HttpClient _client;

    public PayeesController(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("api");
    }

    // GET: Payees/Index (with postcode filtering)
    public async Task<IActionResult> Index(string? postcode)
    {
        if (HttpContext.Session.GetString("Username") == null)
            return RedirectToAction("Login", "Home");

        using var response = await _client.GetAsync("api/payees");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        var payees = JsonConvert.DeserializeObject<List<PayeeDto>>(result) ?? new List<PayeeDto>();

        // Filter locally by postcode
        if (!string.IsNullOrWhiteSpace(postcode))
        {
            payees = payees.Where(p => p.Postcode != null && p.Postcode.Contains(postcode)).ToList();
            ViewData["CurrentPostcode"] = postcode; // keep value in search box
        }

        return View(payees);
    }


    // GET: Payees/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Payees/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(PayeeDto payee)
    {
        if (ModelState.IsValid)
        {
            var content =
                new StringContent(JsonConvert.SerializeObject(payee), Encoding.UTF8, MediaTypeNames.Application.Json);

            using var response = _client.PostAsync("api/payees", content).Result;
            //using var response = PayeeApi.InitializeClient().PostAsync("api/payees", content).Result;

            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }

        return View(payee);
    }

    // GET: Payees/Edit/1
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        using var response = await _client.GetAsync($"api/payees/{id}");
        //using var response = await PayeeApi.InitializeClient().GetAsync($"api/payees/{id}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        var payee = JsonConvert.DeserializeObject<PayeeDto>(result);

        return View(payee);
    }

    // POST: Payees/Edit/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, PayeeDto payee)
    {
        if (id != payee.PayeeID)
            return NotFound();

        if (ModelState.IsValid)
        {
            var content =
                new StringContent(JsonConvert.SerializeObject(payee), Encoding.UTF8, MediaTypeNames.Application.Json);

            using var response = _client.PutAsync("api/payees", content).Result;
            //using var response = PayeeApi.InitializeClient().PutAsync("api/payees", content).Result;

            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }

        return View(payee);
    }

    // GET: Payees/Delete/1
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        using var response = await _client.GetAsync($"api/payees/{id}");
        //using var response = await PayeeApi.InitializeClient().GetAsync($"api/payees/{id}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        var payee = JsonConvert.DeserializeObject<PayeeDto>(result);

        return View(payee);
    }

    // POST: Payees/Delete/1
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        using var response = _client.DeleteAsync($"api/payees/{id}").Result;
        //using var response = PayeeApi.InitializeClient().DeleteAsync($"api/payees/{id}").Result;

        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index");
    }
}
