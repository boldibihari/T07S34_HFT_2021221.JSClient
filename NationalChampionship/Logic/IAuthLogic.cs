using NationalChampionship.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalChampionship.Logic
{
    public interface IAuthLogic
    {
        Task<User> GetUser(string userId);

        IEnumerable<User> GetAllUser();

        Task<string> RegisterUser(RegistrationViewModel model);

        Task<TokenViewModel> LoginUser(LoginViewModel model);

        void AddFavouriteClub(string userId, Club club);

        void DeleteFavouriteClub(string userId, int clubId);

        bool IsFavourite(string userId, int clubId);

        IEnumerable<object> Search(string text);
    }
}
