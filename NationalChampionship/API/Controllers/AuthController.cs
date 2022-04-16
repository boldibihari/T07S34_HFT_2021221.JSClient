using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NationalChampionship.API.Hubs;
using NationalChampionship.Logic;
using NationalChampionship.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NationalChampionship.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthLogic authLogic;
        private readonly IHubContext<EventHub> hub;

        public AuthController(IAuthLogic authLogic, IHubContext<EventHub> hub)
        {
            this.authLogic = authLogic;
            this.hub = hub;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUser([FromBody] RegistrationViewModel model)
        {
            string result = await authLogic.RegisterUser(model);
            return Ok(new { UserName = result });
        }

        [HttpGet("{userId}")]
        public Task<User> GetUser(string userId)
        {
            return authLogic.GetUser(userId);
        }

        [HttpGet()]
        public IEnumerable<User> GetAllUser()
        {
            return authLogic.GetAllUser();
        }

        [HttpPut]
        public async Task<ActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                return Ok(await authLogic.LoginUser(model));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        //[Authorize(Roles = "User")]
        [HttpPost("Club/{userId}")]
        public void AddFavouriteClub(string userId, [FromBody] Club club)
        {
            authLogic.AddFavouriteClub(userId, club);
            hub.Clients.All.SendAsync("AddFavouriteClub", club.ClubId);
        }

        //[Authorize(Roles = "User")]
        [HttpDelete("Club/{userId}/{clubId}")]
        public void DeleteFavouriteClub(string userId, int clubId)
        {
            authLogic.DeleteFavouriteClub(userId, clubId);
            hub.Clients.All.SendAsync("DeleteFavouriteClub", clubId);
        }

        [HttpGet("Favourite/{userId}/{clubId}")]
        public bool IsFavourite(string userId, int clubId)
        {
            return authLogic.IsFavourite(userId, clubId);
        }

        [HttpGet("Search/{text}")]
        public IEnumerable<object> Search(string text)
        {
            return authLogic.Search(text);
        }
    }
}
