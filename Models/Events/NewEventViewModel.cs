using PAWEventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using static PAWEventive.ApplicationLogic.DataModel.Event;

namespace PAWEventive.Models.Events
{
    public enum NewEventStatus
    {
        NotInitiated = 0,
        Created,
        Failed
    }

    public class NewEventViewModel
    {
        [Required]
        public Guid? CreatorId { get; set; }

        [Required(ErrorMessage = "There needs to a title!")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "A category must be specified")]
        [Display(Name = "Category")]
        public EventCategory Category { get; set; }

        [Required]
        [Display(Name = "Application Deadline")]
        public string Deadline { get; set; }

        [Required(ErrorMessage = "You don't want them stranded, do you?")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        public System.Drawing.Image EventImage { get; set; }
        
        [Display(Name = "Maximum number of participants")]
        public int? MaximumParticipants { get; set; }
        
        [Display(Name = "Participation Fee")]
        public decimal? ParticipationFee { get; set; }
        
        [Display(Name = "Event Description")]
        public string EventDescription { get; set; }
        public NewEventStatus CreationStatus { get; set; }
    }
}
