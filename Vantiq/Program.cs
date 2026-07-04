using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Vantiq.Data;
using Vantiq.Services; // Asegúrate de que este using esté presente para ICarritoService y CarritoService

var builder = WebApplication.CreateBuilder(args);

// MVC + acceso a datos
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<VantiqDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VantiqConnection")));

// ==========================================
// REGISTRO DE TUS SERVICIOS PERSONALIZADOS
// ==========================================
builder.Services.AddScoped<ICarritoService, CarritoService>();

// Carrito en Session (Invitado y Cliente)
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Autenticacion por cookies + RBAC con [Authorize(Roles = "...")]
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Acceso/Login";
        options.AccessDeniedPath = "/Acceso/Denegado";
        options.ExpireTimeSpan = TimeSpan.FromHours(4);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Catalogo/Index");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// MIDDLEWARES DE SESIÓN Y SEGURIDAD
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// El catalogo es la portada del e-commerce
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalogo}/{action=Index}/{id?}");

app.Run();