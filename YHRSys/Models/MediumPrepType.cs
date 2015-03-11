using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YHRSys.Models
{
    public partial class MediumPrepType
    {
        public MediumPrepType()
        {
            this.activities = new HashSet<Activity>();
        }

        [Key]
        public int typeId { get; set; }

        [Required]
        public string typename { get; set; }

        public BaseDateEntity baseDateEntity { get; set; }
        public VersionedEntity version { get; set; }

        public BaseUserEntity baseUserEntity { get; set; }


        public virtual ICollection<Activity> activities { get; set; }
    }
}
