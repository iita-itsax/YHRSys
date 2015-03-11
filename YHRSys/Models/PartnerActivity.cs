using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public partial class PartnerActivity
    {
        private const int DEFAULT_VALUE = 0;

        [Key]
        public int partnerActivityId { get; set; }

        public int partnerId { get; set; }

        public int reagentId { get; set; }

        [MaxLength(255)]
        public string backStopping { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        public int tcPlantletsGiven { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        public int bioreactorplantsGiven { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        public int tubersGiven { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        public int tcPlantletsAvailable { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        public int tibPlantletsAvailable { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        public int tubersAvailable { get; set; }

        //This property line is for the user that is in-charge of this partner activity
        [DisplayName("Officer-In-Charge")]
        public ApplicationUser officerInCharge { get; set; }

        public Nullable<System.DateTime> activityDate { get; set; }

        public BaseDateEntity baseDateEntity { get; set; }
        public VersionedEntity version { get; set; }

        public BaseUserEntity baseUserEntity { get; set; }

        [ForeignKey("reagentId")]
        public virtual Reagent reagent { get; set; }

        [ForeignKey("partnerId")]
        public virtual Partner partner { get; set; }
    }
}
