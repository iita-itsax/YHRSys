using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YHRSys.Models
{
    public partial class MediumPrepType : BaseEntity
    {
        public MediumPrepType()
        {
            this.activities = new HashSet<Activity>();
        }

        [Key]
        public int typeId { get; set; }

        [DisplayName("Type Name"), Required]
        public string typename { get; set; }

        public virtual ICollection<Activity> activities { get; set; }
    }
}
