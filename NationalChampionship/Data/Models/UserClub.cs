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
    public class UserClub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserClubId { get; set; }

        public string UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        public int ClubId { get; set; }

        public virtual Club Club { get; set; }
    }
}
