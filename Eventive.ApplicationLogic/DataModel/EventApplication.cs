using Eventive.ApplicationLogic.Abstraction;
using System;

namespace Eventive.ApplicationLogic.DataModel
{
    public class EventApplication : IEventInteraction
    {
        public enum ApplicationStatus
        {
            Pending,
            Approved,
            Rejected
        }

        public Guid Id { get; set; }
        public Participant Participant { get; set; }
        public EventOrganized EventOrganized { get; set; }
        public ApplicationStatus Status { get; set; }
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
                Status = ApplicationStatus.Pending,
                Timestamp = DateTime.UtcNow
            };

            return newInteraction;
        }

        public EventApplication JudgeApplication(ApplicationStatus status)
        {
            Status = status;
            return this;
        }
    }
}
