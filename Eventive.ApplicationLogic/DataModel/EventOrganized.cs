using System;
using System.Collections.Generic;

namespace Eventive.ApplicationLogic.DataModel
{
    public class EventOrganized
    {
        public enum EventCategory
        {
            Education,
            Entertainment,
            Networking,
            Hangout,
            Other
        }

        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public string Title { get; set; }
        public string ImageByteArray { get; set; }
        public EventDetails EventDetails { get; set; }
        public EventCategory Category { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Application> Applications { get; set; }
        public List<Interaction> Interactions { get; set; }

        public EventOrganized UpdateEvent(string title, 
                    EventCategory category, 
                    string description, 
                    string location,
                    DateTime deadline, 
                    DateTime occurenceDate, 
                    string image,
                    int maximumParticipants,
                    decimal fee,
                    bool applicationRequired)
        {
            if (string.IsNullOrEmpty(image)) {
                ImageByteArray = image;
            }

            Title = title;
            Category = category;
            EventDetails.UpdateDetails(description, location, 
                deadline, occurenceDate, maximumParticipants, fee, applicationRequired);

            return this;
        }
    }

}
