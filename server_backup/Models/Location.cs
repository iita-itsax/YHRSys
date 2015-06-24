using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public partial class Location : BaseEntity
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
        [DisplayName("Name")]
        public string name { get; set; }

        [MaxLength(50)]
        [DisplayName("LocNo")]
        public string locNumber { get; set; }

        [MaxLength(50)]
        [DisplayName("Source")]
        public string source { get; set; }

        [Column(TypeName = "ntext")]
        [MaxLength]
        [DisplayName("Description")]
        public string description { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<LocationUser> LocationUsers { get; set; }
    }
}