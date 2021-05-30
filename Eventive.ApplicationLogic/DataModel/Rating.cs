using System;

namespace Eventive.ApplicationLogic.DataModel
{
    public class Rating
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid ParticipantId { get; set; }
        public int Score { get; set; }

        public static Rating Create(Guid eventId, Guid participantId, int score = 3)
        {
            var newRating = new Rating()
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                ParticipantId = participantId,
                Score = score
            };

            return newRating;
        }
    }
}
