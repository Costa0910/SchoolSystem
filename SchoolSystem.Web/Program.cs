using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Web.Data;
using SchoolSystem.Web.Models;
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
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.ConfigureApplicationCookie(
    op =>
    {
        op.LoginPath = "/Account/Login";
        op.AccessDeniedPath = "/Errors/AccessDenied";
    });

// TODO: Change configuration to be more secure after development
builder.Services.AddIdentity<User, IdentityRole>(
        config =>
        {
            config.Tokens.AuthenticatorTokenProvider
                = TokenOptions.DefaultAuthenticatorProvider;
            //config.SignIn.RequireConfirmedEmail = true;
            config.User.RequireUniqueEmail = true;
            config.Password.RequireDigit = false;
            config.Password.RequiredUniqueChars = 0;
            config.Password.RequireUppercase = false;
            config.Password.RequireLowercase = false;
            config.Password.RequireNonAlphanumeric = false;
        })
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<AppDbContext>();

var connectionString
    = builder.Configuration.GetConnectionString("DefaultConnection") ??
      throw new ArgumentException("Connection string not found");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
