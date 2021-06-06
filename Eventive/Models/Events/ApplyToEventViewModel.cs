using System.ComponentModel.DataAnnotations;

namespace Eventive.Models.Events
{
    public class ApplyToEventViewModel
    {
        public string EventId { get; set; }
        public string EventName { get; set; }
        public bool AlreadyApplied { get; set; }
        public bool ApplicationRequired { get; set; }

        [Display(Name = "Write down a motivational letter to participate in this event:")]
        public string ApplicationText { get; set; }
    }
}
