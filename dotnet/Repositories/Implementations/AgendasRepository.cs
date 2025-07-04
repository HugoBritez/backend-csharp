using Api.Data;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Api.Repositories.Implementations
{
  public class AgendasRepository : DapperRepositoryBase, IAgendasRepository
  {
    private readonly ApplicationDbContext _context;

    public AgendasRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
    {
      _context = context;
    }

    public async Task<Agenda> Crear(Agenda data)
    {
      var agendaCreada = await _context.Agendas.AddAsync(data);
      await _context.SaveChangesAsync();
      return agendaCreada.Entity;
    }

    public async Task<Agenda> Update(Agenda data)
    {
      _context.Agendas.Update(data);
      await _context.SaveChangesAsync();
      return data;
    }

    public async Task<Agenda?> GetById(uint id)
    {
      var agenda = await _context.Agendas.FirstOrDefaultAsync(ag => ag.Id == id);
      return agenda;
    }

    public async Task<IEnumerable<AgendaViewModel>> GetTodas(
        string? fechaDesde = null,
        string? fechaHasta = null,
        string? cliente = null,
        string? vendedor = null,
        int visitado = -1,
        int estado = -1,
        int planificacion = -1,
        int notas = -1,
        string orden = "0")
    {
      var where = "1=1";
      var having = "";
      var order = "";
      var parameters = new DynamicParameters();

      // Construcción de filtros WHERE
      if (!string.IsNullOrEmpty(fechaDesde))
      {
        where += " AND a.a_fecha >= @fechaDesde";
        parameters.Add("fechaDesde", fechaDesde);
      }

      if (!string.IsNullOrEmpty(fechaHasta))
      {
        where += " AND a.a_fecha <= @fechaHasta";
        parameters.Add("fechaHasta", fechaHasta);
      }

      if (!string.IsNullOrEmpty(cliente) && cliente != "0")
      {
        where += " AND a.a_cliente IN (@cliente)";
        parameters.Add("cliente", cliente.Split(',').Select(int.Parse));
      }

      if (!string.IsNullOrEmpty(vendedor) && vendedor != "0")
      {
        where += " AND a.a_vendedor IN (@vendedor)";
        parameters.Add("vendedor", vendedor.Split(',').Select(int.Parse));
      }

      if (visitado != -1)
      {
        where += " AND a.a_visitado = @visitado";
        parameters.Add("visitado", visitado);
      }

      if (estado != -1)
      {
        where += " AND a.a_estado = @estado";
        parameters.Add("estado", estado);
      }

      if (planificacion != -1)
      {
        where += " AND a.a_planificacion = @planificacion";
        parameters.Add("planificacion", planificacion);
      }

      // Filtro HAVING para notas
      if (notas == 0) having = " HAVING cant_notas = 0";
      if (notas == 1) having = " HAVING cant_notas > 0";

      // Ordenamiento
      order = orden switch
      {
        "0" => "ORDER BY a.a_prioridad DESC, a.a_fecha ASC",
        "1" => "ORDER BY a.a_codigo ASC",
        "2" => "ORDER BY a.a_fecha DESC, a.a_hora ASC",
        _ => "ORDER BY a.a_fecha DESC"
      };

      var query = $@"
        SELECT
            a.a_codigo,
            a.a_fecha,
            a.a_hora,
            a.a_cliente,
            a.a_vendedor,
            a.a_visitado,
            a.a_estado,
            a.a_planificacion,
            a.a_prioridad,
            NULLIF(a.a_prox_llamada, '0000-00-00 00:00:00') AS a_prox_llamada,
            a.a_prox_acti,
            a.a_visitado_prox,
            IFNULL(a.a_prox_acti, '') AS prox_acti,
            IF(a.a_visitado=0, 'No', 'Sí') AS visitado,
            IF(a.a_prioridad=0, 'Común', 
               IF(a.a_prioridad=1, 'Baja', 
                  IF(a.a_prioridad=2, 'Moderada', 
                     IF(a.a_prioridad=3, 'Alta', '-')))) AS prioridad,
            IF(a.a_visitado_prox=0, 'No', 'Sí') AS visita_prox,
            IF(a.a_planificacion=0, 'No', 'Sí') AS planificacion,
            DATE_FORMAT(a.a_fecha, '%Y-%m-%d') AS fecha,
            CASE 
                WHEN a.a_prox_llamada IS NULL OR a.a_prox_llamada = '0000-00-00 00:00:00' OR a.a_prox_llamada = '1000-01-01 00:00:00' OR a.a_prox_llamada < '1900-01-01'
                THEN NULL 
                ELSE DATE_FORMAT(a.a_prox_llamada, '%Y-%m-%d') 
            END AS f_prox,
            cli.cli_codigo as cliente_id,
            cli.cli_razon AS cliente,
            (SELECT SUM(ve_saldo) FROM ventas WHERE ve_cliente = cli.cli_codigo AND ve_estado = 1) AS deudas_cliente,
            cli.cli_ruc,
            cli.cli_tel,
            cli.cli_dir,
            op.op_codigo AS vendcod,
            op.op_nombre AS vendedor,
            l.l_latitud,
            l.l_longitud,
            l.l_hora_inicio,
            l.l_hora_fin,
            IF(l.l_hora_inicio IS NOT NULL AND l.l_hora_inicio != '', 1, 0) AS visita_en_curso,
            CASE 
              WHEN TIMESTAMPDIFF(MINUTE, 
                CONCAT(DATE_FORMAT(NOW(), '%Y-%m-%d'), ' ', l.l_hora_inicio),
                CONCAT(DATE_FORMAT(NOW(), '%Y-%m-%d'), ' ', l.l_hora_fin)
              ) < 60 THEN 
                CONCAT(TIMESTAMPDIFF(MINUTE, 
                  CONCAT(DATE_FORMAT(NOW(), '%Y-%m-%d'), ' ', l.l_hora_inicio),
                  CONCAT(DATE_FORMAT(NOW(), '%Y-%m-%d'), ' ', l.l_hora_fin)
                ), ' minutos')
              ELSE 
                CONCAT(
                  FLOOR(TIMESTAMPDIFF(MINUTE, 
                    CONCAT(DATE_FORMAT(NOW(), '%Y-%m-%d'), ' ', l.l_hora_inicio),
                    CONCAT(DATE_FORMAT(NOW(), '%Y-%m-%d'), ' ', l.l_hora_fin)
                  ) / 60), ' horas ',
                  MOD(TIMESTAMPDIFF(MINUTE, 
                    CONCAT(DATE_FORMAT(NOW(), '%Y-%m-%d'), ' ', l.l_hora_inicio),
                    CONCAT(DATE_FORMAT(NOW(), '%Y-%m-%d'), ' ', l.l_hora_fin)
                  ), 60), ' minutos'
                )
            END AS tiempo_transcurrido,
            (SELECT COUNT(a_codigo) FROM agendas WHERE a_cliente = cli.cli_codigo) AS total_visitas_cliente,
            (SELECT COUNT(a_codigo) FROM agendas b WHERE b.a_vendedor = a.a_vendedor) AS mis_visitas,
            (SELECT COUNT(a_codigo) FROM agendas c WHERE c.a_vendedor = a.a_vendedor AND c.a_cliente = a.a_cliente) AS mis_visitas_cliente,
            COUNT(DISTINCT an.an_codigo) AS cant_notas,
            (
              SELECT JSON_ARRAYAGG(
                JSON_OBJECT(
                  'id', an_codigo,
                  'fecha', DATE_FORMAT(an_fecha, '%d/%m/%Y'),
                  'hora', TIME_FORMAT(an_hora, '%H:%i'),
                  'nota', an_nota
                )
              )
              FROM (
                SELECT DISTINCT an_codigo, an_fecha, an_hora, an_nota 
                FROM agendas_notas 
                WHERE an_agenda_id = a.a_codigo
              ) temp_notas
            ) AS notas_json,
            (
              SELECT JSON_ARRAYAGG(
                JSON_OBJECT(
                  'id', id,
                  'nombre_cliente', nombre_cliente,
                  'motivo_visita', motivo_visita,
                  'resultado_visita', resultado_visita
                )
              )
              FROM (
                SELECT DISTINCT id, nombre_cliente, motivo_visita, resultado_visita 
                FROM agenda_subvisitas 
                WHERE id_agenda = a.a_codigo
              ) temp_subvisitas
            ) AS subvisitas_json
        FROM agendas a
        INNER JOIN clientes cli ON a.a_cliente = cli.cli_codigo
        INNER JOIN operadores op ON a.a_vendedor = op.op_codigo
        LEFT JOIN agendas_notas an ON a.a_codigo = an.an_agenda_id
        LEFT JOIN localizacion l ON a.a_codigo = l.l_agenda
        WHERE {where}
        GROUP BY a.a_codigo
        {having}
        {order}";
    
      try
      {
        using var connection = GetConnection();
        var result = await connection.QueryAsync<AgendaViewModel>(query, parameters);
        return result;
      }
      catch (MySqlException ex) when (ex.Message.Contains("Incorrect DATE value"))
      {
        // Si falla por fechas incorrectas, intentamos con una consulta más defensiva
        var defensiveQuery = $@"
            SELECT
                a.a_codigo,
                a.a_fecha,
                a.a_hora,
                a.a_cliente,
                a.a_vendedor,
                a.a_visitado,
                a.a_estado,
                a.a_planificacion,
                a.a_prioridad,
                NULL AS a_prox_llamada,
                a.a_prox_acti,
                a.a_visitado_prox,
                IFNULL(a.a_prox_acti, '') AS prox_acti,
                IF(a.a_visitado=0, 'No', 'Sí') AS visitado,
                IF(a.a_prioridad=0, 'Común', 
                   IF(a.a_prioridad=1, 'Baja', 
                      IF(a.a_prioridad=2, 'Moderada', 
                         IF(a.a_prioridad=3, 'Alta', '-')))) AS prioridad,
                IF(a.a_visitado_prox=0, 'No', 'Sí') AS visita_prox,
                IF(a.a_planificacion=0, 'No', 'Sí') AS planificacion,
                DATE_FORMAT(a.a_fecha, '%Y-%m-%d') AS fecha,
                NULL AS f_prox,
                cli.cli_codigo as cliente_id,
                cli.cli_razon AS cliente,
                (SELECT SUM(ve_saldo) FROM ventas WHERE ve_cliente = cli.cli_codigo AND ve_estado = 1) AS deudas_cliente,
                cli.cli_ruc,
                cli.cli_tel,
                cli.cli_dir,
                op.op_codigo AS vendcod,
                op.op_nombre AS vendedor,
                l.l_latitud,
                l.l_longitud,
                l.l_hora_inicio,
                l.l_hora_fin,
                IF(l.l_hora_inicio IS NOT NULL AND l.l_hora_inicio != '', 1, 0) AS visita_en_curso,
                CASE 
                  WHEN TIMESTAMPDIFF(MINUTE, 
                    CONCAT(DATE_FORMAT(NOW(), '%Y-%m-%d'), ' ', l.l_hora_inicio),
                    CONCAT(DATE_FORMAT(NOW(), '%Y-%m-%d'), ' ', l.l_hora_fin)
                  ) < 60 THEN 
                    CONCAT(TIMESTAMPDIFF(MINUTE, 
                      CONCAT(DATE_FORMAT(NOW(), '%Y-%m-%d'), ' ', l.l_hora_inicio),
                      CONCAT(DATE_FORMAT(NOW(), '%Y-%m-%d'), ' ', l.l_hora_fin)
                    ), ' minutos')
                  ELSE 
                    CONCAT(
                      FLOOR(TIMESTAMPDIFF(MINUTE, 
                        CONCAT(DATE_FORMAT(NOW(), '%Y-%m-%d'), ' ', l.l_hora_inicio),
                        CONCAT(DATE_FORMAT(NOW(), '%Y-%m-%d'), ' ', l.l_hora_fin)
                      ) / 60), ' horas ',
                      MOD(TIMESTAMPDIFF(MINUTE, 
                        CONCAT(DATE_FORMAT(NOW(), '%Y-%m-%d'), ' ', l.l_hora_inicio),
                        CONCAT(DATE_FORMAT(NOW(), '%Y-%m-%d'), ' ', l.l_hora_fin)
                      ), 60), ' minutos'
                    )
                END AS tiempo_transcurrido,
                (SELECT COUNT(a_codigo) FROM agendas WHERE a_cliente = cli.cli_codigo) AS total_visitas_cliente,
                (SELECT COUNT(a_codigo) FROM agendas b WHERE b.a_vendedor = a.a_vendedor) AS mis_visitas,
                (SELECT COUNT(a_codigo) FROM agendas c WHERE c.a_vendedor = a.a_vendedor AND c.a_cliente = a.a_cliente) AS mis_visitas_cliente,
                COUNT(DISTINCT an.an_codigo) AS cant_notas,
                (
                  SELECT JSON_ARRAYAGG(
                    JSON_OBJECT(
                      'id', an_codigo,
                      'fecha', DATE_FORMAT(an_fecha, '%d/%m/%Y'),
                      'hora', TIME_FORMAT(an_hora, '%H:%i'),
                      'nota', an_nota
                    )
                  ) 
                  FROM (
                    SELECT DISTINCT an_codigo, an_fecha, an_hora, an_nota 
                    FROM agendas_notas 
                    WHERE an_agenda_id = a.a_codigo
                  ) temp_notas
                ) AS notas_json,
                (
                  SELECT JSON_ARRAYAGG(
                    JSON_OBJECT(
                      'id', id,
                      'nombre_cliente', nombre_cliente,
                      'motivo_visita', motivo_visita,
                      'resultado_visita', resultado_visita
                    )
                  )
                  FROM (
                    SELECT DISTINCT id, nombre_cliente, motivo_visita, resultado_visita 
                    FROM agenda_subvisitas 
                    WHERE id_agenda = a.a_codigo
                  ) temp_subvisitas
                ) AS subvisitas_json
            FROM agendas a
            INNER JOIN clientes cli ON a.a_cliente = cli.cli_codigo
            INNER JOIN operadores op ON a.a_vendedor = op.op_codigo
            LEFT JOIN agendas_notas an ON a.a_codigo = an.an_agenda_id
            LEFT JOIN localizacion l ON a.a_codigo = l.l_agenda
            WHERE {where}
            GROUP BY a.a_codigo
            {having}
            {order}";

        using var fallbackConnection = GetConnection();
        var fallbackResult = await fallbackConnection.QueryAsync<AgendaViewModel>(defensiveQuery, parameters);
        return fallbackResult;
      }
    }

    private AgendaViewModel MapToAgendaViewModel(dynamic row)
    {
      var agenda = new AgendaViewModel();
      // Mapeo de campos básicos
      foreach (var property in typeof(AgendaViewModel).GetProperties())
      {
        var columnName = property.Name.ToLower();
        if (((IDictionary<string, object>)row).ContainsKey(columnName))
        {
          var value = ((IDictionary<string, object>)row)[columnName];
          if (value != null && value != DBNull.Value)
          {
            property.SetValue(agenda, Convert.ChangeType(value, property.PropertyType));
          }
        }
      }
      // Deserialización de JSON para notas y subvisitas
      if (row.notas_json != null)
      {
        agenda.Notas = System.Text.Json.JsonSerializer.Deserialize<List<AgendaNotaViewModel>>(row.notas_json.ToString()) ?? new List<AgendaNotaViewModel>();
      }

      if (row.subvisitas_json != null)
      {
        agenda.Subvisitas = System.Text.Json.JsonSerializer.Deserialize<List<AgendaSubvisitaViewModel>>(row.subvisitas_json.ToString()) ?? new List<AgendaSubvisitaViewModel>();
      }

      return agenda;
    }
  }
}