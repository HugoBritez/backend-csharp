namespace Api.Auth.Models
{

    public class LoginProveedor
    {
        public uint ProCodigo {get; set;}
        public string ProRazon {get; set;} = string.Empty;
    }
    public class LoginProveedorResponse
    {
        public string Token {get; set;} = string.Empty;
        public LoginProveedor Proveedor {get; set;} = new();
    }
}