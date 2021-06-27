using Eventive.ApplicationLogic.DataModel;
using Eventive.ApplicationLogic.Services;
using Eventive.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Eventive.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserService userService;
        private readonly EventService eventService;
        private readonly UserManager<IdentityUser> userManager;

        public ProfileController(UserManager<IdentityUser> userManager, UserService userService, EventService eventService)
        {
            this.userManager = userManager;
            this.userService = userService;
            this.eventService = eventService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetProfile()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var thisUserId = userManager.GetUserId(User);
                    var user = userService.GetParticipantByUserId(thisUserId);
                    var viewModel = GetUserProfileViewModel(user);

                    return PartialView("_ProfilePartial", viewModel);
                } 
                else
                {
                    return PartialView("_ProfilePartial", new UserProfileViewModel());
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [AllowAnonymous]
        public IActionResult UserApplication(string Id, Guid applicationId)
        {
            try
            {
                var user = userService.GetParticipantByUserId(Id);
                var profileViewModel = GetUserProfileViewModel(user);
                var application = eventService.GetApplication(applicationId);

                var viewModel = new UserApplicationViewModel()
                {
                    Profile = profileViewModel,
                    ApplicationMessage = application.ApplicationText
                };

                return View("_ApplicationPartial", viewModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        private UserProfileViewModel GetUserProfileViewModel(Participant user)
        {
            var viewModel = new UserProfileViewModel()
            {
                Id = user.Id.ToString(),
                FullName = $"{user.FirstName} {user.LastName}",
                ProfileImage = user.ProfileImage,
                Email = user.ContactDetails.Email,
                City = user.ContactDetails.City,
                Country = user.ContactDetails.Country,
                Address = user.ContactDetails.Address,
                PhoneNo = user.ContactDetails.PhoneNo,
                LinkToSocialM = user.ContactDetails.LinkToSocialM,
                Age = user.Age,
                Description = user.Description
            };

            return viewModel;
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            try
            {
                var thisUserId = userManager.GetUserId(User);
                Participant user = userService.GetParticipantByUserId(thisUserId);

                var editProfileViewModel = new EditProfileViewModel()
                {
                    Id = user.Id.ToString(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.ContactDetails.Address,
                    City = user.ContactDetails.City,
                    Country = user.ContactDetails.Country,
                    PhoneNo = user.ContactDetails.PhoneNo,
                    LinkToSocialM = user.ContactDetails.LinkToSocialM,
                    Age = user.Age,
                    Description = user.Description
                };

                return PartialView("_EditProfilePartial", editProfileViewModel);

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public IActionResult EditProfile([FromForm] EditProfileViewModel updatedData)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditProfilePartial", new EditProfileViewModel());
            }

            try
            {
                var thisUserId = userManager.GetUserId(User);
                Participant userToUpdate = userService.GetParticipantByUserId(thisUserId);

                string image = string.Empty;
                if (updatedData.ProfileImage != null)
                {
                    image = HelperService.CompressImage(updatedData.ProfileImage);
                }

                userService.UpdateParticipant(userToUpdate.Id,
                                        updatedData.FirstName,
                                        updatedData.LastName,
                                        image,
                                        updatedData.Address,
                                        updatedData.City,
                                        updatedData.Country,
                                        updatedData.PhoneNo,
                                        updatedData.LinkToSocialM,
                                        updatedData.Age,
                                        updatedData.Description);

                return PartialView("_EditProfilePartial", updatedData);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}