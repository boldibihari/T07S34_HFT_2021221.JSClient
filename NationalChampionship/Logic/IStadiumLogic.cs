using NationalChampionship.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalChampionship.Logic
{
    public interface IStadiumLogic
    {
        Stadium GetOneStadium(int stadiumId);

        IList<Stadium> GetAllStadium();

        void AddStadium(Stadium stadium);

        void DeleteStadium(int stadiumId);

        void UpdateStadium(int stadiumId, Stadium newStadium);
    }
}
