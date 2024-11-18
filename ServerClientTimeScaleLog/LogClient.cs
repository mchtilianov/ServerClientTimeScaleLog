using System.Text;
using MQTTnet;
using MQTTnet.Client;

namespace ServerClientTimeScaleLog;

public class LogClient
{
    IMqttClient mqttClient;
    MqttFactory clientFactory;
    MqttClientOptions mqttClientOptions;
    string clientId;
    
    TimeScaleSqlClient timeScaleSqlClient;
    

    public LogClient()
    {
        clientFactory = new MqttFactory();
        mqttClient = clientFactory.CreateMqttClient();
        clientId = "DummyClient" + Guid.NewGuid();

        timeScaleSqlClient = new TimeScaleSqlClient();
        timeScaleSqlClient.OpenConnection();
        timeScaleSqlClient.CreateMessageLogTable();
        
        mqttClientOptions = new MqttClientOptionsBuilder()
            .WithClientId(clientId)
            .WithTcpServer("localhost")
            .Build();
        
        // Define the handler for incoming messages
        mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            // Decode and log the message
            var message = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            
            timeScaleSqlClient.InsertData(e.ApplicationMessage.Topic , message);
            //Console.WriteLine($"Topic: {e.ApplicationMessage.Topic}");
            //Console.WriteLine($"Message: {message}");
            //Console.WriteLine("------------------------------");
            return Task.CompletedTask;
        };
    }

    public async Task Connect()
    {
        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        Console.WriteLine($"{clientId} Connected");
    }

    public async Task Disconnect()
    {
        timeScaleSqlClient.CloseConnection();
        await mqttClient.DisconnectAsync();
    }
    
    public async Task Subscribe_Topic()
    {
        var mqttSubscribeOptions = clientFactory.CreateSubscribeOptionsBuilder().WithTopicFilter("#").Build();

        var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions);

        Console.WriteLine("MQTT client subscribed to topic.");

        // The response contains additional data sent by the server after subscribing.
        response.DumpToConsole();
    }
}