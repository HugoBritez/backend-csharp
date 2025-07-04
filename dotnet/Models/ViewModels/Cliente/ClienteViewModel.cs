namespace Api.Models.ViewModels
{
    public class ClienteViewModel
    {
        public uint cli_codigo { get; set; }
        public string cli_razon { get; set; } = string.Empty;
        public string cli_descripcion { get; set; } = string.Empty;
        public string cli_ruc { get; set; } = string.Empty;
        public string cli_interno { get; set; } = string.Empty;
        public uint cli_ciudad { get; set; }
        public string zona { get; set; } = string.Empty;
        public string cli_ciudad_descripcion { get; set; } = string.Empty;
        public uint cli_departamento { get; set; }
        public string dep_descripcion { get; set; } = string.Empty;
        public uint cli_distrito { get; set; }
        public string cli_distrito_descripcion { get; set; } = string.Empty;
        public string cli_limitecredito { get; set; } = string.Empty;
        public string deuda_actual { get; set; } = string.Empty;
        public string credito_disponible { get; set; } = string.Empty;
        public uint vendedor_cliente { get; set; }
        public string cli_dir { get; set; } = string.Empty;
        public string cli_tel { get; set; } = string.Empty;
        public string cli_mail { get; set; } = string.Empty;
        public string cli_ci { get; set; } = string.Empty;
        public uint cli_tipo_doc { get; set; }
        public int estado { get; set; }

    }
}