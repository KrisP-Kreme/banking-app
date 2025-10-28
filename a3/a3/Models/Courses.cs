using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a3.Models;

public enum CourseType
{
    Undergraduate,
    Postgraduate,
}

public class Courses
{
    [Key]
    [StringLength(8)]
    [RegularExpression(@"^COSC\d{4}$", ErrorMessage = "CourseID must start with 'COSC' followed by 4 digits.")]
    [Required(ErrorMessage = "CourseID is required.")]
    public int CourseID { get; set; }

    [StringLength(100)]
    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; }

    [Range(1, 12, ErrorMessage = "Credit points must be between 1 and 12.")]
    public int CreditPoints { get; set; }

    [Required(ErrorMessage = "Career is required.")]
    [RegularExpression(@"^(Undergraduate|Postgraduate)$", ErrorMessage = "Career must be either 'Undergraduate' or 'Postgraduate'.")]
    [StringLength(30)]
    public string Career { get; set; }

    [StringLength(50)]
    [RegularExpression(@"^[A-Z][a-zA-Z ]*$", ErrorMessage = "Coordinator name must start with a capital letter and contain only letters and spaces.")]
    [Required(ErrorMessage = "Coordinator is required.")]
    public string Coordinator { get; set; }

}