using Eventive.ApplicationLogic.Abstraction;
using System;

namespace Eventive.ApplicationLogic.DataModel
{
    public class EventRating : IEventInteraction
    {
        public Guid Id { get; set; }
        public EventOrganized EventOrganized { get; set; }
        public Participant Participant { get; set; }
        public int Score { get; set; }
        
        public static EventRating Create(EventOrganized eventOrganized, Participant participant, int score = 3)
        {
            var newRating = new EventRating()
            {
                Id = Guid.NewGuid(),
                EventOrganized = eventOrganized,
                Participant = participant,
                Score = score
            };

            return newRating;
        }
    }
}
