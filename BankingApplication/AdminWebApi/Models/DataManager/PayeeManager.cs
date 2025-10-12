using AdminWebApi.Data;
using AdminWebApi.Models.Repository;

namespace AdminWebApi.Models.DataManager;

public class PayeeManager : IPayeeRepository, IDataRepository<Payee, int>
{
    private readonly MvcAdminContext _context;

    // context's "payee" reference is "Payees" table
    public PayeeManager(MvcAdminContext context)
    {
        _context = context;
    }

    public Payee Get(int id)
    {
        return _context.Payees.Find(id);
    }

    public IEnumerable<Payee> GetAll()
    {
        return _context.Payees.ToList();
    }

    public int Update(int id, Payee payee)
    {
        _context.Payees.Update(payee);
        _context.SaveChanges();

        return id;
    }
}
