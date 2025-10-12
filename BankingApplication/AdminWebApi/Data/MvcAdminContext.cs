using Microsoft.EntityFrameworkCore;
using AdminWebApi.Models;

namespace AdminWebApi.Data;

public class MvcAdminContext : DbContext
{
	public MvcAdminContext(DbContextOptions<MvcAdminContext> options) : base(options)
	{ }

	// these are the tables from the database that we want to access
	public DbSet<Account> Accounts { get; set; }
	public DbSet<BillPay> BillPays { get; set; }
	public DbSet<Payee> Payees { get; set; }

}