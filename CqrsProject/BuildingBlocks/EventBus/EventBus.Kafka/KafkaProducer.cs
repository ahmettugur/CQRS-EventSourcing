using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EventBus.Kafka.Contracts;
using EventBus.Kafka.Events;
using EventBus.Base;
using Banking.Cqrs.Core.Contracts;

namespace EventBus.Kafka;

public class KafkaProducer : IBaseEventBus
{
    //private readonly ILog _log;
    private readonly IProducer<Null, string> _producer;
    private readonly KafkaEventProducerConfiguration _options;

    public KafkaProducer(IOptions<KafkaEventProducerConfiguration> options)
    {
        _options = options.Value;
        var config = new ProducerConfig
        {
            BootstrapServers = _options.Server,
            MessageSendMaxRetries = _options.MaxRetries
            // SaslMechanism = SaslMechanism.ScramSha256,
            // SecurityProtocol = SecurityProtocol.SaslSsl,
            // SaslUsername = "",
            // SaslPassword = ""

        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
 
    }

    public void Publish(Event @event, string topic="")
    {
        if (@event == null) throw new ArgumentNullException(nameof(@event));
        if (string.IsNullOrEmpty(topic)) throw new ArgumentNullException(nameof(topic));

        var stringMessage = GetMessage(@event);
        var message = new Message<Null,string> {Value = stringMessage};
        var result = _producer.ProduceAsync(topic, message).GetAwaiter().GetResult();
    }

    private string GetMessage(Event @event)
    {
        var attribute = @event.GetType().GetCustomAttribute<EventAttribute>();
        if (attribute == null)
        {
            throw new ArgumentException($"{nameof(EventAttribute)} missing on {nameof(@event)}");
        }

        if (string.IsNullOrEmpty(attribute.Name))
        {
            throw new ArgumentNullException(
                $"{nameof(EventAttribute)}.Name missing on {nameof(@event)}");
        }

        var message = new KMessage
        {
            Name = attribute.Name,
            EventData = JsonConvert.SerializeObject(@event, _options.SerializerSettings)
        };

        return JsonConvert.SerializeObject(message, _options.SerializerSettings);
    }
}