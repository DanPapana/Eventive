using Eventive.ApplicationLogic.Abstraction;
using System;

namespace Eventive.ApplicationLogic.DataModel
{
    public class EventClick : IEventInteraction
    {
        public Guid Id { get; set; }
        public Participant Participant { get; set; }
        public EventOrganized EventOrganized { get; set; }
        public DateTime Timestamp { get; set; }

        public static EventClick Create(EventOrganized eventOrganized, Participant participant)
        {
            var newInteraction = new EventClick()
            {
                Id = Guid.NewGuid(),
                EventOrganized = eventOrganized,
                Participant = participant,
                Timestamp = DateTime.Now
            };

            return newInteraction;
        }
    }
}
