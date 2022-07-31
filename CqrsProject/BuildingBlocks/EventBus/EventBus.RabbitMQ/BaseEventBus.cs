using EventBus.Base;
using EventBus.RabbitMQ.Abstraction;
using EventBus.RabbitMQ.SubManagers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ;

public abstract class BaseEventBus : IEventBus, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    protected readonly IEventBusSubscriptionManager SubsManager;

    protected EventBusConfig EventBusConfig { get; private set; }

    protected BaseEventBus(EventBusConfig config, IServiceProvider serviceProvider)
    {
        EventBusConfig = config;
        _serviceProvider = serviceProvider;
        SubsManager = new InMemoryEventBusSubscriptionManager(ProcessEventName);
    }

    protected virtual string ProcessEventName(string eventName)
    {
        if (EventBusConfig.DeleteEventPrefix)
            eventName = eventName.TrimStart(EventBusConfig.EventNamePrefix.ToArray());

        if (EventBusConfig.DeleteEventSuffix)
            eventName = eventName.TrimEnd(EventBusConfig.EventNameSuffix.ToArray());

        return eventName;
    }

    protected virtual string GetSubName(string eventName)
    {
        return $"{EventBusConfig.SubscriberClientAppName}.{ProcessEventName(eventName)}";
    }

    public virtual void Dispose()
    {
        EventBusConfig = null!;
        SubsManager.Clear();
    }

    protected async Task<bool> ProcessEvent(string eventName, string message)
    {
        eventName = ProcessEventName(eventName);

        var processed = false;

        if (SubsManager.HasSubscriptionsForEvent(eventName))
        {
            var subscriptions = SubsManager.GetHandlersForEvent(eventName);

            using (var scope = _serviceProvider.CreateScope())
            {
                foreach (var subscription in subscriptions)
                {
                    var handler = _serviceProvider.GetService(subscription.HandlerType);
                    if (handler == null) continue;

                    var eventType =
                        SubsManager.GetEventTypeByName(
                            $"{EventBusConfig.EventNamePrefix}{eventName}{EventBusConfig.EventNameSuffix}");
                    var integrationEvent = JsonConvert.DeserializeObject(message, eventType);

                    var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                    await ((Task)concreteType.GetMethod("Handle")!.Invoke(handler, new object?[] { integrationEvent, null })!)!;
                }
            }

            processed = true;
        }

        return processed;
    }


    public abstract void Publish(Event @event, string topic = "");

    public abstract void Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>;

    public abstract void UnSubscribe<T, TH>() where T : Event where TH : IEventHandler<T>;
}