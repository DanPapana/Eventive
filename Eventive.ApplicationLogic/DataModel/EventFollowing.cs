using Eventive.ApplicationLogic.Abstraction;
using System;

namespace Eventive.ApplicationLogic.DataModel
{
    public class EventFollowing : IEventInteraction
    {
        public Guid Id { get; set; }
        public Participant Participant { get; set; }
        public EventOrganized EventOrganized { get; set; }
        public DateTime Timestamp { get; set; }

        public static EventFollowing Create(EventOrganized eventOrganized, Participant participant)
        {
            var newInteraction = new EventFollowing()
            {
                Id = Guid.NewGuid(),
                EventOrganized = eventOrganized,
                Participant = participant,
                Timestamp = DateTime.UtcNow
            };

            return newInteraction;
        }
    }
}
