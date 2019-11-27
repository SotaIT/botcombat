using System.Collections.Generic;
using System.Linq;
using BotCombat.Web.Data;
using BotCombat.Web.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace BotCombat.Web.Services
{
    public abstract class BaseDataService<T> where T : class, IEntity
    {
        protected readonly ApplicationDbContext Db;

        protected BaseDataService(ApplicationDbContext db)
        {
            Db = db;
            db.Database.SetCommandTimeout(60);
        }

        public List<T> GetAll()
        {
            return GetQuery().ToList();
        }

        protected DbSet<T> GetQuery()
        {
            return Db.Set<T>();
        }

        public T Get(int id)
        {
            return GetQuery().FirstOrDefault(m => m.Id == id);
        }

        public List<T> Get(List<int> ids)
        {
            return GetQuery().Where(i => ids.Contains(i.Id)).ToList();
        }
    }
}