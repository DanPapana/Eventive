using Eventive.ApplicationLogic.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventive.ApplicationLogic.Abstraction
{
    public interface IUserRepository : IRepository<Participant>
    {
        Participant GetParticipantByGuid(Guid id);
        Participant GetUserByUserId(Guid userId);
        IEnumerable<EventOrganized> GetEventsCreatedByUser(Guid id);
    }
}
