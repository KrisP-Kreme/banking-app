using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication.BankgroundServices;
public class BillPayBackgroundService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<BillPayBackgroundService> _logger;

    public BillPayBackgroundService(IServiceProvider services, ILogger<BillPayBackgroundService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("BillPay Background Service is running.");

        while (!cancellationToken.IsCancellationRequested)
        {
            await ProcessPaymentsAsync(cancellationToken);

            _logger.LogInformation("BillPay Background Service is waiting a minute.");

            await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
        }
    }

    private async Task ProcessPaymentsAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("BillPay Background Service is working.");

        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BankingApplicationContext>();

        var now = DateTime.UtcNow;
        var duePayments = context.BillPays
            .Where(x => x.Status == BillPayStatus.Pending && x.ScheduleTimeUtc <= now)
            .Include(x => x.Account)
            .ToList();

        Console.WriteLine($"Processed {duePayments.Count} due bill payments at {DateTime.UtcNow}");


        foreach (var bill in duePayments)
        {
            try
            {
                var account = bill.Account;
                if (account.Balance >= (decimal)bill.Amount)
                {
                    account.Balance -= (decimal)bill.Amount;
                    bill.Status = BillPayStatus.Paid;

                    //if monthly, schedule next month
                    if (bill.Period == Period.Monthly)
                    {
                        bill.ScheduleTimeUtc = bill.ScheduleTimeUtc.AddMonths(1);
                        bill.Status = BillPayStatus.Pending;
                    }
                }
                else
                {
                    bill.Status = BillPayStatus.Failed;
                }
            }
            catch
            {
                bill.Status = BillPayStatus.Failed;
            }
        }

        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("People Background Service work complete.");
    }
}
