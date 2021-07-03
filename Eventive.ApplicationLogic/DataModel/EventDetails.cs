using System;

namespace Eventive.ApplicationLogic.DataModel
{
    public class EventDetails
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime OccurenceDate { get; set; }
        public int MaximumParticipantNo { get; set; }
        public decimal ParticipationFee { get; set; }
        public bool ApplicationRequired { get; set; }

        public static EventDetails Create(string description, string location, double? latitude, double? longitude, string cityName,
            DateTime deadline, DateTime occurenceDate, int maximumParticipants, decimal fee, bool applicationRequired)
        {
            var newEventDetails = new EventDetails()
            {
                Id = Guid.NewGuid(),
                Location = location,
                Latitude = latitude,
                Longitude = longitude,
                City = cityName,
                Description = description,
                Deadline = deadline,
                OccurenceDate = occurenceDate,
                MaximumParticipantNo = maximumParticipants,
                ParticipationFee = fee,
                ApplicationRequired = applicationRequired
            };

            return newEventDetails;
        }

        public EventDetails UpdateDetails(string description, string location, double? latitude, double? longitude, string cityName,
            DateTime deadline, DateTime occurenceDate, int maximumParticipants, decimal fee, bool applicationRequired)
        {
            Location = location;
            Latitude = latitude;
            Longitude = longitude;
            City = cityName;
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
