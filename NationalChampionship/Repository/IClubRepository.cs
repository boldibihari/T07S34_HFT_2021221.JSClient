using NationalChampionship.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalChampionship.Repository
{
    public interface IClubRepository : ICommonRepository<Club>
    {
        void AddPlayerToClub(int clubId, Player player);

        void AddManagerToClub(int clubId, Manager manager);

        void AddStadiumToClub(int clubId, Stadium stadium);
    }
}
