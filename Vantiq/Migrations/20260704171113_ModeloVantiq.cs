using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Vantiq.Migrations
{
    /// <inheritdoc />
    public partial class ModeloVantiq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CATEGORIA",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCategoria = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    EstaActiva = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATEGORIA", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "CLIENTE_VISITANTE",
                columns: table => new
                {
                    IdClienteVisitante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombresCliVisit = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    ApellidosCliVisit = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumCelular = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    EstaActivo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENTE_VISITANTE", x => x.IdClienteVisitante);
                });

            migrationBuilder.CreateTable(
                name: "CONCEPTO_KARDEX",
                columns: table => new
                {
                    IdConcepto = table.Column<byte>(type: "tinyint", nullable: false),
                    NombreConcepto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TipoMovimiento = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    EstaActivo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONCEPTO_KARDEX", x => x.IdConcepto);
                });

            migrationBuilder.CreateTable(
                name: "ESTADO_RELOJ",
                columns: table => new
                {
                    IdEstadoReloj = table.Column<byte>(type: "tinyint", nullable: false),
                    NombreEstadoReloj = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    EstaActivo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESTADO_RELOJ", x => x.IdEstadoReloj);
                });

            migrationBuilder.CreateTable(
                name: "MARCA",
                columns: table => new
                {
                    IdMarca = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreMarca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EstaActiva = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARCA", x => x.IdMarca);
                });

            migrationBuilder.CreateTable(
                name: "METODO_PAGO",
                columns: table => new
                {
                    IdMetodoPago = table.Column<byte>(type: "tinyint", nullable: false),
                    NombreMetodoPago = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EstaActivo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_METODO_PAGO", x => x.IdMetodoPago);
                });

            migrationBuilder.CreateTable(
                name: "NEGOCIO",
                columns: table => new
                {
                    IdNegocio = table.Column<byte>(type: "tinyint", nullable: false),
                    NombreNegocio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RucNegocio = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    NumCelular = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NEGOCIO", x => x.IdNegocio);
                });

            migrationBuilder.CreateTable(
                name: "OPCION_MENU",
                columns: table => new
                {
                    IdOpcionMenu = table.Column<byte>(type: "tinyint", nullable: false),
                    NombreOpcionMenu = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    UrlDestino = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Orden = table.Column<byte>(type: "tinyint", nullable: false),
                    EstaActiva = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OPCION_MENU", x => x.IdOpcionMenu);
                });

            migrationBuilder.CreateTable(
                name: "ROL",
                columns: table => new
                {
                    IdRol = table.Column<byte>(type: "tinyint", nullable: false),
                    NombreRol = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    EstaActivo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROL", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContraseniaHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    FechaHoraModificacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    EstaActivo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "MODELO_RELOJ",
                columns: table => new
                {
                    IdModeloReloj = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreModelo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    FechaHoraRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    EstaActivo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MODELO_RELOJ", x => x.IdModeloReloj);
                    table.ForeignKey(
                        name: "FK_MODELO_RELOJ_CATEGORIA_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "CATEGORIA",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ROL_OPCION_MENU",
                columns: table => new
                {
                    IdRolOpcionMenu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRol = table.Column<byte>(type: "tinyint", nullable: false),
                    IdOpcionMenu = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROL_OPCION_MENU", x => x.IdRolOpcionMenu);
                    table.ForeignKey(
                        name: "FK_ROL_OPCION_MENU_OPCION_MENU_IdOpcionMenu",
                        column: x => x.IdOpcionMenu,
                        principalTable: "OPCION_MENU",
                        principalColumn: "IdOpcionMenu",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ROL_OPCION_MENU_ROL_IdRol",
                        column: x => x.IdRol,
                        principalTable: "ROL",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO_ROL",
                columns: table => new
                {
                    IdUsuarioRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdRol = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO_ROL", x => x.IdUsuarioRol);
                    table.ForeignKey(
                        name: "FK_USUARIO_ROL_ROL_IdRol",
                        column: x => x.IdRol,
                        principalTable: "ROL",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_USUARIO_ROL_USUARIO_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "USUARIO",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VENTA",
                columns: table => new
                {
                    IdVenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: true),
                    IdClienteVisitante = table.Column<int>(type: "int", nullable: false),
                    IdMetodoPago = table.Column<byte>(type: "tinyint", nullable: false),
                    MontoTotal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    EstadoVenta = table.Column<byte>(type: "tinyint", nullable: false),
                    DireccionEnvio = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NumSeguimiento = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    FechaHoraVenta = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VENTA", x => x.IdVenta);
                    table.ForeignKey(
                        name: "FK_VENTA_CLIENTE_VISITANTE_IdClienteVisitante",
                        column: x => x.IdClienteVisitante,
                        principalTable: "CLIENTE_VISITANTE",
                        principalColumn: "IdClienteVisitante",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VENTA_METODO_PAGO_IdMetodoPago",
                        column: x => x.IdMetodoPago,
                        principalTable: "METODO_PAGO",
                        principalColumn: "IdMetodoPago",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VENTA_USUARIO_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "USUARIO",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RELOJ",
                columns: table => new
                {
                    IdReloj = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoSKU = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    IdModeloReloj = table.Column<int>(type: "int", nullable: false),
                    IdMarca = table.Column<int>(type: "int", nullable: false),
                    IdEstadoReloj = table.Column<byte>(type: "tinyint", nullable: false),
                    NumOrden = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UrlImagen = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    StockActual = table.Column<int>(type: "int", nullable: false),
                    FechaHoraRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RELOJ", x => x.IdReloj);
                    table.ForeignKey(
                        name: "FK_RELOJ_ESTADO_RELOJ_IdEstadoReloj",
                        column: x => x.IdEstadoReloj,
                        principalTable: "ESTADO_RELOJ",
                        principalColumn: "IdEstadoReloj",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RELOJ_MARCA_IdMarca",
                        column: x => x.IdMarca,
                        principalTable: "MARCA",
                        principalColumn: "IdMarca",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RELOJ_MODELO_RELOJ_IdModeloReloj",
                        column: x => x.IdModeloReloj,
                        principalTable: "MODELO_RELOJ",
                        principalColumn: "IdModeloReloj",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DETALLE_VENTA",
                columns: table => new
                {
                    IdDetalleVenta = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdVenta = table.Column<int>(type: "int", nullable: false),
                    IdReloj = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<short>(type: "smallint", nullable: false),
                    PrecioUnitarioVenta = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DETALLE_VENTA", x => x.IdDetalleVenta);
                    table.ForeignKey(
                        name: "FK_DETALLE_VENTA_RELOJ_IdReloj",
                        column: x => x.IdReloj,
                        principalTable: "RELOJ",
                        principalColumn: "IdReloj",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DETALLE_VENTA_VENTA_IdVenta",
                        column: x => x.IdVenta,
                        principalTable: "VENTA",
                        principalColumn: "IdVenta",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KARDEX",
                columns: table => new
                {
                    IdKardex = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdConcepto = table.Column<byte>(type: "tinyint", nullable: false),
                    IdReloj = table.Column<int>(type: "int", nullable: false),
                    IdVenta = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    StockResultante = table.Column<int>(type: "int", nullable: false),
                    FechaHoraMovimiento = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KARDEX", x => x.IdKardex);
                    table.ForeignKey(
                        name: "FK_KARDEX_CONCEPTO_KARDEX_IdConcepto",
                        column: x => x.IdConcepto,
                        principalTable: "CONCEPTO_KARDEX",
                        principalColumn: "IdConcepto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KARDEX_RELOJ_IdReloj",
                        column: x => x.IdReloj,
                        principalTable: "RELOJ",
                        principalColumn: "IdReloj",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KARDEX_USUARIO_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "USUARIO",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KARDEX_VENTA_IdVenta",
                        column: x => x.IdVenta,
                        principalTable: "VENTA",
                        principalColumn: "IdVenta",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CATEGORIA",
                columns: new[] { "IdCategoria", "Descripcion", "EstaActiva", "NombreCategoria" },
                values: new object[,]
                {
                    { 1, "Relojes de cuerda automática", true, "Automatico" },
                    { 2, "Relojes con segundo huso horario", true, "GMT" },
                    { 3, "Relojes con función de cronómetro", true, "Cronografo" }
                });

            migrationBuilder.InsertData(
                table: "CONCEPTO_KARDEX",
                columns: new[] { "IdConcepto", "Descripcion", "EstaActivo", "NombreConcepto", "TipoMovimiento" },
                values: new object[,]
                {
                    { (byte)1, "Ingreso de mercadería nueva", true, "Compra a proveedor", "ENTRADA" },
                    { (byte)2, "Salida por venta confirmada", true, "Venta", "SALIDA" },
                    { (byte)3, "Reingreso por venta cancelada o devuelta", true, "Devolucion de cliente", "ENTRADA" },
                    { (byte)4, "Corrección por conteo físico", true, "Ajuste positivo de inventario", "ENTRADA" },
                    { (byte)5, "Corrección por merma o daño", true, "Ajuste negativo de inventario", "SALIDA" }
                });

            migrationBuilder.InsertData(
                table: "ESTADO_RELOJ",
                columns: new[] { "IdEstadoReloj", "Descripcion", "EstaActivo", "NombreEstadoReloj" },
                values: new object[,]
                {
                    { (byte)1, "Visible y comprable en el catálogo", true, "Disponible" },
                    { (byte)2, "Sin stock; visible pero no comprable", true, "Agotado" },
                    { (byte)3, "Retirado del catálogo público", true, "Descontinuado" }
                });

            migrationBuilder.InsertData(
                table: "MARCA",
                columns: new[] { "IdMarca", "EstaActiva", "NombreMarca" },
                values: new object[] { 1, true, "VANTIQ" });

            migrationBuilder.InsertData(
                table: "METODO_PAGO",
                columns: new[] { "IdMetodoPago", "EstaActivo", "NombreMetodoPago" },
                values: new object[,]
                {
                    { (byte)1, true, "Yape" },
                    { (byte)2, true, "Plin" },
                    { (byte)3, true, "Transferencia bancaria" },
                    { (byte)4, true, "Tarjeta credito/debito" },
                    { (byte)5, true, "Efectivo contra entrega" }
                });

            migrationBuilder.InsertData(
                table: "NEGOCIO",
                columns: new[] { "IdNegocio", "Direccion", "NombreNegocio", "NumCelular", "RucNegocio" },
                values: new object[] { (byte)1, null, "VANTIQ Perú", "51999000000", null });

            migrationBuilder.InsertData(
                table: "OPCION_MENU",
                columns: new[] { "IdOpcionMenu", "EstaActiva", "NombreOpcionMenu", "Orden", "UrlDestino" },
                values: new object[,]
                {
                    { (byte)1, true, "USUARIOS", (byte)1, "/Usuarios" },
                    { (byte)2, true, "INVENTARIO", (byte)2, "/Inventario" },
                    { (byte)3, true, "VENTAS", (byte)3, "/Ventas" }
                });

            migrationBuilder.InsertData(
                table: "ROL",
                columns: new[] { "IdRol", "Descripcion", "EstaActivo", "NombreRol" },
                values: new object[,]
                {
                    { (byte)1, "Navega el catálogo y realiza pedidos como invitado", true, "Invitado" },
                    { (byte)2, "Historial de compras e identidad de comprador", true, "Cliente" },
                    { (byte)3, "Control total: usuarios, inventario, ventas y dashboards", true, "Administrador" }
                });

            migrationBuilder.InsertData(
                table: "USUARIO",
                columns: new[] { "IdUsuario", "ContraseniaHash", "Email", "EstaActivo", "FechaHoraModificacion", "FechaHoraRegistro", "NombreUsuario" },
                values: new object[] { 1, "$2b$11$JbMqojLBfUvjvHuJT9LfI.Aap7ZZ6wUhiZJhiyi1flDkfzos57w5S", "admin@vantiq.pe", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin" });

            migrationBuilder.InsertData(
                table: "MODELO_RELOJ",
                columns: new[] { "IdModeloReloj", "EstaActivo", "FechaHoraRegistro", "IdCategoria", "NombreModelo" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Navigator" },
                    { 2, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Meridian" },
                    { 3, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Chronos" }
                });

            migrationBuilder.InsertData(
                table: "ROL_OPCION_MENU",
                columns: new[] { "IdRolOpcionMenu", "IdOpcionMenu", "IdRol" },
                values: new object[,]
                {
                    { 1, (byte)1, (byte)3 },
                    { 2, (byte)2, (byte)3 },
                    { 3, (byte)3, (byte)3 }
                });

            migrationBuilder.InsertData(
                table: "USUARIO_ROL",
                columns: new[] { "IdUsuarioRol", "IdRol", "IdUsuario" },
                values: new object[] { 1, (byte)3, 1 });

            migrationBuilder.InsertData(
                table: "RELOJ",
                columns: new[] { "IdReloj", "CodigoSKU", "Descripcion", "FechaHoraRegistro", "IdEstadoReloj", "IdMarca", "IdModeloReloj", "NumOrden", "Precio", "StockActual", "UrlImagen" },
                values: new object[,]
                {
                    { 1, "VANTIQ-NAVIGATOR-001", "Movimiento automático NH35, caja acero 316L, cristal zafiro, WR 100m.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)1, 1, 1, 1, 890m, 5, "https://placehold.co/400x400/1A1815/C8A96E?text=NAV-001" },
                    { 2, "VANTIQ-NAVIGATOR-002", "Movimiento automático ETA 2824, esfera negra, correa cuero marrón.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)1, 1, 1, 2, 1290m, 3, "https://placehold.co/400x400/1A1815/C8A96E?text=NAV-002" },
                    { 3, "VANTIQ-MERIDIAN-001", "GMT bicolor, movimiento automático, bisel de cerámica, WR 200m.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)1, 1, 2, 1, 1590m, 4, "https://placehold.co/400x400/1A1815/C8A96E?text=MER-001" },
                    { 4, "VANTIQ-MERIDIAN-002", "GMT con función world-time, esfera azul marino, acero jubilee.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)1, 1, 2, 2, 2190m, 2, "https://placehold.co/400x400/1A1815/C8A96E?text=MER-002" },
                    { 5, "VANTIQ-CHRONOS-001", "Cronógrafo manual Valjoux 7750, contador 30 min / 12 h, acero.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)1, 1, 3, 1, 1890m, 6, "https://placehold.co/400x400/1A1815/C8A96E?text=CHR-001" },
                    { 6, "VANTIQ-CHRONOS-002", "Cronógrafo flyback ETA 7750, luneta taquímetro, correa de titanio.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)1, 1, 3, 2, 2490m, 1, "https://placehold.co/400x400/1A1815/C8A96E?text=CHR-002" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CATEGORIA_NombreCategoria",
                table: "CATEGORIA",
                column: "NombreCategoria",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CONCEPTO_KARDEX_NombreConcepto",
                table: "CONCEPTO_KARDEX",
                column: "NombreConcepto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DETALLE_VENTA_IdReloj",
                table: "DETALLE_VENTA",
                column: "IdReloj");

            migrationBuilder.CreateIndex(
                name: "IX_DETALLE_VENTA_IdVenta",
                table: "DETALLE_VENTA",
                column: "IdVenta");

            migrationBuilder.CreateIndex(
                name: "IX_ESTADO_RELOJ_NombreEstadoReloj",
                table: "ESTADO_RELOJ",
                column: "NombreEstadoReloj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KARDEX_IdConcepto",
                table: "KARDEX",
                column: "IdConcepto");

            migrationBuilder.CreateIndex(
                name: "IX_KARDEX_IdReloj",
                table: "KARDEX",
                column: "IdReloj");

            migrationBuilder.CreateIndex(
                name: "IX_KARDEX_IdUsuario",
                table: "KARDEX",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_KARDEX_IdVenta",
                table: "KARDEX",
                column: "IdVenta");

            migrationBuilder.CreateIndex(
                name: "IX_MARCA_NombreMarca",
                table: "MARCA",
                column: "NombreMarca",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_METODO_PAGO_NombreMetodoPago",
                table: "METODO_PAGO",
                column: "NombreMetodoPago",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MODELO_RELOJ_IdCategoria",
                table: "MODELO_RELOJ",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_MODELO_RELOJ_NombreModelo",
                table: "MODELO_RELOJ",
                column: "NombreModelo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OPCION_MENU_NombreOpcionMenu",
                table: "OPCION_MENU",
                column: "NombreOpcionMenu",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RELOJ_CodigoSKU",
                table: "RELOJ",
                column: "CodigoSKU",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RELOJ_IdEstadoReloj",
                table: "RELOJ",
                column: "IdEstadoReloj");

            migrationBuilder.CreateIndex(
                name: "IX_RELOJ_IdMarca",
                table: "RELOJ",
                column: "IdMarca");

            migrationBuilder.CreateIndex(
                name: "IX_RELOJ_IdModeloReloj",
                table: "RELOJ",
                column: "IdModeloReloj");

            migrationBuilder.CreateIndex(
                name: "IX_ROL_NombreRol",
                table: "ROL",
                column: "NombreRol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ROL_OPCION_MENU_IdOpcionMenu",
                table: "ROL_OPCION_MENU",
                column: "IdOpcionMenu");

            migrationBuilder.CreateIndex(
                name: "IX_ROL_OPCION_MENU_IdRol_IdOpcionMenu",
                table: "ROL_OPCION_MENU",
                columns: new[] { "IdRol", "IdOpcionMenu" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_Email",
                table: "USUARIO",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_NombreUsuario",
                table: "USUARIO",
                column: "NombreUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_ROL_IdRol",
                table: "USUARIO_ROL",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_ROL_IdUsuario_IdRol",
                table: "USUARIO_ROL",
                columns: new[] { "IdUsuario", "IdRol" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VENTA_IdClienteVisitante",
                table: "VENTA",
                column: "IdClienteVisitante");

            migrationBuilder.CreateIndex(
                name: "IX_VENTA_IdMetodoPago",
                table: "VENTA",
                column: "IdMetodoPago");

            migrationBuilder.CreateIndex(
                name: "IX_VENTA_IdUsuario",
                table: "VENTA",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DETALLE_VENTA");

            migrationBuilder.DropTable(
                name: "KARDEX");

            migrationBuilder.DropTable(
                name: "NEGOCIO");

            migrationBuilder.DropTable(
                name: "ROL_OPCION_MENU");

            migrationBuilder.DropTable(
                name: "USUARIO_ROL");

            migrationBuilder.DropTable(
                name: "CONCEPTO_KARDEX");

            migrationBuilder.DropTable(
                name: "RELOJ");

            migrationBuilder.DropTable(
                name: "VENTA");

            migrationBuilder.DropTable(
                name: "OPCION_MENU");

            migrationBuilder.DropTable(
                name: "ROL");

            migrationBuilder.DropTable(
                name: "ESTADO_RELOJ");

            migrationBuilder.DropTable(
                name: "MARCA");

            migrationBuilder.DropTable(
                name: "MODELO_RELOJ");

            migrationBuilder.DropTable(
                name: "CLIENTE_VISITANTE");

            migrationBuilder.DropTable(
                name: "METODO_PAGO");

            migrationBuilder.DropTable(
                name: "USUARIO");

            migrationBuilder.DropTable(
                name: "CATEGORIA");
        }
    }
}
