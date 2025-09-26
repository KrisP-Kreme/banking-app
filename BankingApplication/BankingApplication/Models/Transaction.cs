using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApplication.Models;

public enum TransactionType
{
    D = 1, // Deposit
    W = 2, // Debit withdraw
    T = 3, // outgoing transfer transaction (debit transf from source) & incoming transfer transaction (credit transf from target)
    S = 4, // debit service charge
    B = 5 // debit billpay
}

public class Transaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TransactionID { get; set; }

    [Required]
    public TransactionType TransactionType { get; set; }

    [ForeignKey(nameof(Account))]
    public int AccountNumber { get; set; }
    public virtual Account Account { get; set; }

    [ForeignKey("DestinationAccount")]
    public int? DestinationAccountNumber { get; set; }
    public virtual Account DestinationAccount { get; set; }

    [Required]
    [Column(TypeName = "money")]
    public decimal Amount { get; set; }

    [StringLength(30), RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
    public string? Comment { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime TransactionTimeUtc { get; set; }
}