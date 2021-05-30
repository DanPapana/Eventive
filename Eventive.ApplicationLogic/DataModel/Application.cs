using System;

namespace Eventive.ApplicationLogic.DataModel
{
    public class Application
    {
        public Guid Id { get; set; }
        public Guid EventOrganizedId { get; set; }
        public Guid ParticipantId { get; set; }
        public string ApplicationText { get; set; }
        public DateTime Timestamp { get; set; }
        
        public static Application Create(Guid eventId, Guid participantId, string applicationText)
        {
            var newApplication = new Application()
            {
                Id = Guid.NewGuid(),
                EventOrganizedId = eventId,
                ParticipantId = participantId,
                Timestamp = DateTime.Now,
                ApplicationText = applicationText
            };

            return newApplication;
        }
    }
}
