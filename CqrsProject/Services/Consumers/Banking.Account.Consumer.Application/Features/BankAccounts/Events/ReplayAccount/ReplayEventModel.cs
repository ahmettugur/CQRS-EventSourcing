namespace Banking.Account.Consumer.Kafka.Application.Features.BankAccounts.Events.ReplayAccount
{
    public class ReplayEventModel
    {
        public string Id { get; set; } = string.Empty;
        public DateTime CreatedDate => DateTime.Now;
        public DateTime Timestamp { get; set; }
        public string AggregateIdentifier { get; set; } = string.Empty;
        public string AggregateType { get; set; } = string.Empty;
        public int Version { get; set; }
        public string EventType { get; set; } = string.Empty;
        public object? EventData { get; set; }
    }
}
