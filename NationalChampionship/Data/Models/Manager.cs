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
    public class Manager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ManagerId { get; set; }

        [StringLength(100)]
        public string ManagerName { get; set; }

        [StringLength(6)]
        public string CountryCode { get; set; }

        [StringLength(100)]
        public string ManagerCountry { get; set; }

        public DateTime ManagerBirthdate { get; set; }

        public int ManagerStartYear { get; set; }

        [StringLength(8)]
        public string PreferredFormation { get; set; }

        public bool WonChampionship { get; set; }

        [ForeignKey(nameof(Club))]
        public int ClubId { get; set; }

        [JsonIgnore]
        [NotMapped]
        public virtual Club Club { get; set; }
    }
}
