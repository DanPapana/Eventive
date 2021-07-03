using System;

namespace Eventive.ApplicationLogic.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public Guid EntityId { get; private set; }

        public EntityNotFoundException(Guid id) : base($"Entity with id {id} was not found")
        {
        }

        public EntityNotFoundException(string email) : base($"Entity with email {email} was not found")
        {

        }
    }
}
