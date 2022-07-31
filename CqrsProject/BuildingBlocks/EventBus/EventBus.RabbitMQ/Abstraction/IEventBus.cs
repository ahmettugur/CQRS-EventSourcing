using EventBus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ.Abstraction
{
    public interface IEventBus: IBaseEventBus
    {
        void Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>;

        void UnSubscribe<T, TH>() where T : Event where TH : IEventHandler<T>;
    }
}
