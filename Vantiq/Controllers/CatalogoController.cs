using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vantiq.Data;
using Vantiq.Services;

namespace Vantiq.Controllers
{
    /// <summary>Catalogo publico: portada, filtros por categoria, busqueda y detalle.</summary>
    public class CatalogoController : Controller
    {
        private readonly VantiqDbContext _db;
        private readonly ICarritoService _carrito;

        public CatalogoController(VantiqDbContext db, ICarritoService carrito)
        {
            _db = db;
            _carrito = carrito;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? q, short? idCategoria)
        {
            var consulta = _db.Relojes
                .Include(r => r.ModeloReloj).ThenInclude(m => m.Categoria)
                .Include(r => r.Marca)
                .Include(r => r.EstadoReloj)
                .Where(r => r.IdEstadoReloj != 3);   // oculta descontinuados

            if (idCategoria.HasValue)
                consulta = consulta.Where(r => r.ModeloReloj.IdCategoria == idCategoria.Value);

            if (!string.IsNullOrWhiteSpace(q))
            {
                var texto = q.Trim();
                consulta = consulta.Where(r =>
                    r.CodigoSKU.Contains(texto) ||
                    r.ModeloReloj.NombreModelo.Contains(texto) ||
                    r.Marca.NombreMarca.Contains(texto) ||
                    (r.Descripcion != null && r.Descripcion.Contains(texto)));
            }

            ViewBag.Q = q;
            ViewBag.IdCategoria = idCategoria;
            return View(await consulta.OrderBy(r => r.ModeloReloj.NombreModelo)
                                      .ThenBy(r => r.NumOrden).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var reloj = await _db.Relojes
                .Include(r => r.ModeloReloj).ThenInclude(m => m.Categoria)
                .Include(r => r.Marca)
                .Include(r => r.EstadoReloj)
                .FirstOrDefaultAsync(r => r.IdReloj == id && r.IdEstadoReloj != 3);

            if (reloj == null) return NotFound();

            ViewBag.EnCarrito = _carrito.CantidadDe(id);
            ViewBag.Celular = await _db.Negocios.Select(n => n.NumCelular).FirstOrDefaultAsync() ?? "51999999999";
            return View(reloj);
        }
    }
}
