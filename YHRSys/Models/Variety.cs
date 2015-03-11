using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public partial class Variety
    {
        public Variety()
        {
            this.varietyProcessFlows = new HashSet<VarietyProcessFlow>();
        }

        [Key]
        public int varietyId { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string sampleNumber { get; set; }

        //This is the user initiated this variety process flow
        [Required]
        [DisplayName("Rep. Officer")]
        public ApplicationUser repUser { get; set; }

        public Nullable<System.DateTime> testDate { get; set; }
        public string releaseStatus { get; set; }
        public string resistance { get; set; }
        public string storability { get; set; }
        public string poundability { get; set; }
        public string species { get; set; }
        public string uniformity { get; set; }
        public string stability { get; set; }
        public string distinctness { get; set; }

        public Nullable<int> value { get; set; }

        public Nullable<int> activityId { get; set; }

        public Nullable<int> locationId { get; set; }

        public BaseDateEntity baseDateEntity { get; set; }
        public VersionedEntity version { get; set; }

        public BaseUserEntity baseUserEntity { get; set; }

        [ForeignKey("activityId")]
        public virtual Activity activity { get; set; }

        [ForeignKey("locationId")]
        public virtual Location location { get; set; }

        public virtual ICollection<VarietyProcessFlow> varietyProcessFlows { get; set; }
    }
}
