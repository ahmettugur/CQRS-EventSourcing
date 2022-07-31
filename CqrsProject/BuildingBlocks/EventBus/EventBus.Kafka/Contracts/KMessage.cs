namespace EventBus.Kafka.Contracts;

public class KMessage
{
    public string Name { get; set; } = null!;
    public string EventData { get; set; } = null!;
}