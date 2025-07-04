using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Api.Models.Entities
{
    [Table("operadores")]
    public class Operador
    {
        [Key]
        [Column("op_codigo")]
        public int OpCodigo { get; set; }

        [Column("op_nombre")]
        public string OpNombre { get; set; } = string.Empty;

        [Column("op_documento")]
        public string OpDocumento { get; set; } = string.Empty;

        [Column("op_dir")]
        public string OpDireccion { get; set; } = string.Empty;

        [Column("op_tel")]
        public string OpTelefono { get; set; } = string.Empty;

        [Column("op_email")]
        public string OpEmail { get; set; } = string.Empty;

        [Column("op_fechaingreso")]
        public DateTime OpFechaIngreso { get; set; }

        [Column("op_descuento")]
        public decimal OpDescuento { get; set; }

        [Column("op_fechaNacimiento")]
        public DateTime OpFechaNacimiento { get; set;}

        [Column("op_obs")]
        public string OpObservacion { get; set; } = string.Empty;

        [Column("op_estado")]
        public int OpEstado { get; set; }

        [Column("op_usuario")]
        public string OpUsuario { get; set; } = string.Empty;

        [Column("op_sucursal")]
        public int OpSucursal {get; set;}

        [Column("op_fechaAlta")]   
        public DateOnly OpFechaAlta { get; set; }

        [Column("op_area")]
        public int OpArea { get; set; }

        [Column("op_comision")]
        public decimal OpComision { get; set; }

        [Column("op_numven")]
        public int OpNumVen { get; set; }

        [Column("op_movimiento")]
        public int OpMovimiento { get; set;}

        [Column("op_tipo_vendedor")]
        public int OpTipoVendedor { get; set; }

        [Column("op_vendedor_actividad")]
        public int OpVendedorActividad { get; set; }

        [Column("op_autorizar")]
        public int OpAutorizar { get; set;}

        [Column("op_dia_camb_clave")]
        public int OpDiaCambClave { get; set; }

        [Column("op_f_cambio_clave")]
        public DateOnly OpFechaCambioClave { get; set; }

        [Column("op_ver_utilidad")]
        public int OpVerUtilidad { get; set;}

        [Column("op_cantidadturno")]
        public int OpCantidadTurno { get; set; }

        [Column("op_modificarfecha")]
        public int OpModificarFecha { get; set; }

        [Column("op_graficos")]
        public int OpGraficos { get; set; }

        [Column("op_aplicar_descuento")]
        public int OpAplicarDescuento { get ;  set ;}

        [Column("op_desc_aplicar")]
        public decimal OpDescuentoAplicar { get; set; }

        [Column("op_ver_proveedor")]
        public int OpVerProveedor { get; set;}

        [Column("op_contrasena")]
        public string OpContrasena { get;  set; } = string.Empty;

        [Column("op_uti")]
        public int OpUti { get; set; }

        [Column("op_tipo_operador")]
        public int OpTipoOperador { get; set; }

        [Column("op_cliente")]
        public int OpCliente { get; set;}

    }
}