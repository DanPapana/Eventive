using Eventive.ApplicationLogic.Abstraction;
using Eventive.ApplicationLogic.DataModel;
using Eventive.ApplicationLogic.Exceptions;
using System;
using System.Collections.Generic;
using static Eventive.ApplicationLogic.DataModel.Event;

namespace Eventive.ApplicationLogic.Services
{
    public class UserService
    {
        private readonly IUserRepository userRepository;
        private readonly IEventRepository eventRepository;

        public UserService(IUserRepository userRepository, IEventRepository eventRepository)
        {
            this.userRepository = userRepository;
            this.eventRepository = eventRepository;
        }

        private Guid ParseID(string userId)
        {
            if (!Guid.TryParse(userId, out Guid userIdGuid))
            {
                throw new Exception("Invalid Guid Format");
            }
            return userIdGuid;
        }

        public User GetCreatorByGuid(Guid userGuid)
        {
            var user = userRepository.GetUserByGuid(userGuid);
            if (user == null)
            {
                throw new EntityNotFoundException(userGuid);
            }

            return user;
        }

        public User GetUserByUserId(string userId)
        {
            Guid userIdGuid = ParseID(userId);
            return userRepository.GetUserByUserId(userIdGuid);
        }

        public IEnumerable<Event> GetUserEvents(string userId)
        {
            Guid userIdGuid = ParseID(userId);
            GetCreatorByGuid(userIdGuid);
            return userRepository.GetEventsCreatedByUser(userIdGuid);
        }

        public User RegisterNewUser(string userId, string firstName, string lastName, string socialId, string email)
        {
            var newUser = User.CreateUser(Guid.Parse(userId), firstName, lastName, socialId, email);
            newUser = userRepository.Add(newUser);
            userRepository.Update(newUser);
            return newUser;
        }

        public User UpdateUser(Guid updateId, string firstName, string lastName,
                                string profileImage, string address,
                                string city, string country,
                                string phoneNo, string email,
                                string linkToSocialM)
        {
            var userToUpdate = userRepository.GetUserByGuid(updateId);

            userToUpdate.UpdateUser(firstName, lastName, profileImage, 
                address, city, country, phoneNo, email, linkToSocialM);
            
            userRepository.Update(userToUpdate);

            return userToUpdate;
        }

        public void AddEvent(Guid creatorId, string title, EventCategory category, 
                            string image, EventDetails details)
        {
            GetCreatorByGuid(creatorId);

            eventRepository.Add(new Event() { 
                Id = Guid.NewGuid(), 
                Title = title, 
                CreatorId = creatorId, 
                Category = category,
                EventDetails = details,
                ImageByteArray = image
            });
        }
    }
}
