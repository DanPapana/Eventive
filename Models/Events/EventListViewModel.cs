using PAWEventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAWEventive.Models.Events
{
    public class EventListViewModel
    {
        public IEnumerable<Event> EventList { get; set; }
    }
}
