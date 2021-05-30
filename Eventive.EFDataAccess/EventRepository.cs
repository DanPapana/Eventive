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

        public IEnumerable<EventOrganized> GetActiveEvents()
        {
            return dbContext.Events
                .Include(evnt => evnt.EventDetails)
                .Include(evnt => evnt.Comments)
                .Where(evnt => evnt
                .EventDetails.Deadline > DateTime.UtcNow)
                .OrderBy(evnt => evnt.EventDetails.Deadline)
                .AsEnumerable();
        }

        public IEnumerable<EventOrganized> GetPastEvents(Guid userId)
        {
            return dbContext.Events.Include(evnt => evnt.EventDetails).Include(evnt => evnt.Comments)
                .Where(evnt => evnt
                .EventDetails.Deadline < DateTime.UtcNow 
                && evnt.CreatorId == userId)
                .OrderBy(evnt => evnt.EventDetails.Deadline)
                .AsEnumerable();
        }

        public EventOrganized GetEventById(Guid eventId)
        {
            return dbContext.Events.Include(ev => ev.EventDetails)
                    .Include(evnt => evnt.Comments)
                    .Where(evnt => evnt.Id == eventId)
                    .SingleOrDefault();
        }

        public IEnumerable<EventOrganized> GetEventsByCategory(EventOrganized.EventCategory eventCategory)
        {
            return dbContext.Events
                .Where(evnt => evnt.Category == eventCategory);
        }

        public Interaction GetParticipation(Guid eventId, Guid userId, Interaction.Type type)
        {
            return dbContext.Interactions
                        .Where(ev => ev.ParticipantId == userId 
                        && ev.UserParticipationType == type 
                        && ev.EventOrganizedId == eventId)
                        .FirstOrDefault();
        }

        private IEnumerable<Interaction> GetUserParticipations(Guid userId, Interaction.Type type)
        {
            return dbContext.Interactions
                .Where(p => p.UserParticipationType == type
                && p.ParticipantId == userId)
                .AsEnumerable();
        }

        public IEnumerable<Comment> GetComments(Guid eventId)
        {
            return GetEventById(eventId).Comments;
        }

        public IEnumerable<EventOrganized> GetEventsForUser(Guid userId, Interaction.Type type)
        {
            List<EventOrganized> participatedEvents = new List<EventOrganized>();

            var participations = GetUserParticipations(userId, type);

            foreach (Interaction part in participations)
            {
                participatedEvents.Add(GetEventById(part.EventOrganizedId));
            }

            return participatedEvents.AsEnumerable();
        }

        public IEnumerable<Guid> GetEventsGuidForUser(Guid userId, Interaction.Type type)
        {
            List<Guid> participatedEvents = new List<Guid>();

            var participations = GetUserParticipations(userId, type);

            foreach (Interaction part in participations)
            {
                participatedEvents.Add(part.EventOrganizedId);
            }

            return participatedEvents.AsEnumerable();
        }

        public Interaction CreateParticipation(Interaction participation)
        {
            dbContext.Interactions.Add(participation);
            SaveChanges();
            return participation;
        }

        public Comment AddComment(Comment comment)
        {
            dbContext.Comments.Add(comment);
            SaveChanges();
            return comment;
        }

        public bool RemoveParticipation(Interaction participationToRemove)
        {

            if (participationToRemove is null)
            {
                return false;
            }
            
            dbContext.Remove(participationToRemove);
            SaveChanges();
            return true;
        }

        public bool RemoveEvent(Guid eventId)
        {
            var eventToRemove = GetEventById(eventId);
            if (eventToRemove != null)
            {
                dbContext.Remove(eventToRemove.Comments);
                dbContext.Remove(eventToRemove.EventDetails);
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
    }
}
