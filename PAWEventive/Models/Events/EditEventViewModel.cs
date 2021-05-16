using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using static PAWEventive.ApplicationLogic.DataModel.Event;

namespace PAWEventive.Models.Events
{
    public class EditEventViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "There needs to a title!")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "A category must be specified")]
        [Display(Name = "Category")]
        public EventCategory Category { get; set; }

        [Required(ErrorMessage = "Deadline is Required")]
        [Display(Name = "Application Deadline")]
        public DateTime Deadline { get; set; }

        [Required(ErrorMessage = "Occurence Date is Required")]
        [Display(Name = "Occurence Date")]
        public DateTime OccurenceDate { get; set; }

        [Required(ErrorMessage = "You don't want them stranded, do you?")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Add an image if you feel like it")]
        public IFormFile EventImage { get; set; }

        [Display(Name = "Maximum number of participants")]
        public int Participants { get; set; }

        [Display(Name = "Participation Fee")]
        public decimal ParticipationFee { get; set; }

        [Display(Name = "Event Description")]
        public string Description { get; set; }
    }
}
