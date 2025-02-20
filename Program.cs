using QLDaoTao.Data;
using QLDaoTao.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security;
using QLDaoTao.Areas.Admin.Services;
using DinkToPdf.Contracts;
using DinkToPdf;
using QLDaoTao.Areas.Teacher.Models;
using Hangfire;
using QLDaoTao.Services;
using QLDaoTao.Areas.Teacher.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("default");
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddSession();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IStudent, ItemStudentService>();
builder.Services.AddScoped<IPhieuDangKyNghiDayDayBu, ItemPhieuDangKyNghiDayDayBuService>();
builder.Services.AddScoped<IPDF, ItemPDFService>();
builder.Services.AddScoped<ILopHocPhan, ItemLopHocPhanService>();
builder.Services.AddScoped<IKhoa, ItemKhoaService>();
builder.Services.AddScoped<IHangFire, HangFireService>();
builder.Services.AddScoped<INotificationTeacher, NotificationTeacherService>();

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(connectionString));
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromHours(2));
// Cấu hình đăng nhập cho User
builder.Services.AddIdentity<AppUser, IdentityRole>(
    options =>
    {
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;

    })
   .AddEntityFrameworkStores<AppDbContext>()
   .AddDefaultTokenProviders()
   .AddDefaultUI();

builder.Services.AddSingleton<IConverter, SynchronizedConverter>(provider =>
    new SynchronizedConverter(new PdfTools()));



// SignalR
builder.Services.AddSignalR();

// Cấu hình Quartz
//builder.Services.AddQuartz(q =>
//{
//    var jobKey = new JobKey("ProcessPhieuDangKyJob");
//    q.AddJob<ProcessPhieuDangKyJob>(opts => opts.WithIdentity(jobKey));

//    q.AddTrigger(opts => opts
//        .ForJob(jobKey)
//        .WithIdentity("ProcessPhieuDangKy-trigger")
//        .WithSimpleSchedule(schedule => schedule
//            .WithIntervalInSeconds(3) // Chạy mỗi 3 giây
//            .RepeatForever()));
//});
//builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

// Cấu hình đăng nhập cho Student
//builder.Services.AddIdentity<AppStudent, IdentityRole>(
//    options =>
//    {
//        options.Password.RequiredUniqueChars = 0;
//        options.Password.RequireUppercase = false;
//        options.Password.RequiredLength = 8;
//        options.Password.RequireNonAlphanumeric = false;
//        options.Password.RequireLowercase = false;
//    })
//   .AddEntityFrameworkStores<AppDbContext>()
//   .AddDefaultTokenProviders()
//   .AddDefaultUI();

// Cấu hình Hangfire sử dụng SQL Server
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(connectionString));

builder.Services.AddHangfireServer();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Đường dẫn đăng nhập cho User
    options.LoginPath = "/login";
    // Đường dẫn đăng nhập cho Student
    options.Cookie.Name = "StudentScheme";
    options.LoginPath = "/student/login";
    // Đường dẫn đăng nhập cho Admin
    options.Cookie.Name = "AdminScheme";
    options.LoginPath = "/Account/AdminLogin";
    //options.Cookie.Name = ".AspNetCore.Identity.Application";
    //options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    //options.SlidingExpiration = true;
    options.AccessDeniedPath = "/admin/denied";


});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

// SignalR
app.MapHub<NotificationHub>("/notificationHub");
// Thêm giao diện quản lý Hangfire Dashboard
app.UseHangfireDashboard("/hangfire");
//var options = new BackgroundJobServerOptions
//{
//    WorkerCount = 10 // Cho phép 10 công việc chạy song song
//};
//app.UseHangfireServer(options);


app.MapControllerRoute(
    name: "LoginRedirect",
    pattern: "/admin/login",
    defaults: new { controller = "Account", action = "AdminLogin" });

app.MapControllerRoute(
    name: "Login",
    pattern: "/login",
    defaults: new { controller = "Account", action = "Login" });

app.MapControllerRoute(
    name: "StudentLogin",
    pattern: "/student/login",
    defaults: new { controller = "Student", action = "Login" });

app.MapControllerRoute(
    name: "AdminLogin",
    pattern: "/admin/login",
    defaults: new { controller = "Account", action = "AdminLogin" });


app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
