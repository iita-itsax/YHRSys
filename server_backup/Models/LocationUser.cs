using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public enum STATUS { 
        ACTIVE, INACTIVE
    }

    public partial class LocationUser : BaseEntity
    {
        public LocationUser()
        {
            this.locationSubordinates = new HashSet<LocationSubordinate>();
        }


        [Key]
        public int locationUserId { get; set; }
        //This property line is for the user that is in-charge of this location
        [DisplayName("Officer-in-Charge")]
        public string userId { get; set; }

        [DisplayName("Location")]
        public int locationId { get; set; }

        [DisplayName("Resump. Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> startDate { get; set; }

        [DisplayName("Exit Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> endDate { get; set; }

        [DisplayName("Status")]
        public STATUS status { get; set; }

        [ForeignKey("locationId")]
        public virtual Location location { get; set; }

        [ForeignKey("userId")]
        public virtual ApplicationUser user { get; set; }

        public virtual ICollection<LocationSubordinate> locationSubordinates { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return user.FirstName + " " + user.LastName;
            }
        }
    }
}