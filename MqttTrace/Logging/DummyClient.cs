using MQTTnet;
using MQTTnet.Client;

namespace MqttTrace.Logging;

public class DummyClient
{
    IMqttClient mqttClient;
    MqttFactory clientFactory;
    MqttClientOptions mqttClientOptions;
    string clientId;

    public DummyClient()
    {
        clientFactory = new MqttFactory();
        mqttClient = clientFactory.CreateMqttClient();
        clientId = "DummyClient" + Guid.NewGuid();

        mqttClientOptions = new MqttClientOptionsBuilder()
            .WithClientId(clientId)
            .WithTcpServer("localhost")
            .Build();
    }

    public async Task Connect()
    {
        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        Console.WriteLine($"{clientId} Connected");
    }

    public async Task Publish()
    {
        var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic($"{clientId}/timestamp")
                .WithPayload(DateTime.Now.ToString())
                .Build();
        await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
    }

    public async Task Disconnect()
    {
        await mqttClient.DisconnectAsync();
    }
}