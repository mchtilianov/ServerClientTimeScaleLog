namespace ServerClientTimeScaleLog;

using Microsoft.Extensions.Logging;
using Npgsql;

public class TimeScaleSqlClient
{
    string connectionString;
    private NpgsqlConnection connection;

    public TimeScaleSqlClient()
    {
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        NpgsqlLoggingConfiguration.InitializeLogging(factory, parameterLoggingEnabled: true);
        connectionString = "Host=localhost;Port=5432;Database=timescale_test;User Id=postgres;Password=password;";
        connection = new NpgsqlConnection(connectionString);
    }

    public void OpenConnection()
    {
        connection.Open();
    }

    public void CloseConnection()
    {
        connection.Close();
    }

    public void CreateMessageLogTable()
    {
        string ensureExtensionCmd = File.ReadAllText("/home/martin/RiderProjects/ServerClientTimeScaleLog/ServerClientTimeScaleLog/SqlQueries/EnsureTimeScaleExtension.sql");
        using NpgsqlCommand cmdEnsureExtension = new NpgsqlCommand(ensureExtensionCmd, connection);
        cmdEnsureExtension.ExecuteNonQuery();
        
        string createMessageLogTableCmd = File.ReadAllText("/home/martin/RiderProjects/ServerClientTimeScaleLog/ServerClientTimeScaleLog/SqlQueries/CreateLogTable.sql");
        using NpgsqlCommand cmdCreateMessageLogTable = new NpgsqlCommand(createMessageLogTableCmd, connection);
        cmdCreateMessageLogTable.ExecuteNonQuery();
    }

    public void InsertData(string topic, string message)
    {
        string updateTableCmd = File.ReadAllText("/home/martin/RiderProjects/ServerClientTimeScaleLog/ServerClientTimeScaleLog/SqlQueries/UpdateTable.sql");
        using NpgsqlCommand cmdUpdateTable = new NpgsqlCommand(updateTableCmd, connection);
        
        cmdUpdateTable.Parameters.AddWithValue("topic", topic);
        cmdUpdateTable.Parameters.AddWithValue("message", message);

        
        Console.WriteLine(cmdUpdateTable.CommandText + " --- " + topic + " -- " + message);
        foreach (NpgsqlParameter parameter in cmdUpdateTable.Parameters)
        {
            Console.WriteLine($"Parameter: {parameter.ParameterName}, Value: {parameter.Value}");
        }
        
        Console.WriteLine(cmdUpdateTable.ExecuteNonQuery());
    }
}