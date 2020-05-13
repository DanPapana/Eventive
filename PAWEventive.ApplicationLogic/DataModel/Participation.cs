using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace PAWEventive.ApplicationLogic.DataModel
{
    public class Participation
    {
        public enum Type
        {
            Following,
            Applied
        }
        public Guid Id { get; set; }
        public Guid ParticipantId { get; set; }
        public Guid EventId { get; set; }
        public Type UserParticipationType { get; set; }

        public static Participation Create(Guid eventId, Guid participantId, Type type)
        {
            var participation = new Participation()
            {
                Id = Guid.NewGuid(),
               EventId = eventId,
               ParticipantId = participantId,
               UserParticipationType = type
            };

            return participation;
        }

    }
}
