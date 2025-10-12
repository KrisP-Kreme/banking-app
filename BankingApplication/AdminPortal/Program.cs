using System.Net.Http.Headers;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure api client.
builder.Services.AddHttpClient("api", client =>
{
    client.BaseAddress = new Uri("http://localhost:5063");
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
});

builder.Services.AddControllersWithViews();
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

app.UseRouting();

// session for usage of login
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapDefaultControllerRoute().WithStaticAssets();

app.Run();
