using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using praktika.Data;
using praktika.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!context.Categories.Any())
    {
        context.Categories.AddRange(
            new Category { Name = "Кирпич и блоки", Description = "Кирпич, газоблок, пеноблок" },
            new Category { Name = "Цемент и смеси", Description = "Цемент, песок, бетон" },
            new Category { Name = "Отделочные материалы", Description = "Шпаклёвка, штукатурка, краска" },
            new Category { Name = "Инструменты", Description = "Ручной и электроинструмент" },
            new Category { Name = "Сантехника", Description = "Трубы, унитазы, ванны" },
            new Category { Name = "Кровля и фасад", Description = "Профнастил, черепица, сайдинг" },
            new Category { Name = "Пиломатериалы", Description = "Доски, брус, фанера" }
        );
        context.SaveChanges();
    }

    if (!context.Products.Any())
    {
        var cats = context.Categories.ToList();
        context.Products.AddRange(
            new Product { Name = "Кирпич облицовочный М150", Price = 4.50m, OldPrice = 5.00m, Article = "KB-M150", InStock = true, IsHit = true, IdCategory = cats[0].IdCategory },
            new Product { Name = "Газоблок Hebel D500", Price = 85.00m, Article = "GB-D500", InStock = true, IdCategory = cats[0].IdCategory },
            new Product { Name = "Цемент М500 50кг Кронос", Price = 18.90m, OldPrice = 22.20m, Article = "C-M500-50", InStock = true, IsHit = true, IdCategory = cats[1].IdCategory },
            new Product { Name = "Пескобетон М300 40кг", Price = 9.50m, Article = "PB-M300", InStock = true, IdCategory = cats[1].IdCategory },
            new Product { Name = "Шпаклёвка финишная Knauf 20кг", Price = 12.30m, Article = "KN-FIN-20", InStock = true, IdCategory = cats[2].IdCategory },
            new Product { Name = "Дрель-шуруповёрт Bosch GSR", Price = 89.00m, Article = "BS-GSR18", InStock = true, IsNew = true, IdCategory = cats[3].IdCategory },
            new Product { Name = "Унитаз подвесной Cersanit", Price = 215.00m, Article = "CS-UNI-01", InStock = true, IdCategory = cats[4].IdCategory },
            new Product { Name = "Профнастил С-21 оцинкованный", Price = 320.00m, OldPrice = 457.00m, Article = "PN-C21-Z", InStock = true, IsHit = true, IdCategory = cats[5].IdCategory }
        );
        context.SaveChanges();
    }
}

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