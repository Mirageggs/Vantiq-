using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vantiq.Data;
using Vantiq.Models;
using Vantiq.ViewModels;

namespace Vantiq.Controllers
{
    /// <summary>Modulo de acceso: login por email + BCrypt, registro y salida.</summary>
    public class AccesoController : Controller
    {
        private readonly VantiqDbContext _db;
        public AccesoController(VantiqDbContext db) => _db = db;

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Catalogo");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm, string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid) return View(vm);

            var usuario = await _db.Usuarios
                .Include(u => u.Roles).ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Email == vm.Email && u.EstaActivo);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(vm.Contrasenia, usuario.ContraseniaHash))
            {
                ModelState.AddModelError(string.Empty, "Correo o contrasenia incorrectos.");
                return View(vm);
            }

            // Claims: identidad + roles (RBAC)
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new(ClaimTypes.Name, usuario.NombreUsuario),
                new(ClaimTypes.Email, usuario.Email)
            };
            foreach (var ur in usuario.Roles.Where(r => r.Rol.EstaActivo))
                claims.Add(new Claim(ClaimTypes.Role, ur.Rol.NombreRol));

            var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identidad));

            if (claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Administrador"))
                return RedirectToAction("Index", "Ventas");

            return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                ? Redirect(returnUrl)
                : RedirectToAction("Index", "Catalogo");
        }

        [HttpGet]
        public IActionResult Registro() =>
            User.Identity?.IsAuthenticated == true
                ? RedirectToAction("Index", "Catalogo")
                : View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (await _db.Usuarios.AnyAsync(u => u.Email == vm.Email))
            {
                ModelState.AddModelError(nameof(vm.Email), "Ese correo ya esta registrado.");
                return View(vm);
            }

            var usuario = new Usuario
            {
                NombreUsuario = vm.NombreUsuario.Trim(),
                Email = vm.Email.Trim().ToLower(),
                ContraseniaHash = BCrypt.Net.BCrypt.HashPassword(vm.Contrasenia)
                // FechaHoraRegistro / Modificacion: las asigna GETDATE() en BD
            };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            _db.UsuariosRoles.Add(new UsuarioRol { IdUsuario = usuario.IdUsuario, IdRol = 2 }); // Cliente
            await _db.SaveChangesAsync();

            TempData["ok"] = "Cuenta creada. Ya puedes iniciar sesion.";
            return RedirectToAction(nameof(Login));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Catalogo");
        }

        [HttpGet]
        public IActionResult Denegado() => View();
    }
}
