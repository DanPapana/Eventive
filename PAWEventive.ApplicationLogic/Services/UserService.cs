using PAWEventive.ApplicationLogic.Abstraction;
using PAWEventive.ApplicationLogic.DataModel;
using PAWEventive.ApplicationLogic.Exceptions;
using System;
using System.Collections.Generic;
using static PAWEventive.ApplicationLogic.DataModel.Event;

namespace PAWEventive.ApplicationLogic.Services
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

        public User RegisterNewUser(string userId, string firstName, string lastName, string socialId)
        {
            var newUser = User.CreateUser(Guid.Parse(userId), firstName, lastName, socialId);
            newUser = userRepository.Add(newUser);
            userRepository.Update(newUser);
            return newUser;
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
