using Microsoft.AspNetCore.Identity;
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
    public class User : IdentityUser
    {
        public User() : base() { }

        public User(string userName) : base(userName)
        {
            this.FavouriteClubs = new HashSet<UserClub>();
        }

        public virtual ICollection<UserClub> FavouriteClubs { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(450)]
        public string RoleId { get; set; }

        [StringLength(100)]
        public string Role { get; set; }
    }
}
