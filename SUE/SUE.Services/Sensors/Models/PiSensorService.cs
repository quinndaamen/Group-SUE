using System.Text;
using MQTTnet;
using MQTTnet.Client;

namespace SUE.Services.Sensors.Models;

class PiSensorService
{
    static async Task Main()
    {
        // Create factory & client
        var mqttFactory = new MqttFactory();
        using var mqttClient = mqttFactory.CreateMqttClient();

        // Configure options
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("broker.hivemq.com", 1883)
            .WithClientId($"csharp-client-{Guid.NewGuid()}")
            .Build();

        // When connected
        mqttClient.ConnectedAsync += async e =>
        {
            Console.WriteLine("✅ Connected to MQTT broker");

            // Subscribe after connecting
            await mqttClient.SubscribeAsync("esp32/sensors");
            Console.WriteLine("📡 Subscribed to esp32/sensors");
        };

        // When message received
        mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            var msg = e.ApplicationMessage;
            if (msg is not null && msg.Payload.Length > 0)
            {
                var payloadBytes = msg.Payload.ToArray();
                var payload = Encoding.UTF8.GetString(payloadBytes);
                Console.WriteLine($"📨 Message received on '{msg.Topic}':");
                Console.WriteLine(payload);
            }
            else
            {
                Console.WriteLine($"📨 Empty payload received on '{msg?.Topic}'");
            }

            return Task.CompletedTask;
        };

        // When disconnected
        mqttClient.DisconnectedAsync += e =>
        {
            Console.WriteLine("❌ Disconnected from MQTT broker");
            return Task.CompletedTask;
        };

        // Connect
        await mqttClient.ConnectAsync(options);

        Console.WriteLine("Press ENTER to exit");
        Console.ReadLine();

        await mqttClient.DisconnectAsync();
    }
}