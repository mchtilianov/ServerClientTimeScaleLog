// See https://aka.ms/new-console-template for more information

using MqttTrace;
using ServerClientTimeScaleLog;

var logClient = new LogClient("test.mosquitto.org", 1883);
await logClient.InitLogClient();
await logClient.Connect();
// Subscribe to as many topics as you'd like just add more logClient.Subscribe_Topic
await logClient.Subscribe_Topic("#", 100);


// Due to the nature of async/await if we don't add an infinite delay at the end of the program, the program exits
await Task.Delay(-1);


