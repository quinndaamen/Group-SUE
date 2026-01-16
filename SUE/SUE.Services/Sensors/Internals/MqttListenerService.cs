using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using SUE.Services.Sensors.Contracts;
using SUE.Services.Sensors.Models;

namespace SUE.Services.Sensors.Internals
{
    public class MqttListenerService : BackgroundService
    {
        private readonly ISensorService _sensorService;
        private IMqttClient _client;

        // For now, no real SensorNode
        private static readonly Guid SENSOR_NODE_ID = Guid.Empty;

        public MqttListenerService(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new MqttFactory();
            _client = factory.CreateMqttClient();

            // Define options once
            var mqttOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("broker.hivemq.com", 1883)
                .WithClientId($"sue-{Guid.NewGuid()}")
                .Build();

            // Connected
            _client.ConnectedAsync += async e =>
            {
                Console.WriteLine("✅ Connected to MQTT broker");
                await _client.SubscribeAsync("esp32/sensors");
            };

            // Disconnected
            _client.DisconnectedAsync += async e =>
            {
                Console.WriteLine("❌ Disconnected, reconnecting in 5s...");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                try
                {
                    await _client.ConnectAsync(mqttOptions, stoppingToken); // use mqttOptions
                }
                catch
                {
                    Console.WriteLine("⚠ Reconnect failed, will retry...");
                }
            };

            // Messages
            _client.ApplicationMessageReceivedAsync += async e =>
            {
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine($"📨 Received: {payload}");
            };

            // Connect initially
            await _client.ConnectAsync(mqttOptions, stoppingToken);

            // Keep service alive
            try { await Task.Delay(Timeout.Infinite, stoppingToken); }
            catch (TaskCanceledException) { }
        }

    }
}
