using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ;

    public class EventBusConfig
    {
        public int ConnectionRetryCount { get; set; } = 5;

        public string DefaultTopicName { get; set; } = "EventBus";
        public string DefaultTopicType { get; set; } = "direct";

        public string EventBusConnectionString { get; set; } = String.Empty;

        public string SubscriberClientAppName { get; set; } = String.Empty;

        public string EventNamePrefix { get; set; } = String.Empty;

        public string EventNameSuffix { get; set; } = "";

        public EventBusType EventBusType { get; set; } = EventBusType.RabbitMQ;

        public object? Connection { get; set; } = null;


        public bool DeleteEventPrefix => !string.IsNullOrEmpty(EventNamePrefix);
        public bool DeleteEventSuffix => !string.IsNullOrEmpty(EventNameSuffix);
    }

    public enum EventBusType
    { 
        RabbitMQ = 0,
        // AzureServiceBus = 1
    }

