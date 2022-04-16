using NationalChampionship.Data;
using NationalChampionship.Data.Interfaces;
using NationalChampionship.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NationalChampionship
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DatabaseDbContext database = new DatabaseDbContext();

            Console.WriteLine("Uploading database...");

            //Fill database
            await FillDatabaseAsync(1925, database);
            await FillDatabaseAsync(1926, database);
            await FillDatabaseAsync(25596, database);
            await FillDatabaseAsync(25608, database);
            await FillDatabaseAsync(1916, database);
            await FillDatabaseAsync(1924, database);
            await FillDatabaseAsync(1921, database);
            await FillDatabaseAsync(6059, database);
            await FillDatabaseAsync(25397, database);
            await FillDatabaseAsync(1922, database);
            await FillDatabaseAsync(1915, database);
            await FillDatabaseAsync(8057, database);

            Console.Clear();

            Console.WriteLine("Database uploaded!");

            Console.ReadLine();
        }

        public static PlayerPosition ToPlayerPosition(string position)
        {
            if (position == "G")
            {
                return PlayerPosition.Goalkeeper;
            }
            else if (position == "D")
            {
                return PlayerPosition.Defender;
            }
            else if (position == "M")
            {
                return PlayerPosition.Midfielder;
            }
            else
            {
                return PlayerPosition.Forward;
            }
        }

        public static PreferredFoot ToPreferredFoot(string preferredFoot)
        {
            if (preferredFoot == "Left")
            {
                return PreferredFoot.Left;
            }
            else if (preferredFoot == "Right")
            {
                return PreferredFoot.Right;
            }
            else
            {
                return PreferredFoot.Both;
            }
        }

        public static DateTime ToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static string Between(string data, string first, string last)
        {
            int pos1 = data.IndexOf(first) + first.Length;
            int pos2 = data.IndexOf(last);
            string final = data.Substring(pos1, pos2 - pos1);
            return final;
        }

        public static async Task FillDatabaseAsync(int clubId, DatabaseDbContext database)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://divanscore.p.rapidapi.com/teams/get-squad?teamId=" + clubId),
                Headers =
                {
                     { "x-rapidapi-host", "divanscore.p.rapidapi.com" },
                     { "x-rapidapi-key", "xxxx" },
                },
            };

            using var response1 = await client.SendAsync(request);
            response1.EnsureSuccessStatusCode();
            var body = response1.Content.ReadAsStringAsync().Result;

            var result = Between(body, "[", "]");

            // },{"player":
            var split = "},{" + '"'.ToString() + "player" + '"' + ':';

            var q1 = result.Split(new string[] { split }, StringSplitOptions.None);
            string[] q2 = new string[q1.Length];

            for (int i = 0; i < q1.Length; i++)
            {
                q2[i] = q1[i].Insert(q1[i].Length, ",");
            }

            var text1 = string.Concat(q2).Remove(0, 10);
            var text2 = text1.Remove(text1.Length - 2);
            var text3 = text2.Insert(0, "[");
            var text4 = text3.Insert(text3.Length, "]");

            var players = JsonConvert.DeserializeObject<List<IPlayer>>(text4);

            foreach (var player in players)
            {
                database.Players.Add(new Player { PlayerId = player.Id, PlayerName = player.Name, CountryCode = player.Country.Alpha2.ToLower(), PlayerCountry = player.Country.Name, ShirtNumber = player.ShirtNumber, PreferredFoot = ToPreferredFoot(player.PreferredFoot), Height = player.Height, PlayerPosition = ToPlayerPosition(player.Position), PlayerBirthdate = ToDateTime(player.DateOfBirthTimestamp), PlayerValue = player.ProposedMarketValue / 1000000, ClubId = database.Set<Club>().Where(club => club.ClubId == clubId).Select(club => club.ClubId).First(), Club = database.Set<Club>().Where(club => club.ClubId == clubId).First() });
            }
            database.SaveChanges();
        }
    }
}
