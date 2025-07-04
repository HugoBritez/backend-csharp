using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Audit.Models
{
    [Table("auditoria")]
    public class Auditoria
    {
        [Key]
        [Column("id")]
        public int Id { get; set;}

        [Column("entidad")] // 1- Articulos, 2- Clientes, 3- Proveedores, 4- Compras, 5- Ventas, 42- Inventario , 65 Pedidos , 73 Presupuestos 
        public int Entidad { get; set;}

        [Column("accion")] //1- insertar, 2- modificar, 3- Anular, 4- Acceso o login, 5- Habilitar
        public int Accion { get; set;}

        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Column("id_referencia")]
        public int IdReferencia { get; set; }

        [Column("usuario")]
        public string Usuario { get; set; } = string.Empty;

        [Column("vendedor")]
        public int Vendedor { get; set; }

        [Column("obs")]
        public string Obs { get; set; } = string.Empty;
        
    }
}
