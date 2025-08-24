using IvanSusaninProject_BusinessLogic.Implementations;
using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.Enums;
using IvanSusaninProject_Contracts.Infrastructure;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_DataBase.Implementations;
using IvanSusaninProject_DataBase.Implementationsl;
using Microsoft.AspNetCore.Authentication.Cookies;
using MVC_Project.Adapters;
using MVC_Project.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "AuthCookie";
        options.LoginPath = "/Account/Login"; 
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

using var loggerFactory = new LoggerFactory();
loggerFactory.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger());
builder.Services.AddSingleton(loggerFactory.CreateLogger("Any"));

builder.Services.AddAuthentication("AuthAppCookie")
    .AddCookie("AuthAppCookie", options =>
    {
        options.Cookie.Name = "AuthAppCookie";
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireExecutor", policy =>
        policy.RequireRole(UserRole.Executor.ToString()));
    options.AddPolicy("RequireGuarantor", policy =>
        policy.RequireRole(UserRole.Guarantor.ToString()));
});

builder.Services.AddSingleton<IConfigurationDatabase, ConfigurationDatabase>();
builder.Services.AddTransient<IUserStorageContract, UserStorageContract>();
builder.Services.AddTransient<IUserBusinessLogicContract, UserBusinessLogicContract>();
builder.Services.AddTransient<IUserAdapter, UserAdapter>();

builder.Services.AddSingleton<IExcursionAdapter, ExcursionAdapter>();
builder.Services.AddSingleton<IGroupAdapter, GroupAdapter>();
builder.Services.AddSingleton<IGuideAdapter, GuideAdapter>();
builder.Services.AddSingleton<IPlaceAdapter, PlaceAdapter>();
builder.Services.AddSingleton<ITourAdapter, TourAdapter>();
builder.Services.AddSingleton<ITripAdapter, TripAdapter>();

builder.Services.AddSingleton<ITripPlaceBusinessLogicContract, TripPlaceBusinessLogicContract>();
builder.Services.AddSingleton<ITripGuideBusinessLogicContract, TripGuideBusinessLogicContract>();
builder.Services.AddSingleton<ITourGroupBusinessLogicContract, TourGroupBusinessLogicContract>();
builder.Services.AddSingleton<ITourExcursionBusinessLogicContract, TourExcursionBusinessLogicContract>();
builder.Services.AddSingleton<IExcursionBusinessLogicsContract, ExcursionBusinessLogicsContracts>();
builder.Services.AddSingleton<IGroupBusinessLogicsContract, GroupBusinessLogicsContract>();
builder.Services.AddSingleton<IGuideBusinessLogicsContract, GuideBusinessLogicsContract>();
builder.Services.AddSingleton<IPlaceBusinessLogicContract, PlaceBusinessLogicContract>();
builder.Services.AddSingleton<ITourBusinessLogicsContract, TourBusinessLogicsContract>();
builder.Services.AddSingleton<ITripBusinessLogicContract, TripBusinessLogicContract>();

builder.Services.AddSingleton<ITourGroupStorageContract, TourGroupStorageContract>();
builder.Services.AddSingleton<ITourExcursionStorageContract, TourExcursionStorageContract>();
builder.Services.AddSingleton<ITripGuideStorageContract, TripGuideStorageContract>();
builder.Services.AddSingleton<ITripPlaceStorageContract, TripPlaceStorageContract>();
builder.Services.AddSingleton<IExcursionStorageContract, ExcursionStorageContract>();
builder.Services.AddSingleton<IGroupStorageContract, GroupStorageContract>();
builder.Services.AddSingleton<IGuideStrorageContract, GuideStrorageContract>();
builder.Services.AddSingleton<IPlaceStorageContract, PlaceStorageContract>();
builder.Services.AddSingleton<ITourStorageContract, TourStorageContract>();
builder.Services.AddSingleton<ITripStorageContract, TripStorageContract>();

builder.Services.AddTransient<IvanSusaninProject_DbContext>();




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

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// :)
app.Run();
