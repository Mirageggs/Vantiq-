using Microsoft.EntityFrameworkCore;
using Vantiq.Models;

namespace Vantiq.Data
{
    /// <summary>
    /// Contexto de datos de VANTIQ (EF Core, enfoque Code-First).
    /// Contiene la configuracion Fluent API del modelo fisico y los datos
    /// semilla de los catalogos. Listo para: Add-Migration InicialVantiq.
    /// </summary>
    public class VantiqDbContext : DbContext
    {
        public VantiqDbContext(DbContextOptions<VantiqDbContext> options)
            : base(options)
        {
        }

        // ===== DbSets: una propiedad por tabla del modelo ER =====
        public DbSet<Negocio> Negocios { get; set; } = null!;
        public DbSet<OpcionMenu> OpcionesMenu { get; set; } = null!;
        public DbSet<Rol> Roles { get; set; } = null!;
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<RolOpcionMenu> RolesOpcionMenu { get; set; } = null!;
        public DbSet<UsuarioRol> UsuariosRoles { get; set; } = null!;
        public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<Marca> Marcas { get; set; } = null!;
        public DbSet<ModeloReloj> ModelosReloj { get; set; } = null!;
        public DbSet<EstadoReloj> EstadosReloj { get; set; } = null!;
        public DbSet<Reloj> Relojes { get; set; } = null!;
        public DbSet<Carrito> Carritos { get; set; } = null!;
        public DbSet<DetalleCarrito> DetallesCarrito { get; set; } = null!;
        public DbSet<MetodoPago> MetodosPago { get; set; } = null!;
        public DbSet<EstadoPedido> EstadosPedido { get; set; } = null!;
        public DbSet<Pedido> Pedidos { get; set; } = null!;
        public DbSet<DetallePedido> DetallesPedido { get; set; } = null!;
        public DbSet<TipoComprobante> TiposComprobante { get; set; } = null!;
        public DbSet<Comprobante> Comprobantes { get; set; } = null!;
        public DbSet<ConceptoKardex> ConceptosKardex { get; set; } = null!;
        public DbSet<Kardex> MovimientosKardex { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =================================================================
            // 1. CATALOGOS SEMBRADOS: claves sin IDENTITY (valores fijos)
            //    Los catalogos byte/short se siembran con IDs conocidos por el
            //    codigo (ej. 1 = Pendiente), por eso no se autogeneran.
            // =================================================================
            modelBuilder.Entity<Negocio>().Property(x => x.IdNegocio).ValueGeneratedNever();
            modelBuilder.Entity<Rol>().Property(x => x.IdRol).ValueGeneratedNever();
            modelBuilder.Entity<OpcionMenu>().Property(x => x.IdOpcionMenu).ValueGeneratedNever();
            modelBuilder.Entity<EstadoReloj>().Property(x => x.IdEstadoReloj).ValueGeneratedNever();
            modelBuilder.Entity<MetodoPago>().Property(x => x.IdMetodoPago).ValueGeneratedNever();
            modelBuilder.Entity<EstadoPedido>().Property(x => x.IdEstadoPedido).ValueGeneratedNever();
            modelBuilder.Entity<TipoComprobante>().Property(x => x.IdTipoComprobante).ValueGeneratedNever();
            modelBuilder.Entity<ConceptoKardex>().Property(x => x.IdConcepto).ValueGeneratedNever();

            // =================================================================
            // 2. INDICES UNICOS (los UK del diagrama ER)
            // =================================================================
            modelBuilder.Entity<Rol>().HasIndex(x => x.NombreRol).IsUnique();
            modelBuilder.Entity<OpcionMenu>().HasIndex(x => x.NombreOpcionMenu).IsUnique();
            modelBuilder.Entity<Usuario>().HasIndex(x => x.NombreUsuario).IsUnique();
            modelBuilder.Entity<Usuario>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<Categoria>().HasIndex(x => x.NombreCategoria).IsUnique();
            modelBuilder.Entity<Marca>().HasIndex(x => x.NombreMarca).IsUnique();
            modelBuilder.Entity<ModeloReloj>().HasIndex(x => x.NombreModelo).IsUnique();
            modelBuilder.Entity<EstadoReloj>().HasIndex(x => x.NombreEstadoReloj).IsUnique();
            modelBuilder.Entity<Reloj>().HasIndex(x => x.CodigoSKU).IsUnique();
            modelBuilder.Entity<MetodoPago>().HasIndex(x => x.NombreMetodoPago).IsUnique();
            modelBuilder.Entity<EstadoPedido>().HasIndex(x => x.NombreEstadoPedido).IsUnique();
            modelBuilder.Entity<Pedido>().HasIndex(x => x.CodigoPedido).IsUnique();
            modelBuilder.Entity<TipoComprobante>().HasIndex(x => x.NombreTipo).IsUnique();
            modelBuilder.Entity<ConceptoKardex>().HasIndex(x => x.NombreConcepto).IsUnique();

            // Unicos compuestos: evitan duplicar la misma asignacion o linea
            modelBuilder.Entity<UsuarioRol>().HasIndex(x => new { x.IdUsuario, x.IdRol }).IsUnique();
            modelBuilder.Entity<RolOpcionMenu>().HasIndex(x => new { x.IdRol, x.IdOpcionMenu }).IsUnique();
            modelBuilder.Entity<DetalleCarrito>().HasIndex(x => new { x.IdCarrito, x.IdReloj }).IsUnique();

            // Unico filtrado: muchos CLIENTE invitados con IdUsuario NULL,
            // pero como maximo un CLIENTE por cada USUARIO registrado.
            modelBuilder.Entity<Cliente>()
                .HasIndex(x => x.IdUsuario)
                .IsUnique()
                .HasFilter("[IdUsuario] IS NOT NULL");

            // Un comprobante por pedido (lado unico de la relacion 1:0..1)
            modelBuilder.Entity<Comprobante>().HasIndex(x => x.IdPedido).IsUnique();

            // =================================================================
            // 3. RELACIONES 1 : 0..1 (Fluent API define el lado dependiente)
            // =================================================================
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Usuario)
                .WithOne(u => u.Cliente)
                .HasForeignKey<Cliente>(c => c.IdUsuario);

            modelBuilder.Entity<Comprobante>()
                .HasOne(c => c.Pedido)
                .WithOne(p => p.Comprobante)
                .HasForeignKey<Comprobante>(c => c.IdPedido);

            // =================================================================
            // 4. CONVERSION DE ENUM: TipoMovimiento se guarda como texto
            //    ('ENTRADA' / 'SALIDA'), legible en la BD y en reportes.
            // =================================================================
            modelBuilder.Entity<ConceptoKardex>()
                .Property(x => x.TipoMovimiento)
                .HasConversion<string>()
                .HasMaxLength(10);

            // =================================================================
            // 5. VALORES POR DEFECTO EN SERVIDOR para fechas inmutables:
            //    si la aplicacion no envia valor, SQL Server registra GETDATE().
            // =================================================================
            modelBuilder.Entity<Usuario>().Property(x => x.FechaHoraRegistro).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Usuario>().Property(x => x.FechaHoraModificacion).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<ModeloReloj>().Property(x => x.FechaHoraRegistro).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Reloj>().Property(x => x.FechaHoraRegistro).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Carrito>().Property(x => x.FechaHoraCreacion).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Carrito>().Property(x => x.FechaHoraModificacion).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Pedido>().Property(x => x.FechaHoraPedido).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Comprobante>().Property(x => x.FechaHoraEmision).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Kardex>().Property(x => x.FechaHoraMovimiento).HasDefaultValueSql("GETDATE()");

            // =================================================================
            // 6. ELIMINACION LOGICA => ninguna FK borra en cascada.
            //    El sistema desactiva registros (estaActivo = false); ademas,
            //    Restrict evita el error clasico de SQL Server por "multiple
            //    cascade paths" que este esquema produciria (KARDEX/PEDIDO/
            //    CLIENTE/USUARIO comparten rutas de borrado).
            // =================================================================
            foreach (var fk in modelBuilder.Model.GetEntityTypes()
                                                 .SelectMany(t => t.GetForeignKeys())
                                                 .Where(f => !f.IsOwnership))
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // =================================================================
            // 7. DATOS SEMILLA
            // =================================================================
            SeedCatalogos(modelBuilder);
        }

        private static void SeedCatalogos(ModelBuilder modelBuilder)
        {
            // ----- Negocio (fila unica de configuracion) -----
            modelBuilder.Entity<Negocio>().HasData(new Negocio
            {
                IdNegocio = 1,
                NombreNegocio = "VANTIQ Peru",
                RucNegocio = null,          // completar con el RUC real
                NumCelular = null,          // completar con el WhatsApp del negocio
                Direccion = null
            });

            // ----- Roles (RBAC) -----
            modelBuilder.Entity<Rol>().HasData(
                new Rol { IdRol = 1, NombreRol = "Invitado", Descripcion = "Navega el catalogo, arma carrito de sesion y realiza pedidos basicos", EstaActivo = true },
                new Rol { IdRol = 2, NombreRol = "Cliente", Descripcion = "Carrito persistente entre sesiones e historial de compras", EstaActivo = true },
                new Rol { IdRol = 3, NombreRol = "Administrador", Descripcion = "Control total: usuarios, inventario, ventas y dashboards", EstaActivo = true });

            // ----- Menu del Administrador (estructura EXACTA del requerimiento) -----
            modelBuilder.Entity<OpcionMenu>().HasData(
                new OpcionMenu { IdOpcionMenu = 1, NombreOpcionMenu = "USUARIOS", UrlDestino = "/Usuarios", Orden = 1, EstaActiva = true },
                new OpcionMenu { IdOpcionMenu = 2, NombreOpcionMenu = "INVENTARIO", UrlDestino = "/Inventario", Orden = 2, EstaActiva = true },
                new OpcionMenu { IdOpcionMenu = 3, NombreOpcionMenu = "VENTAS", UrlDestino = "/Ventas", Orden = 3, EstaActiva = true });

            modelBuilder.Entity<RolOpcionMenu>().HasData(
                new RolOpcionMenu { IdRolOpcionMenu = 1, IdRol = 3, IdOpcionMenu = 1 },
                new RolOpcionMenu { IdRolOpcionMenu = 2, IdRol = 3, IdOpcionMenu = 2 },
                new RolOpcionMenu { IdRolOpcionMenu = 3, IdRol = 3, IdOpcionMenu = 3 });

            // ----- Categorias del catalogo (elicitacion del negocio) -----
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { IdCategoria = 1, NombreCategoria = "Automatico", Descripcion = "Relojes de cuerda automatica", EstaActiva = true },
                new Categoria { IdCategoria = 2, NombreCategoria = "GMT", Descripcion = "Relojes con segundo huso horario", EstaActiva = true },
                new Categoria { IdCategoria = 3, NombreCategoria = "Cronografo", Descripcion = "Relojes con funcion de cronometro", EstaActiva = true });

            // ----- Marca propia -----
            modelBuilder.Entity<Marca>().HasData(
                new Marca { IdMarca = 1, NombreMarca = "VANTIQ", EstaActiva = true });

            // ----- Estados del producto -----
            modelBuilder.Entity<EstadoReloj>().HasData(
                new EstadoReloj { IdEstadoReloj = 1, NombreEstadoReloj = "Disponible", Descripcion = "Visible y comprable en el catalogo", EstaActivo = true },
                new EstadoReloj { IdEstadoReloj = 2, NombreEstadoReloj = "Agotado", Descripcion = "Sin stock; visible pero no comprable", EstaActivo = true },
                new EstadoReloj { IdEstadoReloj = 3, NombreEstadoReloj = "Descontinuado", Descripcion = "Retirado del catalogo publico", EstaActivo = true });

            // ----- Metodos de pago (elicitacion: pagos locales) -----
            modelBuilder.Entity<MetodoPago>().HasData(
                new MetodoPago { IdMetodoPago = 1, NombreMetodoPago = "Yape", EstaActivo = true },
                new MetodoPago { IdMetodoPago = 2, NombreMetodoPago = "Plin", EstaActivo = true },
                new MetodoPago { IdMetodoPago = 3, NombreMetodoPago = "Transferencia bancaria", EstaActivo = true },
                new MetodoPago { IdMetodoPago = 4, NombreMetodoPago = "Tarjeta de credito o debito", EstaActivo = true },
                new MetodoPago { IdMetodoPago = 5, NombreMetodoPago = "Efectivo en tienda", EstaActivo = true });

            // ----- Estados del pedido (diagrama de estados de la Fase 2) -----
            modelBuilder.Entity<EstadoPedido>().HasData(
                new EstadoPedido { IdEstadoPedido = 1, NombreEstadoPedido = "Pendiente", Descripcion = "Registrado; en espera de verificacion del pago", EstaActivo = true },
                new EstadoPedido { IdEstadoPedido = 2, NombreEstadoPedido = "Pagado", Descripcion = "Pago verificado por el administrador", EstaActivo = true },
                new EstadoPedido { IdEstadoPedido = 3, NombreEstadoPedido = "Enviado", Descripcion = "Entregado al courier con numero de seguimiento", EstaActivo = true },
                new EstadoPedido { IdEstadoPedido = 4, NombreEstadoPedido = "Entregado", Descripcion = "Recibido por el cliente; cierra el ciclo", EstaActivo = true },
                new EstadoPedido { IdEstadoPedido = 5, NombreEstadoPedido = "Cancelado", Descripcion = "Anulado; repone stock via kardex de entrada", EstaActivo = true });

            // ----- Tipos de comprobante -----
            modelBuilder.Entity<TipoComprobante>().HasData(
                new TipoComprobante { IdTipoComprobante = 1, NombreTipo = "Boleta", EstaActivo = true },
                new TipoComprobante { IdTipoComprobante = 2, NombreTipo = "Factura", EstaActivo = true });

            // ----- Conceptos de kardex -----
            modelBuilder.Entity<ConceptoKardex>().HasData(
                new ConceptoKardex { IdConcepto = 1, NombreConcepto = "Compra a proveedor", TipoMovimiento = TipoMovimiento.ENTRADA, Descripcion = "Ingreso de mercaderia nueva", EstaActivo = true },
                new ConceptoKardex { IdConcepto = 2, NombreConcepto = "Venta", TipoMovimiento = TipoMovimiento.SALIDA, Descripcion = "Salida por pedido confirmado", EstaActivo = true },
                new ConceptoKardex { IdConcepto = 3, NombreConcepto = "Devolucion de cliente", TipoMovimiento = TipoMovimiento.ENTRADA, Descripcion = "Reingreso por pedido cancelado o devuelto", EstaActivo = true },
                new ConceptoKardex { IdConcepto = 4, NombreConcepto = "Ajuste positivo de inventario", TipoMovimiento = TipoMovimiento.ENTRADA, Descripcion = "Correccion por conteo fisico", EstaActivo = true },
                new ConceptoKardex { IdConcepto = 5, NombreConcepto = "Ajuste negativo de inventario", TipoMovimiento = TipoMovimiento.SALIDA, Descripcion = "Correccion por merma o dano", EstaActivo = true });

            // ----- Usuario administrador inicial -----
            // Credenciales de arranque: admin / Vantiq#2026  (cambiar tras el primer login)
            // El hash es BCrypt real ($2b$, costo 11), verificable con BCrypt.Net-Next.
            modelBuilder.Entity<Usuario>().HasData(new Usuario
            {
                IdUsuario = 1,
                NombreUsuario = "admin",
                Email = "admin@vantiq.pe",
                ContraseniaHash = "$2b$11$JbMqojLBfUvjvHuJT9LfI.Aap7ZZ6wUhiZJhiyi1flDkfzos57w5S",
                FechaHoraRegistro = new DateTime(2026, 1, 1),
                FechaHoraModificacion = new DateTime(2026, 1, 1),
                EstaActivo = true
            });

            modelBuilder.Entity<UsuarioRol>().HasData(
                new UsuarioRol { IdUsuarioRol = 1, IdUsuario = 1, IdRol = 3 });
        }
    }
}
