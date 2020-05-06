using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using PAWEventive.ApplicationLogic.DataModel;
using PAWEventive.ApplicationLogic.Services;
using PAWEventive.Models.Events;
using System;
using System.Collections.Generic;
using static PAWEventive.Models.Events.NewEventViewModel;

namespace PAWEventive.Controllers
{
    public class EventController : Controller
    {
        private readonly EventService eventService;
        private readonly UserService userService;
        private readonly UserManager<IdentityUser> userManager;
        
        public EventController(UserManager<IdentityUser> userManager, EventService eventService, UserService userService)
        {
            this.eventService = eventService;
            this.userService = userService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            try
            {
                List<EventViewModel> eventViewModels = new List<EventViewModel>();

                foreach (Event oneEvent in eventService.GetCurrentEvents())
                {
                    Event theEvent = eventService.GetEventWithDetails(oneEvent.Id);
                    User hostingUser = userService.GetCreatorByGuid(theEvent.CreatorId);
                    string participationFee = theEvent.EventDetails.ParticipationFee?.ToString("#.##");

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

                var userId = userManager.GetUserId(User);
                var host = userService.GetUserByUserId(userId);

                EventListViewModel viewModel = new EventListViewModel()
                {
                    CreatorId = host.Id,
                    EventViewModelList = eventViewModels    
                };

                return View(viewModel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IActionResult NewEvent()
        {
            var userGuid = Guid.Parse(userManager.GetUserId(User));
            var creator = userService.GetUserByUserId(userGuid.ToString());
            
            var viewModel = new NewEventViewModel()
            {
                CreatorId = creator.Id,
                CreationStatus = NewEventStatus.NotInitiated
            };

            return PartialView("_AddEventPartial", viewModel);
        }

        [HttpPost]
        public IActionResult NewEvent([FromForm]NewEventViewModel eventData)
        {
            NewEventViewModel viewModelResult = new NewEventViewModel()
            {
                CreationStatus = NewEventStatus.Failed
            };

            if (!ModelState.IsValid || eventData == null)
                return RedirectToAction("Index");
            
            ModelState.Clear();
            try
            {
                var userId = userManager.GetUserId(User);
                var creatingUser = userService.GetUserByUserId(userId);

                if (eventData.ParticipationFee == null)
                {
                    eventData.ParticipationFee = 0;
                }

                if (eventData.MaximumParticipants == null)
                {
                    eventData.MaximumParticipants = 0;
                }

                EventDetails details = new EventDetails(eventData.EventDescription,
                                        eventData.Location, 
                                        DateTime.Parse(eventData.Deadline), 
                                        eventData.MaximumParticipants, 
                                        eventData.ParticipationFee);

                userService.AddEvent(creatingUser.Id,
                                       eventData.Title,
                                       eventData.Category,
                                       eventData.EventImage,
                                       details);

                viewModelResult.CreationStatus = NewEventStatus.Created;
            }
            catch (Exception)
            {
                viewModelResult.CreationStatus = NewEventStatus.Failed;
            }

            //return PartialView("_AddEventPartial", viewModelResult);
            return RedirectToAction("Index");
        }
    }
}