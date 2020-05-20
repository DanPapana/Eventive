using PAWEventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using static PAWEventive.ApplicationLogic.DataModel.Event;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace PAWEventive.Models.Events
{

    public class NewEventViewModel
    {
       
        [Required(ErrorMessage = "There needs to a title!")]
        [Display(Name = "Title")]
        public string Title { get; set; }


        [Required(ErrorMessage = "A category must be specified")]
        [Display(Name = "Category")]
        public EventCategory Category { get; set; }

        //[CurrentDate(ErrorMessage = "Date must be after or equal to current date")]
        //[DataType(DataType.DateTime, ErrorMessage = "Invalid Date Format")]
        //[Required(ErrorMessage = "Deadline is Required")]
        //[Range(typeof(DateTime), "00:00 01/01/2020", "00:00 01/01/2100", ErrorMessage = "Date is out of Range")]
        //[DisplayFormat(DataFormatString = "{0:HH:mm dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        [Required(ErrorMessage = "Deadline is Required")]
        [Display(Name = "Application Deadline")]
        public DateTime Deadline { get; set; }

        [Required(ErrorMessage = "You don't want them stranded, do you?")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Add an image if you feel like it")]
        public IFormFile EventImage { get; set; }
        
        [Display(Name = "Maximum number of participants")]
        public int MaximumParticipants { get; set; }
        
        [Display(Name = "Participation Fee")]
        public decimal ParticipationFee { get; set; }
        
        [Display(Name = "Event Description")]
        public string EventDescription { get; set; }
    }
}
