using Banking.Cqrs.Core.Contracts;
using EventBus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Banking.Cqrs.Core.Events;

[Event("ReplayAccountEvent")]
public class ReplayAccountEvent : Event
{
    public ReplayAccountEvent()
    {
    }

    public List<object> ReplayEvents { get; set; } = new List<object>();
}
