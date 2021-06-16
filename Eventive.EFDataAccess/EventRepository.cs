using Microsoft.EntityFrameworkCore;
using Eventive.ApplicationLogic.Abstraction;
using Eventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eventive.EFDataAccess
{
    public class EventRepository : BaseRepository<EventOrganized>, IEventRepository
    {
        public EventRepository(EventManagerDbContext dbContext) : base(dbContext)
        {
        }
        
        public EventOrganized GetEventById(Guid eventId)
        {
            return dbContext.Events.Include(ev => ev.EventDetails)
                    .Include(evnt => evnt.Comments)
                    .Include(evnt => evnt.Applications)
                    .Include(evnt => evnt.Followings)
                    .Include(evnt => evnt.Clicks)
                    .Include(evnt => evnt.Ratings)
                    .Where(evnt => evnt.Id == eventId)
                    .FirstOrDefault();
        }

        public IEnumerable<EventOrganized> GetActiveEvents(Guid? participantId = null)
        {
            return dbContext.Events
                    .Include(evnt => evnt.EventDetails)
                    .Include(evnt => evnt.Comments)
                    .Where(evnt => evnt
                    .EventDetails.Deadline > DateTime.Now
                        && evnt.CreatorId != participantId)
                    .OrderBy(evnt => evnt.EventDetails.Deadline)
                    .AsEnumerable();
        }

        public IEnumerable<EventOrganized> GetActiveEvents(EventOrganized.EventCategory eventCategory, Guid? participantId = null)
        {
            return dbContext.Events
                    .Include(evnt => evnt.EventDetails)
                    .Include(evnt => evnt.Comments)
                    .Where(evnt => evnt
                    .EventDetails.Deadline > DateTime.Now
                        && evnt.CreatorId != participantId
                        && evnt.Category == eventCategory)
                    .OrderBy(evnt => evnt.EventDetails.Deadline)
                    .AsEnumerable();
        }

        public EventApplication GetApplication(Guid eventId, Guid participantId)
        {
            return dbContext.Applications
                    .Include(e => e.Participant)
                    .Include(e => e.EventOrganized)
                    .Where(ev => ev.Participant.Id == participantId
                        && ev.EventOrganized.Id == eventId)
                    .FirstOrDefault();
        }

        public EventFollowing GetFollowing(Guid eventId, Guid participantId)
        {
            return dbContext.Followings
                    .Include(e => e.Participant)
                    .Include(e => e.EventOrganized)
                    .Where(ev => ev.Participant.Id == participantId
                        && ev.EventOrganized.Id == eventId)
                    .FirstOrDefault();
        }

        public IEnumerable<EventApplication> GetUserApplications(Guid participantId)
        {
            return dbContext.Applications
                    .Include(e => e.Participant)
                    .Include(e => e.EventOrganized)
                    .Where(app => app.Participant.Id == participantId)
                    .AsEnumerable();
        }

        public IEnumerable<EventFollowing> GetUserFollowings(Guid participantId)
        {
            return dbContext.Followings
                    .Include(e => e.Participant)
                    .Include(e => e.EventOrganized)
                    .Where(follow => follow.Participant.Id == participantId)
                    .AsEnumerable();
        }

        public IEnumerable<Comment> GetEventComments(Guid eventId)
        {
            return dbContext.Comments
                    .Include(e => e.Participant)
                    .ThenInclude(e => e.ContactDetails)
                    .Include(e => e.EventOrganized)
                    .Where(e => e.EventOrganized.Id.Equals(eventId))
                    .AsEnumerable();
        }

        public IEnumerable<EventClick> GetClicks(Guid eventId, Guid participantId)
        {
            return dbContext.Clicks
                    .Include(e => e.Participant)
                    .Include(e => e.EventOrganized)
                    .Where(e => e.EventOrganized.Id.Equals(eventId)
                        && e.Participant.Id.Equals(participantId))
                    .AsEnumerable();                
        }

        public IEnumerable<EventRating> GetEventRatings(Guid eventId)
        {
            return dbContext.Ratings
                    .Include(e => e.Participant)
                    .Include(e => e.EventOrganized)
                    .Where(e => e.EventOrganized.Id.Equals(eventId))
                    .AsEnumerable();
        }

        public IEnumerable<EventRating> GetUserRatings(Guid participantId)
        {
            return dbContext.Ratings
                    .Include(e => e.Participant)
                    .Include(e => e.EventOrganized)
                    .Where(e => e.Participant.Id.Equals(participantId))
                    .AsEnumerable();
        }

        public IEnumerable<EventOrganized> GetAppliedEventsForUser(Guid participantId)
        {
            List<EventOrganized> appliedEvents = new List<EventOrganized>();

            var applications = GetUserApplications(participantId);

            foreach (EventApplication application in applications)
            {
                appliedEvents.Add(GetEventById(application.EventOrganized.Id));
            }

            return appliedEvents.AsEnumerable();
        }

        public IEnumerable<EventOrganized> GetFollowedEventsForUser(Guid participantId)
        {
            List<EventOrganized> followedEvents = new List<EventOrganized>();

            var followings = GetUserFollowings(participantId);

            foreach (EventFollowing follow in followings)
            {
                followedEvents.Add(GetEventById(follow.EventOrganized.Id));
            }

            return followedEvents.AsEnumerable();
        }

        public IEnumerable<Guid> GetAppliedEventsGuidForUser(Guid participantId)
        {
            List<Guid> appliedEventIds = new List<Guid>();

            var applications = GetUserApplications(participantId);

            foreach (IEventInteraction application in applications)
            {
                appliedEventIds.Add(application.EventOrganized.Id);
            }

            return appliedEventIds.AsEnumerable();
        }

        public IEnumerable<Guid> GetFollowedEventsGuidForUser(Guid participantId)
        {
            List<Guid> followedEventIds = new List<Guid>();

            var followings = GetUserFollowings(participantId);

            foreach (IEventInteraction follow in followings)
            {
                followedEventIds.Add(follow.EventOrganized.Id);
            }

            return followedEventIds.AsEnumerable();
        }

        public IEnumerable<EventOrganized> GetPastAppliedEvents(Guid participantId)
        {
            var allPastEvents = dbContext.Events
                    .Include(evnt => evnt.EventDetails)
                    .Include(evnt => evnt.Comments)
                    .Include(evnt => evnt.Followings)
                    .Include(evnt => evnt.Applications)
                    .ThenInclude(app => app.Participant)
                    .Where(evnt => evnt
                        .EventDetails.Deadline < DateTime.UtcNow
                        && evnt.Applications.Count > 0)
                    .OrderBy(evnt => evnt.EventDetails.Deadline)
                    .AsEnumerable();

            List<EventOrganized> pastEventsList = new List<EventOrganized>();

            foreach (var pastEvent in allPastEvents)
            {
                ///Wouldn't make sense for the organizer to rate his own event
                if (pastEvent.CreatorId != participantId) 
                {
                    foreach (var application in pastEvent.Applications)
                    {
                        if (application.Participant.Id.Equals(participantId))
                        {
                            pastEventsList.Add(pastEvent);
                        }
                    }
                }
            }

            return pastEventsList;
        }

        public string GetUserApplicationText(Guid eventId, Guid participantId)
        {
            var appliedEvent = GetEventById(eventId);
            var application = dbContext.Applications
                    .Include(e => e.Participant)
                    .Include(e => e.EventOrganized)
                    .Where(ap => ap.EventOrganized.Id == eventId
                        && ap.Participant.Id == participantId)
                    .FirstOrDefault();

            if (!(application is null))
            {
                return application.ApplicationText;
            }

            return null;
        }

        public EventRating GetUserRating(Guid eventId, Guid participantId)
        {
            return dbContext.Ratings
                    .Include(e => e.EventOrganized)
                    .Include(e => e.Participant)
                    .Where(e => e.EventOrganized.Id.Equals(eventId)
                        && e.Participant.Id.Equals(participantId))
                    .FirstOrDefault();
        }

        public Comment GetCommentById(Guid commentId)
        {
            return dbContext.Comments
                    .Include(com => com.Participant)
                    .Include(com => com.EventOrganized)
                    .Where(com => com.Id.Equals(commentId))
                    .FirstOrDefault();
        }

        public EventFollowing AddInteraction(EventFollowing followInteraction)
        {
            dbContext.Followings.Add(followInteraction);
            SaveChanges();
            return followInteraction;
        }

        public EventApplication AddInteraction(EventApplication applyInteraction)
        {
            dbContext.Applications.Add(applyInteraction);
            SaveChanges();
            return applyInteraction;
        }

        public EventClick AddInteraction(EventClick clickInteraction)
        {
            dbContext.Clicks.Add(clickInteraction);
            SaveChanges();
            return clickInteraction;
        }

        public EventRating AddInteraction(EventRating eventRating)
        {
            dbContext.Ratings.Add(eventRating);
            SaveChanges();
            return eventRating;
        }

        public Comment AddInteraction(Comment comment)
        {
            dbContext.Comments.Add(comment);
            SaveChanges();
            return comment;
        }

        public bool RemoveInteraction(IEventInteraction interactionToRemove)
        {
            if (interactionToRemove is null)
            {
                return false;
            }
            
            dbContext.Remove(interactionToRemove);
            SaveChanges();
            return true;
        }

        public bool RemoveEvent(Guid eventId)
        {
            var eventToRemove = GetEventById(eventId);
            if (eventToRemove != null)
            {
                dbContext.RemoveRange(eventToRemove?.Comments);
                dbContext.RemoveRange(eventToRemove?.Clicks);
                dbContext.RemoveRange(eventToRemove?.Applications);
                dbContext.RemoveRange(eventToRemove?.Followings);
                dbContext.RemoveRange(eventToRemove?.Ratings);
                dbContext.Remove(eventToRemove?.EventDetails);
                dbContext.Remove(eventToRemove);
                SaveChanges();
                
                return true;
            }

            return false;
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }

        public IEnumerable<EventApplication> GetEventApplications(Guid eventId)
        {
            return dbContext.Applications
                    .Include(e => e.Participant)
                    .Include(e => e.EventOrganized)
                    .Where(e => e.EventOrganized.Id.Equals(eventId))
                    .AsEnumerable();
        }

        public IEnumerable<EventFollowing> GetEventFollowings(Guid eventId)
        {
            return dbContext.Followings
                    .Include(e => e.Participant)
                    .Include(e => e.EventOrganized)
                    .Where(e => e.EventOrganized.Id.Equals(eventId))
                    .AsEnumerable();
        }

        public IEnumerable<EventClick> GetEventClicks(Guid eventId)
        {
            return dbContext.Clicks
                    .Include(e => e.Participant)
                    .Include(e => e.EventOrganized)
                    .Where(e => e.EventOrganized.Id.Equals(eventId))
                    .AsEnumerable();
        }
    }
}
