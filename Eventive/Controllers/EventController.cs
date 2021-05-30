using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Eventive.ApplicationLogic.DataModel;
using Eventive.ApplicationLogic.Services;
using Eventive.Models.Events;
using System;
using System.Collections.Generic;
using System.IO;

namespace Eventive.Controllers
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

        private EventListViewModel GetEventListViewModel(IEnumerable<EventOrganized> events)
        {
            List<EventViewModel> eventViewModels = new List<EventViewModel>();

            foreach (EventOrganized theEvent in events)
            {
                if (theEvent != null)
                {
                    eventViewModels.Add(GetEventViewModel(theEvent));
                }
            }

            IEnumerable<Guid> eventsFollowed = null;
            IEnumerable<Guid> eventsApplied = null;

            if (User.Identity.IsAuthenticated)
            {
                var userId = userManager.GetUserId(User);
                var currentUser = userService.GetUserByUserId(userId);

                eventsFollowed = eventService
                            .GetEventsGuidForUser(currentUser.Id, Interaction.Type.Follow);

                eventsApplied = eventService
                            .GetEventsGuidForUser(currentUser.Id, Interaction.Type.Apply);
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
            try
            {
                var events = eventService.GetCurrentEvents();
                var viewModel = GetEventListViewModel(events);
                return PartialView("_ContainerPartial", viewModel);
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
                Participant user = userService.GetUserByUserId(thisUserId);

                IEnumerable<EventOrganized> appliedEvents = eventService
                            .GetEventsForUser(user.Id, Interaction.Type.Apply);

                EventListViewModel viewModel = GetEventListViewModel(appliedEvents);

                return PartialView("_ContainerPartial", viewModel);
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
                Participant user = userService.GetUserByUserId(thisUserId);

                IEnumerable<EventOrganized> followingEvents = eventService
                            .GetEventsForUser(user.Id, Interaction.Type.Follow);

                EventListViewModel viewModel = GetEventListViewModel(followingEvents);

                return PartialView("_ContainerPartial", viewModel);
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
                Participant user = userService.GetUserByUserId(thisUserId);

                IEnumerable<EventOrganized> createdEvents = userService
                            .GetUserEvents(user.Id.ToString());

                EventListViewModel viewModel = GetEventListViewModel(createdEvents);

                return PartialView("_AdminEventPartial", viewModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        public IActionResult PastEvents()
        {
            try
            {
                var thisUserId = userManager.GetUserId(User);
                Participant user = userService.GetUserByUserId(thisUserId);

                IEnumerable<EventOrganized> pastEvents = eventService
                            .GetPastEventsOfUser(user.Id);

                EventListViewModel viewModel = GetEventListViewModel(pastEvents);

                return PartialView("_AdminEventPartial", viewModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public IActionResult NewEvent()
        {
            return PartialView("_AddEventPartial", new AddEventViewModel());
        }

        [HttpPost]
        public IActionResult NewEvent([FromForm]AddEventViewModel eventData)
        {
            if (!ModelState.IsValid || eventData is null || eventData.Deadline == null)
                return PartialView("_AddEventPartial", eventData);

            try
            {
                string image = string.Empty;
                if (eventData.EventImage != null)
                {
                    using var memoryStream = new MemoryStream();
                    eventData.EventImage.CopyTo(memoryStream);

                    image = Convert.ToBase64String(memoryStream.ToArray());
                }

                var userId = userManager.GetUserId(User);
                var creatingUser = userService.GetUserByUserId(userId);

                EventDetails details = EventDetails.Create(eventData.EventDescription,
                                        eventData.Location,
                                        eventData.Deadline,
                                        eventData.OccurenceDate,
                                        eventData.MaximumParticipants,
                                        eventData.ParticipationFee,
                                        eventData.ApplicationRequired);

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

        public IActionResult Details([FromRoute] string Id)
        {
            try
            {
                Guid.TryParse(Id, out Guid eventGuid);
                var eventToGo = eventService.GetEventById(eventGuid);
                var viewModel = GetEventViewModel(eventToGo);

                if (User.Identity.IsAuthenticated)
                {
                    var userId = userManager.GetUserId(User);
                    var participant = userService.GetUserByUserId(userId);
                    eventService.RegisterClick(eventGuid, participant.Id);
                }

                return PartialView("_DetailsPartial", viewModel);
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
                var type = Interaction.Type.Follow;
                   
                var potentialParticipation = eventService.GetParticipation(eventId, participantId, type);

                if (potentialParticipation is null)
                {
                    eventService.ParticipateInEvent(eventId, participantId, type);
                }
                else
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

            var eventApplied = eventService.GetParticipation(eventGuid, followingUser.Id, Interaction.Type.Apply);

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
                var type = Interaction.Type.Apply;

                var potentialParticipation = eventService.GetParticipation(eventId, participantId, type);
                var potentialFollowing = eventService.GetParticipation(eventId, participantId, Interaction.Type.Follow);

                if (potentialFollowing is null && potentialParticipation is null)
                {
                    eventService.ParticipateInEvent(eventId, participantId, Interaction.Type.Follow);
                }

                if (potentialParticipation is null)
                {
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
                    OccurenceDate = eventToUpdate.EventDetails.OccurenceDate,
                    Location = eventToUpdate.EventDetails.Location,
                    EventDescription = eventToUpdate.EventDetails.Description,
                    MaximumParticipants = eventToUpdate.EventDetails.MaximumParticipantNo,
                    ParticipationFee = eventToUpdate.EventDetails.ParticipationFee,
                    ApplicationRequired = eventToUpdate.EventDetails.ApplicationRequired
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
                string image = string.Empty;
                Guid.TryParse(updatedData.Id, out Guid eventGuid);
                var eventToUpdate = eventService.GetEventById(eventGuid);

                if (updatedData.EventImage != null)
                {
                    using var memoryStream = new MemoryStream();
                    updatedData.EventImage.CopyTo(memoryStream);
                    image = Convert.ToBase64String(memoryStream.ToArray());
                }

                eventService.UpdateEvent(eventToUpdate.Id,
                                        updatedData.Title,
                                        updatedData.Category,
                                        updatedData.EventDescription,
                                        updatedData.Location,
                                        updatedData.Deadline,
                                        updatedData.OccurenceDate,
                                        image,
                                        updatedData.MaximumParticipants,
                                        updatedData.ParticipationFee,
                                        updatedData.ApplicationRequired);

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
        
        [HttpPost]
        public IActionResult AddComment([FromForm] EventViewModel viewModel)
        {
            if (!ModelState.IsValid || viewModel is null || string.IsNullOrWhiteSpace(viewModel.NewCommentMessage))
            {
                return PartialView("_DetailsPartial", viewModel);
            }

            var userId = userManager.GetUserId(User);
            eventService.AddComment(userId, viewModel.NewCommentId, viewModel.NewCommentMessage);
            
            return PartialView("_DetailsPartial", viewModel);

            /*
            var options = new PusherOptions
            {
                Cluster = "eu"
            };

            var pusher = new Pusher("1204504", "c70525fa1aa658a4627b", "a8ae2c2790f252e5f78d", options);
            ITriggerResult result = await pusher.TriggerAsync("asp_channel", "asp_event", data);*/
        }

        private string FormatParticipationFee(decimal participationFee)
        {
            string formattedFee = participationFee.ToString("#.##");

            if (formattedFee.Length > 0)
            {
                return $"${formattedFee}";
            }

            return "None";
        }

        private string FormatMaximumParticipants(int maximumParticipants)
        {
            if (maximumParticipants > 0)
            {
                return $"{maximumParticipants}";
            }

            return "None";
        }

        private EventViewModel GetEventViewModel(EventOrganized organizedEvent)
        {
            Participant hostingUser = userService.GetCreatorByGuid(organizedEvent.CreatorId);
            string participationFee = FormatParticipationFee(organizedEvent.EventDetails.ParticipationFee);
            string maximumParticipants = FormatMaximumParticipants(organizedEvent.EventDetails.MaximumParticipantNo);
            string currentUsername = null;
            string currentProfileImage = null;

            if (User.Identity.IsAuthenticated)
            {
                var userId = userManager.GetUserId(User);
                var currentUser = userService.GetUserByUserId(userId);
                currentUsername = FormatUserName(currentUser);
                currentProfileImage = currentUser.ProfileImage;
            }

            var eventViewModel = new EventViewModel
            {
                Id = organizedEvent.Id,
                Title = organizedEvent.Title,
                EventImage = organizedEvent.ImageByteArray,
                HostName = FormatUserName(hostingUser),
                HostEmail = hostingUser.ContactDetails.Email,
                HostPhoneNo = hostingUser.ContactDetails.PhoneNo,
                Location = organizedEvent.EventDetails.Location,
                ParticipationFee = participationFee,
                Deadline = FormatEventDate(organizedEvent.EventDetails.Deadline),
                OccurenceDate = FormatEventDate(organizedEvent.EventDetails.OccurenceDate),
                EventTime = FormatEventTime(organizedEvent.EventDetails.OccurenceDate),
                EventDescription = organizedEvent.EventDetails.Description,
                MaximumParticipants = maximumParticipants,
                Category = organizedEvent.Category,
                Comments = organizedEvent.Comments,
                HostProfileImage = hostingUser.ProfileImage,
                UserName = currentUsername,
                UserProfileImage = currentProfileImage
            };

            return eventViewModel;
        }

        private string FormatEventDate(DateTime eventDate)
        {
            return $"{eventDate:dd/MM/yyyy}";
        }

        private string FormatEventTime(DateTime eventTime)
        {
            return $" {eventTime:H:mm}";
        }

        private string FormatUserName(Participant hostingUser)
        {
            return $"{hostingUser.FirstName} {hostingUser.LastName}";
        }

        /*
        public IActionResult GetComments(Guid Id)
        {
            if (Id.Equals(Guid.Empty))
            {
                return View();
            }

            var comments = eventService.GetComments(Id);
            return Json(comments, new Newtonsoft.Json.JsonSerializerSettings());
        }*/
    }
}