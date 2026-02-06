using NpgsqlTypes;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL.ColumnWriters;

namespace PulseApp.Logging;

/// <summary>
/// Создание GUID записи лога
/// </summary>
public class GuidColumnWriter : ColumnWriterBase
{
    public GuidColumnWriter(NpgsqlDbType dbType, bool skipOnInsert = false, int? order = null) : base(dbType, skipOnInsert, order)
    {
    }

    /// <inheritdoc/>
    public override object? GetValue(LogEvent logEvent, IFormatProvider? formatProvider = null)
    {
        return Guid.NewGuid();
    }
}
