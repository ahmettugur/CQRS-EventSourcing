namespace Banking.Cqrs.Core.Messages;

public abstract class MessageOld
{
    public string Id { get;} = string.Empty;

    public MessageOld(string id)
    {
        Id = id;
    }
}