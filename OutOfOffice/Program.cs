using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Application;
using OutOfOffice.Application.IServices;
using OutOfOffice.Application.Services;
using OutOfOffice.Core.Entities;
using OutOfOffice.Data;
using static OutOfOffice.Core.Enums;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddDbContext<OutOfOfficeDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
builder.Services.AddScoped<IApprovalRequestService, ApprovalRequestService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddAuthorization();

builder.Services.AddIdentity<Employee, IdentityRole<int>>()
    .AddEntityFrameworkStores<OutOfOfficeDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var roles = new[] { "Employee", "HR Manager", "Project Manager", "Administrator" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<int>(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Employee>>();

    string email = "admin@gmail.com";
    string password = "Test1234!";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new Employee
        {
            UserName = email,
            Email = email,
            FullName = "Admin User",
            Subdivision = Subdivision.None,
            Position = Position.Administrator,
            Status = ActivityStatus.Active,
            PeoplePartnerId = 1,
            OutOfOfficeBalance = 30
        };

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Administrator");
    }
}

app.Run();
