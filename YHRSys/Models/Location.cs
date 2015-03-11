using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public partial class Location
    {
        public Location()
        {
            this.Activities = new HashSet<Activity>();
            this.LocationUsers = new HashSet<LocationUser>();
        }

        [Key]
        public int locationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string name { get; set; }

        [MaxLength(50)]
        public string locNumber { get; set; }

        [MaxLength(50)]
        public string source { get; set; }

        [Column(TypeName = "ntext")]
        [MaxLength]
        public string description { get; set; }

        public BaseDateEntity baseDateEntity { get; set; }
        public BaseUserEntity baseUserEntity { get; set; }
        public VersionedEntity version { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<LocationUser> LocationUsers { get; set; }
    }
}