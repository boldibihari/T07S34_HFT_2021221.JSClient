using Microsoft.EntityFrameworkCore;
using NationalChampionship.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NationalChampionship.Repository
{
    public class PlayerRepository : CommonRepository<Player>, IPlayerRepository
    {
        public PlayerRepository(DbContext context) : base(context)
        {
        }

        public override void Update(int playerId, Player newPlayer)
        {
            var oldPlayer = GetOne(playerId);

            if (oldPlayer == null || newPlayer == null)
            {
                throw new ArgumentNullException(nameof(newPlayer), nameof(oldPlayer));
            }
            else
            {
                oldPlayer.PlayerName = newPlayer.PlayerName;
                oldPlayer.CountryCode = newPlayer.CountryCode;
                oldPlayer.PlayerCountry = newPlayer.PlayerCountry;
                oldPlayer.PlayerBirthdate = newPlayer.PlayerBirthdate;
                oldPlayer.PlayerPosition = newPlayer.PlayerPosition;
                oldPlayer.ShirtNumber = newPlayer.ShirtNumber;
                oldPlayer.Height = newPlayer.Height;
                oldPlayer.PreferredFoot = newPlayer.PreferredFoot;
                oldPlayer.PlayerValue = newPlayer.PlayerValue;
                oldPlayer.Captain = newPlayer.Captain;
                oldPlayer.ClubId = newPlayer.ClubId;
                context.SaveChanges();
            }
        }

        public override Player GetOne(int playerId)
        {
            return GetAll().SingleOrDefault(x => x.PlayerId == playerId);
        }
    }
}
