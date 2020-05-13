using PAWEventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAWEventive.Models.Events
{
    public class EventListViewModel
    {
        public List<EventViewModel> EventViewModelList { get; set; }
        public IEnumerable<Guid> EventsFollowed { get; set; }
        public IEnumerable<Guid> EventsApplied { get; set; }
    }
}
