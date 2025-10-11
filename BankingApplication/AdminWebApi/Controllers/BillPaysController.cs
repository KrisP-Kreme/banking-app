using Microsoft.AspNetCore.Mvc;
using AdminWebApi.Models;
using AdminWebApi.Models.Repository;

namespace AdminWebApi.Controllers;

// See here for more information:
// https://learn.microsoft.com/en-au/aspnet/core/web-api/?view=aspnetcore-9.0

[ApiController]
[Route("api/[controller]")]
public class BillPaysController : ControllerBase
{
    private readonly IBillPayRepository _repo;

    public BillPaysController(IBillPayRepository repo)
    {
        _repo = repo;
    }

    // GET: api/billpays
    [HttpGet]
    public IEnumerable<BillPay> Get()
    {
        return _repo.GetAll();
    }

    // GET api/billpay/1
    [HttpGet("{id:int}")]
    public BillPay Get(int id)
    {
        return _repo.Get(id);
    }

    // POST api/billpays
    [HttpPost]
    public int Post([FromBody] BillPay billpay)
    {
        _repo.Add(billpay);

        return billpay.BillPayID;
    }

    // PUT api/billpays
    [HttpPut]
    public void Put([FromBody] BillPay billpay)
    {
        _repo.Update(billpay.BillPayID, billpay);
    }

    // DELETE api/billpays/1
    [HttpDelete("{id:int}")]
    public int Delete(int id)
    {
        return _repo.Delete(id);
    }

    // PUT api/billpays/block/1
    [HttpPut("block/{id:int}")]
    public void Block(int id)
    {
        var bill = _repo.Get(id);

        bill.Status = BillPayStatus.Blocked;
        _repo.Update(id, bill);
    }

    // PUT api/billpays/unblock/1
    [HttpPut("unblock/{id:int}")]
    public void Unblock(int id)
    {
        var bill = _repo.Get(id);

        bill.Status = BillPayStatus.Pending;
        _repo.Update(id, bill);
    }
}
