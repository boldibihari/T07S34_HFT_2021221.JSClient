using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NationalChampionship.API.Hubs;
using NationalChampionship.Data.Models;
using NationalChampionship.Logic;
using System.Collections.Generic;
using System.Linq;

namespace NationalChampionship.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClubController : ControllerBase
    {
        private readonly IClubLogic clubLogic;
        private readonly IHubContext<EventHub> hub;

        public ClubController(IClubLogic clubLogic, IHubContext<EventHub> hub)
        {
            this.clubLogic = clubLogic;
            this.hub = hub;
        }

        [HttpGet("{clubId}")]
        public Club GetOneClub(int clubId)
        {
            return clubLogic.GetOneClub(clubId);
        }

        [HttpGet]
        public IEnumerable<Club> GetAllClub()
        {
            return clubLogic.GetAllClub().AsQueryable();
        }

        [HttpPost]
        public void AddClub([FromBody] Club club)
        {
            clubLogic.AddClub(club);
            hub.Clients.All.SendAsync("ClubCreated", club);
        }

        [HttpDelete("{clubId}")]
        public void DeleteClub(int clubId)
        {
            var club = clubLogic.GetOneClub(clubId);
            clubLogic.DeleteClub(clubId);
            hub.Clients.All.SendAsync("ClubDeleted", club);
        }

        [HttpPut("{clubId}")]
        public void UpdateClub(int clubId, [FromBody] Club newClub)
        {
            clubLogic.UpdateClub(clubId, newClub);
            hub.Clients.All.SendAsync("ClubUpdated", newClub);
        }

        [HttpGet("AverageAge/{clubId}")]
        public double ClubAverageAge(int clubId)
        {
            return clubLogic.ClubAverageAge(clubId);
        }

        [HttpGet("Value/{clubId}")]
        public double ClubValue(int clubId)
        {
            return clubLogic.ClubValue(clubId);
        }

        [HttpGet("AverageValue")]
        public double AverageClubValue()
        {
            return clubLogic.AverageClubValue();
        }

        [HttpGet("AveragePlayerValue/{clubId}")]
        public double ClubAveragePlayerValue(int clubId)
        {
            return clubLogic.ClubAveragePlayerValue(clubId);
        }

        [HttpGet("Country/{clubId}")]
        public IEnumerable<Country> GetCountryOneClub(int clubId)
        {
            return clubLogic.GetCountryOneClub(clubId).AsQueryable();
        }

        [HttpGet("Position/{clubId}")]
        public IEnumerable<Position> GetPositionOneClub(int clubId)
        {
            return clubLogic.GetPositionOneClub(clubId).AsQueryable();
        }
    }
}
