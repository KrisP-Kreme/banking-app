using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApplication.Models;

public class Login
{
    [Required, StringLength(8)]
    public string LoginID { get; set; }

    [ForeignKey(nameof(Customer))]
    public int CustomerID { get; set; }
    public virtual Customer Customer { get; set; }

    [Column(TypeName = "char")]
    [Required, StringLength(94)]
    public string PasswordHash { get; set; }
}
