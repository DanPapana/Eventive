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
        public User EventHost { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ContactDetails ContactDetails { get; set; }
        public IEnumerable<Event> CreatedEvents { get;  set; }
        public ParticipationType UserParticipationType { get; set; }
    }
}
