using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OnlineJewelleryShop.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews();
// Set up the database connection string
var connectionString = "Server=localhost\\SQLEXPRESS;Database=Jjewellery;Integrated Security=SSPI;trustServerCertificate=yes; user id=jdneu; Trusted_Connection=True; MultipleActiveResultSets=true";
builder.Services.AddDbContext<JjewelleryContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));
// Set up cookie-based authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Set the login, logout, and access denied paths
        options.LoginPath = "/Login/Login";
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Login/AccessDenied";
        // Set the expiration time for the authentication cookie
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        // Allow the authentication cookie to be renewed on each request
        options.SlidingExpiration = true;
    });

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=NewArrival}/{action=NewArrival}/{id?}");

app.Run();