using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApplication.Models;

public class Customer
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int CustomerID { get; set; }

    [Required(ErrorMessage = "Name is required."), StringLength(50)]
    [RegularExpression(@"^[A-Za-z\s]{2,}$", ErrorMessage = "Name must contain only letters and be at least 2 characters long.")]
    public string Name { get; set; }

    [RegularExpression(@"^(\d{11})?$", ErrorMessage = "TFN must be exactly 11 digits or left blank.")]
    [StringLength(11)]
    public string TFN { get; set; }

    [RegularExpression(@"^.{2,}$|^$", ErrorMessage = "Address must be at least 2 characters or left blank.")]
    [StringLength(50)]
    public string Address { get; set; }

    [RegularExpression(@"^.{2,}$|^$", ErrorMessage = "City must be at least 2 characters or left blank.")]
    [StringLength(40)]
    public string City { get; set; }

    [RegularExpression(@"^(VIC|NSW|QLD|WA|SA|TAS|ACT|NT)$", ErrorMessage = "State must be a valid Australian state (VIC, NSW, QLD, WA, SA, TAS, ACT, NT) or left blank.")]
    [StringLength(3)]
    public string State { get; set; }

    [RegularExpression(@"^(\d{4})?$", ErrorMessage = "Postcode must be exactly 4 digits or left blank.")]
    [StringLength(4)]
    public string Postcode { get; set; }

    [RegularExpression(@"^(?:61\d{9}|0\d{9}|\+\d{10,12})$",
    ErrorMessage = "Mobile must start with 0 or 61 followed by digits, or '+' with up to 12 digits or left blank.")]
    [StringLength(12)]
    public string Mobile { get; set; }

    public Login Login { get; set; }

    public virtual List<Account> Accounts { get; set; }
}