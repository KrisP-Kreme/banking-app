using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApplication.Models;

public class Customer
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int CustomerID { get; set; }

    [Required, StringLength(50)]
    public string Name { get; set; }

    [StringLength(11)]
    public int TFN { get; set; }

    [StringLength(50)]
    public string Address { get; set; }

    [StringLength(40)]
    public string City { get; set; }

    [StringLength(3)]
    public string State { get; set; }

    [StringLength(4)]
    public string Postcode { get; set; }

    [StringLength(12)]
    public int Mobile { get; set; }

    public Login Login { get; set; }

    public virtual List<Account> Accounts { get; set; }
}