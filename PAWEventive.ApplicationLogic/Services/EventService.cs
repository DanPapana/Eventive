using PAWEventive.ApplicationLogic.Abstraction;
using PAWEventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;
using static PAWEventive.ApplicationLogic.DataModel.Event;

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

        public IEnumerable<Event> GetPastEventsOfUser(Guid userId)
        {
            return eventRepository.GetPastEvents(userId);
        }

        public IEnumerable<Guid> GetEventsGuidForUser(Guid userId, Participation.Type type)
        {
            return eventRepository.GetEventsGuidForUser(userId, type);
        }

        public Event UpdateEvent(Guid eventId, string title,
                    EventCategory category,
                    string description,
                    string location,
                    DateTime deadline,
                    string image,
                    int maximumParticipants,
                    decimal fee)
        {

            var eventToUpdate = GetEventById(eventId);

            eventToUpdate.UpdateEvent(title, category, description, location, deadline, 
                                                image, maximumParticipants, fee);
            eventRepository.Update(eventToUpdate);

            return eventToUpdate;
        }

        public bool RemoveParticipation(Guid eventId, Guid userId, Participation.Type type)
        {
            var participationToRemove = GetParticipation(eventId, userId, type);
            return eventRepository.RemoveParticipation(participationToRemove);
        }

        public bool RemoveEvent(string Id)
        {
            Guid.TryParse(Id, out Guid eventGuid);
            return eventRepository.RemoveEvent(eventGuid);
        }
    }
}
