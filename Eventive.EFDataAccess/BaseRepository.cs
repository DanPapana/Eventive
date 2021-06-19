using Eventive.ApplicationLogic.Abstraction;
using System.Collections.Generic;
using System.Linq;

namespace Eventive.EFDataAccess
{
    public class BaseRepository<T> : IRepository<T> where T : class, new()
    {
        protected readonly EventManagerDbContext dbContext;
        public BaseRepository(EventManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public T Add(T itemToAdd)
        {
            var entity = dbContext.Add<T>(itemToAdd);
            dbContext.SaveChanges();
            return entity.Entity;
        }

        public bool Delete(T itemToDelete)
        {
            dbContext.Remove<T>(itemToDelete);
            dbContext.SaveChanges();
            return true;
        }

        public IEnumerable<T> GetAll()
        {
            return dbContext.Set<T>()
                            .AsEnumerable();
        }

        public T Update(T itemToUpdate)
        {
            var entity = dbContext.Update(itemToUpdate);
            dbContext.SaveChanges();
            return entity.Entity;
        }
    }
}
