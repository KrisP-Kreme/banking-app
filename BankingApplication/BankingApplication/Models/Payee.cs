using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApplication.Models;

public class Payee
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int PayeeID { get; set; }

    [Required, StringLength(50)]
    public string Name { get; set; }

    [Required, StringLength(50)]
    public string Address { get; set; }

    [Required, StringLength(40), RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
    public string City { get; set; }

    [Required, StringLength(3), RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
    public string State { get; set; }

    [Required, StringLength(4)]
    public string Postcode { get; set; }

    [Required, StringLength(14)]
    public int Phone { get; set; }
}