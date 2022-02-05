

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TrunkeyWebApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<movieContext>(options => options.UseMySQL(Environment.GetEnvironmentVariable("CONNECTION_STRING")));
builder.Services.AddControllers().AddJsonOptions(x =>
                { x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; x.JsonSerializerOptions.WriteIndented = true; });
builder.Services.AddTransient<TrunkeyWebApp.Middlewares.ICookiesAction, TrunkeyWebApp.Middlewares.CookiesAction>();
builder.Services.AddTransient<TrunkeyWebApp.Middlewares.IAuthorization, TrunkeyWebApp.Middlewares.Authorization>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
