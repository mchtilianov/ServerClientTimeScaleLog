using System.Text;
using MQTTnet;
using MQTTnet.Client;

namespace ServerClientTimeScaleLog;

public class LogClient
{
    IMqttClient _mqttClient;
    MqttFactory _clientFactory;
    MqttClientOptions _mqttClientOptions;
    string _clientId;

    private TimeScaleSqlClient _timeScaleSqlClient;
    
    public LogClient()
    {
        _clientFactory = new MqttFactory();
        _mqttClient = _clientFactory.CreateMqttClient();
        _clientId = "DummyClient" + Guid.NewGuid();
        
        _mqttClientOptions = new MqttClientOptionsBuilder()
            .WithClientId(_clientId)
            .WithTcpServer("localhost")
            .Build();

        _timeScaleSqlClient = new TimeScaleSqlClient();
    }

    public async Task InitLogClient()
    {
        await _timeScaleSqlClient.OpenConnection();
        await _timeScaleSqlClient.CreateMessageLogTable();
        
        // Define the handler for incoming messages
        _mqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            // Decode and log the message
            var message = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            
            await _timeScaleSqlClient.InsertData(e.ApplicationMessage.Topic , message);
        };
    }
    
    public async Task Connect()
    {
        await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);

        Console.WriteLine($"{_clientId} Connected");
    }

    public async Task Disconnect()
    {
        await _timeScaleSqlClient.CloseConnection();
        await _mqttClient.DisconnectAsync();
    }
    
    public async Task Subscribe_Topic()
    {
        var mqttSubscribeOptions = _clientFactory.CreateSubscribeOptionsBuilder().WithTopicFilter("#").Build();

        var response = await _mqttClient.SubscribeAsync(mqttSubscribeOptions);

        Console.WriteLine("MQTT client subscribed to topic.");

        // The response contains additional data sent by the server after subscribing.
        response.DumpToConsole();
    }
}