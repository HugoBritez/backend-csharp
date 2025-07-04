using System.Text.Json.Serialization;

namespace Api.Auth.Models
{
    public class LoginResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("usuario")]
        public List<UsuarioResponse> Usuario { get; set; } = [];
    }

    public class UsuarioResponse
    {
        [JsonPropertyName("op_codigo")]
        public uint OpCodigo { get; set; }

        [JsonPropertyName("op_nombre")]
        public string OpNombre { get; set; } = string.Empty;

        [JsonPropertyName("op_sucursal")]
        public uint OpSucursal { get; set; }

        [JsonPropertyName("op_autorizar")]
        public int OpAutorizar { get; set; }

        [JsonPropertyName("op_ver_utilidad")]
        public int OpVerUtilidad { get; set; }

        [JsonPropertyName("op_ver_proveedor")]
        public int OpVerProveedor { get; set; }

        [JsonPropertyName("op_aplicar_descuento")]
        public int OpAplicarDescuento { get; set; }

        [JsonPropertyName("op_movimiento")]
        public int OpMovimiento { get; set; }

        [JsonPropertyName("or_rol")]
        public int OrRol { get; set; }

        [JsonPropertyName("permisos_menu")]
        public List<PermisoMenu> PermisosMenu { get; set; } = [];
    }

    public class PermisoMenu
    {
        [JsonPropertyName("menu_id")]
        public int MenuId { get; set; }

        [JsonPropertyName("menu_grupo")]
        public int MenuGrupo { get; set; }

        [JsonPropertyName("menu_orden")]
        public int MenuOrden { get; set; }

        [JsonPropertyName("menu_descripcion")]
        public string MenuDescripcion { get; set; } = string.Empty;

        [JsonPropertyName("acceso")]
        public int Acceso { get; set; }

        [JsonPropertyName("crear")]
        public int Crear { get; set; }
        [JsonPropertyName("editar")]
        public int Editar { get; set; }
    }
}