using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalChampionship.Repository
{
    public interface ICommonRepository<T> where T : class
    {
        void Add(T item);

        void Delete(T item);

        void Delete(int id);

        void Update(int id, T newItem);

        T GetOne(int id);

        IQueryable<T> GetAll();
    }
}
