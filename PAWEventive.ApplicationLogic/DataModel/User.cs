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
        public byte[] ProfileImageByteArray { get; set; }
        public string SocialId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ContactDetails ContactDetails { get; set; }

        public static User CreateUser(Guid userId, string firstName, string lastName, string socialId)
        {
            var newUser = new User()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FirstName = firstName,
                LastName = lastName,
                SocialId = socialId,
                ContactDetails = new ContactDetails()
            };
            return newUser;
        }
    }
}