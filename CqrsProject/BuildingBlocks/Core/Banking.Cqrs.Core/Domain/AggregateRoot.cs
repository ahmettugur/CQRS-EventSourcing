using System.Xml.Serialization;
using Banking.Cqrs.Core.Events;

namespace Banking.Cqrs.Core.Domain;

public abstract class AggregateRoot
{
    public string Id { get; protected set; } = string.Empty;
    private int _version = -1;
    private readonly List<BaseEvent> _changes = new();

    public int GetVersion()
    {
        return _version;
    }

    public void SetVersion(int version)
    {
        this._version = version;
    }

    public List<BaseEvent> GetUncommittedChanges()
    {
        return _changes;
    }

    public void MarkChangesAsCommitted()
    {
        _changes.Clear();
    }

    private void ApplyChange(BaseEvent @event,bool isNewEvent)
    {
        try
        {
            var eventClass = @event.GetType();
            var method = GetType().GetMethod("Apply", new[] {eventClass});
            method!.Invoke(this, new object[] {@event});

        }
        catch (Exception)
        {
            // ignored
        }
        finally
        {
            if (isNewEvent)
                _changes.Add(@event);
        }
    }

    protected void RaiseEvent(BaseEvent @event)
    {
        ApplyChange(@event,true);
    }

    public void ReplayEvents(IEnumerable<BaseEvent> events)
    {
        events = events.OrderBy(_ => _.Version);
        foreach (var evt in events)
        {
            ApplyChange(evt,false);
        }
    }

}