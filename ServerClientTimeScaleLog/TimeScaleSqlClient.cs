namespace ServerClientTimeScaleLog;

using Microsoft.Extensions.Logging;
using Npgsql;

public class TimeScaleSqlClientOptions(
    string? host = "localhost",
    string? database = "timescale_test",
    string? port = "5432",
    string? userId = "postgres",
    string? password = "password")
{
    public string? Host { get; set; } = host;
    public string? Database { get; set; } = database;
    public string? Port { get; set; } = port;
    public string? UserId { get; set; } = userId;
    public string? Password { get; set; } = password;
}

public class TimeScaleSqlClient
{
    string _connectionString;
    NpgsqlConnection _connection;

    public TimeScaleSqlClient(string host = "localhost", string database = "timescale_test", string port = "5432",
                              string userId = "postgres", string password = "password")
    {
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        NpgsqlLoggingConfiguration.InitializeLogging(factory, parameterLoggingEnabled: true);
        _connectionString = $"Host={host};Port={port};Database={database};User Id={userId};Password={password};";
        _connection = new NpgsqlConnection(_connectionString);
    }
    
    public TimeScaleSqlClient(TimeScaleSqlClientOptions options)
    {
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        NpgsqlLoggingConfiguration.InitializeLogging(factory, parameterLoggingEnabled: true);
        _connectionString = $"Host={options.Host};Port={options.Port};Database={options.Database};User Id={options.UserId};Password={options.Password};";
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

    public async Task InsertData(string topic, byte[] message)
    {
        string updateTableCmd = await File.ReadAllTextAsync("./SqlQueries/UpdateTable.sql");
        await using NpgsqlCommand cmdUpdateTable = new NpgsqlCommand(updateTableCmd, _connection);
        
        cmdUpdateTable.Parameters.AddWithValue("topic", topic);
        cmdUpdateTable.Parameters.AddWithValue("message", message);
        
        Console.WriteLine(await cmdUpdateTable.ExecuteNonQueryAsync());
    }
}