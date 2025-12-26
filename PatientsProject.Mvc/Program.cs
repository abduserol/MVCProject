using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PatientsProject.App.Domain;
using PatientsProject.App.Models;
using PatientsProject.App.Services;
using PatientsProject.Core.App.Services.Authentication.MVC;
using PatientsProject.Core.App.Services.MVC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DbContext, Db>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Db")));
builder.Services.AddScoped<PatientObsoleteService>();

// Add Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login";
        options.LogoutPath = "/Users/Logout";
        options.AccessDeniedPath = "/Users/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // 30 dakika timeout
        options.SlidingExpiration = true;
        // Cookie ayarları - tarayıcı kapanınca silinsin
        options.Cookie.IsEssential = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

// Add HttpContextAccessor for authentication services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICookieAuthService, CookieAuthService>();

// Add Session
builder.Services.AddSession(config =>
{
    config.IdleTimeout = TimeSpan.FromMinutes(30);
});

// Service registrations
builder.Services.AddScoped<IService<PatientRequest, PatientResponse>, PatientService>();
builder.Services.AddScoped<IService<DoctorRequest, DoctorResponse>, DoctorService>();
builder.Services.AddScoped<IService<BranchRequest, BranchResponse>, BranchService>();
builder.Services.AddScoped<IService<UserRequest, UserResponse>, UserService>();
builder.Services.AddScoped<IService<GroupRequest, GroupResponse>, GroupService>();
builder.Services.AddScoped<IService<RoleRequest, RoleResponse>, RoleService>();

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

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

