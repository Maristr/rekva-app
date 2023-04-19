using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Test02.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Test02ContextConnection") ?? throw new InvalidOperationException("Connection string 'Test02ContextConnection' not found.");

builder.Services.AddDbContext<Test02Context>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<Test02Context>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed the database with the Admin user
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

    // Create the Admin role if it doesn't exist
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // Create the Admin user if it doesn't exist
    var adminUser = await userManager.FindByEmailAsync("admin@test.ts");
    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = "Admin",
            Email = "admin@test.ts"
        };
        await userManager.CreateAsync(adminUser, "Ahoj123!");
    }

    // Add the Admin user to the Admin role if they're not already a member
    if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Use(async (context, next) =>
{
    var user = context.User;
    if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (context.Request.Path.StartsWithSegments("/Admin") && !user.IsInRole("Admin"))
        {
            context.Response.StatusCode = 401;
            return;
        }
    }

    await next();
});

app.Run();
