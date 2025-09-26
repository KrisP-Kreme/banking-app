using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApplication.Models;

public enum AccountType
{
    Checking = 1,
    Saving = 2,
}

public class Account
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    public int AccountNumber { get; set; }

    [Required, Display(Name = "Type")]
    public AccountType AccountType { get; set; }

    [ForeignKey(nameof(Customer))]
    public int CustomerID { get; set; }
    public virtual Customer Customer { get; set; }

    [Column(TypeName = "money")]
    [Required, DataType(DataType.Currency)]
    public decimal Balance { get; set; }

    [InverseProperty("Account")]
    public virtual List<Transaction> Transactions { get; set; }
}