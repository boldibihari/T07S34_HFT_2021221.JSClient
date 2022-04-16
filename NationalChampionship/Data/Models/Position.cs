using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalChampionship.Data.Models
{
    public class Position
    {
        public PlayerPosition PlayerPosition { get; set; }

        public int Count { get; set; }

        public override string ToString()
        {
            return $"{PlayerPosition}: {Count}";
        }

        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   PlayerPosition == position.PlayerPosition &&
                   Count == position.Count;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PlayerPosition, Count);
        }
    }
}
