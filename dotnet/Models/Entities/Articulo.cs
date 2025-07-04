using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    [Table("articulos")]
    public class Articulo
    {
        [Key]
        [Column("ar_codigo")]
        public uint ArCodigo { get; set; }

        [Column("ar_codbarra")]
        public string? ArCodBarra { get; set; }

        [Column("ar_unidadmedida")]
        public uint ArUnidadMedida { get; set; }

        [Column("ar_descripcion")]
        public string ArDescripcion { get; set; } = string.Empty;

        [Column("ar_marca")]
        public uint ArMarca { get; set; }

        [Column("ar_subcategoria")]
        public uint ArSubCategoria { get; set; } = 1;

        [Column("ar_ubicacicion")]
        public uint ArUbicacion { get; set; } = 1;

        [Column("ar_moneda")]
        public uint ArMoneda { get; set; }

        [Column("ar_iva")]
        public uint ArIva { get; set; }

        [Column("ar_pcg")]
        public decimal ArPrecioCompraGuaranies { get; set; } = 0.00M;

        [Column("ar_pcd")]
        public decimal ArPrecioCompraDolar { get; set; } = 0.00M;

        [Column("ar_pcr")]
        public decimal ArPrecioCompraReal { get; set; } = 0.00M;

        [Column("ar_pcp")]
        public decimal ArPrecioCompraPeso { get; set; } = 0.00M;

        [Column("ar_pvg")]
        public decimal ArPrecioVentaGuaranies { get; set; } = 0.00M;

        [Column("ar_pvd")]
        public decimal ArPrecioVentaDolar { get; set; } = 0.00M;

        [Column("ar_pvr")]
        public decimal ArPrecioVentaReal { get; set; } = 0.00M;

        [Column("ar_pvp")]
        public decimal ArPrecioVentaPeso { get; set; } = 0.00M;

        [Column("ar_stkmin")]
        public decimal ArStockMinimo { get; set; } = 0.00M;

        [Column("ar_comision")]
        public decimal ArComision { get; set; } = 0.00M;

        [Column("ar_ganancia")]
        public decimal ArGanancia { get; set; } = 0.00M;

        [Column("ar_descmax")]
        public decimal ArDescuentoMaximo { get; set; } = 0.00M;

        [Column("ar_servicio")]
        public byte ArServicio { get; set; } = 0;

        [Column("ar_stockneg")]
        public byte ArStockNegativo { get; set; } = 0;

        [Column("ar_movpeso")]
        public byte ArMovPeso { get; set; } = 0;

        [Column("ar_obs")]
        public string? ArObservaciones { get; set; }

        [Column("ar_disponible")]
        public byte ArDisponible { get; set; } = 1;

        [Column("ar_estado")]
        public byte ArEstado { get; set; } = 1;

        [Column("ar_recargo")]
        public decimal ArRecargo { get; set; } = 0.00M;

        [Column("ar_vencimiento")]
        public byte ArVencimiento { get; set; } = 0;

        [Column("ar_fechaVen")]
        public DateTime ArFechaVencimiento { get; set; } = new DateTime(1, 1, 1);

        [Column("ar_cbunidad")]
        public byte ArCbUnidad { get; set; } = 0;

        [Column("ar_kilos")]
        public decimal ArKilos { get; set; } = 0.00M;

        [Column("ar_fechaAlta")]
        public DateTime? ArFechaAlta { get; set; }

        [Column("ar_UsuarioAlta")]
        public string? ArUsuarioAlta { get; set; }

        [Column("ar_legal")]
        public byte ArLegal { get; set; } = 0;

        [Column("ar_nom_generico")]
        public string? ArNombreGenerico { get; set; }

        [Column("ar_cod_acri")]
        public string? ArCodAcri { get; set; }

        [Column("ar_fabricante")]
        public uint ArFabricante { get; set; } = 1;

        [Column("ar_indicacion")]
        public string? ArIndicacion { get; set; }

        [Column("ar_tipo_control")]
        public uint ArTipoControl { get; set; } = 1;

        [Column("ar_pvcredito")]
        public decimal ArPrecioVentaCredito { get; set; } = 0.00M;

        [Column("ar_pvmostrador")]
        public decimal ArPrecioVentaMostrador { get; set; } = 0.00M;

        [Column("ar_pais")]
        public uint ArPais { get; set; } = 1;

        [Column("ar_via")]
        public uint ArVia { get; set; } = 1;

        [Column("ar_gancredito")]
        public decimal ArGananciaCredito { get; set; } = 0.00M;

        [Column("ar_ganmostrador")]
        public decimal ArGananciaMostrador { get; set; } = 0.00M;

        [Column("ar_linea")]
        public uint ArLinea { get; set; } = 1;

        [Column("ar_foto")]
        public string? ArFoto { get; set; }

        [Column("ar_cant_caja")]
        public decimal ArCantidadCaja { get; set; } = 0.00M;

        [Column("ar_documento")]
        public string? ArDocumento { get; set; }

        [Column("ar_trasmision")]
        public uint ArTransmision { get; set; } = 1;

        [Column("ar_estado_veh")]
        public uint ArEstadoVehiculo { get; set; } = 1;

        [Column("ar_combustible")]
        public uint ArCombustible { get; set; } = 1;

        [Column("ar_traccion")]
        public uint ArTraccion { get; set; } = 1;

        [Column("ar_chassi")]
        public string? ArChassi { get; set; }

        [Column("ar_modelo")]
        public string? ArModelo { get; set; }

        [Column("ar_chapa")]
        public string? ArChapa { get; set; }

        [Column("ar_anho")]
        public string? ArAnho { get; set; }

        [Column("ar_km")]
        public int ArKilometraje { get; set; } = 0;

        [Column("ar_color")]
        public string? ArColor { get; set; }

        [Column("ar_cod_interno")]
        public string? ArCodInterno { get; set; }

        [Column("ar_pvdcredito")]
        public decimal ArPrecioVentaDolarCredito { get; set; } = 0.00M;

        [Column("ar_pvdmostrador")]
        public decimal ArPrecioVentaDolarMostrador { get; set; } = 0.00M;

        [Column("ar_gandcontado")]
        public decimal ArGananciaDolarContado { get; set; } = 0.00M;

        [Column("ar_gandcredito")]
        public decimal ArGananciaDolarCredito { get; set; } = 0.00M;

        [Column("ar_gandmostrador")]
        public decimal ArGananciaDolarMostrador { get; set; } = 0.00M;

        [Column("ar_principio_activo")]
        public string? ArPrincipioActivo { get; set; }

        [Column("ar_concentracion")]
        public string? ArConcentracion { get; set; }

        [Column("ar_kit")]
        public byte ArKit { get; set; } = 0;

        [Column("ar_precio_desc_ds")]
        public decimal ArPrecioDescuentoDolar { get; set; } = 0.00M;

        [Column("ar_cantidad_desde")]
        public decimal ArCantidadDesde { get; set; } = 0.00M;

        [Column("ar_precio_desc_gs")]
        public decimal ArPrecioDescuentoGuaranies { get; set; } = 0.00M;

        [Column("ar_ganancia_desc")]
        public decimal ArGananciaDescuento { get; set; } = 0.00M;

        [Column("ar_materia_prima")]
        public byte ArMateriaPrima { get; set; } = 0;

        [Column("ar_mov_boutique")]
        public byte ArMovBoutique { get; set; } = 0;

        [Column("ar_promo")]
        public byte ArPromo { get; set; } = 0;

        [Column("ar_seccion")]
        public uint ArSeccion { get; set; } = 1;

        [Column("ar_incluir_inventario")]
        public byte ArIncluirInventario { get; set; } = 1;

        [Column("ar_receptor")]
        public string ArReceptor { get; set; } = string.Empty;

        [Column("ar_dvl")]
        public uint ArDvl { get; set; } = 1;

        [Column("ar_d_venta_l")]
        public uint ArDVentaL { get; set; } = 1;

        [Column("ar_serie")]
        public string ArSerie { get; set; } = string.Empty;

        [Column("ar_garantia")]
        public byte ArGarantia { get; set; } = 0;

        [Column("ar_lote")]
        public string? ArLote { get; set; }

        [Column("ar_sububicacion")]
        public uint ArSubUbicacion { get; set; } = 1;

        [Column("ar_editar_desc")]
        public byte ArEditarDesc { get; set; } = 0;

        [Column("ar_granatura")]
        public uint ArGranatura { get; set; } = 1;

        [Column("ar_medida")]
        public uint ArMedida { get; set; } = 1;

        [Column("ar_caracteristica")]
        public uint ArCaracteristica { get; set; } = 1;

        [Column("ar_colores")]
        public uint ArColores { get; set; } = 1;

        [Column("ar_alicuotaiva")]
        public byte ArAlicuotaIva { get; set; } = 0;

        [Column("ar_control_codbarra")]
        public byte ArControlCodBarra { get; set; } = 0;

        [Column("ar_opcion_set")]
        public byte ArOpcionSet { get; set; } = 0;

        [Column("ar_opcion_set10")]
        public byte ArOpcionSet10 { get; set; } = 0;

        [Column("ar_precio_4")]
        public decimal ArPrecio4 { get; set; } = 0.00M;

        [Column("ar_ganprecio_4")]
        public decimal ArGanPrecio4 { get; set; } = 0.00M;

        [Column("ar_preciod_4")]
        public decimal ArPrecioD4 { get; set; } = 0.00M;

        [Column("ar_ganpreciod_4")]
        public decimal ArGanPrecioD4 { get; set; } = 0.00M;

        [Column("ar_pvgxcaja")]
        public decimal ArPvgxCaja { get; set; } = 0.00M;

        [Column("ar_gangpvxcaja")]
        public decimal ArGangPvxCaja { get; set; } = 0.00M;

        [Column("ar_control_registro")]
        public byte ArControlRegistro { get; set; } = 0;

        [Column("ar_ignorarmovcxc")]
        public byte ArIgnorarMovCxc { get; set; } = 0;

        [Column("ar_actualizar_pventa")]
        public byte ArActualizarPVenta { get; set; } = 0;

        [Column("ar_costo_mt2")]
        public decimal ArCostoMt2 { get; set; } = 0.00M;

        [Column("ar_bloque")]
        public uint ArBloque { get; set; } = 1;

        [Column("ar_pvgxcaja_cred")]
        public decimal ArPvgxCajaCred { get; set; } = 0.00M;

        [Column("ar_pvgxcajag_cred")]
        public decimal ArPvgxCajaGCred { get; set; } = 0.00M;

        [Column("ar_pvgxcaja_most")]
        public decimal ArPvgxCajaMost { get; set; } = 0.00M;

        [Column("ar_pvgxcajag_most")]
        public decimal ArPvgxCajaGMost { get; set; } = 0.00M;

        [Column("ar_pvgxcaja_most2")]
        public decimal ArPvgxCajaMost2 { get; set; } = 0.00M;

        [Column("ar_pvgxcajag_most2")]
        public decimal ArPvgxCajaGMost2 { get; set; } = 0.00M;

        [Column("ar_actcostosubeprod")]
        public byte ArActCostoSubeProd { get; set; } = 0;

        [Column("ar_noactcostobajaprod")]
        public byte ArNoActCostoBajaProd { get; set; } = 0;

        [Column("ar_bloquear")]
        public byte ArBloquear { get; set; } = 0;

        [Column("ar_fraccionamiento")]
        public uint ArFraccionamiento { get; set; } = 1;

        [Column("ar_cant_fraccion")]
        public decimal ArCantFraccion { get; set; } = 0.000M;

        [Column("ar_precio_fraccion")]
        public decimal ArPrecioFraccion { get; set; } = 0.00M;

        [Column("ar_comprimido")]
        public byte ArComprimido { get; set; } = 0;

        [Column("ar_formula_pintura")]
        public byte ArFormulaPintura { get; set; } = 0;

        [Column("ar_base_pintura")]
        public byte ArBasePintura { get; set; } = 0;

        [Column("ar_cod_marcas_origen")]
        public string ArCodMarcasOrigen { get; set; } = string.Empty;

        [Column("ar_ms_ensamble")]
        public byte ArMsEnsamble { get; set; } = 0;

        public virtual ICollection<ArticuloLote>? ArticuloLotes { get; set; }
    }
}