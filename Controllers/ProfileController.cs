using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using PAWEventive.ApplicationLogic.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PAWEventive.Models.Users;
using Microsoft.AspNetCore.Authorization;
using PAWEventive.ApplicationLogic.DataModel;

namespace PAWEventive.Controllers
{
    public class ProfileController : Controller
    {
        private readonly EventService eventService;
        private readonly UserService userService;
        private readonly UserManager<IdentityUser> userManager;

        public ProfileController(UserManager<IdentityUser> userManager, EventService eventService, UserService userService)
        {
            this.userManager = userManager;
            this.eventService = eventService;
            this.userService = userService;
        }

        [Authorize]
        public ActionResult Index()
        {
            try
            {
                var thisUserId = userManager.GetUserId(User);
                User user = userService.GetUserByUserId(thisUserId);

                //List<Event> createdEvents = new List<Event>();

                //foreach (Event oneEvent in userService.GetUserEvents(user.Id.ToString()))
                //{
                //    createdEvents.Add(eventService.GetEventById(oneEvent.Id));
                //}

                IEnumerable<Event> createdEvents = userService.GetUserEvents(user.Id.ToString());

                IEnumerable<Event> followingEvents = eventService
                            .GetEventsForUser(user.Id, Participation.Type.Following);

                IEnumerable<Event> appliedEvents = eventService
                            .GetEventsForUser(user.Id, Participation.Type.Applied);

                UserProfileViewModel viewModel = new UserProfileViewModel()
                {
                    DateOfBirth = $"{user.DateOfBirth:MMMM dd yyyy} 🎂",
                    FullName = $"{user.FirstName} {user.LastName}",
                    ProfileImageByteArray = user.ProfileImageByteArray,
                    Email = user.ContactDetails.Email,
                    CityCountry = $"{user.ContactDetails.City}, {user.ContactDetails.Country}",
                    PhoneNo = user.ContactDetails.PhoneNo,
                    LinkToSocialM = user.ContactDetails.LinkToSocialM,
                    CreatedEvents = createdEvents,
                    FollowingEvents = followingEvents,
                    AppliedEvents = appliedEvents
                };

                return View(viewModel);
            }

            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
