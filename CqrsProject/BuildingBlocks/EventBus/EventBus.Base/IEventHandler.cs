using EventBus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base;

public interface IEventHandler<in TEvent> : IBaseEventHandler where TEvent : Event
{
    Task Handle(TEvent @event, CancellationToken? cancellationToken = null);
}

public interface IBaseEventHandler
{

}