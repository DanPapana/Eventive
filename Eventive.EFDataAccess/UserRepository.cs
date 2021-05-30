using Microsoft.EntityFrameworkCore;
using Eventive.ApplicationLogic.Abstraction;
using Eventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eventive.EFDataAccess
{
    public class UserRepository : BaseRepository<Participant>, IUserRepository
    {
        public UserRepository(EventManagerDbContext dbContext) : base(dbContext)
        {
        }

        public Participant GetUserByUserId(Guid userId)
        {
            Participant foundUser = dbContext.Participants
                            .Include(user => user.ContactDetails)
                            .Where(user => user.UserId == userId)
                            .SingleOrDefault();

            return foundUser;
        }

        public Participant GetUserByGuid(Guid searchId)
        {
            Participant foundUser = dbContext.Participants
                            .Include(user=> user.ContactDetails)
                            .Where(user => user.Id == searchId)
                            .SingleOrDefault();

            return foundUser;
        }

        public IEnumerable<EventOrganized> GetEventsCreatedByUser(Guid userId)
        {
            return dbContext.Events
                            .Include(ev => ev.EventDetails)
                            .Where(evnt => evnt.CreatorId == userId)
                            .AsEnumerable();
        }

    }
}
