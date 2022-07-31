using EventBus.Base;
using EventBus.RabbitMQ;
using EventBus.RabbitMQ.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ
{
    public static class EventBusFactory
    {
        public static IEventBus Create(EventBusConfig config, IServiceProvider serviceProvider)
        {
            return config.EventBusType switch
            {
                _ => new EventBusRabbitMQ(config, serviceProvider),
            };
        }
    }
}
