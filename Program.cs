using ContractMonthlyClaimSystem.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("LecturerOnly", policy => policy.RequireRole(UserRole.Lecturer.ToString()));
    options.AddPolicy("CoordinatorOnly", policy => policy.RequireRole(UserRole.ProgrammeCoordinator.ToString()));
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole(UserRole.AcademicManager.ToString()));
    options.AddPolicy("CoordinatorOrManager", policy => policy.RequireRole(UserRole.ProgrammeCoordinator.ToString(), UserRole.AcademicManager.ToString()));
});


builder.Services.AddControllersWithViews();

var app = builder.Build();

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
    pattern: "{controller=Account}/{action=Login}/{id?}"); 

app.Run();