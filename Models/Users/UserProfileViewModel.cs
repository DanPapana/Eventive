using PAWEventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PAWEventive.ApplicationLogic.DataModel.Participation;

namespace PAWEventive.Models.Users
{
    public class UserProfileViewModel
    {
        public string FullName { get; set; }
        public string DateOfBirth { get; set; }
        public byte[] ProfileImageByteArray { get; set; }
        public string CityCountry { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string LinkToSocialM { get; set; }
        public IEnumerable<Event> CreatedEvents { get;  set; }
        public IEnumerable<Event> FollowingEvents { get; set; }
        public IEnumerable<Event> AppliedEvents { get; set; }
    }
}
