using Eventive.ApplicationLogic.DataModel;
using System;

namespace Eventive.ApplicationLogic.Abstraction
{
    public interface IEventInteraction
    {
        Guid Id { get; set; }
        Participant Participant { get; set; }
        EventOrganized EventOrganized { get; set; }
    }
}
