using Eventive.ApplicationLogic.Abstraction;
using System;

namespace Eventive.ApplicationLogic.DataModel
{
    public class EventApplication : IEventInteraction
    {
        public Guid Id { get; set; }
        public Participant Participant { get; set; }
        public EventOrganized EventOrganized { get; set; }
        public string ApplicationText { get; set; }
        public DateTime Timestamp { get; set; }
        public static EventApplication Create(EventOrganized eventOrganized, Participant participant, string applicationText = null)
        {
            var newInteraction = new EventApplication()
            {
                Id = Guid.NewGuid(),
                EventOrganized = eventOrganized,
                Participant = participant,
                ApplicationText = applicationText,
                Timestamp = DateTime.UtcNow
            };

            return newInteraction;
        }
    }
}
