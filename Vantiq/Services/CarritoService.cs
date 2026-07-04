using System.Text.Json;
using Vantiq.Models;
using Vantiq.ViewModels;

namespace Vantiq.Services
{
    public interface ICarritoService
    {
        List<ItemCarrito> Obtener();
        short CantidadDe(int idReloj);
        void Agregar(Reloj reloj, short cantidad);
        void ActualizarCantidad(int idReloj, short cantidad);
        void Quitar(int idReloj);
        void Vaciar();
        decimal MontoTotal();
        int TotalUnidades();
    }

    /// <summary>
    /// Carrito de compras en Session (JSON). Aplica por igual a Invitado y
    /// Cliente; la ventaja del Cliente esta en el historial y el checkout
    /// precargado con sus datos.
    /// </summary>
    public class CarritoService : ICarritoService
    {
        private const string ClaveCarrito = "CARRITO";
        private const string ClaveContador = "CARRITO_COUNT";
        private readonly ISession _session;

        public CarritoService(IHttpContextAccessor accessor)
        {
            _session = accessor.HttpContext!.Session;
        }

        public List<ItemCarrito> Obtener()
        {
            var json = _session.GetString(ClaveCarrito);
            return string.IsNullOrEmpty(json)
                ? new List<ItemCarrito>()
                : JsonSerializer.Deserialize<List<ItemCarrito>>(json) ?? new List<ItemCarrito>();
        }

        public short CantidadDe(int idReloj) =>
            Obtener().FirstOrDefault(i => i.IdReloj == idReloj)?.Cantidad ?? 0;

        public void Agregar(Reloj reloj, short cantidad)
        {
            var items = Obtener();
            var existente = items.FirstOrDefault(i => i.IdReloj == reloj.IdReloj);
            if (existente != null)
            {
                existente.Cantidad += cantidad;
            }
            else
            {
                items.Add(new ItemCarrito
                {
                    IdReloj = (int)reloj.IdReloj,
                    CodigoSKU = reloj.CodigoSKU,
                    Nombre = reloj.ModeloReloj.NombreModelo + " " + reloj.Marca.NombreMarca,
                    UrlImagen = reloj.UrlImagen,
                    Precio = reloj.Precio,
                    Cantidad = cantidad
                });
            }
            Guardar(items);
        }

        public void ActualizarCantidad(int idReloj, short cantidad)
        {
            var items = Obtener();
            var item = items.FirstOrDefault(i => i.IdReloj == idReloj);
            if (item == null) return;
            if (cantidad <= 0) items.Remove(item);
            else item.Cantidad = cantidad;
            Guardar(items);
        }

        public void Quitar(int idReloj)
        {
            var items = Obtener();
            items.RemoveAll(i => i.IdReloj == idReloj);
            Guardar(items);
        }

        public void Vaciar() => Guardar(new List<ItemCarrito>());

        public decimal MontoTotal() => Obtener().Sum(i => i.Subtotal);

        public int TotalUnidades() => Obtener().Sum(i => i.Cantidad);

        private void Guardar(List<ItemCarrito> items)
        {
            _session.SetString(ClaveCarrito, JsonSerializer.Serialize(items));
            _session.SetInt32(ClaveContador, items.Sum(i => i.Cantidad));
        }
    }
}
