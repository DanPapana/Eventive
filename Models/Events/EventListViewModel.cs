using PAWEventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAWEventive.Models.Events
{
    public class EventListViewModel
    {
        public Guid CreatorId { get; set; }
        public List<EventViewModel> EventViewModelList { get; set; }
    }
}
