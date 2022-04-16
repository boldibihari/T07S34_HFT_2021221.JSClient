using Microsoft.EntityFrameworkCore;
using NationalChampionship.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalChampionship.Repository
{
    public class UserClubRepository : CommonRepository<UserClub>, IUserClubRepository
    {
        public UserClubRepository(DbContext context) : base(context)
        {
        }

        public override UserClub GetOne(int userClubId)
        {
            return GetAll().SingleOrDefault(x => x.UserClubId == userClubId);
        }

        public override void Update(int userClubId, UserClub newItem)
        {
            throw new NotImplementedException();
        }
    }
}
