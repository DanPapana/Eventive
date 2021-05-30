using Eventive.ApplicationLogic.Abstraction;
using Eventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;
using static Eventive.ApplicationLogic.DataModel.EventOrganized;

namespace Eventive.ApplicationLogic.Services
{
    public class EventService
    {
        private readonly IEventRepository eventRepository;
        private readonly IUserRepository userRepository;

        public EventService(IEventRepository eventRepository, IUserRepository userRepository)
        {
            this.eventRepository = eventRepository;
            this.userRepository = userRepository;
        }

        public IEnumerable<EventOrganized> GetCurrentEvents()
        {
            return eventRepository.GetActiveEvents();
        }

        public EventOrganized GetEventById(Guid eventId)
        {            
            return eventRepository.GetEventById(eventId);
        }

        public Interaction ParticipateInEvent(Guid eventId, Guid userId, Interaction.Type type)
        {
            Interaction participation = Interaction.Create(eventId, userId, type);
            eventRepository.CreateParticipation(participation);
            return participation;
        }

        public Interaction GetParticipation(Guid eventId, Guid userId, Interaction.Type type)
        {
            return eventRepository.GetParticipation(eventId, userId, type);
        }

        public IEnumerable<EventOrganized> GetEventsForUser(Guid userId, Interaction.Type type)
        {
            return eventRepository.GetEventsForUser(userId, type);
        }

        public IEnumerable<EventOrganized> GetPastEventsOfUser(Guid userId)
        {
            return eventRepository.GetPastEvents(userId);
        }

        public IEnumerable<Guid> GetEventsGuidForUser(Guid userId, Interaction.Type type)
        {
            return eventRepository.GetEventsGuidForUser(userId, type);
        }

        public IEnumerable<Comment> GetComments(Guid eventId)
        {
            return eventRepository.GetComments(eventId);
        }

        public Comment AddComment(string creatorId, Guid eventGuid, string message)
        {
            Guid.TryParse(creatorId, out Guid creatorGuid);
            var commenter = userRepository.GetUserByUserId(creatorGuid);

            var comment = Comment.Create(commenter, eventGuid, message);
            eventRepository.AddComment(comment);
            return comment;
        }

        public EventOrganized UpdateEvent(Guid eventId, string title,
                    EventCategory category,
                    string description,
                    string location,
                    DateTime deadline,
                    DateTime occurenceDate,
                    string image,
                    int maximumParticipants,
                    decimal fee,
                    bool applicationRequired)
        {

            var eventToUpdate = GetEventById(eventId);

            eventToUpdate.UpdateEvent(title, category, description, location, deadline, occurenceDate,
                                                image, maximumParticipants, fee, applicationRequired);
            eventRepository.Update(eventToUpdate);

            return eventToUpdate;
        }

        public bool RemoveParticipation(Guid eventId, Guid userId, Interaction.Type type)
        {
            var participationToRemove = GetParticipation(eventId, userId, type);
            return eventRepository.RemoveParticipation(participationToRemove);
        }

        public bool RemoveEvent(string Id)
        {
            Guid.TryParse(Id, out Guid eventGuid);
            return eventRepository.RemoveEvent(eventGuid);
        }

        public Interaction RegisterClick(Guid eventId, Guid participantId)
        {
            var newInteraction = Interaction.Create(eventId, participantId, Interaction.Type.Click);
            eventRepository.CreateParticipation(newInteraction);
            return newInteraction;
        }
    }
}
