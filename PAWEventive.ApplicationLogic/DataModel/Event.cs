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
        public byte[] ImageByteArray { get; set; }
        public EventDetails EventDetails { get; set; }
        public EventCategory Category { get; set; }
    }
}
