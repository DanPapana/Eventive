using System;
using System.ComponentModel.DataAnnotations;
using static Eventive.ApplicationLogic.DataModel.EventOrganized;
using Microsoft.AspNetCore.Http;

namespace Eventive.Models.Events
{
    public class AddEventViewModel
    {
        [Required(ErrorMessage = "There needs to a title!")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "A category must be specified")]
        [Display(Name = "Category")]
        public EventCategory Category { get; set; }

        [Required(ErrorMessage = "Deadline is required")]
        [Display(Name = "Application Deadline")]
        public DateTime Deadline { get; set; }

        [Required(ErrorMessage = "Occurence date is required")]
        [Display(Name = "Event Date")]
        public DateTime OccurenceDate { get; set; }

        [Required(ErrorMessage = "You don't want them stranded, do you?")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Image for the event")]
        public IFormFile EventImage { get; set; }
        
        [Display(Name = "Maximum number of participants")]
        public int MaximumParticipants { get; set; }
        
        [Display(Name = "Attendance fee")]
        public decimal ParticipationFee { get; set; }
        
        [Display(Name = "Event description")]
        public string EventDescription { get; set; }

        [Display(Name = "Application Required?")]
        public bool ApplicationRequired { get; set; }
    }
}

//[CurrentDate(ErrorMessage = "Date must be after or equal to current date")]
//[DataType(DataType.DateTime, ErrorMessage = "Invalid Date Format")]
//[Required(ErrorMessage = "Deadline is Required")]
//[Range(typeof(DateTime), "00:00 01/01/2020", "00:00 01/01/2100", ErrorMessage = "Date is out of Range")]
//[DisplayFormat(DataFormatString = "{0:HH:mm dd/MM/yyyy}", ApplyFormatInEditMode = true)]
