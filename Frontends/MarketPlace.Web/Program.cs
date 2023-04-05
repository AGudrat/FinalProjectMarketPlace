using FluentValidation.AspNetCore;
using MarketPlace.Shared.Services;
using MarketPlace.Web.Extensions;
using MarketPlace.Web.Handler;
using MarketPlace.Web.Helpers;
using MarketPlace.Web.Models;
using MarketPlace.Web.Services;
using MarketPlace.Web.Services.Interfaces;
using MarketPlace.Web.Validators;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
builder.Services.Configure<ClientSettings>(builder.Configuration.GetSection("ClientSettings"));
builder.Services.Configure<ServiceAPISettings>(builder.Configuration.GetSection("ServiceAPISettings"));


builder.Services.AddAccessTokenManagement();

builder.Services.AddSingleton<PhotoHelper>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ResourceOwnerPasswordTokenHandler>();
builder.Services.AddScoped<ClientCredentialTokenHandler>();

builder.Services.AddHttpClientServices(builder.Configuration);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
{
    opt.LoginPath = "/Auth/SignIn";
    opt.ExpireTimeSpan = TimeSpan.FromDays(61);
    opt.SlidingExpiration = true;
    opt.Cookie.Name = "X-ACCESS-TOKEN";
});
builder.Services.AddControllersWithViews()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ProductCreateInputValidator>());
builder.Services.AddElasticSearch(builder.Configuration);
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
