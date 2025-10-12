namespace AdminWebApi.Models.Repository;

public interface IBillPayRepository
{
    BillPay Get(int id);
    IEnumerable<BillPay> GetAll();
    int Add(BillPay billPay);
    int Delete(int id);
    int Update(int id, BillPay billPay);
}
