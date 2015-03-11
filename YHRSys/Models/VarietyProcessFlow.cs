using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public partial class VarietyProcessFlow
    {
        [Key]
        public int processId { get; set; }

        public int varietyId { get; set; }

        public string form { get; set; }
        public Nullable<System.DateTime> processDate { get; set; }

        //This is the user initiated this variety process flow
        [DisplayName("Staff-In-Charge")]
        public ApplicationUser userId { get; set; }

        public string rank { get; set; }
        public string barcode { get; set; }

        [MaxLength]
        public string description { get; set; }

        public BaseDateEntity baseDateEntity { get; set; }
        public VersionedEntity version { get; set; }

        public BaseUserEntity baseUserEntity { get; set; }

        [ForeignKey("varietyId")]
        public virtual Variety variety { get; set; }
    }
}
