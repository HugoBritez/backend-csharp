export interface PedidoDetalle {
  det_codigo: number;
  art_codigo: number;
  codbarra: string;
  descripcion: string;
  costo: number;
  cantidad: number;
  precio: number;
  descuento: number;
  exentas: number;
  cinco: number;
  diez: number;
  codlote: number;
  lote: string;
  ar_editar_desc: boolean;
  bonificacion: number;
  largura: string;
  altura: string;
  mt2: string;
  precio_compra: number | null;
  porcentaje_utilidad: number;
} 