using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalChampionship.Data.Models
{
    public class Country
    {
        public string PlayerCountry { get; set; }

        public int Count { get; set; }

        public override string ToString()
        {
            return $"{PlayerCountry}: {Count}";
        }

        public override bool Equals(object obj)
        {
            return obj is Country nationality &&
                   PlayerCountry == nationality.PlayerCountry &&
                   Count == nationality.Count;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PlayerCountry, Count);
        }
    }
}
