using AdminWebApi.Data;
using AdminWebApi.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace AdminWebApi.Models.DataManager;

public class BillPayManager : IBillPayRepository, IDataRepository<BillPay, int>
{
    private readonly MvcAdminContext _context;

    public BillPayManager(MvcAdminContext context)
    {
        _context = context;
    }

    public BillPay Get(int id)
    {
        return _context.BillPays
            .Include(bp => bp.Payee)
            .FirstOrDefault(bp => bp.BillPayID == id);
    }

    public IEnumerable<BillPay> GetAll()
    {
        return _context.BillPays
            .Include(bp => bp.Payee)
            .ToList();
    }

    public int Add(BillPay billPay)
    {
        _context.BillPays.Add(billPay);
        _context.SaveChanges();

        return billPay.BillPayID;
    }

    public int Delete(int id)
    {
        _context.BillPays.Remove(_context.BillPays.Find(id));
        _context.SaveChanges();

        return id;
    }

    public int Update(int id, BillPay billPay)
    {
        _context.BillPays.Update(billPay);
        _context.SaveChanges();

        return id;
    }
}
