using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vantiq.Data;

namespace Vantiq.Controllers
{
    /// <summary>Menu USUARIOS del Administrador: cuentas, roles y estado.</summary>
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly VantiqDbContext _db;
        public UsuariosController(VantiqDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Index() =>
            View(await _db.Usuarios
                .Include(u => u.Roles).ThenInclude(ur => ur.Rol)
                .OrderBy(u => u.IdUsuario)
                .ToListAsync());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var idActual = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (id == idActual)
            {
                TempData["error"] = "No puedes desactivar tu propia cuenta.";
                return RedirectToAction(nameof(Index));
            }

            var usuario = await _db.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            usuario.EstaActivo = !usuario.EstaActivo;
            usuario.FechaHoraModificacion = DateTime.Now;
            await _db.SaveChangesAsync();

            TempData["ok"] = $"Usuario {usuario.NombreUsuario} ahora esta {(usuario.EstaActivo ? "activo" : "inactivo")}.";
            return RedirectToAction(nameof(Index));
        }
    }
}
