using Eventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;
using static Eventive.ApplicationLogic.DataModel.EventOrganized;

namespace Eventive.ApplicationLogic.Abstraction
{
    public interface IEventRepository: IRepository<EventOrganized>
    {
        EventOrganized GetEventById(Guid eventId);
        IEnumerable<EventOrganized> GetActiveEvents();
        IEnumerable<EventOrganized> GetActiveEvents(Guid? participantId);
        IEnumerable<EventOrganized> GetActiveEvents(EventCategory eventCategory);
        IEnumerable<EventOrganized> GetActiveEvents(EventCategory eventCategory, Guid? participantId);
        IEnumerable<EventOrganized> GetEventsToRecommend(Guid participantId);

        IEnumerable<Comment> GetEventComments(Guid eventId);
        IEnumerable<EventRating> GetEventRatings(Guid eventId);
        IEnumerable<EventApplication> GetEventApplications(Guid eventId);
        IEnumerable<EventFollowing> GetEventFollowings(Guid eventId);
        IEnumerable<EventClick> GetEventClicks(Guid eventId);

        UserBehaviour GetUserBehaviour(Guid participantId);
        Comment GetCommentById(Guid commentId);
        IEnumerable<EventClick> GetClicks(Guid eventId, Guid participantId);
        EventApplication GetApplication(Guid eventId, Guid participantId);
        EventFollowing GetFollowing(Guid eventId, Guid participantId);
        EventRating GetUserRating(Guid eventId, Guid participantId);

        int GetClicksPerCategoryForUser(Guid participantId, EventCategory category);
        int GetTotalNumberOfClicksForUser(Guid participantId);
        IEnumerable<EventRating> GetUserRatings(Guid participantId);
        IEnumerable<EventApplication> GetUserApplications(Guid participantId);
        IEnumerable<EventFollowing> GetUserFollowings(Guid participantId);
        IEnumerable<Guid> GetAppliedEventsGuidForUser(Guid participantId);
        IEnumerable<Guid> GetFollowedEventsGuidForUser(Guid participantId);
        IEnumerable<EventOrganized> GetAppliedEventsForUser(Guid userId);
        IEnumerable<EventOrganized> GetFollowedEventsForUser(Guid userId);
        IEnumerable<EventOrganized> GetPastAppliedEvents(Guid userId);

        Comment AddInteraction(Comment comment);
        EventRating AddInteraction(EventRating eventRating);
        EventFollowing AddInteraction(EventFollowing followInteraction);
        EventApplication AddInteraction(EventApplication applyInteraction);
        EventClick AddInteraction(EventClick clickInteraction);
        UserBehaviour AddUserBehaviour(UserBehaviour userBehaviour);
        UserBehaviour Update(UserBehaviour userBehaviour);

        bool RemoveInteraction(IEventInteraction interactionToRemove);
        bool RemoveEvent(Guid eventId);
        string GetUserApplicationText(Guid eventId, Guid participantId);
    }
}
