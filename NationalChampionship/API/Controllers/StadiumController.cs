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
    public class StadiumController : ControllerBase
    {
        private readonly IStadiumLogic stadiumLogic;
        private readonly IClubLogic clubLogic;
        private readonly IHubContext<EventHub> hub;

        public StadiumController(IStadiumLogic stadiumLogic, IClubLogic clubLogic, IHubContext<EventHub> hub)
        {
            this.stadiumLogic = stadiumLogic;
            this.clubLogic = clubLogic;
            this.hub = hub;
        }

        [HttpGet("{stadiumId}")]
        public Stadium GetOneStadium(int stadiumId)
        {
            return stadiumLogic.GetOneStadium(stadiumId);
        }

        [HttpGet]
        public IEnumerable<Stadium> GetAllStadium()
        {
            return stadiumLogic.GetAllStadium().AsQueryable();
        }

        [HttpPost("{clubId}")]
        public void AddStadiumToClub(int clubId, [FromBody] Stadium stadium)
        {
            clubLogic.AddStadiumToClub(clubId, stadium);
            hub.Clients.All.SendAsync("StadiumCreated", stadium);
        }

        [HttpDelete("{stadiumId}")]
        public void DeleteStadium(int stadiumId)
        {
            stadiumLogic.DeleteStadium(stadiumId);
            hub.Clients.All.SendAsync("StadiumDeleted", stadiumId);
        }

        [HttpPut("{stadiumId}")]
        public void UpdateStadium(int stadiumId, [FromBody] Stadium newStadium)
        {
            stadiumLogic.UpdateStadium(stadiumId, newStadium);
            hub.Clients.All.SendAsync("StadiumUpdated", newStadium);
        }
    }
}
