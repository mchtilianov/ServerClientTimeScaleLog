namespace ServerClientTimeScaleLog;

using MQTTnet.Diagnostics;
using MQTTnet;
using MQTTnet.Server;

public class Server
{
    private MqttServerOptions _mqttServerOptions;
    private MqttFactory _mqttServerFactory;
    private MqttServer _mqttServer;

    //Default Port = 1883
    public Server(int port = 1883)
    {
        _mqttServerOptions = new MqttServerOptionsBuilder().WithDefaultEndpointPort(port).WithDefaultEndpoint().Build();
        _mqttServerFactory = new MqttFactory();
        _mqttServer = _mqttServerFactory.CreateMqttServer(_mqttServerOptions);
    }
    
    public async Task StartServer()
    {
        await _mqttServer.StartAsync();
    }
}