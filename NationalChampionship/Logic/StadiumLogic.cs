using NationalChampionship.Data.Models;
using NationalChampionship.Repository;
using System.Collections.Generic;
using System.Linq;

namespace NationalChampionship.Logic
{
    public class StadiumLogic : IStadiumLogic
    {
        private readonly IStadiumRepository stadiumRepository;

        public StadiumLogic(IStadiumRepository stadiumRepository)
        {
            this.stadiumRepository = stadiumRepository;
        }

        public void AddStadium(Stadium stadium)
        {
            stadiumRepository.Add(stadium);
        }

        public void DeleteStadium(int stadiumId)
        {
            stadiumRepository.Delete(stadiumId);
        }

        public IList<Stadium> GetAllStadium()
        {
            return stadiumRepository.GetAll().OrderBy(stadium => stadium.StadiumName).ToList();
        }

        public Stadium GetOneStadium(int stadiumId)
        {
            return stadiumRepository.GetOne(stadiumId);
        }

        public void UpdateStadium(int stadiumId, Stadium newStadium)
        {
            stadiumRepository.Update(stadiumId, newStadium);
        }
    }
}
