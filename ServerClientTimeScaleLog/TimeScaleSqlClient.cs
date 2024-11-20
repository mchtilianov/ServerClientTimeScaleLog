namespace ServerClientTimeScaleLog;

using Microsoft.Extensions.Logging;
using Npgsql;

public class TimeScaleSqlClient
{
    string _connectionString;
    NpgsqlConnection _connection;

    public TimeScaleSqlClient()
    {
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        NpgsqlLoggingConfiguration.InitializeLogging(factory, parameterLoggingEnabled: true);
        _connectionString = "Host=localhost;Port=5432;Database=timescale_test;User Id=postgres;Password=password;";
        _connection = new NpgsqlConnection(_connectionString);
    }

    public async Task OpenConnection()
    {
        await _connection.OpenAsync();
    }

    public async Task CloseConnection()
    {
        await _connection.CloseAsync();
    }

    public async Task CreateMessageLogTable()
    {
        string ensureExtensionCmd = await File.ReadAllTextAsync("./SqlQueries/EnsureTimeScaleExtension.sql");
        await using NpgsqlCommand cmdEnsureExtension = new NpgsqlCommand(ensureExtensionCmd, _connection);
        await cmdEnsureExtension.ExecuteNonQueryAsync();
        
        string createMessageLogTableCmd = await File.ReadAllTextAsync("./SqlQueries/CreateLogTable.sql");
        await using NpgsqlCommand cmdCreateMessageLogTable = new NpgsqlCommand(createMessageLogTableCmd, _connection);
        await cmdCreateMessageLogTable.ExecuteNonQueryAsync();
    }

    public async Task InsertData(string topic, string message)
    {
        string updateTableCmd = await File.ReadAllTextAsync("./SqlQueries/UpdateTable.sql");
        await using NpgsqlCommand cmdUpdateTable = new NpgsqlCommand(updateTableCmd, _connection);
        
        cmdUpdateTable.Parameters.AddWithValue("topic", topic);
        cmdUpdateTable.Parameters.AddWithValue("message", message);
        
        await cmdUpdateTable.ExecuteNonQueryAsync();
    }
}