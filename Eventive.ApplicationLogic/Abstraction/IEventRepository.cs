using Eventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;

namespace Eventive.ApplicationLogic.Abstraction
{
    public interface IEventRepository: IRepository<EventOrganized>
    {
        IEnumerable<EventOrganized> GetActiveEvents();
        IEnumerable<EventOrganized> GetEventsByCategory(EventOrganized.EventCategory eventCategory);
        EventOrganized GetEventById(Guid eventId);
        Interaction GetParticipation(Guid eventId, Guid userId, Interaction.Type type);
        IEnumerable<EventOrganized> GetEventsForUser(Guid userId, Interaction.Type type);
        IEnumerable<Guid> GetEventsGuidForUser(Guid userId, Interaction.Type type);
        Comment AddComment(Comment comment);
        Interaction CreateParticipation(Interaction participation);
        IEnumerable<EventOrganized> GetPastEvents(Guid userId);
        IEnumerable<Comment> GetComments(Guid eventId);
        bool RemoveParticipation(Interaction participationToRemove);
        bool RemoveEvent(Guid eventId);
    }
}
