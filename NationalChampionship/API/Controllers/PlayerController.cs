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
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerLogic playerLogic;
        private readonly IClubLogic clubLogic;
        private readonly IHubContext<EventHub> hub;

        public PlayerController(IPlayerLogic playerLogic, IClubLogic clubLogic, IHubContext<EventHub> hub)
        {
            this.playerLogic = playerLogic;
            this.clubLogic = clubLogic;
            this.hub = hub;
        }

        [HttpGet("{playerId}")]
        public Player GetOnePlayer(int playerId)
        {
            return playerLogic.GetOnePlayer(playerId);
        }

        [HttpGet]
        public IEnumerable<Player> GetAllPlayer()
        {
            return playerLogic.GetAllPlayer().AsQueryable();
        }

        [HttpGet("GetAllCaptain")]
        public IEnumerable<Player> GetAllCaptain()
        {
            return playerLogic.GetAllCaptain().AsQueryable();
        }

        [HttpGet("GetAllPlayerId")]
        public IEnumerable<int> GetAllPlayerId()
        {
            return playerLogic.GetAllPlayerId().AsQueryable();
        }

        [HttpGet("ClubSquad/{clubId}")]
        public IEnumerable<Player> ClubSquad(int clubId)
        {
            return playerLogic.GetAllPlayer().Where(x => x.ClubId == clubId).AsQueryable();
        }

        [HttpPost("{clubId}")]
        public void AddPlayerToClub(int clubId, [FromBody] Player player)
        {
            clubLogic.AddPlayerToClub(clubId, player);
            hub.Clients.All.SendAsync("PlayerCreated", player);
        }

        [HttpDelete("{playerId}")]
        public void DeletePlayer(int playerId)
        {
            var player = playerLogic.GetOnePlayer(playerId);
            playerLogic.DeletePlayer(playerId);
            hub.Clients.All.SendAsync("PlayerDeleted", player);
        }

        [HttpPut("{clubId}")]
        public void UpdatePlayer(int playerId, [FromBody] Player newPlayer)
        {
            playerLogic.UpdatePlayer(playerId, newPlayer);
            hub.Clients.All.SendAsync("PlayerUpdated", newPlayer);
        }

        [HttpGet("Count")]
        public int AllPlayerCount()
        {
            return playerLogic.AllPlayerCount();
        }

        [HttpGet("AverageAge")]
        public double AllAverageAge()
        {
            return playerLogic.AllAverageAge();
        }

        [HttpGet("Value")]
        public double AllValue()
        {
            return playerLogic.AllValue();
        }

        [HttpGet("AverageValue")]
        public double AveragePlayerValue()
        {
            return playerLogic.AveragePlayerValue();
        }

        [HttpGet("Country")]
        public IEnumerable<Country> GetAllNationality()
        {
            return playerLogic.GetAllCountry().AsQueryable();
        }

        [HttpGet("Position")]
        public IEnumerable<Position> GetAllPosition()
        {
            return playerLogic.GetAllPosition().AsQueryable();
        }
    }
}
