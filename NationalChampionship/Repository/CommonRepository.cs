using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace NationalChampionship.Repository
{
    public abstract class CommonRepository<T> : ICommonRepository<T> where T : class
    {
        protected DbContext context;

        protected CommonRepository(DbContext context)
        {
            this.context = context;
        }

        public void Add(T item)
        {
            context.Set<T>().Add(item);
            context.SaveChanges();
        }

        public void Delete(T item)
        {
            context.Set<T>().Remove(item);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            context.Set<T>().Remove(GetOne(id));
            context.SaveChanges();
        }

        public abstract void Update(int id, T newItem);

        public abstract T GetOne(int id);

        public IQueryable<T> GetAll()
        {
            return context.Set<T>();
        }
    }
}
