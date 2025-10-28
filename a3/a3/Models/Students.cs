using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a3.Models;

public class Students
{
    [Key]
    [StringLength(8)]
    [Required(ErrorMessage = "StudentID is required.")]
    public int StudentID { get; set; }

    [StringLength(30)]
    [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "FirstName name must start with a capital letter and contain only letters and spaces.")]
    [Required(ErrorMessage = "FirstName is required.")]
    public string FirstName { get; set; }

    [StringLength(30)]
    [Required(ErrorMessage = "LastName is required.")]
    [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "LastName name must start with a capital letter and contain only letters and spaces.")]
    public string LastName { get; set; }

    [StringLength(320)]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string Email { get; set; }

    [StringLength(10)]
    public string MobilePhone { get; set; }

}