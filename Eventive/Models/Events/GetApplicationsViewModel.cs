using Eventive.ApplicationLogic.DataModel;
using System.Collections.Generic;

namespace Eventive.Models.Events
{
    public class GetApplicationsViewModel
    {
        public string CreatorId { get; set; }
        public EventOrganized Event { get; set; }
        public IEnumerable<EventApplication> Applications { get; set; }
    }
}
