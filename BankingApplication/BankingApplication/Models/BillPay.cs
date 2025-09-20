using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace BankingApplication.Models;

public enum Period
{
    OneOff = 1,
    Monthly = 2,
}

public class BillPay
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int BillPayID { get; set; }

    [ForeignKey(nameof(Account))]
    public int AccountNumber { get; set; }
    public virtual Account Account { get; set; }

    [ForeignKey(nameof(Payee))]
    public int PayeeID { get; set; }
    public virtual Payee Payee { get; set; }

    [Required]
    [Column(TypeName = "money")]
    public double Amount { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
    public DateTime ScheduleTimeUtc { get; set; }
        
    [Required]
    public Period Period { get; set; } // how often scheduled payment will occur
}