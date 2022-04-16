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
    public enum PlayerPosition
    {
        Goalkeeper, Defender, Midfielder, Forward
    }

    public enum PreferredFoot
    {
        Left, Right, Both
    }

    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PlayerId { get; set; }

        [StringLength(100)]
        public string PlayerName { get; set; }

        [StringLength(6)]
        public string CountryCode { get; set; }

        [StringLength(100)]
        public string PlayerCountry { get; set; }

        public DateTime PlayerBirthdate { get; set; }

        public PlayerPosition PlayerPosition { get; set; }

        public int? ShirtNumber { get; set; }

        public int? Height { get; set; }

        public PreferredFoot PreferredFoot { get; set; }

        public double PlayerValue { get; set; }

        public bool Captain { get; set; }

        [ForeignKey(nameof(Club))]
        public int ClubId { get; set; }

        [JsonIgnore]
        [NotMapped]
        public virtual Club Club { get; set; }
    }
}
