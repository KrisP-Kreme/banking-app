using Microsoft.EntityFrameworkCore;
using AdminWebApi.Data;
using AdminWebApi.Models.DataManager;
using AdminWebApi.Models.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MvcAdminContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MvcAdminContext")));

builder.Services.AddScoped<IBillPayRepository, BillPayManager>();
builder.Services.AddScoped<IPayeeRepository, PayeeManager>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("api/UsingMapGet", (string name, int? repeat) =>
{
    if (string.IsNullOrWhiteSpace(name))
        name = "(empty)";
    if (repeat is null or < 1)
        repeat = 1;

    return string.Join(' ', Enumerable.Repeat(name, repeat.Value));
});

app.Run();
