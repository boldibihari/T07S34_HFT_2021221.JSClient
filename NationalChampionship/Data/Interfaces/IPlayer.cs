using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalChampionship.Data.Interfaces
{
    public class IPlayer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DateOfBirthTimestamp { get; set; }

        public string Position { get; set; }

        public int ShirtNumber { get; set; }

        public int Height { get; set; }

        public string PreferredFoot { get; set; }

        public ICountry Country { get; set; }

        public double ProposedMarketValue { get; set; }
    }
}
