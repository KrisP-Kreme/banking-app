using AdminPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AdminPortal.Controllers
{
    public class BillPayController : Controller
    {
        private readonly HttpClient _client;

        public BillPayController(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("api");
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToAction("Login", "Home");

            using var response = await _client.GetAsync("api/billpays");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            // Deserializing the response received from web api and storing into a list.
            var billpays = JsonConvert.DeserializeObject<List<BillPayDto>>(result);

            return View(billpays);
        }

        [HttpPost]
        public async Task<IActionResult> Block(int id)
        {
            await _client.PutAsync($"api/billpays/block/{id}", null);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Unblock(int id)
        {
            await _client.PutAsync($"api/billpays/unblock/{id}", null);
            return RedirectToAction(nameof(Index));
        }


    }

}
