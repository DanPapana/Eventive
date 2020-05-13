using PAWEventive.ApplicationLogic.Abstraction;
using PAWEventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;

namespace PAWEventive.ApplicationLogic.Services
{
    public class EventService
    {

        private readonly IEventRepository eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public IEnumerable<Event> GetCurrentEvents()
        {
            return eventRepository.GetActiveEvents();
        }

        public Event GetEventById(Guid eventId)
        {            
            return eventRepository.GetEventById(eventId);
        }

        public Participation ParticipateInEvent(Guid eventId, Guid userId, Participation.Type type)
        {
            Participation participation = Participation.Create(eventId, userId, type);
            eventRepository.CreateParticipation(participation);
            return participation;
        }

        public Participation GetParticipation(Guid eventId, Guid userId, Participation.Type type)
        {
            return eventRepository.GetParticipation(eventId, userId, type);
        }

        public IEnumerable<Event> GetEventsForUser(Guid userId, Participation.Type type)
        {
            return eventRepository.GetEventsForUser(userId, type);
        }

        public IEnumerable<Guid> GetEventsGuidForUser(Guid userId, Participation.Type type)
        {
            return eventRepository.GetEventsGuidForUser(userId, type);
        }

        public bool RemoveParticipation(Guid eventId, Guid userId, Participation.Type type)
        {
            var participationToRemove = GetParticipation(eventId, userId, type);
            return eventRepository.RemoveParticipation(participationToRemove);
        }
    }
}
