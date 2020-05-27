using PAWEventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PAWEventive.ApplicationLogic.Abstraction
{
    public interface IEventRepository: IRepository<Event>
    {
        IEnumerable<Event> GetActiveEvents();
        IEnumerable<Event> GetEventsByCategory(Event.EventCategory eventCategory);
        Event GetEventById(Guid eventId);
        Participation GetParticipation(Guid eventId, Guid userId, Participation.Type type);
        IEnumerable<Event> GetEventsForUser(Guid userId, Participation.Type type);
        IEnumerable<Guid> GetEventsGuidForUser(Guid userId, Participation.Type type);
        Participation CreateParticipation(Participation participation);
        IEnumerable<Event> GetPastEvents(Guid userId);
        bool RemoveParticipation(Participation participationToRemove);
        bool RemoveEvent(Guid eventId);
    }
}
