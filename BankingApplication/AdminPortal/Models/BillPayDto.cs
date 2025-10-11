using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPortal.Models;

public enum Period
{
    OneOff = 1,
    Monthly = 2,
}

public enum BillPayStatus
{
    Pending = 1,
    Paid = 2,
    Failed = 3,
    Blocked = 4
}

public class BillPayDto
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BillPayID { get; set; }

    [ForeignKey(nameof(AccountDto))]
    public int AccountNumber { get; set; }
    public virtual AccountDto Account { get; set; }

    [ForeignKey(nameof(PayeeDto))]
    public int PayeeID { get; set; }
    public virtual PayeeDto Payee { get; set; }

    [Required]
    [Column(TypeName = "money")]
    public double Amount { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
    public DateTime ScheduleTimeUtc { get; set; }

    [Required]
    public Period Period { get; set; }

    public BillPayStatus Status { get; set; } = BillPayStatus.Pending;
}