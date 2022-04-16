using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NationalChampionship.Data.Models
{
    public class Club
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClubId { get; set; }

        [StringLength(100)]
        public string ClubName { get; set; }

        [StringLength(100)]
        public string ClubColour { get; set; }

        [StringLength(100)]
        public string ClubCity { get; set; }

        public int ClubFounded { get; set; }

        [NotMapped]
        public virtual Stadium Stadium { get; set; }

        [NotMapped]
        public virtual Manager Manager { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserClub> Users { get; set; }

        public Club()
        {
            this.Players = new HashSet<Player>();
            this.Users = new HashSet<UserClub>();
        }
    }
}
