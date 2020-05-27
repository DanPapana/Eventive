using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PAWEventive.ApplicationLogic.DataModel;
using PAWEventive.ApplicationLogic.Services;
using PAWEventive.Models.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

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
            return View();
        }

        private EventListViewModel GetEventListViewModel(IEnumerable<Event> events)
        {
            List<EventViewModel> eventViewModels = new List<EventViewModel>();

            foreach (Event theEvent in events)
            {
                if (theEvent != null)
                {
                    User hostingUser = userService.GetCreatorByGuid(theEvent.CreatorId);
                    string participationFee = theEvent.EventDetails.ParticipationFee.ToString("#.##");

                    if (participationFee.Length > 0)
                    {
                        participationFee = "Fee: $" + participationFee;
                    }
                    else
                    {
                        participationFee = "Free admission";
                    }

                    eventViewModels.Add(new EventViewModel
                    {
                        Id = theEvent.Id,
                        Title = theEvent.Title,
                        EventImage = theEvent.ImageByteArray,
                        UserName = $"{hostingUser.FirstName} {hostingUser.LastName}",
                        UserEmail = hostingUser.ContactDetails.Email,
                        UserPhoneNo = hostingUser.ContactDetails.PhoneNo,
                        Location = theEvent.EventDetails.Location,
                        ParticipationFee = participationFee,
                        EventDeadline = theEvent.EventDetails.Deadline.ToString("H:mm dd/MM/yyyy"),
                        EventDescription = theEvent.EventDetails.Description,
                        EventMaximumParticipants = theEvent.EventDetails.MaximumParticipantNo,
                        Category = theEvent.Category
                    });
                }
            }

            IEnumerable<Guid> eventsFollowed = null;
            IEnumerable<Guid> eventsApplied = null;

            if (User.Identity.IsAuthenticated)
            {
                var userId = userManager.GetUserId(User);
                var currentUser = userService.GetUserByUserId(userId);

                eventsFollowed = eventService
                               .GetEventsGuidForUser(currentUser.Id, Participation.Type.Following);

                eventsApplied = eventService
                            .GetEventsGuidForUser(currentUser.Id, Participation.Type.Applied);

            }
            EventListViewModel eventListViewModel = new EventListViewModel()
            {
                EventViewModelList = eventViewModels,
                EventsFollowed = eventsFollowed,
                EventsApplied = eventsApplied
            };

            return eventListViewModel;
        }

        public IActionResult EventContainer()
        {
            try {

                var events = eventService.GetCurrentEvents();
                var viewModel = GetEventListViewModel(events);
                return PartialView("_EventContainerPartial", viewModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        public IActionResult AppliedEvents()
        {
            try
            {
                var thisUserId = userManager.GetUserId(User);
                User user = userService.GetUserByUserId(thisUserId);

                IEnumerable<Event> appliedEvents = eventService
                            .GetEventsForUser(user.Id, Participation.Type.Applied);

                EventListViewModel viewModel = GetEventListViewModel(appliedEvents);

                return PartialView("_EventContainerPartial", viewModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        public IActionResult FollowingEvents()
        {
            try
            {
                var thisUserId = userManager.GetUserId(User);
                User user = userService.GetUserByUserId(thisUserId);

                IEnumerable<Event> followingEvents = eventService
                            .GetEventsForUser(user.Id, Participation.Type.Following);

                EventListViewModel viewModel = GetEventListViewModel(followingEvents);

                return PartialView("_EventContainerPartial", viewModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        public IActionResult CreatedEvents()
        {
            try
            {
                var thisUserId = userManager.GetUserId(User);
                User user = userService.GetUserByUserId(thisUserId);

                IEnumerable<Event> createdEvents = userService
                            .GetUserEvents(user.Id.ToString());

                return PartialView("_AdminEventPartial", createdEvents);

            } catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        public IActionResult PastEvents()
        {
            try
            {
                var thisUserId = userManager.GetUserId(User);
                User user = userService.GetUserByUserId(thisUserId);

                IEnumerable<Event> pastEvents = eventService
                            .GetPastEventsOfUser(user.Id);

                return PartialView("_AdminEventPartial", pastEvents);

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }



        [HttpGet]
        public IActionResult NewEvent()
        {
            return PartialView("_AddEventPartial", new NewEventViewModel());
        }


        [HttpPost]
        public IActionResult NewEvent([FromForm]NewEventViewModel eventData)
        {
            string image = "";

            if (!ModelState.IsValid || eventData == null || eventData.Deadline == null)
                return PartialView("_AddEventPartial", eventData);
              
            try
            {

                using (var memoryStream = new MemoryStream())
                {
                    eventData.EventImage.CopyTo(memoryStream);

                    image = Convert.ToBase64String(memoryStream.ToArray());
                }

                    var userId = userManager.GetUserId(User);
                var creatingUser = userService.GetUserByUserId(userId);

                EventDetails details = new EventDetails(eventData.EventDescription,
                                        eventData.Location, 
                                        eventData.Deadline, 
                                        eventData.MaximumParticipants, 
                                        eventData.ParticipationFee);

                    userService.AddEvent(creatingUser.Id,
                                           eventData.Title,
                                           eventData.Category,
                                           image,
                                           details);

                return PartialView("_AddEventPartial", eventData);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        public IActionResult Follow(string id)
        {
            try
            {
                Guid.TryParse(id, out Guid eventGuid);
                var eventToFollow = eventService.GetEventById(eventGuid);

                var userId = userManager.GetUserId(User);
                var followingUser = userService.GetUserByUserId(userId);

                var eventId = eventToFollow.Id;
                var participantId = followingUser.Id;
                var type = Participation.Type.Following;

                var potentialParticipation = eventService.GetParticipation(eventId, participantId, type);

                if (potentialParticipation == null)
                {
                    eventService.ParticipateInEvent(eventId, participantId, type);
                } else
                {
                    eventService.RemoveParticipation(eventId, participantId, type);
                }

                return RedirectToAction("Index");
            } 
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public IActionResult Apply([FromRoute]string id)
        {
            Guid.TryParse(id, out Guid eventGuid);

            var userId = userManager.GetUserId(User);
            var followingUser = userService.GetUserByUserId(userId);

            string eventName = eventService.GetEventById(eventGuid).Title;
            bool applied = false;

            var eventApplied = eventService.GetParticipation(eventGuid, followingUser.Id, Participation.Type.Applied);

            if (eventApplied != null)
            {
                applied = true;
            }
                
            var applyViewModel = new ApplyToEventViewModel()
            {
                EventId = id,
                EventName = eventName,
                AlreadyApplied = applied
            };

            return PartialView("_ApplyToEventPartial", applyViewModel);
        }


        [HttpPost]
        public IActionResult Apply(ApplyToEventViewModel applyViewModel)
        {
            try
            {
                Guid.TryParse(applyViewModel.EventId, out Guid eventGuid);
                var eventToApply = eventService.GetEventById(eventGuid);

                var userId = userManager.GetUserId(User);
                var applyingUser = userService.GetUserByUserId(userId);

                var eventId = eventToApply.Id;
                var participantId = applyingUser.Id;
                var type = Participation.Type.Applied;

                var potentialParticipation = eventService.GetParticipation(eventId, participantId, type);
                
                var potentialFollowing = eventService.GetParticipation(eventId, participantId, Participation.Type.Following);

                if (potentialFollowing == null && potentialParticipation == null) {
                    eventService.ParticipateInEvent(eventId, participantId, Participation.Type.Following);
                }

                if (potentialParticipation == null) {
                    eventService.ParticipateInEvent(eventId, participantId, type);
                }
                else
                {
                    eventService.RemoveParticipation(eventId, participantId, type);
                }

                return PartialView("_ApplyToEventPartial", applyViewModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpGet]
        public IActionResult Edit([FromRoute]string id)
        {
            try
            {
                Guid.TryParse(id, out Guid eventGuid);
                var eventToUpdate = eventService.GetEventById(eventGuid);

                var editEventViewModel = new EditEventViewModel()
                {
                    Id = id,
                    Title = eventToUpdate.Title,
                    Category = eventToUpdate.Category,
                    Deadline = eventToUpdate.EventDetails.Deadline,
                    Location = eventToUpdate.EventDetails.Location,
                    Description = eventToUpdate.EventDetails.Description,
                    Participants = eventToUpdate.EventDetails.MaximumParticipantNo,
                    ParticipationFee = eventToUpdate.EventDetails.ParticipationFee                    
                };

                return PartialView("_EditEventPartial", editEventViewModel);

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public IActionResult Edit([FromForm] EditEventViewModel updatedData)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditEventPartial", new EditEventViewModel());
            }

            try
            {
                string image = "";
                Guid.TryParse(updatedData.Id, out Guid eventGuid);
                var eventToUpdate = eventService.GetEventById(eventGuid);

                if (updatedData.EventImage != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        updatedData.EventImage.CopyTo(memoryStream);
                        image = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }

                eventService.UpdateEvent(eventToUpdate.Id,
                                                updatedData.Title,
                                                updatedData.Category,
                                                updatedData.Description,
                                                updatedData.Location,
                                                updatedData.Deadline,
                                                image,
                                                updatedData.Participants,
                                                updatedData.ParticipationFee);

                return PartialView("_EditEventPartial", updatedData);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public IActionResult Remove([FromRoute]string Id)
        {

            RemoveEventViewModel removeViewModel = new RemoveEventViewModel()
            {
                Id = Id
            };

            return PartialView("_RemoveEventPartial", removeViewModel);
        }

        [HttpPost]
        public IActionResult Remove(RemoveEventViewModel removeData)
        {
            try
            {
                eventService.RemoveEvent(removeData.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return PartialView("_RemoveEventPartial", removeData);
        }


    }
}