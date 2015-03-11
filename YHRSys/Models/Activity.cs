using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public partial class Activity
    {
        public Activity()
        {
            this.varieties = new HashSet<Variety>();
        }

        [Key]
        public int activityId { get; set; }

        [Required]
        public string name { get; set; }

        public int locationId { get; set; }
        public int typeId { get; set; }

        [MaxLength]
        public string description { get; set; }

        public BaseDateEntity baseDateEntity { get; set; }
        public VersionedEntity version { get; set; }
        
        [ForeignKey("locationId")]
        public virtual Location location { get; set; }

        [ForeignKey("typeId")]
        public virtual MediumPrepType mediumPrepType { get; set; }

        public BaseUserEntity baseUserEntity { get; set; }

        public virtual ICollection<Variety> varieties { get; set; }
    }
}
