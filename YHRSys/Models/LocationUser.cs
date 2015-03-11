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

    public partial class LocationUser
    {
        [Key]
        public int locationUserId { get; set; }
        //This property line is for the user that is in-charge of this location
        public string userId { get; set; }

        public int locationId { get; set; }

        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }

        public STATUS status { get; set; }

        public BaseDateEntity baseDateEntity { get; set; }
        public VersionedEntity version { get; set; }

        [ForeignKey("locationId")]
        public virtual Location location { get; set; }

        public BaseUserEntity baseUserEntity { get; set; }
    }
}