namespace Api.Models.ViewModels
{
    public class AgendaViewModel
    {
        // Campos principales de la agenda (a.*)
        public uint A_Codigo { get; set; }
        public DateTime A_Fecha { get; set; }
        public string A_Hora { get; set; } = string.Empty;
        public uint A_Cliente { get; set; }
        public uint A_Vendedor { get; set; }
        public byte A_Visitado { get; set; }
        public byte A_Estado { get; set; }
        public byte A_Planificacion { get; set; }
        public byte A_Prioridad { get; set; }
        public DateTime? A_Prox_Llamada { get; set; }
        public string? A_Prox_Acti { get; set; }
        public byte A_Visitado_Prox { get; set; }
        
        // Campos calculados
        public string ProxActi { get; set; } = string.Empty;
        public string Visitado { get; set; } = string.Empty;
        public string Prioridad { get; set; } = string.Empty;
        public string VisitaProx { get; set; } = string.Empty;
        public string Planificacion { get; set; } = string.Empty;
        public string Fecha { get; set; } = string.Empty;
        public string? FProx { get; set; }
        
        // Información del cliente
        public uint ClienteId { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public decimal? DeudasCliente { get; set; }
        public string? CliRuc { get; set; }
        public string? CliTel { get; set; }
        public string? CliDir { get; set; }
        
        // Información del vendedor
        public uint VendCod { get; set; }
        public string Vendedor { get; set; } = string.Empty;
        
        // Información de localización
        public decimal? LLatitud { get; set; }
        public decimal? LLongitud { get; set; }
        public string? LHoraInicio { get; set; }
        public string? LHoraFin { get; set; }
        public byte VisitaEnCurso { get; set; }
        public string? TiempoTranscurrido { get; set; }
        
        // Contadores
        public int TotalVisitasCliente { get; set; }
        public int MisVisitas { get; set; }
        public int MisVisitasCliente { get; set; }
        public int CantNotas { get; set; }
        
        // Arrays JSON
        public List<AgendaNotaViewModel> Notas { get; set; } = new List<AgendaNotaViewModel>();
        public List<AgendaSubvisitaViewModel> Subvisitas { get; set; } = new List<AgendaSubvisitaViewModel>();
    }

    public class AgendaNotaViewModel
    {
        public uint Id { get; set; }
        public string Fecha { get; set; } = string.Empty;
        public string Hora { get; set; } = string.Empty;
        public string Nota { get; set; } = string.Empty;
    }

    public class AgendaSubvisitaViewModel
    {
        public uint Id { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public string MotivoVisita { get; set; } = string.Empty;
        public string ResultadoVisita { get; set; } = string.Empty;
    }
}
