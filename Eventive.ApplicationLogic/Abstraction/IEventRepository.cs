using Eventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;

namespace Eventive.ApplicationLogic.Abstraction
{
    public interface IEventRepository: IRepository<EventOrganized>
    {
        EventOrganized GetEventById(Guid eventId);
        IEnumerable<EventOrganized> GetActiveEvents(Guid? participantId);
        IEnumerable<EventOrganized> GetEventsByCategory(EventOrganized.EventCategory eventCategory);
        IEnumerable<EventClick> GetClicks(Guid eventId, Guid participantId);
        IEnumerable<Comment> GetComments(Guid eventId);
        Comment GetCommentById(Guid commentId);
        EventApplication GetApplication(Guid eventId, Guid participantId);
        EventFollowing GetFollowing(Guid eventId, Guid participantId);
        EventRating GetUserRating(Guid eventId, Guid participantId);
        IEnumerable<EventRating> GetEventRatings(Guid eventId);
        IEnumerable<EventRating> GetUserRatings(Guid participantId);
        IEnumerable<EventApplication> GetUserApplications(Guid participantId);
        IEnumerable<EventFollowing> GetUserFollowings(Guid participantId);
        IEnumerable<Guid> GetAppliedEventsGuidForUser(Guid participantId);
        IEnumerable<Guid> GetFollowedEventsGuidForUser(Guid participantId);
        Comment AddInteraction(Comment comment);
        EventRating AddInteraction(EventRating eventRating);
        EventFollowing AddInteraction(EventFollowing followInteraction);
        EventApplication AddInteraction(EventApplication applyInteraction);
        EventClick AddInteraction(EventClick clickInteraction);
        IEnumerable<EventOrganized> GetAppliedEventsForUser(Guid userId);
        IEnumerable<EventOrganized> GetFollowedEventsForUser(Guid userId);
        IEnumerable<EventOrganized> GetPastAppliedEvents(Guid userId);
        bool RemoveInteraction(IEventInteraction interactionToRemove);
        bool RemoveEvent(Guid eventId);
        string GetUserApplicationText(Guid eventId, Guid participantId);
    }
}
