using System.Globalization;

using DotNetEnv;

using Microsoft.AspNetCore.Localization;

using QuestPDF.Infrastructure;

using Serilog;

using SICAF.Business;
using SICAF.Common;
using SICAF.Data;
using SICAF.Data.Initialization.Extensions;
using SICAF.Web;
using SICAF.Web.Middlewares;

Env.Load(); // Load .env file
QuestPDF.Settings.License = LicenseType.Community; // Set QuestPDF license

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Services.AddSerilog((services, lc) => lc
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

// Configure culture info for the application
var cultureInfo = new CultureInfo("es-CO");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Configure localization options
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultureInfo),
    SupportedCultures = [cultureInfo],
    SupportedUICultures = [cultureInfo]
};

// Add services to the container.
builder.Services.RegisterWebServices();
builder.Services.RegisterBusinessServices();
builder.Services.RegisterDataServices(builder.Configuration);
builder.Services.RegisterCommonServices(builder.Configuration);

var app = builder.Build();

// Ensure the database is initialized at startup
await app.Services.InitializeDatabaseAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseRequestLocalization(localizationOptions);
app.UseSerilogRequestLogging(); // Enable Serilog request HTTP logging
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

if (!app.Environment.IsDevelopment())
{
    // Configure the middleware to handle global exceptions, Global middleware after UseRouting and UseAuthorization
    app.UseMiddleware<GlobalExceptionMiddleware>();
}

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
