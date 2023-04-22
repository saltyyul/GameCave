using GameCave.Controllers;
using GameCave.Data;
using GameCave.Repositories;
using GameCave.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 5;
})
    .AddRoles<IdentityRole>() // added
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IGenreService, GenreService>();

var app = builder.Build();

// Ensure that database is created upon starting the app
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Games}/{action=Index}/{id?}");
app.MapRazorPages();


using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // Seed the database with the 'admin' role and user
    var adminRoleExists = roleManager.RoleExistsAsync("Admin").Result;
    if (!adminRoleExists)
    {
        // Create the 'admin' role
        var role = new IdentityRole("Admin");
        var createRoleResult = roleManager.CreateAsync(role).Result;
        if (!createRoleResult.Succeeded)
        {
            // Handle error creating 'admin' role
        }
    }

    var adminUserExists = userManager.FindByEmailAsync("admin@admin.com").Result;

    if (adminUserExists == null)
    {
        // Create the 'admin' user
        var user = new IdentityUser
        {
            UserName = "admin@admin.com",
            Email = "admin@admin.com",
            EmailConfirmed = true
        };
        var createAdminResult = userManager.CreateAsync(user, "Admin_123").Result;

        if (createAdminResult.Succeeded)
        {
            // Add the 'admin' role to the 'admin' user
            var addToRoleResult = userManager.AddToRoleAsync(user, "Admin").Result;
        }
    }
}

await app.RunAsync();