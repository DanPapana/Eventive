using System;

namespace Eventive.ApplicationLogic.DataModel
{
    public class EventDetails
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime OccurenceDate { get; set; }
        public int MaximumParticipantNo { get; set; }
        public decimal ParticipationFee { get; set; }
        public bool ApplicationRequired { get; set; }

        public EventDetails() { }

        public static EventDetails Create(string description, string location,
            DateTime deadline, DateTime occurenceDate, int maximumParticipants, decimal fee, bool applicationRequired)
        {
            var newEventDetails = new EventDetails()
            {
                Id = Guid.NewGuid(),
                Location = location,
                Description = description,
                Deadline = deadline,
                OccurenceDate = occurenceDate,
                MaximumParticipantNo = maximumParticipants,
                ParticipationFee = fee,
                ApplicationRequired = applicationRequired
            };

            return newEventDetails;
        }

        public EventDetails UpdateDetails(string description, string location, 
            DateTime deadline, DateTime occurenceDate, int maximumParticipants, decimal fee, bool applicationRequired)
        {
            Location = location;
            Description = description;
            Deadline = deadline;
            OccurenceDate = occurenceDate;
            MaximumParticipantNo = maximumParticipants;
            ParticipationFee = fee;
            ApplicationRequired = applicationRequired;

            return this;
        }
    }
}
