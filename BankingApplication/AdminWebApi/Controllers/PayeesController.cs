using Microsoft.AspNetCore.Mvc;
using AdminWebApi.Models;
using AdminWebApi.Models.Repository;

namespace AdminWebApi.Controllers;


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
    // Retrieves all payees from the repository.
    [HttpGet]
    public IEnumerable<Payee> Get()
    {
        return _repo.GetAll();
    }

    // GET api/payees/{id}
    // Retrieves a single payee by its ID.
    // {id} is the route parameter specifying which payee to get.
    [HttpGet("{id:int}")]
    public Payee Get(int id)
    {
        return _repo.Get(id);
    }

    // POST api/payees
    // Adds a new payee to the repository.
    // The payee object is sent in the request body as JSON.
    // Returns the ID of the newly created payee.
    [HttpPost]
    public int Post([FromBody] Payee payee)
    {
        _repo.Add(payee);

        return payee.PayeeID;
    }

    // PUT api/payees
    // Updates an existing payee.
    // The updated payee object is sent in the request body as JSON.
    [HttpPut]
    public void Put([FromBody] Payee payee)
    {
        _repo.Update(payee.PayeeID, payee);
    }

    // DELETE api/payees/{id}
    // Deletes a payee by its ID.
    // {id} is the route parameter specifying which payee to delete.
    // Returns the ID of the deleted payee.
    [HttpDelete("{id:int}")]
    public int Delete(int id)
    {
        return _repo.Delete(id);
    }
}
