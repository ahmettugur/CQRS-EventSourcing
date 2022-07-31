using EventBus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base;

public interface IBaseEventBus
{
    void Publish(Event @event,string topic="");


}