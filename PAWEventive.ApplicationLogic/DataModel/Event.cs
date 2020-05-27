using System;
using System.Collections.Generic;
using System.Text;

namespace PAWEventive.ApplicationLogic.DataModel
{
    public class Event
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


        public Event UpdateEvent(string title, 
                    EventCategory category, 
                    string description, 
                    string location,
                    DateTime deadline, 
                    string image,
                    int maximumParticipants,
                    decimal fee)
        {
            if (image != null && image.Length > 1) {
                ImageByteArray = image;
            }

            Title = title;
            Category = category;
            EventDetails.UpdateDetails(description, location, 
                deadline, maximumParticipants, fee);

            return this;
        }
    }

}
