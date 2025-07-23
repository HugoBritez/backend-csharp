using Microsoft.EntityFrameworkCore;
using Api.Models.Entities;
using Api.Model.Entities;

namespace Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<ArticuloLote> ArticuloLotes { get; set; }
        public DbSet<Deposito> Depositos { get; set; }
        public DbSet<Ubicacion> Ubicaciones { get; set; }
        public DbSet<Sububicacion> SubUbicaciones { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<SubCategoria> SubCategorias { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Iva> Ivas { get; set; }
        public DbSet<ListaPrecio> ListaPrecio { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Moneda> Moneda { get; set; }
        public DbSet<Venta> Venta { get; set; }
        public DbSet<DetalleVenta> DetalleVenta { get; set; }
        public DbSet<DetalleVentaBonificacion> DetalleVentaBonificacion { get; set; }
        public DbSet<DetalleArticulosEditado> DetalleArticulosEditados { get; set; }
        public DbSet<DetalleVentaVencimiento> DetalleVentaVencimiento { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<DetallePedido> DetallePedido { get; set; }
        public DbSet<DetallePedidoFaltante> DetallePedidoFaltante { get; set; }
        public DbSet<AreaSecuencia> AreaSecuencia { get; set; }
        public DbSet<PedidosEstados> PedidoEstado { get; set; }
        public DbSet<Presupuesto> Presupuesto { get; set; }
        public DbSet<DetallePresupuesto> DetallePresupuesto { get; set; }
        public DbSet<PresupuestoObservacion> PresupuestoObservacion { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Zona> Zonas { get; set; }
        public DbSet<UnidadMedida> UnidadMedidas { get; set; }
        public DbSet<Configuracion> Configuraciones { get; set; }
        public DbSet<InventarioAuxiliar> InventariosAuxiliares { get; set; }
        public DbSet<InventarioAuxiliarItems> InventarioAuxiliarItems { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<DetalleCompra> DetalleCompras { get; set; }

        public DbSet<OrdenCompra> OrdenesCompra { get; set; }

        public DbSet<Transferencia> Transferencias { get; set; }
        public DbSet<TransferenciaItem> TransferenciaItems { get; set; }
        public DbSet<TransferenciaItemVencimiento> TransferenciaItemsVencimiento { get; set; }
        public DbSet<Agenda> Agendas { get; set; }
        public DbSet<AgendasNotas> AgendasNotas { get; set; }
        public DbSet<Localizacion> Localizaciones { get; set; }
        public DbSet<AgendaSubvisita> AgendaSubvisitas { get; set; }
        public DbSet<ConfiguracionAsiento> ConfiguracionAsientos { get; set; }
        public DbSet<Cotizacion> Cotizaciones { get; set; }
        public DbSet<MetodoPago> MetodosPago { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Operador> Operadores { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<TipoDocumento> TipoDocumentos { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<GrupoCliente> GrupoClientes { get; set; }
        public DbSet<Talle> Talles { get; set; }
        public DbSet<ClientesListaDePrecio> ClientesListaDePrecios { get; set; }
        public DbSet<TipoPlazo> TipoPlazos { get; set; }
        public DbSet<MetaVentaArticulo> MetaVentaArticulos { get; set; }
        public DbSet<DetalleCompraVencimiento> DetalleCompraVencimiento { get; set; }
        public DbSet<MetasVentaGeneral> MetasVentaGeneral { get; set; }

        public DbSet<OportunidadCRM> Oportunidades { get; set; }
        public DbSet<TareaCRM> Tareas { get; set; }
        public DbSet<TipoTareaCRM> TipoTareas { get; set; }
        public DbSet<EstadoCRM> EstadoCRM { get; set; }
        public DbSet<ContactoCRM> ContactosCRM { get; set; }
        public DbSet<AgendamientoCRM> AgendamientosCRM { get; set; }
        public DbSet<Doctor> Doctores { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<RecordatorioCRM> RecordatoriosCRM { get; set; }
        public DbSet<ProyectosColaboradoresCRM> ProyectosColaboradoresCRM { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar las relaciones entre entidades
            modelBuilder.Entity<ArticuloLote>()
                .HasOne(al => al.Articulo)
                .WithMany(a => a.ArticuloLotes)
                .HasForeignKey(al => al.AlArticulo);

            modelBuilder.Entity<ArticuloLote>()
                .HasOne(al => al.Deposito)
                .WithMany(d => d.ArticulosLote)
                .HasForeignKey(al => al.AlDeposito);

            modelBuilder.Entity<SubCategoria>()
                .HasOne(sc => sc.Categoria)
                .WithMany()
                .HasForeignKey(sc => sc.ScCategoria);

            modelBuilder.Entity<InventarioAuxiliarItems>()
                .HasOne(item => item.Inventario)
                .WithMany(inv => inv.Items)
                .HasForeignKey(item => item.IdInventario);

            modelBuilder.Entity<DetalleCompra>()
                .HasOne(detalle => detalle.CompraNavigation)
                .WithMany(Compra => Compra.Items)
                .HasForeignKey(detalle => detalle.Compra);

            modelBuilder.Entity<TransferenciaItem>()
                .HasOne(item => item.TransferenciaNavigation)
                .WithMany(trans => trans.Items)
                .HasForeignKey(item => item.Transferencia);

            // Configurar clave primaria compuesta para DetalleCompraVencimiento
            modelBuilder.Entity<DetalleCompraVencimiento>()
                .HasKey(dv => new { dv.Compra, dv.DetalleCompra });

            modelBuilder.Entity<ClientesListaDePrecio>()
        .HasKey(clp => new { clp.ClienteId, clp.ListaDePrecioId });

            modelBuilder.Entity<ClientesListaDePrecio>()
                .HasOne(clp => clp.Cliente)
                .WithMany(c => c.ClientesListaDePrecios)
                .HasForeignKey(clp => clp.ClienteId);

            modelBuilder.Entity<ClientesListaDePrecio>()
                .HasOne(clp => clp.ListaDePrecio)
                .WithMany(lp => lp.ClientesListaDePrecios)
                .HasForeignKey(clp => clp.ListaDePrecioId);
        }
    }
}
