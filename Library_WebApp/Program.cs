using Library_WebApp.Helpers;
using Library_WebApp.Services.Books;
using Library_WebApp.Services.ConsumeAPI;
using Library_WebApp.Services.User;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add custom services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IConsumeAPI, ConsumeAPI>();
builder.Services.AddScoped<IBookService, BookService>();

//IHttpContextAccessor register
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Service for form validation
builder.Services.AddAntiforgery(options => options.HeaderName = "XSRF-TOKEN");

// Session management
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(config =>
{
    config.Cookie.Name = "LMA_Cookie";
    config.AccessDeniedPath = "/Home/AccessDenied";
    config.LoginPath = "/User/LoginPage";
});
// Authorization
builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy("AdminRole", policy => policy.RequireRole("ADMIN"));
    options.AddPolicy("AdminRole", policy => policy.RequireRole("admin"));
    options.AddPolicy("ManagerRole", policy => policy.RequireRole("ADMIN", "TEACHER"));
    options.AddPolicy("UserRole", policy => policy.RequireRole("ADMIN", "STUDENT"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Initializing AppSettingsHelper with IConfiguration 
AppSettingsHelper.Initialize(app.Services.GetRequiredService<IConfiguration>());

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();

// Session management
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=LoginPage}/{id?}");

app.Run();
