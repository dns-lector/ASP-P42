using ASP_P42.Data;
using ASP_P42.Middleware.AuthSession;
using ASP_P42.Services.Hash;
using ASP_P42.Services.Kdf;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHash();
builder.Services.AddKdf();
// БД додається як сервіс, але специфічним методом-розширенням
builder.Services.AddDbContext<DataContext>(options => 
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("LocalDB")
    )
);

// Налаштування сесій
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Налаштування CORS
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => 
        policy
        .AllowAnyOrigin()   // відкритий АРІ - для всіх споживачів
        .AllowAnyHeader()   // дозволяємо усі заголовки
        .AllowAnyMethod()   // та усі методи запиту
                            // .WithMethods("GET", "POST") - якщо обмежуємо
    )
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.MapStaticAssets();

app.UseSession();   // включаємо сесії

// додаємо custom middleware
app.UseAuthSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
