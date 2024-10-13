using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Web.Data;
using SchoolSystem.Web.Data.Interfaces;
using SchoolSystem.Web.Data.Repository;
using SchoolSystem.Web.Helpers;
using SchoolSystem.Web.Helpers.Interfaces;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Services;
using SchoolSystem.Web.Services.Interfaces;
using Serilog;
using Syncfusion.Licensing;

var builder = WebApplication.CreateBuilder(args);

SyncfusionLicenseProvider.RegisterLicense(
  builder.Configuration["SyncfusionLicense"]);

Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Error()
  .WriteTo.File(
    path: "Logs/app-log-.txt",
    rollingInterval: RollingInterval.Day,
    retainedFileCountLimit: 7,
    outputTemplate:
    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
    retainedFileTimeLimit: TimeSpan.FromDays(7),
    shared: true,
    fileSizeLimitBytes: 10485760 // 10 MB
  ).CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.ConfigureApplicationCookie(
  op =>
  {
    op.LoginPath = "/Auth/Login";
    op.AccessDeniedPath = "/Error/AccessDenied";
  });
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddIdentity<User, IdentityRole>(
    config =>
    {
      config.Tokens.AuthenticatorTokenProvider
        = TokenOptions.DefaultAuthenticatorProvider;
      config.SignIn.RequireConfirmedEmail = true;
    })
  .AddDefaultTokenProviders()
  .AddEntityFrameworkStores<AppDbContext>();

var connectionString
  = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new ArgumentException("Connection string not found");

builder.Services.AddDbContext<AppDbContext>(options =>
  options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<ICreateMailHtmlHelper, CreateMailHtmlHelper>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IGradeRepository, GradeRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error/Exception");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/Error", "?code={0}");

// Seed database
using var scoped = app.Services.CreateScope();
var services = scoped.ServiceProvider;
var context = services.GetRequiredService<AppDbContext>();
var useHelper = services.GetRequiredService<IUserHelper>();
await Seed.SeedAsync(context, useHelper);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
