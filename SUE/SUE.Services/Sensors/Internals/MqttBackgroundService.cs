using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using SUE.Services.Sensors.Contracts;
using SUE.Services.Sensors.Models;

namespace SUE.Services.Sensors.Internals;

public class MqttBackgroundService : BackgroundService
    {
        private readonly MqttMessageStore _store;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<MqttBackgroundService> _logger;
        private IMqttClient? _client;

        private static readonly Guid SENSOR_NODE_ID = Guid.Empty;

        public MqttBackgroundService(
            MqttMessageStore store,
            IServiceScopeFactory scopeFactory,
            ILogger<MqttBackgroundService> logger)
        {
            _store = store;
            _scopeFactory = scopeFactory;
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

            _client.ApplicationMessageReceivedAsync += async e =>
            {
                try
                {
                    var payloadBytes = e.ApplicationMessage?.Payload;
                    if (payloadBytes == null || payloadBytes.Length == 0)
                        return;

                    var payload = Encoding.UTF8.GetString(payloadBytes);

                    _store.Add(e.ApplicationMessage!.Topic, payload);

                    var measurement = RawSensorParser.Parse(payload);
                    if (measurement == null)
                        return;

                    // 🔑 Create scope manually
                    using var scope = _scopeFactory.CreateScope();
                    var sensorService = scope.ServiceProvider
                        .GetRequiredService<ISensorService>();

                    await sensorService.SaveMeasurementAsync(
                        SENSOR_NODE_ID,
                        measurement
                    );

                    _logger.LogInformation(
                        "Sensor measurement saved at {Timestamp}",
                        measurement.Timestamp
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error handling MQTT message");
                }
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