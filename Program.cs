using Attendance.Data;
using Attendance.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("con")));

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<EmailService>();
builder.Services.AddHttpClient<WhatsAppService>();
builder.Services.AddHostedService<LectureSeederService>();




builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";   
        options.AccessDeniedPath = "/Auth/AccessDenied";  
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

builder.Services.AddAuthorization();


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
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Auth}/{action=Login}/{id?}");
});

app.Run();
