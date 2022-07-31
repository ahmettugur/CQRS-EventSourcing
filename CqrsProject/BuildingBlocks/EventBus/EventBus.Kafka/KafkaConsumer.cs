using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using EventBus.Kafka.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using EventBus.Base;

namespace EventBus.Kafka;

public sealed class KafkaConsumer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly KafkaEventConsumerConfiguration _options;
    private readonly ILogger<KafkaConsumer> _logger;

    public KafkaConsumer(IOptions<KafkaEventConsumerConfiguration> options, IServiceProvider serviceProvider, ILogger<KafkaConsumer> logger)
    {
        _options = options.Value;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = _options.GroupId,
            BootstrapServers = _options.Server,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            AllowAutoCreateTopics = true,
            EnableAutoOffsetStore = false,
            EnableAutoCommit = false,
            // SaslMechanism = SaslMechanism.ScramSha256,
            // SecurityProtocol = SecurityProtocol.SaslSsl,
            // SaslUsername = "",
            // SaslPassword = ""
        };

        using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
        {
            consumerBuilder.Subscribe(_options.Topics);
            var cancelToken = new CancellationTokenSource();
  
            
            while (!cancellationToken.IsCancellationRequested)
            {
                var kMessage = default(KMessage);
                try
                {
                    var result = consumerBuilder.Consume(cancelToken.Token);
                    kMessage = ParseAsKMessage(result.Message);
                    
                    var subscriptionInfo = GetHandlerType(kMessage!)!;
                    using var scope = _serviceProvider.CreateScope();
                    var handler = GetEventHandler(scope,subscriptionInfo);
                    var eventType = subscriptionInfo.EventType!;
                    var eventData = GetEventData(kMessage!.EventData, eventType);
                    
                    var concreteType = MakeGenericType(eventType);
                    await (Task)concreteType.GetMethod("Handle")!.Invoke(handler, new [] { eventData! ,cancellationToken})!;
                    
                    consumerBuilder.Commit(result);
                    consumerBuilder.StoreOffset(result);
                }
                catch (OperationCanceledException)
                {
                    consumerBuilder.Close();
                }
                catch (Exception ex)
                {
                    if (kMessage != null)
                    {
                        ex.Data.Add("IntegrationMessage", kMessage);
                        _logger.LogInformation("IntegrationMessage Error {IntegrationMessage}",ex);
                    }
                }
                
            }
            
            consumerBuilder.Close();

      
        }

        await Task.CompletedTask;
    }

    private static Type MakeGenericType(Type eventType)
    {
        var genericType = typeof(IEventHandler<>).MakeGenericType(eventType);
        return genericType;
    }

    private static object? GetEventHandler(IServiceScope scope, SubscriptionInfo subscriptionInfo)
    {
        var handler = scope.ServiceProvider.GetService(subscriptionInfo.HandlerType!)!;
        return handler;
    }
    private static object? GetEventData(string eventJson, Type eventType)
    {
        var eventData = JsonConvert.DeserializeObject(eventJson, eventType)!;
        return eventData;
    }

    private static KMessage? ParseAsKMessage(Message<Ignore, string> message)
    {
        var kMessage = JsonConvert.DeserializeObject<KMessage>(message.Value);
        return kMessage;
    }
        

    private SubscriptionInfo? GetHandlerType(KMessage message)
    {
        if (message.Name == null)
        {
            throw new ArgumentNullException($"{nameof(KafkaConsumer)} exception: event Name is missing");
        }

        return _options.Subscriptions.TryGetValue(message.Name, out var handlerType) ? handlerType : null;
    }
    
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}