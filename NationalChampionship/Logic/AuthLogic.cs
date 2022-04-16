using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NationalChampionship.Repository;
using NationalChampionship.Data.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NationalChampionship.Logic
{
    public class AuthLogic : IAuthLogic
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IClubRepository clubRepository;
        private readonly IManagerRepository managerRepository;
        private readonly IPlayerRepository playerRepository;
        private readonly IStadiumRepository stadiumRepository;
        private readonly IUserClubRepository userClubRepository;

        public AuthLogic(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IClubRepository clubRepository, IManagerRepository managerRepository, IPlayerRepository playerRepository, IStadiumRepository stadiumRepository, IUserClubRepository userClubRepository)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.clubRepository = clubRepository;
            this.managerRepository = managerRepository;
            this.playerRepository = playerRepository;
            this.stadiumRepository = stadiumRepository;
            this.userClubRepository = userClubRepository;
        }

        public async Task<User> GetUser(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }

        public IEnumerable<User> GetAllUser()
        {
            return userManager.Users;
        }

        public async Task<string> RegisterUser(RegistrationViewModel model)
        {
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                RoleId = (from role in roleManager.Roles
                          where role.Name == "User"
                          select role.Id).First(),
                Role = (from role in roleManager.Roles
                        where role.Name == "User"
                        select role.Name).First()
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");

            }

            return user.UserName;
        }

        public async Task<TokenViewModel> LoginUser(LoginViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var claims = new List<Claim>
                {
                  new Claim(JwtRegisteredClaimNames.Sub, model.Username),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

                var roles = await userManager.GetRolesAsync(user);

                claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

                var signinKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes("Paris Berlin Cairo Sydney Tokyo Beijing Rome London Athens"));

                var token = new JwtSecurityToken(
                  issuer: "http://www.security.org",
                  audience: "http://www.security.org",
                  claims: claims,
                  expires: DateTime.Now.AddMinutes(60),
                  signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );
                return new TokenViewModel
                {
                    UserId = user.Id,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                };
            }

            throw new ArgumentException("Login failed");
        }

        public void AddFavouriteClub(string userId, Club club)
        {
            userClubRepository.Add(new UserClub { UserId = userId, ClubId = club.ClubId });
        }

        public void DeleteFavouriteClub(string userId, int clubId)
        {
            var userClub = userClubRepository.GetAll().SingleOrDefault(x => x.ClubId == clubId && x.UserId == userId);
            userClubRepository.Delete(userClub.UserClubId);
        }

        public bool IsFavourite(string userId, int clubId)
        {
            var result = clubRepository.GetOne(clubId).Users.SingleOrDefault(x => x.UserId == userId);
            ;
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<object> Search(string text)
        {
            if (text.Length >= 3)
            {
                var result = new List<object>
                {
                    clubRepository.GetAll().Where(club => club.ClubName.Contains(text)),
                    playerRepository.GetAll().Where(player => player.PlayerName.Contains(text)),
                    managerRepository.GetAll().Where(manager => manager.ManagerName.Contains(text)),
                    stadiumRepository.GetAll().Where(stadium => stadium.StadiumName.Contains(text))
                };

                return result;
            }
            else
            {
                throw new ArgumentException("The text is not long enough!");
            }
        }
    }
}