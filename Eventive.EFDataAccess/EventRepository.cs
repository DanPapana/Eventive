using Microsoft.EntityFrameworkCore;
using Eventive.ApplicationLogic.Abstraction;
using Eventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eventive.EFDataAccess
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(EventManagerDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Event> GetActiveEvents()
        {
            return dbContext.Events
                .Include(evnt => evnt.EventDetails)
                .Include(evnt => evnt.Comments)
                .Where(evnt => evnt
                .EventDetails.Deadline > DateTime.UtcNow)
                .OrderBy(evnt => evnt.EventDetails.Deadline)
                .AsEnumerable();
        }

        public IEnumerable<Event> GetPastEvents(Guid userId)
        {
            return dbContext.Events.Include(evnt => evnt.EventDetails).Include(evnt => evnt.Comments)
                .Where(evnt => evnt
                .EventDetails.Deadline < DateTime.UtcNow 
                && evnt.CreatorId == userId)
                .OrderBy(evnt => evnt.EventDetails.Deadline)
                .AsEnumerable();
        }

        public Event GetEventById(Guid eventId)
        {
            return dbContext.Events.Include(ev => ev.EventDetails)
                    .Include(evnt => evnt.Comments)
                    .Where(evnt => evnt.Id == eventId)
                    .SingleOrDefault();
        }

        public IEnumerable<Event> GetEventsByCategory(Event.EventCategory eventCategory)
        {
            return dbContext.Events
                .Where(evnt => evnt.Category == eventCategory);
        }

        public Participation GetParticipation(Guid eventId, Guid userId, Participation.Type type)
        {
            return dbContext.Participations
                        .Where(ev => ev.ParticipantId == userId 
                        && ev.UserParticipationType == type 
                        && ev.EventId == eventId)
                        .SingleOrDefault();
        }

        private IEnumerable<Participation> GetUserParticipations(Guid userId, Participation.Type type)
        {
            return dbContext.Participations
                .Where(p => p.UserParticipationType == type
                && p.ParticipantId == userId)
                .AsEnumerable();
        }

        public IEnumerable<Event> GetEventsForUser(Guid userId, Participation.Type type)
        {
            List<Event> participatedEvents = new List<Event>();

            var participations = GetUserParticipations(userId, type);

            foreach (Participation part in participations)
            {
                participatedEvents.Add(GetEventById(part.EventId));
            }

            return participatedEvents.AsEnumerable();
        }

        public IEnumerable<Guid> GetEventsGuidForUser(Guid userId, Participation.Type type)
        {
            List<Guid> participatedEvents = new List<Guid>();

            var participations = GetUserParticipations(userId, type);

            foreach (Participation part in participations)
            {
                participatedEvents.Add(part.EventId);
            }

            return participatedEvents.AsEnumerable();
        }

        public Participation CreateParticipation(Participation participation)
        {
            dbContext.Participations.Add(participation);
            dbContext.SaveChanges();
            return participation;
        }

        public bool RemoveParticipation(Participation participationToRemove)
        {
            
            if (participationToRemove == null)
                return false;
            
            dbContext.Remove(participationToRemove);
            dbContext.SaveChanges();
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
                dbContext.SaveChanges();
                
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
