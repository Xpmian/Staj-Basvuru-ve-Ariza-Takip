using Aibu.InternshipAutomation;
using Aibu.InternshipAutomation.Configure;
using Aibu.InternshipAutomation.Data.Base;
using Aibu.InternshipAutomation.Data.Context;
using Aibu.InternshipAutomation.Data.Dal;
using Aibu.InternshipAutomation.Helper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OfficeOpenXml;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews(options => options.Filters.Add(typeof(RequestAuthenticationFilter)));
builder.Services.AddSession();
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
});
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = "Deneme";
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
    options.SlidingExpiration = true;

});

builder.Services.AddScoped<IStudentDal, StudentDal>();
builder.Services.AddScoped<ICompanyDal, CompanyDal>();
builder.Services.AddScoped<ILoginDal, LoginDal>();
builder.Services.AddScoped<IAuthorizedDal, AuthorizedDal>();
builder.Services.AddScoped<IAdminDal, AdminDal>();
builder.Services.AddScoped<IDepartmentDal, DepartmentDal>();
builder.Services.AddScoped<IPdfHelper, PdfHelper>();
builder.Services.AddScoped<ILogDal, LogDal>();
builder.Services.AddScoped<ILdapService, LdapService>();
QuestPDF.Settings.License = LicenseType.Community;
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

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

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
