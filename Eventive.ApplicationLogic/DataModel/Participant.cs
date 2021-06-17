using System;
using System.Collections.Generic;

namespace Eventive.ApplicationLogic.DataModel
{
    public class Participant
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImage { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ContactDetails ContactDetails { get; set; }

        public static Participant CreateUser(Guid userId, string firstName, string lastName, string country, string city, string email)
        {
            var newUser = new Participant()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FirstName = firstName,
                LastName = lastName,
                ContactDetails = new ContactDetails(email, country, city)
            };

            return newUser;
        }

        public Participant UpdateUser(string firstName,        
                                string lastName, 
                                string profileImage,    
                                string address, 
                                string city,            
                                string country, 
                                string phoneNo,
                                string linkToSocialM)
        {
            if (!string.IsNullOrEmpty(profileImage))
            {
                ProfileImage = profileImage;
            }

            FirstName = firstName;
            LastName = lastName;

            ContactDetails.UpdateDetails(address, city, country, phoneNo, linkToSocialM);

            return this;
        }
    }
}