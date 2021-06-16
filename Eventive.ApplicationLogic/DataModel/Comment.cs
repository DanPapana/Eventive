using Eventive.ApplicationLogic.Abstraction;
using System;

namespace Eventive.ApplicationLogic.DataModel
{
    public class Comment : IEventInteraction
    {
        public Guid Id { get; set; }
        public Participant Participant { get; set; }
        public EventOrganized EventOrganized { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }

        public static Comment Create(Participant participant, EventOrganized organizedEvent, string message)
        {
            var newComment = new Comment()
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Participant = participant,
                EventOrganized = organizedEvent,
                Message = message
            };

            return newComment;
        }
    }
}
