#nullable enable

using Dapper;
using Newtonsoft.Json;
using System;
using System.Data;

public class JsonTypeHandler<T> : SqlMapper.TypeHandler<T>
{
    public override void SetValue(IDbDataParameter parameter, T? value)
    {
        parameter.Value = value != null
            ? JsonConvert.SerializeObject(value)
            : DBNull.Value;
    }

    public override T? Parse(object value)
    {
        if (value is string json && !string.IsNullOrWhiteSpace(json))
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonException)
            {
                // Logueo opcional aquí
                return default;
            }
        }

        return default;
    }
}
