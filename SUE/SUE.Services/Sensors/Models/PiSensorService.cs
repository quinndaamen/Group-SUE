using MQTTnet.Client;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Protocol;
using SUE.Services.Sensors.Contracts;
using SUE.Services.Sensors.Models;

class Program
{
    // Replace with your actual SensorNode GUID from DB
    private static readonly Guid SENSOR_NODE_ID = Guid.Parse("PUT-YOUR-SENSORNODE-GUID-HERE");

    static async Task Main()
    {
        // 1️⃣ Setup DI to resolve your SensorService
        var services = new ServiceCollection();
        services.AddScoped<ISensorService, SensorService>(); // Your implementation
        var serviceProvider = services.BuildServiceProvider();
        var sensorService = serviceProvider.GetRequiredService<ISensorService>();

        // 2️⃣ Create MQTT client
        var mqttFactory = new MqttFactory();
        var mqttClient = mqttFactory.CreateMqttClient();

        // 3️⃣ Handle connection
        mqttClient.UseConnectedHandler(async e =>
        {
            Console.WriteLine("✅ Connected to MQTT broker");

            await mqttClient.SubscribeAsync(new MQTTnet.Client.Subscribing.MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter("esp32/sensors", MqttQualityOfServiceLevel.AtLeastOnce)
                .Build());

            Console.WriteLine("📡 Subscribed to esp32/sensors");
        });

        // 4️⃣ Handle incoming messages
        mqttClient.UseApplicationMessageReceivedHandler(async e =>
        {
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

            var measurement = RawSensorParser.Parse(payload); // Your parser
            if (measurement == null) return;

            await sensorService.AddMeasurementAsync(SENSOR_NODE_ID, measurement);
            Console.WriteLine($"💾 Measurement saved to DB: Temp={measurement.Temperature}, Humidity={measurement.Humidity}");
        });

        // 5️⃣ Handle disconnection
        mqttClient.UseDisconnectedHandler(e =>
        {
            Console.WriteLine("❌ Disconnected from MQTT broker");
        });

        // 6️⃣ Connect to broker
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("broker.hivemq.com", 1883)
            .WithClientId($"sue-{Guid.NewGuid()}")
            .Build();

        await mqttClient.ConnectAsync(options);
        Console.WriteLine("Press ENTER to exit");
        Console.ReadLine();

        await mqttClient.DisconnectAsync();
    }
}
