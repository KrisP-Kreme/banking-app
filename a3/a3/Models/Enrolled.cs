using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a3.Models;

public class Enrolled
{


    [StringLength(8)]
    [Required(ErrorMessage = "CourseID is required.")]
    public string CourseID { get; set; }

    [StringLength(8)]
    [Required(ErrorMessage = "StudentID is required.")]
    public string StudentID { get; set; }

}