using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Conduit.Images.DataAccess;

public class ConnectionProvider : IAsyncDisposable, IDisposable
{
    private readonly Npgsql.NpgsqlConnection _connection;
    private readonly ILogger<ConnectionProvider> _logger;

    public ConnectionProvider(
        IConfiguration configuration,
        ILogger<ConnectionProvider> logger)
    {
        var connectionString = configuration.GetConnectionString("ImageDatabase");
        _connection = new Npgsql.NpgsqlConnection(connectionString);
        _logger = logger;
    }

    public void Dispose()
    {
        _connection.Dispose();
        _logger.LogInformation("Disposing");
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
        _logger.LogInformation("Disposing");
        GC.SuppressFinalize(this);
    }

    public async Task<Npgsql.NpgsqlConnection> ProvideAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Providing");
        await OpenAsync(cancellationToken);
        return _connection;
    }

    private async Task OpenAsync(CancellationToken cancellationToken)
    {
        if (_connection.State != System.Data.ConnectionState.Open)
        {
            _logger.LogInformation("Openning");
            await _connection.OpenAsync(cancellationToken);
        }
    }
}
