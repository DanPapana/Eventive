using Eventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventive.ApplicationLogic.Abstraction
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByGuid(Guid id);
        User GetUserByUserId(Guid userId);
        IEnumerable<Event> GetEventsCreatedByUser(Guid id);
    }
}
