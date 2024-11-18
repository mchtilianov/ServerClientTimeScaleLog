// See https://aka.ms/new-console-template for more information

using ServerClientTimeScaleLog;

Console.WriteLine("Hello, World!");

var server = new Server();
await server.StartServer();

Console.WriteLine("We're starting up...");

List<DummyClient> clients = new List<DummyClient>();
for (int i = 0; i < 10; i++)
{
    clients.Add(new DummyClient());
    await clients[i].Connect();
}

var logClient = new LogClient();
await logClient.Connect();
await logClient.Subscribe_Topic();

while (true)
{
    for (int i = 0; i < 10; i++)
    {
        await clients[i].Publish();
    }
    await Task.Delay(500);
}