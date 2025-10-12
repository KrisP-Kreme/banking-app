using Microsoft.AspNetCore.Mvc;
using AdminWebApi.Models;
using AdminWebApi.Models.Repository;

namespace AdminWebApi.Controllers;

// this ensures that api routes start with http://localhost:5063/api/payees
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
    // retrieves all payees from the repository, so that we can display it later.
    [HttpGet]
    public IEnumerable<Payee> Get()
    {
        return _repo.GetAll();
    }

    // GET api/payees/{id}
    // retrieves a single payee by its ID. {id} is the route parameter specifying which payee to get.
    [HttpGet("{id:int}")]
    public Payee Get(int id)
    {
        return _repo.Get(id);
    }

    // PUT api/payees
    // Updates an existing payee. The updated payee object is sent in the request body as JSON.
    // this is for when we want to update the payee information - we send a put request to update.
    [HttpPut]
    public void Put([FromBody] Payee payee)
    {
        _repo.Update(payee.PayeeID, payee);
    }
}
