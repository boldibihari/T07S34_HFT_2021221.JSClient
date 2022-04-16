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
    public class Stadium
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StadiumId { get; set; }

        [StringLength(100)]
        public string StadiumName { get; set; }

        public int Completed { get; set; }

        public int Capacity { get; set; }

        [StringLength(100)]
        public string Location { get; set; }

        [ForeignKey(nameof(Club))]
        public int ClubId { get; set; }

        [JsonIgnore]
        [NotMapped]
        public virtual Club Club { get; set; }
    }
}
