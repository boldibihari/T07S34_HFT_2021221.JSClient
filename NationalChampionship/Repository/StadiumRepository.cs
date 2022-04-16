using Microsoft.EntityFrameworkCore;
using NationalChampionship.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalChampionship.Repository
{
    public class StadiumRepository : CommonRepository<Stadium>, IStadiumRepository
    {
        public StadiumRepository(DbContext context) : base(context)
        {
        }

        public override void Update(int stadiumId, Stadium newStadium)
        {
            var oldStadium = GetOne(stadiumId);

            if (oldStadium == null || newStadium == null)
            {
                throw new ArgumentNullException(nameof(newStadium), nameof(oldStadium));
            }
            else
            {
                oldStadium.StadiumName = newStadium.StadiumName;
                oldStadium.Completed = newStadium.Completed;
                oldStadium.Capacity = newStadium.Capacity;
                oldStadium.Location = newStadium.Location;
                context.SaveChanges();
            }
        }

        public override Stadium GetOne(int stadiumId)
        {
            return GetAll().SingleOrDefault(x => x.StadiumId == stadiumId);
        }
    }
}
