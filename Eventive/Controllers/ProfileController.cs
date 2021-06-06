using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Eventive.ApplicationLogic.DataModel;
using Eventive.ApplicationLogic.Services;
using Eventive.Models.Users;
using System;
using System.IO;

namespace Eventive.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserService userService;
        private readonly UserManager<IdentityUser> userManager;

        public ProfileController(UserManager<IdentityUser> userManager, UserService userService)
        {
            this.userManager = userManager;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetProfile()
        {
            try
            {
                var thisUserId = userManager.GetUserId(User);
                Participant user = userService.GetParticipantByUserId(thisUserId);

                UserProfileViewModel viewModel = new UserProfileViewModel()
                {
                    Id = user.Id.ToString(),
                    DateOfBirth = $"{user.DateOfBirth: dd MMMM yyyy}",
                    FullName = $"{user.FirstName} {user.LastName}",
                    ProfileImage = user.ProfileImage,
                    Email = user.ContactDetails.Email,
                    CityCountry = $"{user.ContactDetails.City}, {user.ContactDetails.Country}",
                    PhoneNo = user.ContactDetails.PhoneNo,
                    LinkToSocialM = user.ContactDetails.LinkToSocialM
                };

                if (user.DateOfBirth.Equals(DateTime.MinValue))
                {
                    viewModel.DateOfBirth = null;
                }

                return PartialView("_ProfilePartial", viewModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
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
                    City = user.ContactDetails.City,
                    Country = user.ContactDetails.Country,
                    Email = user.ContactDetails.Email,
                    PhoneNo = user.ContactDetails.PhoneNo,
                    LinkToSocialM = user.ContactDetails.LinkToSocialM
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
                    using var memoryStream = new MemoryStream();
                    updatedData.ProfileImage.CopyTo(memoryStream);
                    image = Convert.ToBase64String(memoryStream.ToArray());
                }

                userService.UpdateParticipant(userToUpdate.Id,
                                        updatedData.FirstName,
                                        updatedData.LastName,
                                        image,
                                        updatedData.Address,
                                        updatedData.City,
                                        updatedData.Country,
                                        updatedData.PhoneNo,
                                        updatedData.Email,
                                        updatedData.LinkToSocialM);

                return PartialView("_EditProfilePartial", updatedData);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}