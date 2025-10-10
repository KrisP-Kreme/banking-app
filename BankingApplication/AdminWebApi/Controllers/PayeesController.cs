using Microsoft.AspNetCore.Mvc;
using AdminWebApi.Models;
using AdminWebApi.Models.Repository;

namespace AdminWebApi.Controllers;

// See here for more information:
// https://learn.microsoft.com/en-au/aspnet/core/web-api/?view=aspnetcore-9.0

[ApiController]
[Route("api/[controller]")]
public class PayeesController : ControllerBase
{
    private readonly IPayeeRepository _repo;

    public PayeesController(IPayeeRepository repo)
    {
        _repo = repo;
    }

    // GET: api/payees
    [HttpGet]
    public IEnumerable<Payee> Get()
    {
        return _repo.GetAll();
    }

    // GET api/payees/1
    [HttpGet("{id:int}")]
    public Payee Get(int id)
    {
        return _repo.Get(id);
    }

    // POST api/payees
    [HttpPost]
    public int Post([FromBody] Payee payee)
    {
        _repo.Add(payee);

        return payee.PayeeID;
    }

    // PUT api/payees
    [HttpPut]
    public void Put([FromBody] Payee payee)
    {
        _repo.Update(payee.PayeeID, payee);
    }

    // DELETE api/payees/1
    [HttpDelete("{id:int}")]
    public int Delete(int id)
    {
        return _repo.Delete(id);
    }
}
