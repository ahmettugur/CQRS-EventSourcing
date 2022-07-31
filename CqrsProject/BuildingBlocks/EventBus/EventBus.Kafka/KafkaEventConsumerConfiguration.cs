using System;
using System.Collections.Generic;
using System.Reflection;
using Banking.Cqrs.Core.Contracts;
using EventBus.Base;
using EventBus.Kafka.Contracts;
using EventBus.Kafka.Events;

namespace EventBus.Kafka;


public class SubscriptionInfo
{
    public Type? HandlerType { get; set; }
    public Type? EventType { get; set; }
}
public class KafkaEventConsumerConfiguration
{
    public IDictionary<string, SubscriptionInfo> Subscriptions { get; set; } = new Dictionary<string, SubscriptionInfo>();
    public List<string>? Topics { get; set; }
    public string? GroupId { get; set; }
    public string? Server { get; set; }
    public TimeSpan Timeout { get; set; }

    public KafkaEventConsumerConfiguration RegisterConsumer<TEvent, TEventHandler>()
        where TEvent : Event
        where TEventHandler : IEventHandler<TEvent>
    {
        var eventName = typeof(TEvent).GetCustomAttribute<EventAttribute>()?.Name;
        if (string.IsNullOrEmpty(eventName))
        {
            throw new InvalidOperationException($"{nameof(EventAttribute)} missing on {typeof(TEvent).Name}");
        }

        var info = new SubscriptionInfo
        {
            HandlerType = typeof(TEventHandler),
            EventType = typeof(TEvent)
        };
        Subscriptions[eventName] = info;
        

        return this;
    }
}