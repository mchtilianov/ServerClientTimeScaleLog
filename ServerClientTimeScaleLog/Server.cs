using MQTTnet.Diagnostics;
using MQTTnet.Server;

namespace ServerClientTimeScaleLog;

using MQTTnet;
using MQTTnet.Server;

public class Server
{
    private MqttServerOptions mqttServerOptions;
    private MqttFactory mqttServerFactory;
    private MqttServer mqttServer;
    public async Task StartServer()
    {
        /*
         * This sample starts a simple MQTT server which will accept any TCP connection.
         */

        //mqttServerFactory = new MqttFactory(new ConsoleLogger());
        mqttServerFactory = new MqttFactory();

        // The port for the default endpoint is 1883.
        // The default endpoint is NOT encrypted!
        // Use the builder classes where possible.
        mqttServerOptions = new MqttServerOptionsBuilder().WithDefaultEndpoint().Build();

        // The port can be changed using the following API (not used in this example).
        // new MqttServerOptionsBuilder()
        //     .WithDefaultEndpoint()
        //     .WithDefaultEndpointPort(1234)
        //     .Build();

        mqttServer = mqttServerFactory.CreateMqttServer(mqttServerOptions);
        await mqttServer.StartAsync();
        

        // Infinite delay
        //await Task.Delay(-1);
        // Stop and dispose the MQTT server if it is no longer needed!
        //await mqttServer.StopAsync();
    }
}


class ConsoleLogger : IMqttNetLogger
{
    readonly object _consoleSyncRoot = new();

    public bool IsEnabled => true;

    public void Publish(MqttNetLogLevel logLevel, string source, string message, object[]? parameters, Exception? exception)
    {
        var foregroundColor = ConsoleColor.White;
        switch (logLevel)
        {
            case MqttNetLogLevel.Verbose:
                foregroundColor = ConsoleColor.White;
                break;

            case MqttNetLogLevel.Info:
                foregroundColor = ConsoleColor.Green;
                break;

            case MqttNetLogLevel.Warning:
                foregroundColor = ConsoleColor.DarkYellow;
                break;

            case MqttNetLogLevel.Error:
                foregroundColor = ConsoleColor.Red;
                break;
        }

        if (parameters?.Length > 0)
        {
            message = string.Format(message, parameters);
        }

        lock (_consoleSyncRoot)
        {
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(message);

            if (exception != null)
            {
                Console.WriteLine(exception);
            }
        }
    }
}