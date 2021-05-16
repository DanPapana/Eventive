using System;
using System.Collections.Generic;
using System.Text;

namespace PAWEventive.ApplicationLogic.DataModel
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImage { get; set; }
        public string SocialId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ContactDetails ContactDetails { get; set; }

        public static User CreateUser(Guid userId, string firstName, string lastName, string socialId, string email)
        {
            var newUser = new User()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FirstName = firstName,
                LastName = lastName,
                SocialId = socialId,
                ContactDetails = new ContactDetails(email)
            };
            return newUser;
        }

        public User UpdateUser(string firstName,        
                                string lastName, 
                                string profileImage,    
                                string address, 
                                string city,            
                                string country, 
                                string phoneNo,         
                                string email,
                                string linkToSocialM)
        {
            if (profileImage != null && profileImage.Length > 1)
            {
                ProfileImage = profileImage;
            }

            FirstName = firstName;
            LastName = lastName;

            ContactDetails.UpdateDetails(address, city, country, phoneNo, email, linkToSocialM);

            return this;
        }
    }
}