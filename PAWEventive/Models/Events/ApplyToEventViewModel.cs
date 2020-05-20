using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAWEventive.Models.Events
{
    public class ApplyToEventViewModel
    {
        public string EventId { get; set; }
        public string EventName { get; set; }
        public bool AlreadyApplied { get; set; }
    }
}
