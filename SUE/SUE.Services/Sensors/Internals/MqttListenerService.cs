using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Protocol;
using SUE.Services.Sensors.Contracts;
using SUE.Services.Sensors.Models;

namespace SUE.Services.Sensors.Internals
{
    public class MqttListenerService : BackgroundService
    {
        private readonly ISensorService _sensorService;
        private IMqttClient _client;

        // Replace with your SensorNode ID in DB
        private static readonly Guid SENSOR_NODE_ID =
            Guid.Parse("PUT-YOUR-SENSORNODE-GUID-HERE");

        public MqttListenerService(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new MqttFactory();
            _client = factory.CreateMqttClient();

            // Message received handler
            _client.UseApplicationMessageReceivedHandler(async e =>
            {
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                var measurement = RawSensorParser.Parse(payload);
                if (measurement != null)
                    await _sensorService.AddMeasurementAsync(SENSOR_NODE_ID, measurement);
            });

            // Connected handler
            _client.UseConnectedHandler(async e =>
            {
                Console.WriteLine("✅ Connected to MQTT broker");

                await _client.SubscribeAsync(new MQTTnet.Client.Subscribing.MqttClientSubscribeOptionsBuilder()
                    .WithTopicFilter("esp32/sensors", MqttQualityOfServiceLevel.AtLeastOnce)
                    .Build());

                Console.WriteLine("📡 Subscribed to esp32/sensors");
            });

            // Disconnected handler
            _client.UseDisconnectedHandler(e =>
            {
                Console.WriteLine("❌ Disconnected from MQTT broker");
            });

            // Client options
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("broker.hivemq.com", 1883)
                .WithClientId($"sue-{Guid.NewGuid()}")
                .Build();

            // Connect
            await _client.ConnectAsync(options, stoppingToken);

            // Keep running until service is stopped
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
