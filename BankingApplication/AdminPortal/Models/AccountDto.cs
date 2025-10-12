using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPortal.Models;

public class AccountDto
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    public int AccountNumber { get; set; }

    [Column(TypeName = "money")]
    [Required, DataType(DataType.Currency)]
    public decimal Balance { get; set; }

    [InverseProperty("Account")]
    public virtual List<BillPayDto> BillPays { get; set; }
}