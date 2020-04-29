using PAWEventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PAWEventive.ApplicationLogic.DataModel.Event;

namespace PAWEventive.Models.Events
{
    public class EventViewModel
    {
        public string Title { get; set; }
        public byte[] ImageByteArray { get; set; }
        public EventCategory Category { get; set; }
        public string EventDeadline { get; set; }
        public int EventMaximumParticipants { get; set; }
        public string Location { get; set; }
        public string ParticipationFee { get; set; }
        public string EventDescription { get; set; }
        public string UserName { get; set; }
        public string UserPhoneNo { get; set; }
        public string UserEmail { get; set; }
    }
}
