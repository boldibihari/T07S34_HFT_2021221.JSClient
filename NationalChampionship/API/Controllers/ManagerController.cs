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
    public class ManagerController : ControllerBase
    {
        private readonly IManagerLogic managerLogic;
        private readonly IClubLogic clubLogic;
        private readonly IHubContext<EventHub> hub;

        public ManagerController(IManagerLogic managerLogic, IClubLogic clubLogic, IHubContext<EventHub> hub)
        {
            this.managerLogic = managerLogic;
            this.clubLogic = clubLogic;
            this.hub = hub;
        }

        [HttpGet("{managerId}")]
        public Manager GetOneManager(int managerId)
        {
            return managerLogic.GetOneManager(managerId);
        }

        [HttpGet]
        public IEnumerable<Manager> GetAllManager()
        {
            return managerLogic.GetAllManager().AsQueryable();
        }

        [HttpPost("{clubId}")]
        public void AddManagerToClub(int clubId, [FromBody] Manager manager)
        {
            clubLogic.AddManagerToClub(clubId, manager);
            hub.Clients.All.SendAsync("ManagerCreated", manager);
        }

        [HttpDelete("{managerId}")]
        public void DeleteManager(int managerId)
        {
            var manager = managerLogic.GetOneManager(managerId);
            managerLogic.DeleteManager(managerId);
            hub.Clients.All.SendAsync("ManagerDeleted", manager);
        }

        [HttpPut("{managerId}")]
        public void UpdateManager(int managerId, [FromBody] Manager newManager)
        {
            managerLogic.UpdateManager(managerId, newManager);
            hub.Clients.All.SendAsync("ManagerUpdated", newManager);
        }
    }
}
