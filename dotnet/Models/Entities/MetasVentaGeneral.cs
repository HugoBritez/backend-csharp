using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{

    [Table("metas_venta_general")]
    public class MetasVentaGeneral
    {
        [Key]
        [Column("id")]
        public uint Id { get; set;}

        [Column("ar_codigo")]
        public uint ArCodigo { get; set;}

        [Column("meta_general")]
        public decimal MetaGeneral { get; set;}

        [Column("periodo")]
        public uint Periodo { get;set;}
        [Column("estado")]
        public int Estado { get; set;}
    }
}
/*

CREATE TABLE `metas_venta_general` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `ar_codigo` int unsigned NOT NULL,
  `meta_general` decimal(15,2) NOT NULL DEFAULT '0.00',
  `periodo` int NOT NULL DEFAULT '0' COMMENT 'Año de la meta',
  `estado` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`),
  UNIQUE KEY `idx_meta_venta_general_unique` (`ar_codigo`,`periodo`),
  CONSTRAINT `metas_venta_general_ibfk_1` FOREIGN KEY (`ar_codigo`) REFERENCES `articulos` (`ar_codigo`)
) ENGINE=InnoDB;

*/