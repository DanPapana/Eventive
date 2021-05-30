using System;

namespace Eventive.ApplicationLogic.DataModel
{
    public class Interaction
    {
        public enum Type
        {
            Follow,
            Apply,
            Click
        }

        public Guid Id { get; set; }
        public Guid ParticipantId { get; set; }
        public Guid EventOrganizedId { get; set; }
        public DateTime Timestamp { get; set; }
        public Type UserParticipationType { get; set; }

        public static Interaction Create(Guid eventId, Guid participantId, Type type)
        {
            var participation = new Interaction()
            {
                Id = Guid.NewGuid(),
                EventOrganizedId = eventId,
                ParticipantId = participantId,
                Timestamp = DateTime.Now,
                UserParticipationType = type
            };

            return participation;
        }
    }
}
