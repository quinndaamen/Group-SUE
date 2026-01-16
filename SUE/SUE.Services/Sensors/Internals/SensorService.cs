using System;
using System.Text;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using SUE.Services.Sensors.Models;

namespace SUE.Services.Sensors
{
    public class MqttBackgroundService : BackgroundService
    {
        private readonly MqttMessageStore _store;
        private readonly ILogger<MqttBackgroundService> _logger;
        private IMqttClient? _client;

        public MqttBackgroundService(MqttMessageStore store, ILogger<MqttBackgroundService> logger)
        {
            _store = store;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new MqttFactory();
            _client = factory.CreateMqttClient();

            // Use the event-based handlers (matching IMqttClient API available in the project)
            _client.ConnectedAsync += async _ =>
            {
                _logger.LogInformation("Connected to MQTT broker, subscribing...");
                // subscribe after connected
                await _client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("esp32/sensors").Build());
            };

            _client.ApplicationMessageReceivedAsync += e =>
            {
                try
                {
                    var topic = e.ApplicationMessage?.Topic ?? "<no-topic>";
                    var payload = string.Empty;

                    // Handle payload as byte[] (some MQTTnet versions expose byte[])
                    var payloadBytes = e.ApplicationMessage?.Payload;
                    if (payloadBytes != null && payloadBytes.Length > 0)
                    {
                        payload = Encoding.UTF8.GetString(payloadBytes);
                    }

                    _store.Add(topic, payload);
                    _logger.LogDebug("MQTT message received on {Topic}", topic);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error handling MQTT message");
                }

                return Task.CompletedTask;
            };

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("broker.hivemq.com", 1883)
                .WithClientId($"webapp-client-{Guid.NewGuid()}")
                .Build();

            try
            {
                await _client.ConnectAsync(options, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MQTT connect failed");
            }

            try
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (TaskCanceledException) { /* shutdown */ }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_client != null && _client.IsConnected)
                    await _client.DisconnectAsync();
            }
            catch { /* ignore */ }

            await base.StopAsync(cancellationToken);
        }
    }
}