using NationalChampionship.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalChampionship.Logic
{
    public interface IPlayerLogic
    {
        FileStream GetPlayerImage(int playerId);

        Player GetOnePlayer(int playerId);

        IList<Player> GetAllPlayer();

        IList<Player> GetAllCaptain();

        IList<int> GetAllPlayerId();

        void AddPlayer(Player player);

        void DeletePlayer(int playerId);

        void UpdatePlayer(int playerId, Player newPlayer);

        int AllPlayerCount();

        double AllAverageAge();

        double AllValue();

        double AveragePlayerValue();

        IList<Country> GetAllCountry();

        IList<Position> GetAllPosition();
    }
}
