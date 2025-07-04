using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    [Table("clientes")]
    public class Cliente
    {
        [Key]
        [Column("cli_codigo")]
        public uint Codigo { get; set; }

        [Column("cli_interno")]
        [StringLength(20)]
        public string? Interno { get; set; }

        [Column("cli_razon")]
        [StringLength(250)]
        public string? Razon { get; set; }

        [Column("cli_ruc")]
        [StringLength(30)]
        public string? Ruc { get; set; }

        [Column("cli_ci")]
        [StringLength(20)]
        public string? Ci { get; set; }

        [Column("cli_ciudad")]
        public uint? Ciudad { get; set; }

        [Column("cli_moneda")]
        public uint? Moneda { get; set; }

        [Column("cli_barrio")]
        [StringLength(45)]
        public string? Barrio { get; set; }

        [Column("cli_dir")]
        [StringLength(250)]
        public string? Dir { get; set; }

        [Column("cli_tel")]
        [StringLength(50)]
        public string? Tel { get; set; }

        [Column("cli_credito")]
        public int Credito { get; set; }

        [Column("cli_limitecredito")]
        public decimal LimiteCredito { get; set; } = 0.00m;

        [Column("cli_vendedor")]
        public uint? Vendedor { get; set; }

        [Column("cli_cobrador")]
        public uint? Cobrador { get; set; }

        [Column("cli_referencias")]
        public string? Referencias { get; set; }

        [Column("cli_estado")]
        public int Estado { get; set; }

        [Column("cli_fechaAd")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaAd { get; set; }

        [Column("cli_descripcion")]
        [StringLength(250)]
        public string? Descripcion { get; set; }

        [Column("cli_condicion")]
        public byte Condicion { get; set; } = 1;

        [Column("cli_tipo")]
        public byte Tipo { get; set; } = 1;

        [Column("cli_grupo")]
        public uint Grupo { get; set; } = 1;

        [Column("cli_plazo")]
        public uint Plazo { get; set; } = 1;

        [Column("cli_zona")]
        public uint Zona { get; set; } = 1;

        [Column("cli_llamada")]
        [DataType(DataType.DateTime)]
        public DateTime? Llamada { get; set; }

        [Column("cli_proxllamada")]
        [DataType(DataType.DateTime)]
        public DateTime? ProxLlamada { get; set; }

        [Column("cli_respuesta")]
        public string? Respuesta { get; set; }

        [Column("cli_fecnac")]
        [DataType(DataType.DateTime)]
        public DateTime? FecNac { get; set; }

        [Column("cli_exentas")]
        public int Exentas { get; set; } = 0;

        [Column("cli_mail")]
        [StringLength(80)]
        public string? Mail { get; set; }

        [Column("cli_agente")]
        public uint Agente { get; set; } = 1;

        [Column("cli_contrato")]
        public byte Contrato { get; set; } = 0;

        [Column("cli_nombre_cod")]
        [StringLength(60)]
        public string NombreCod { get; set; } = string.Empty;

        [Column("cli_doc_cod")]
        [StringLength(25)]
        public string DocCod { get; set; } = string.Empty;

        [Column("cli_telef_cod")]
        [StringLength(15)]
        public string TelefCod { get; set; } = string.Empty;

        [Column("cli_dir_cod")]
        [StringLength(80)]
        public string DirCod { get; set; } = string.Empty;

        [Column("cli_obs_deuda")]
        [StringLength(250)]
        public string ObsDeuda { get; set; } = string.Empty;

        [Column("cli_moroso")]
        public byte Moroso { get; set; } = 0;

        [Column("cli_agente_retentor")]
        public byte AgenteRetentor { get; set; } = 0;

        [Column("cli_consultar")]
        public byte Consultar { get; set; } = 0;

        [Column("cli_plan")]
        public uint Plan { get; set; } = 0;

        [Column("cli_fecha_pago")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaPago { get; set; }

        [Column("cli_departamento")]
        public uint Departamento { get; set; } = 1;

        [Column("cli_gerente")]
        [StringLength(250)]
        public string Gerente { get; set; } = string.Empty;

        [Column("cli_ger_telefono")]
        [StringLength(250)]
        public string GerTelefono { get; set; } = string.Empty;

        [Column("cli_ger_telefono2")]
        [StringLength(250)]
        public string GerTelefono2 { get; set; } = string.Empty;

        [Column("cli_ger_pagina")]
        [StringLength(250)]
        public string GerPagina { get; set; } = string.Empty;

        [Column("cli_ger_mail")]
        [StringLength(250)]
        public string GerMail { get; set; } = string.Empty;

        [Column("cli_permitir_desc")]
        public int PermitirDesc { get; set; }

        [Column("cli_calc_mora")]
        public int CalcMora { get; set; }

        [Column("cli_bloquear_vendedor")]
        public byte BloquearVendedor { get; set; } = 0;

        [Column("cli_sexo")]
        public uint Sexo { get; set; } = 3;

        [Column("cli_tipo_doc")]
        public byte TipoDoc { get; set; } = 1;

        [Column("cli_repetir_ruc")]
        public int RepetirRuc { get; set; }

        [Column("cli_acuerdo")]
        public int Acuerdo { get; set; }
        
        public ICollection<ClientesListaDePrecio>? ClientesListaDePrecios { get; set; }
    }
}