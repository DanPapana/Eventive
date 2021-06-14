using Eventive.ApplicationLogic.DataModel;
using Eventive.ApplicationLogic.Services;
using Eventive.Models.Events;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Eventive.Controllers
{
    public class EventController : Controller
    {
        private readonly BaseService baseService;
        private readonly EventService eventService;
        private readonly UserService userService;
        private readonly UserManager<IdentityUser> userManager;

        public EventController(UserManager<IdentityUser> userManager, 
            EventService eventService, UserService userService, BaseService baseService)
        {
            this.baseService = baseService;
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
                var currentParticipant = userService.GetParticipantByUserId(userId);

                eventsFollowed = eventService
                            .GetFollowedEventsGuidForUser(currentParticipant.Id);

                eventsApplied = eventService
                            .GetAppliedEventsGuidForUser(currentParticipant.Id);
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
                IEnumerable<EventOrganized> events = null;
                if (User.Identity.IsAuthenticated)
                {
                    var userId = userManager.GetUserId(User);
                    var currentParticipant = userService.GetParticipantByUserId(userId);
                    events = eventService.GetActiveEvents(currentParticipant.UserId);
                } else
                {
                    events = eventService.GetActiveEvents();
                }

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
                var currentUserId = userManager.GetUserId(User);
                Participant participant = userService.GetParticipantByUserId(currentUserId);

                IEnumerable<EventOrganized> appliedEvents = eventService
                            .GetAppliedEventsForUser(participant.Id);

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
                var currentUserId = userManager.GetUserId(User);
                Participant participant = userService.GetParticipantByUserId(currentUserId);

                IEnumerable<EventOrganized> followingEvents = eventService
                            .GetFollowedEventsForUser(participant.Id);

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
                var currentUserId = userManager.GetUserId(User);
                Participant participant = userService.GetParticipantByUserId(currentUserId);

                IEnumerable<EventOrganized> createdEvents = userService
                            .GetUserEvents(participant.Id);

                EventListViewModel viewModel = GetEventListViewModel(createdEvents);

                return PartialView("_ContainerPartial", viewModel);
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
                var currentUserId = userManager.GetUserId(User);
                Participant user = userService.GetParticipantByUserId(currentUserId);

                IEnumerable<EventOrganized> pastEvents = eventService
                            .GetPastEventsOfUser(user.Id);

                EventListViewModel viewModel = GetEventListViewModel(pastEvents);

                return PartialView("_PastAttendedPartial", viewModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        public IActionResult Rate(string eventId, string score)
        {
            if (!ModelState.IsValid || eventId is null || score is null)
            {
                return PartialView("_PastAttendedPartial", new EventViewModel());
            }

            try
            {
                Guid.TryParse(eventId, out Guid eventGuid);
                var eventToRate = eventService.GetEventById(eventGuid);

                var userId = userManager.GetUserId(User);
                var ratingUser = userService.GetParticipantByUserId(userId);

                var oldRating = eventService.GetUserRating(eventGuid, ratingUser.Id);

                if (oldRating != null)
                {
                    eventService.RemoveInteraction(oldRating);
                }

                eventService.AddRating(ratingUser.Id, eventGuid, score);

                return PastEvents();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public IActionResult NewEvent()
        {
            return PartialView("_AddModifyEventPartial", new AddModifyEventViewModel());
        }

        [HttpPost]
        public IActionResult NewEvent([FromForm]AddModifyEventViewModel eventData)
        {
            if (!ModelState.IsValid || eventData is null || eventData.Deadline == null)
                return PartialView("_AddModifyEventPartial", eventData);

            try
            {
                string resultImage = string.Empty;

                if (eventData.EventImage != null)
                {
                    resultImage = baseService.CompressImage(eventData.EventImage);
                }

                var userId = userManager.GetUserId(User);
                var creatingUser = userService.GetParticipantByUserId(userId);

                EventDetails details = EventDetails.Create(eventData.EventDescription,
                                        eventData.Location,
                                        eventData.Deadline,
                                        eventData.OccurenceDate,
                                        eventData.MaximumParticipants,
                                        eventData.ParticipationFee,
                                        eventData.ApplicationRequired);

                var createdEvent = userService.AddEvent(creatingUser.Id,
                                       eventData.Title,
                                       eventData.Category,
                                       resultImage,
                                       details);

                ///The organizing user becomes a participant by default
                eventService.ApplyToEvent(createdEvent.Id, creatingUser.Id);

                return PartialView("_AddModifyEventPartial", eventData);
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
                var comments = eventService.GetComments(eventToGo.Id);
                var eventViewModel = GetEventViewModel(eventToGo);

                if (User.Identity.IsAuthenticated)
                {
                    var userId = userManager.GetUserId(User);
                    var participant = userService.GetParticipantByUserId(userId);
                    eventService.RegisterClick(eventGuid, participant.Id);
                }
                
                var detailsViewModel = new DetailsViewModel()
                {
                    Id = eventViewModel.Id,
                    Title = eventViewModel.Title,
                    Image = eventViewModel.Image,
                    HostName = eventViewModel.HostName,
                    HostEmail = eventViewModel.HostEmail,
                    HostPhoneNo = eventViewModel.HostPhoneNo,
                    Location = eventViewModel.Location,
                    ParticipationFee = eventViewModel.ParticipationFee,
                    Deadline = eventViewModel.Deadline,
                    OccurenceDate = eventViewModel.OccurenceDate,
                    OccurenceTime = eventViewModel.OccurenceTime,
                    Description = eventViewModel.Description,
                    MaximumParticipants = eventViewModel.MaximumParticipants,
                    Category = eventViewModel.Category,
                    HostProfileImage = eventViewModel.HostProfileImage,
                    UserName = eventViewModel.UserName,
                    UserProfileImage = eventViewModel.UserProfileImage,
                    Comments = comments
                };

                return PartialView("_DetailsPartial", detailsViewModel);
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
                var followingUser = userService.GetParticipantByUserId(userId);
                
                var previousFollowing = eventService.GetFollowing(eventToFollow.Id, followingUser.Id);

                if (previousFollowing is null)
                {
                    eventService.FollowEvent(eventToFollow.Id, followingUser.Id);
                }
                else
                {
                    eventService.RemoveInteraction(previousFollowing);
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
            try
            {
                Guid.TryParse(id, out Guid eventGuid);

                var userId = userManager.GetUserId(User);
                var applyingUser = userService.GetParticipantByUserId(userId);

                var eventAppliedTo = eventService.GetEventById(eventGuid);
                bool applied = false;

                var eventApplied = eventService.GetApplication(eventGuid, applyingUser.Id);
                var applicationText = eventService.GetApplicationText(eventGuid, applyingUser.Id);

                if (eventApplied != null)
                {
                    applied = true;
                }

                var applyViewModel = new ApplyToEventViewModel()
                {
                    EventId = id,
                    EventName = eventAppliedTo.Title,
                    AlreadyApplied = applied,
                    ApplicationRequired = eventAppliedTo.EventDetails.ApplicationRequired,
                    ApplicationText = applicationText
                };

                return PartialView("_ApplyToEventPartial", applyViewModel);
            } 
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public IActionResult Apply(ApplyToEventViewModel applyViewModel)
        {
            try
            {
                Guid.TryParse(applyViewModel.EventId, out Guid eventGuid);
                var eventToApply = eventService.GetEventById(eventGuid);

                var userId = userManager.GetUserId(User);
                var applyingUser = userService.GetParticipantByUserId(userId);

                var eventId = eventToApply.Id;
                var participantId = applyingUser.Id;

                var previousApplication = eventService.GetApplication(eventId, participantId);
                var previousFollowing = eventService.GetFollowing(eventId, participantId);

                if (previousFollowing is null && previousApplication is null)
                {
                    eventService.FollowEvent(eventId, participantId);
                }

                if (previousApplication is null)
                {
                    eventService.ApplyToEvent(eventId, participantId, applyViewModel.ApplicationText);
                }
                else
                {
                    eventService.RemoveInteraction(previousApplication);
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

                var editEventViewModel = new AddModifyEventViewModel()
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

                return PartialView("_AddModifyEventPartial", editEventViewModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public IActionResult Edit([FromForm] AddModifyEventViewModel updatedData)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_AddModifyEventPartial", new AddModifyEventViewModel());
            }

            try
            {
                Guid.TryParse(updatedData.Id, out Guid eventGuid);
                var eventToUpdate = eventService.GetEventById(eventGuid);

                string image = string.Empty;
                if (updatedData.EventImage != null)
                {
                    image = baseService.CompressImage(updatedData.EventImage);
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

                return PartialView("_AddModifyEventPartial", updatedData);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        public IActionResult RemoveComment(string Id)
        {
            var comment = eventService.GetCommentById(Id);
            eventService.RemoveInteraction(comment);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Remove([FromRoute]string Id)
        {
            Guid.TryParse(Id, out Guid eventGuid);
            var eventToDelete = eventService.GetEventById(eventGuid);

            RemoveEventViewModel removeViewModel = new RemoveEventViewModel()
            {
                Id = Id,
                EventName = eventToDelete.Title
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
        public IActionResult AddComment([FromForm] DetailsViewModel viewModel)
        {
            if (!ModelState.IsValid || viewModel is null || string.IsNullOrWhiteSpace(viewModel.NewCommentMessage))
            {
                return PartialView("_DetailsPartial", viewModel);
            }

            var userId = userManager.GetUserId(User);
            eventService.AddComment(userId, viewModel.NewCommentEventId, viewModel.NewCommentMessage);
            
            return PartialView("_DetailsPartial", viewModel);
        }

        private EventViewModel GetEventViewModel(EventOrganized organizedEvent)
        {
            Participant hostingUser = userService.GetCreatorById(organizedEvent.CreatorId);
            string participationFee = eventService.FormatParticipationFee(organizedEvent.EventDetails.ParticipationFee);
            string maximumParticipants = eventService.FormatMaximumParticipants(organizedEvent.EventDetails.MaximumParticipantNo);
            string currentUsername = null;
            string currentProfileImage = null;
            int? userScore = null;

            if (User.Identity.IsAuthenticated)
            {
                var userId = userManager.GetUserId(User);
                var currentUser = userService.GetParticipantByUserId(userId);
                currentUsername = eventService.FormatUserName(currentUser);
                currentProfileImage = currentUser.ProfileImage;
                userScore = eventService.GetUserRating(organizedEvent.Id, currentUser.Id)?.Score;
            }

            var eventViewModel = new EventViewModel
            {
                Id = organizedEvent.Id,
                Title = organizedEvent.Title,
                Image = organizedEvent.ImageByteArray,
                HostName = eventService.FormatUserName(hostingUser),
                CreatorId = hostingUser.Id.ToString(),
                HostEmail = hostingUser.ContactDetails.Email,
                HostPhoneNo = hostingUser.ContactDetails.PhoneNo,
                HostProfileImage = hostingUser.ProfileImage,
                Location = organizedEvent.EventDetails.Location,
                ParticipationFee = participationFee,
                Deadline = eventService.FormatEventDate(organizedEvent.EventDetails.Deadline),
                OccurenceDate = eventService.FormatEventDate(organizedEvent.EventDetails.OccurenceDate),
                OccurenceTime = eventService.FormatEventTime(organizedEvent.EventDetails.OccurenceDate),
                Description = organizedEvent.EventDetails.Description,
                MaximumParticipants = maximumParticipants,
                Category = organizedEvent.Category,
                UserName = currentUsername,
                UserProfileImage = currentProfileImage,
                Rating = userScore
            };
             
            return eventViewModel;
        }
    }
}