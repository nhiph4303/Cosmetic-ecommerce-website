using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Cosmetic.Data;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;


var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<CosmeticContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("CosmeticContext") ?? throw new InvalidOperationException("Connection string 'CosmeticContext' not found.")));
var connectionString = builder.Configuration.GetConnectionString("CosmeticContext");
builder.Services.AddDbContext<CosmeticContext>(option =>
    option.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);
// Add services to the container.
builder.Services.AddControllersWithViews();

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
