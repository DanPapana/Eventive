using Eventive.ApplicationLogic.Abstraction;
using Eventive.ApplicationLogic.DataModel;
using Eventive.ApplicationLogic.Exceptions;
using System;
using System.Collections.Generic;
using static Eventive.ApplicationLogic.DataModel.EventOrganized;

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

        private Guid ParseId(string userId)
        {
            if (!Guid.TryParse(userId, out Guid userIdGuid))
            {
                throw new Exception("Invalid Guid Format");
            }
            return userIdGuid;
        }

        public Participant GetCreatorById(Guid participantId)
        {
            var user = userRepository.GetParticipantByGuid(participantId);
            if (user is null)
            {
                throw new EntityNotFoundException(participantId);
            }

            return user;
        }

        public Participant GetParticipantByUserId(string userId)
        {
            Guid userGuid = ParseId(userId);
            var participant = userRepository.GetUserByUserId(userGuid);
            if (participant is null)
            {
                throw new EntityNotFoundException(userGuid);
            }

            return participant;
        }

        public IEnumerable<EventOrganized> GetUserEvents(Guid participantId)
        {
            return userRepository.GetEventsCreatedByUser(participantId);
        }

        public Participant RegisterNewUser(string userId, string firstName, string lastName, string country, string city, string email)
        {
            var newUser = Participant.CreateUser(Guid.Parse(userId), firstName, lastName, country, city, email);
            newUser = userRepository.Add(newUser);
            userRepository.Update(newUser);
            return newUser;
        }

        public Participant UpdateParticipant(Guid participantId, string firstName, string lastName,
                                string profileImage, string address,
                                string city, string country,
                                string phoneNo, string linkToSocialM, int age, string description)
        {
            var userToUpdate = userRepository.GetParticipantByGuid(participantId);

            userToUpdate.UpdateUser(firstName, lastName, profileImage, 
                address, city, country, phoneNo, linkToSocialM, age, description);
            
            userRepository.Update(userToUpdate);

            return userToUpdate;
        }

        public EventOrganized AddEvent(Guid creatorId, string title, EventCategory category, 
                            string image, EventDetails details)
        {
            GetCreatorById(creatorId);

            var newEventOrganized = Create(
                creatorId,
                title,
                image,
                category,
                details
            );

            eventRepository.Add(newEventOrganized);
            return newEventOrganized;
        }
    }
}
