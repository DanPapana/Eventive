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
        public User EventHost { get; set; }
        public EventDetails EventDetails { get; set; }
        public EventCategory Category { get; set; }
    }
}
