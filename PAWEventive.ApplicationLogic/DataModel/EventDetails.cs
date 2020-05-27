using System;
using System.Collections.Generic;
using System.Text;

namespace PAWEventive.ApplicationLogic.DataModel
{
    public class EventDetails
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime Deadline { get; set; }
        public int MaximumParticipantNo { get; set; }
        public decimal ParticipationFee { get; set; }

        public EventDetails() { }
        public EventDetails(string description, string location, 
            DateTime deadline, int maximumParticipants, decimal fee)
        {
            Id = Guid.NewGuid();
            Location = location;
            Description = description;
            Deadline = deadline;
            MaximumParticipantNo = maximumParticipants;
            ParticipationFee = fee;
        }

        public EventDetails UpdateDetails(string description, string location, 
            DateTime deadline, int maximumParticipants, decimal fee)
        {
            Location = location;
            Description = description;
            Deadline = deadline;
            MaximumParticipantNo = maximumParticipants;
            ParticipationFee = fee;

            return this;
        }
    }
}
