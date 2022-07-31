using EventBus.Base;
using System;
using System.Collections.Generic;

namespace EventBus.RabbitMQ.Abstraction;

public interface IEventBusSubscriptionManager
{
    bool IsEmpty { get; }

    event EventHandler<string> OnEventRemoved;

    void AddSubscription<T, TH>() where T : Event where TH : IEventHandler<T>;

    void RemoveSubscription<T, TH>() where TH : IEventHandler<T> where T : Event;

    bool HasSubscriptionsForEvent<T>() where T : Event;

    bool HasSubscriptionsForEvent(string eventName);

    Type GetEventTypeByName(string eventName);

    void Clear();

    IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : Event;

    IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

    string GetEventKey<T>();
}