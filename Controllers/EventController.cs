using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PAWEventive.ApplicationLogic.DataModel;
using PAWEventive.ApplicationLogic.Services;
using PAWEventive.Models.Events;
using System;
using System.Collections.Generic;

namespace PAWEventive.Controllers
{
    public class EventController : Controller
    {
        private readonly EventService eventService;
        private readonly UserService userService;
        
        public EventController(UserManager<IdentityUser> userManager, EventService eventService, UserService userService)
        {
            this.eventService = eventService;
            this.userService = userService;
        }

        public ActionResult Index()
        {
            try
            {
                List<EventViewModel> eventViewModels = new List<EventViewModel>();

                foreach (Event oneEvent in eventService.GetCurrentEvents())
                {
                    Event theEvent = eventService.GetEventWithDetails(oneEvent.Id);
                    User hostingUser = userService.GetCreatorByGuid(theEvent.CreatorId);
                    var participationFee = theEvent.EventDetails.ParticipationFee.ToString("#.##");

                    if (participationFee.Length > 0)
                    {
                        participationFee = "Fee: $" + participationFee;
                    } else
                    {
                        participationFee = "Free admission";
                    }

                    eventViewModels.Add(new EventViewModel
                    {
                        Title = theEvent.Title,
                        ImageByteArray = theEvent.ImageByteArray,
                        UserName = $"{hostingUser.FirstName} {hostingUser.LastName}",
                        UserEmail = hostingUser.ContactDetails.Email,
                        UserPhoneNo = hostingUser.ContactDetails.PhoneNo,
                        Location = theEvent.EventDetails.Location,
                        ParticipationFee = participationFee,
                        EventDeadline = theEvent.EventDetails.Deadline.ToString("MM/dd/yyyy H:mm"),
                        EventDescription = theEvent.EventDetails.Description,
                        EventMaximumParticipants = theEvent.EventDetails.MaximumParticipantNo,
                        Category = theEvent.Category
                    });
                }

                EventListViewModel viewModel = new EventListViewModel()
                {
                    EventViewModelList = eventViewModels    
                };

                return View(viewModel);
            }
            catch (Exception e)
            {
                return BadRequest("Invalid request received ");
            }
        }
    }
}