using NationalChampionship.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalChampionship.Logic
{
    public interface IClubLogic
    {
        FileStream GetClubImage(int clubId);

        Club GetOneClub(int clubId);

        IList<Club> GetAllClub();

        void AddClub(Club club);

        void DeleteClub(int clubId);

        void UpdateClub(int clubId, Club newClub);

        void AddPlayerToClub(int clubId, Player player);

        void AddManagerToClub(int clubId, Manager manager);

        void AddStadiumToClub(int clubId, Stadium stadium);

        double ClubAverageAge(int clubId);

        double AverageClubValue();

        double ClubValue(int clubId);

        double ClubAveragePlayerValue(int clubId);

        IList<Country> GetCountryOneClub(int clubId);

        IList<Position> GetPositionOneClub(int clubId);
    }
}
