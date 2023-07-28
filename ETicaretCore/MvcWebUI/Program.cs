using AppCore.DataAccess.Configs;
using AppCore.MvcWebUI;
using AppCore.MvcWebUI.Bases;
using Business.Services;
using Business.Services.Bases;
using DataAccess.EntityFramework.Contexts;
using DataAccess.EntityFramework.Repositories;
using DataAccess.EntityFramework.Repositories.Bases;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MvcWebUI.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(config =>
    {
        config.LoginPath = "/Hesaplar/Giris";
        config.AccessDeniedPath = "/Hesaplar/YetkisizIslem";
        config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        config.SlidingExpiration = true;
    });

builder.Services.AddSession(config =>
{
    config.IdleTimeout = TimeSpan.FromMinutes(30); // default: 20 dakika
});

// appsettings.json dosyasýndaki connection string'i okuma:
string connectionString = builder.Configuration.GetConnectionString("ETicaretContext");
ConnectionConfig.ConnectionString = connectionString;

#region IoC (Inversion of Control) Container 
// DbContext tanýmlama:
//builder.Services.AddDbContext<ETicaretContext>();
builder.Services.AddScoped<DbContext, ETicaretContext>();

builder.Services.AddScoped<UrunRepositoryBase, UrunRepository>();
builder.Services.AddScoped<KategoriRepositoryBase, KategoriRepository>();
builder.Services.AddScoped<KullaniciRepositoryBase, KullaniciRepository>();

builder.Services.AddScoped<IUrunService, UrunService>();
builder.Services.AddScoped<IKategoriService, KategoriService>();
builder.Services.AddScoped<IKullaniciService, KullaniciService>();
builder.Services.AddScoped<IHesapService, HesapService>();
#endregion

// ASP.NET Core:
//IConfigurationSection section = builder.Configuration.GetSection("AppSettings");
//AppSettings appSettings = new AppSettings();
//section.Bind(appSettings);

// AppCore
AppSettingsUtilBase appSettingsUtil = new AppSettingsUtil(builder.Configuration);
appSettingsUtil.Bind<AppSettings>();


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

app.UseAuthentication(); // sen kimsin?

app.UseAuthorization(); // sen iþlem için yetkili misin?

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
