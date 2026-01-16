using System.Collections.Concurrent;

namespace SUE.Services.Sensors.Models
{
    public sealed class MqttMessageStore
    {
        public record MqttMessage(string Topic, string Payload, DateTimeOffset ReceivedAt);

        private readonly ConcurrentQueue<MqttMessage> _queue = new();

        // Keep at most 200 messages
        private const int MaxItems = 200;

        public void Add(string topic, string payload)
        {
            _queue.Enqueue(new MqttMessage(topic, payload, DateTimeOffset.UtcNow));
            while (_queue.Count > MaxItems)
                _queue.TryDequeue(out _);
        }

        // Return newest first
        public IEnumerable<MqttMessage> GetAll() =>
            _queue.ToArray().Reverse();
    }
}