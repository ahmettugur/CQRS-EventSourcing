using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base;

public class Event
{

    [JsonProperty]
    public Guid CorrelationId { get; private set; }

    [JsonProperty]
    public DateTime CreatedDate { get; private set; }



    public Event()
    {
        CorrelationId = Guid.NewGuid();
        CreatedDate = DateTime.Now;
    }

    [JsonConstructor]
    public Event(Guid id, DateTime createdDate)
    {
        CorrelationId = id;
        CreatedDate = createdDate;
    }
}

