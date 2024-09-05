using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HomeWorks.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using HomeWorks.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddTransient<IMailService, MailService>();

builder.Services.AddDbContext<RS0605Context>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("RS0605Connection")));

// ���U IHttpContextAccessor �A��
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";
        options.AccessDeniedPath = "/Login/AccessDenied";
    });

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

// �K�[���v
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Role", "Admin"));
    options.AddPolicy("PremiumUser", policy => policy.RequireClaim("Subscription", "Premium"));
});

// Add services to the container.
builder.Services.AddControllersWithViews();

// �K�[���s���G���w�s�A��
builder.Services.AddDistributedMemoryCache();

// �K�[ Session �A��
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // �]�m Session �W��
    options.Cookie.HttpOnly = true; // ����Ȥ�ݸ}���X��
    options.Cookie.IsEssential = true; // �b GDPR �U�T�O Session �O���ݪ�
});

builder.Services.AddControllersWithViews();

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

app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// �t�m HTTP �ШD�޹D
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();
//app.UseAuthorization();


///
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
//        c.RoutePrefix = string.Empty; // �]�m Swagger UI �b���ήڥؿ�
//    });
//}

//app.UseHttpsRedirection();
//app.UseAuthorization();
//app.MapControllers();




//app.UseRouting();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Home}/{action=Index}");
//});

app.Run();
