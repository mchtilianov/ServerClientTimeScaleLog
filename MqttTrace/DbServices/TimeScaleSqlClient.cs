using MqttTrace.DTOs.Proportioning;
using MqttTrace.DTOs.Tipping;

namespace MqttTrace.DbServices;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;
using Proton.Dto;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

public class TimeScaleSqlClientOptions(
    string? host = "chiloff.com",
    string? database = "timescale_test",
    string? port = "15432",
    string? userId = "postgres",
    string? password = "Pa333556580rd")
{
    public string? Host { get; set; } = host;
    public string? Database { get; set; } = database;
    public string? Port { get; set; } = port;
    public string? UserId { get; set; } = userId;
    public string? Password { get; set; } = password;
}

public class TimeScaleSqlClient
{
    string libraryPath = Assembly.GetExecutingAssembly().Location;
    string _connectionString;
    NpgsqlConnection _connection;

    //Host=chiloff.com;Port=15432;Database=timescale_test;User Id=postgres;Password=Pa333556580rd;
    public TimeScaleSqlClient(string host = "chiloff.com", string database = "timescale_test", string port = "15432",
                              string userId = "postgres", string password = "Pa333556580rd")
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
        Console.WriteLine(libraryPath);
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

    public async Task SellectAllData()
    {
        string updateTableCmd = await File.ReadAllTextAsync("./SqlQueries/SelectAllMessages.sql");
        using NpgsqlCommand command = new NpgsqlCommand(updateTableCmd, _connection);
        
        DtoParser dtoParser = DtoParser.Instance;

        NpgsqlDataReader reader = await command.ExecuteReaderAsync();
        int i = 0;
        while (await reader.ReadAsync())
        {
            Console.WriteLine(reader[0]);
            //Console.WriteLine("------------");
            Console.WriteLine(reader[1]);
            //Console.WriteLine("------------");
            var a = reader[2] as byte[];
            // Console.WriteLine(Payload.Parser.ParseFrom(a));
            // var b = Payload.Parser.ParseFrom(a).GetType().GetProperties();
            string jsonString = System.Text.Encoding.Default.GetString(reader[2] as byte[] ?? []);
            dynamic json = JsonConvert.DeserializeObject(jsonString);
            Console.WriteLine(json["Payload"]);
            //TODO: figure out how to parse data based on topic (using DtoParser)
            ProportioningExecutionDto dto= JsonConvert.DeserializeObject<ProportioningExecutionDto>(json["Payload"].ToString());
            Console.WriteLine("----------------------------------------");
        }
    }
    
    
}
