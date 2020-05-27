using Microsoft.EntityFrameworkCore;
using PAWEventive.ApplicationLogic.Abstraction;
using PAWEventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAWEventive.EFDataAccess
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(EventManagerDbContext dbContext) : base(dbContext)
        {
        }

        public User GetUserByUserId(Guid userId)
        {
            User foundUser = dbContext.Users
                            .Include(user => user.ContactDetails)
                            .Where(user => user.UserId == userId)
                            .SingleOrDefault();

            return foundUser;
        }

        public User GetUserByGuid(Guid searchId)
        {
            User foundUser = dbContext.Users
                            .Include(user=> user.ContactDetails)
                            .Where(user => user.Id == searchId)
                            .SingleOrDefault();

            return foundUser;
        }

        public IEnumerable<Event> GetEventsCreatedByUser(Guid userId)
        {
            return dbContext.Events
                            .Include(ev => ev.EventDetails)
                            .Where(evnt => evnt.CreatorId == userId)
                            .AsEnumerable();
        }

    }
}
