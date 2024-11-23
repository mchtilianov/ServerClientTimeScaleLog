using MQTTnet;
using MQTTnet.Client;

namespace ServerClientTimeScaleLog;

public class LogClient
{
    IMqttClient _mqttClient;
    MqttFactory _clientFactory;
    MqttClientOptions _mqttClientOptions;
    string _clientId;
    List<string> _topics;
    Dictionary<string, int> _topicToUnsubscribeCount;
    Dictionary<string, int> _topicMessageCount;

    private TimeScaleSqlClient _timeScaleSqlClient;
    
    public LogClient(string tcpUrl, int tcpPort)
    {
        _clientFactory = new MqttFactory();
        _mqttClient = _clientFactory.CreateMqttClient();
        _clientId = "DummyClient" + Guid.NewGuid();
        
        _mqttClientOptions = new MqttClientOptionsBuilder()
            .WithClientId(_clientId)
            .WithTcpServer(tcpUrl, tcpPort)
            .Build();

        _timeScaleSqlClient = new TimeScaleSqlClient();
        
        _topics = new List<string>();
        _topicToUnsubscribeCount = new Dictionary<string, int>();
        _topicMessageCount = new Dictionary<string, int>();
    }
    
    public LogClient(string tcpUrl, int tcpPort, TimeScaleSqlClientOptions options)
    {
        _clientFactory = new MqttFactory();
        _mqttClient = _clientFactory.CreateMqttClient();
        _clientId = "DummyClient" + Guid.NewGuid();
        
        _mqttClientOptions = new MqttClientOptionsBuilder()
            .WithClientId(_clientId)
            .WithTcpServer(tcpUrl, tcpPort)
            .Build();

        _timeScaleSqlClient = new TimeScaleSqlClient(options);
        
        _topics = new List<string>();
        _topicToUnsubscribeCount = new Dictionary<string, int>();
        _topicMessageCount = new Dictionary<string, int>();
    }

    public async Task InitLogClient()
    {
        await _timeScaleSqlClient.OpenConnection();
        await _timeScaleSqlClient.CreateMessageLogTable();
        
        // Define the handler for incoming messages
        _mqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            string matchedTopic = "";
            
            // Find matching wildcard topic
            foreach (var wildcardTopic in _topics)
            {
                if (IsMatch(wildcardTopic, e.ApplicationMessage.Topic))
                {
                    matchedTopic = wildcardTopic;
                    
                    Console.WriteLine($"Matched {e.ApplicationMessage.Topic} with {wildcardTopic} <---> {_topicMessageCount[wildcardTopic]}  <---> {_topicToUnsubscribeCount[wildcardTopic]}");
                    
                    _topicMessageCount[wildcardTopic] += 1;
                    break;
                }
            }
            
            if (_topicToUnsubscribeCount[matchedTopic] != -1 && 
                (_topicMessageCount[matchedTopic] - 1) >= _topicToUnsubscribeCount[matchedTopic])
            {
                await Unsubscribe_Topic(matchedTopic);
            }
            else
            {
                var message = e.ApplicationMessage.PayloadSegment.ToArray();

                message.DumpToConsole();
            
                await _timeScaleSqlClient.InsertData(e.ApplicationMessage.Topic , message);   
            }
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
    
    public async Task Subscribe_Topic(string topic, int unsubscribeAfter = -1)
    {
        var mqttSubscribeOptions = _clientFactory.CreateSubscribeOptionsBuilder().WithTopicFilter(topic).Build();

        var response = await _mqttClient.SubscribeAsync(mqttSubscribeOptions);

        Console.WriteLine($"MQTT client subscribed to {topic}.");
        
        _topics.Add(topic);
        _topicToUnsubscribeCount.Add(topic, unsubscribeAfter);
        _topicMessageCount.Add(topic, 0);

        // The response contains additional data sent by the server after subscribing.
        response.DumpToConsole();
    }
    
    // In case you have a bunch of topics where you don't care about automatically unsubscribing from
    public async Task Subscribe_Topics(List<string> topics)
    {
        foreach (var topic in topics)
        {
            await Subscribe_Topic(topic);
        }
    }
    
    // In case you care about automatically unsubscribing from a bunch of topics
    public async Task Subscribe_Topics(List<(string, int)> topics)
    {
        foreach (var topic in topics)
        {
            await Subscribe_Topic(topic.Item1, topic.Item2);
        }
    }
    
    // Manual unsubscribing also exposed to users
    public async Task<bool> Unsubscribe_Topic(string topic)
    {
        if (_topics.Contains(topic))
        {
            await _mqttClient.UnsubscribeAsync(topic);
            
            _topics.Remove(topic);
            _topicMessageCount.Remove(topic);
            _topicToUnsubscribeCount.Remove(topic);
            
            Console.WriteLine($"MQTT client unsubscribed to {topic}.");
            
            return true;
        }
        return false;
    }
    
    private static bool IsMatch(string wildcardTopic, string actualTopic)
    {
        string[] wildcardLevels = wildcardTopic.Split('/');
        string[] topicLevels = actualTopic.Split('/');

        int i = 0;
        for (; i < wildcardLevels.Length; i++)
        {
            if (wildcardLevels[i] == "#")
            {
                return true;
            }
            if (i >= topicLevels.Length || (wildcardLevels[i] != "+" && wildcardLevels[i] != topicLevels[i]))
            {
                return false;
            }
        }

        return i == topicLevels.Length;
    }
}