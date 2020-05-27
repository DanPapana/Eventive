using Microsoft.AspNetCore.Http;
using PAWEventive.ApplicationLogic.DataModel;
using PAWEventive.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAWEventive.Models.Users
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string DateOfBirth { get; set; }
        public string ProfileImage { get; set; }
        public string CityCountry { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string LinkToSocialM { get; set; }
    }
}
