namespace AdminWebApi.Models.Repository;

public interface IPayeeRepository
{
    Payee Get(int id);
    IEnumerable<Payee> GetAll();
    int Add(Payee payee);
    int Delete(int id);
    int Update(int id, Payee payee);
}
