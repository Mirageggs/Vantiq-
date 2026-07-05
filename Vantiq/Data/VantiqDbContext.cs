using Microsoft.EntityFrameworkCore;
using Vantiq.Models;

namespace Vantiq.Data
{
    /// <summary>
    /// Contexto EF Core Code-First de VANTIQ.
    /// Configuración Fluent API + semillas de catálogos.
    /// </summary>
    public class VantiqDbContext : DbContext
    {
        public VantiqDbContext(DbContextOptions<VantiqDbContext> options)
            : base(options) { }

        // ===== DbSets =====
        public DbSet<Negocio>          Negocios           { get; set; } = null!;
        public DbSet<OpcionMenu>       OpcionesMenu       { get; set; } = null!;
        public DbSet<Rol>              Roles              { get; set; } = null!;
        public DbSet<Usuario>          Usuarios           { get; set; } = null!;
        public DbSet<RolOpcionMenu>    RolesOpcionMenu    { get; set; } = null!;
        public DbSet<UsuarioRol>       UsuariosRoles      { get; set; } = null!;
        public DbSet<ClienteVisitante> ClientesVisitantes { get; set; } = null!;
        public DbSet<Categoria>        Categorias         { get; set; } = null!;
        public DbSet<Marca>            Marcas             { get; set; } = null!;
        public DbSet<ModeloReloj>      ModelosReloj       { get; set; } = null!;
        public DbSet<EstadoReloj>      EstadosReloj       { get; set; } = null!;
        public DbSet<Reloj>            Relojes            { get; set; } = null!;
        public DbSet<MetodoPago>       MetodosPago        { get; set; } = null!;
        public DbSet<Venta>            Ventas             { get; set; } = null!;
        public DbSet<DetalleVenta>     DetallesVenta      { get; set; } = null!;
        public DbSet<ConceptoKardex>   ConceptosKardex    { get; set; } = null!;
        public DbSet<Kardex>           MovimientosKardex  { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ================================================================
            // 1. CATÁLOGOS — PK sin IDENTITY (valores fijos en semilla)
            // ================================================================
            modelBuilder.Entity<Negocio>().Property(x => x.IdNegocio).ValueGeneratedNever();
            modelBuilder.Entity<Rol>().Property(x => x.IdRol).ValueGeneratedNever();
            modelBuilder.Entity<OpcionMenu>().Property(x => x.IdOpcionMenu).ValueGeneratedNever();
            modelBuilder.Entity<EstadoReloj>().Property(x => x.IdEstadoReloj).ValueGeneratedNever();
            modelBuilder.Entity<MetodoPago>().Property(x => x.IdMetodoPago).ValueGeneratedNever();
            modelBuilder.Entity<ConceptoKardex>().Property(x => x.IdConcepto).ValueGeneratedNever();

            // ================================================================
            // 2. ÍNDICES ÚNICOS
            // ================================================================
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
            modelBuilder.Entity<ConceptoKardex>().HasIndex(x => x.NombreConcepto).IsUnique();

            // Únicos compuestos
            modelBuilder.Entity<UsuarioRol>()
                .HasIndex(x => new { x.IdUsuario, x.IdRol }).IsUnique();
            modelBuilder.Entity<RolOpcionMenu>()
                .HasIndex(x => new { x.IdRol, x.IdOpcionMenu }).IsUnique();

            // ================================================================
            // 3. CONVERSIONES DE ENUM
            // ================================================================
            // EstadoVenta → byte en columna TINYINT
            modelBuilder.Entity<Venta>()
                .Property(x => x.EstadoVenta)
                .HasConversion<byte>();

            // TipoMovimiento → nvarchar(10)
            modelBuilder.Entity<ConceptoKardex>()
                .Property(x => x.TipoMovimiento)
                .HasConversion<string>()
                .HasMaxLength(10);

            // ================================================================
            // 4. PRECISIÓN DE DECIMALES
            // ================================================================
            modelBuilder.Entity<Venta>()
                .Property(x => x.MontoTotal)
                .HasPrecision(10, 2);

            modelBuilder.Entity<DetalleVenta>()
                .Property(x => x.PrecioUnitarioVenta)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Reloj>()
                .Property(x => x.Precio)
                .HasPrecision(10, 2);

            // ================================================================
            // 5. VALORES POR DEFECTO EN SERVIDOR (GETDATE())
            // ================================================================
            modelBuilder.Entity<Usuario>()
                .Property(x => x.FechaHoraRegistro).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Usuario>()
                .Property(x => x.FechaHoraModificacion).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<ModeloReloj>()
                .Property(x => x.FechaHoraRegistro).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Reloj>()
                .Property(x => x.FechaHoraRegistro).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Venta>()
                .Property(x => x.FechaHoraVenta).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Kardex>()
                .Property(x => x.FechaHoraMovimiento).HasDefaultValueSql("GETDATE()");

            // ================================================================
            // 6. RESTRICCIÓN DE CASCADA GLOBAL (Restrict en todos los FK)
            // ================================================================
            foreach (var fk in modelBuilder.Model.GetEntityTypes()
                                                 .SelectMany(t => t.GetForeignKeys())
                                                 .Where(f => !f.IsOwnership))
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // ================================================================
            // 7. SEMILLAS
            // ================================================================
            SeedCatalogos(modelBuilder);
        }

        private static void SeedCatalogos(ModelBuilder modelBuilder)
        {
            // ── Negocio (fila única de configuración) ──
            modelBuilder.Entity<Negocio>().HasData(new Negocio
            {
                IdNegocio    = 1,
                NombreNegocio = "VANTIQ Perú",
                RucNegocio   = null,
                NumCelular   = "51999000000",  // reemplazar con el celular real de contacto
                Direccion    = null
            });

            // ── Roles (RBAC) ──
            modelBuilder.Entity<Rol>().HasData(
                new Rol { IdRol = 1, NombreRol = "Invitado",       Descripcion = "Navega el catálogo y realiza pedidos como invitado",           EstaActivo = true },
                new Rol { IdRol = 2, NombreRol = "Cliente",        Descripcion = "Historial de compras e identidad de comprador",                EstaActivo = true },
                new Rol { IdRol = 3, NombreRol = "Administrador",  Descripcion = "Control total: usuarios, inventario, ventas y dashboards",     EstaActivo = true });

            // ── Menú del Administrador (solo rol 3) ──
            modelBuilder.Entity<OpcionMenu>().HasData(
                new OpcionMenu { IdOpcionMenu = 1, NombreOpcionMenu = "USUARIOS",    UrlDestino = "/Usuarios",    Orden = 1, EstaActiva = true },
                new OpcionMenu { IdOpcionMenu = 2, NombreOpcionMenu = "INVENTARIO",  UrlDestino = "/Inventario",  Orden = 2, EstaActiva = true },
                new OpcionMenu { IdOpcionMenu = 3, NombreOpcionMenu = "VENTAS",      UrlDestino = "/Ventas",      Orden = 3, EstaActiva = true });

            modelBuilder.Entity<RolOpcionMenu>().HasData(
                new RolOpcionMenu { IdRolOpcionMenu = 1, IdRol = 3, IdOpcionMenu = 1 },
                new RolOpcionMenu { IdRolOpcionMenu = 2, IdRol = 3, IdOpcionMenu = 2 },
                new RolOpcionMenu { IdRolOpcionMenu = 3, IdRol = 3, IdOpcionMenu = 3 });

            // ── Categorías del catálogo ──
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { IdCategoria = 1, NombreCategoria = "Automatico",   Descripcion = "Relojes de cuerda automática",     EstaActiva = true },
                new Categoria { IdCategoria = 2, NombreCategoria = "GMT",          Descripcion = "Relojes con segundo huso horario",  EstaActiva = true },
                new Categoria { IdCategoria = 3, NombreCategoria = "Cronografo",   Descripcion = "Relojes con función de cronómetro", EstaActiva = true });

            // ── Marca propia ──
            modelBuilder.Entity<Marca>().HasData(
                new Marca { IdMarca = 1, NombreMarca = "VANTIQ", EstaActiva = true });

            // ── Estados del reloj ──
            modelBuilder.Entity<EstadoReloj>().HasData(
                new EstadoReloj { IdEstadoReloj = 1, NombreEstadoReloj = "Disponible",    Descripcion = "Visible y comprable en el catálogo",      EstaActivo = true },
                new EstadoReloj { IdEstadoReloj = 2, NombreEstadoReloj = "Agotado",       Descripcion = "Sin stock; visible pero no comprable",     EstaActivo = true },
                new EstadoReloj { IdEstadoReloj = 3, NombreEstadoReloj = "Descontinuado", Descripcion = "Retirado del catálogo público",           EstaActivo = true });

            // ── Métodos de pago ──
            modelBuilder.Entity<MetodoPago>().HasData(
                new MetodoPago { IdMetodoPago = 1, NombreMetodoPago = "Yape",                      EstaActivo = true },
                new MetodoPago { IdMetodoPago = 2, NombreMetodoPago = "Plin",                      EstaActivo = true },
                new MetodoPago { IdMetodoPago = 3, NombreMetodoPago = "Transferencia bancaria",    EstaActivo = true },
                new MetodoPago { IdMetodoPago = 4, NombreMetodoPago = "Tarjeta credito/debito",    EstaActivo = true },
                new MetodoPago { IdMetodoPago = 5, NombreMetodoPago = "Efectivo contra entrega",   EstaActivo = true });

            // ── Conceptos de kardex ──
            modelBuilder.Entity<ConceptoKardex>().HasData(
                new ConceptoKardex { IdConcepto = 1, NombreConcepto = "Compra a proveedor",             TipoMovimiento = TipoMovimiento.ENTRADA, Descripcion = "Ingreso de mercadería nueva",               EstaActivo = true },
                new ConceptoKardex { IdConcepto = 2, NombreConcepto = "Venta",                          TipoMovimiento = TipoMovimiento.SALIDA,  Descripcion = "Salida por venta confirmada",               EstaActivo = true },
                new ConceptoKardex { IdConcepto = 3, NombreConcepto = "Devolucion de cliente",          TipoMovimiento = TipoMovimiento.ENTRADA, Descripcion = "Reingreso por venta cancelada o devuelta",  EstaActivo = true },
                new ConceptoKardex { IdConcepto = 4, NombreConcepto = "Ajuste positivo de inventario",  TipoMovimiento = TipoMovimiento.ENTRADA, Descripcion = "Corrección por conteo físico",              EstaActivo = true },
                new ConceptoKardex { IdConcepto = 5, NombreConcepto = "Ajuste negativo de inventario",  TipoMovimiento = TipoMovimiento.SALIDA,  Descripcion = "Corrección por merma o daño",               EstaActivo = true });

            // ── Usuario administrador inicial ──
            // Contraseña: Vantiq#2026  →  hash BCrypt generado con work factor 11
            modelBuilder.Entity<Usuario>().HasData(new Usuario
            {
                IdUsuario            = 1,
                NombreUsuario        = "admin",
                Email                = "admin@vantiq.pe",
                ContraseniaHash      = "$2b$11$JbMqojLBfUvjvHuJT9LfI.Aap7ZZ6wUhiZJhiyi1flDkfzos57w5S",
                FechaHoraRegistro    = new DateTime(2026, 1, 1),
                FechaHoraModificacion = new DateTime(2026, 1, 1),
                EstaActivo           = true
            });

            modelBuilder.Entity<UsuarioRol>().HasData(
                new UsuarioRol { IdUsuarioRol = 1, IdUsuario = 1, IdRol = 3 });

            // ── Modelos de reloj demo (3, uno por categoría) ──
            modelBuilder.Entity<ModeloReloj>().HasData(
                new ModeloReloj { IdModeloReloj = 1, NombreModelo = "Navigator",  IdCategoria = 1, FechaHoraRegistro = new DateTime(2026, 1, 1), EstaActivo = true },
                new ModeloReloj { IdModeloReloj = 2, NombreModelo = "Meridian",   IdCategoria = 2, FechaHoraRegistro = new DateTime(2026, 1, 1), EstaActivo = true },
                new ModeloReloj { IdModeloReloj = 3, NombreModelo = "Chronos",    IdCategoria = 3, FechaHoraRegistro = new DateTime(2026, 1, 1), EstaActivo = true });

            // ── Relojes demo (~6 unidades) ──
            modelBuilder.Entity<Reloj>().HasData(
                new Reloj
                {
                    IdReloj = 1, CodigoSKU = "VANTIQ-NAVIGATOR-001",
                    IdModeloReloj = 1, IdMarca = 1, NumOrden = 1,
                    Precio = 890m, StockActual = 5, IdEstadoReloj = 1,
                    UrlImagen = "https://placehold.co/400x400/1A1815/C8A96E?text=NAV-001",
                    Descripcion = "Movimiento automático NH35, caja acero 316L, cristal zafiro, WR 100m.",
                    FechaHoraRegistro = new DateTime(2026, 1, 1)
                },
                new Reloj
                {
                    IdReloj = 2, CodigoSKU = "VANTIQ-NAVIGATOR-002",
                    IdModeloReloj = 1, IdMarca = 1, NumOrden = 2,
                    Precio = 1290m, StockActual = 3, IdEstadoReloj = 1,
                    UrlImagen = "https://placehold.co/400x400/1A1815/C8A96E?text=NAV-002",
                    Descripcion = "Movimiento automático ETA 2824, esfera negra, correa cuero marrón.",
                    FechaHoraRegistro = new DateTime(2026, 1, 1)
                },
                new Reloj
                {
                    IdReloj = 3, CodigoSKU = "VANTIQ-MERIDIAN-001",
                    IdModeloReloj = 2, IdMarca = 1, NumOrden = 1,
                    Precio = 1590m, StockActual = 4, IdEstadoReloj = 1,
                    UrlImagen = "https://placehold.co/400x400/1A1815/C8A96E?text=MER-001",
                    Descripcion = "GMT bicolor, movimiento automático, bisel de cerámica, WR 200m.",
                    FechaHoraRegistro = new DateTime(2026, 1, 1)
                },
                new Reloj
                {
                    IdReloj = 4, CodigoSKU = "VANTIQ-MERIDIAN-002",
                    IdModeloReloj = 2, IdMarca = 1, NumOrden = 2,
                    Precio = 2190m, StockActual = 2, IdEstadoReloj = 1,
                    UrlImagen = "https://placehold.co/400x400/1A1815/C8A96E?text=MER-002",
                    Descripcion = "GMT con función world-time, esfera azul marino, acero jubilee.",
                    FechaHoraRegistro = new DateTime(2026, 1, 1)
                },
                new Reloj
                {
                    IdReloj = 5, CodigoSKU = "VANTIQ-CHRONOS-001",
                    IdModeloReloj = 3, IdMarca = 1, NumOrden = 1,
                    Precio = 1890m, StockActual = 6, IdEstadoReloj = 1,
                    UrlImagen = "https://placehold.co/400x400/1A1815/C8A96E?text=CHR-001",
                    Descripcion = "Cronógrafo manual Valjoux 7750, contador 30 min / 12 h, acero.",
                    FechaHoraRegistro = new DateTime(2026, 1, 1)
                },
                new Reloj
                {
                    IdReloj = 6, CodigoSKU = "VANTIQ-CHRONOS-002",
                    IdModeloReloj = 3, IdMarca = 1, NumOrden = 2,
                    Precio = 2490m, StockActual = 1, IdEstadoReloj = 1,
                    UrlImagen = "https://placehold.co/400x400/1A1815/C8A96E?text=CHR-002",
                    Descripcion = "Cronógrafo flyback ETA 7750, luneta taquímetro, correa de titanio.",
                    FechaHoraRegistro = new DateTime(2026, 1, 1)
                });
        }
    }
}
