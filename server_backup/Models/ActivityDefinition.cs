using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YHRSys.Models
{
    public partial class ActivityDefinition : BaseEntity
    {
        public ActivityDefinition()
        {
            this.activities = new HashSet<Activity>();
        }

        [Key]
        public int activityDefinitionId { get; set; }

        [DisplayName("Def. Name"), Required]
        public string name { get; set; }

        public virtual ICollection<Activity> activities { get; set; }
    }
}
