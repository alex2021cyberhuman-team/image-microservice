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
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }

    public async Task<Npgsql.NpgsqlConnection> ProvideAsync(CancellationToken cancellationToken = default)
    {
        if (_connection.State != System.Data.ConnectionState.Open)
        {
            await _connection.OpenAsync(cancellationToken);
        }
        return _connection;
    }
}
