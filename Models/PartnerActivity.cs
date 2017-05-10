using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public partial class PartnerActivity : BaseEntity
    {
        private const int DEFAULT_VALUE = 0;

        public PartnerActivity()
        {
            this.partnerReportings = new HashSet<PartnerReporting>();
        }

        [Key]
        public int partnerActivityId { get; set; }

        [DisplayName("Partner")]
        public int partnerId { get; set; }

        [DisplayName("Giver")]
        [DefaultValue(DEFAULT_VALUE)]
        public int giverId { get; set; }

        [DisplayName("Reagent")]
        public Nullable<int> reagentId { get; set; }

        [DisplayName("Variety")]
        public Nullable<int> varietyId { get; set; }

        [DisplayName("Reagent Qty")]
        [DefaultValue(DEFAULT_VALUE)]
        public Nullable<int> reagentQty { get; set; }
        /*
        [DisplayName("Variety Qty")]
        [DefaultValue(DEFAULT_VALUE)]
        public Nullable<int> varietyQty { get; set; }*/

        [DisplayName("OiC")]
        public string userId { get; set; }

        [DisplayName("Backstopping")]
        public string backStopping { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        //[DisplayName("TC plantlets given")]
        [DisplayName("TCPG")]
        [Range(0, (int)Int32.MaxValue, ErrorMessage = "TC plantlets given must be greater than zero.")]
        public Nullable<int> tcPlantletsGiven { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        //[DisplayName("Bioreactor plants given")]
        [DisplayName("BioRPG")]
        [Range(0, (int)Int32.MaxValue, ErrorMessage = "Bioreactor plantlets given must be greater than zero.")]
        public Nullable<int> bioreactorplantsGiven { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        //[DisplayName("Tubers given")]
        [DisplayName("TG")]
        [Range(0, (int)Int32.MaxValue, ErrorMessage = "Tubers given must be greater than zero.")]
        public Nullable<int> tubersGiven { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        //[DisplayName("TC Plantlets available")]
        [DisplayName("TCPA")]
        [Range(0, (int)Int32.MaxValue, ErrorMessage = "TC Plantlets available must be greater than zero.")]
        public Nullable<int> tcPlantletsAvailable { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        //[DisplayName("TI plantlets available")]
        [DisplayName("TIPA")]
        [Range(0, (int)Int32.MaxValue, ErrorMessage = "TI plantlets available must be greater than zero.")]
        public Nullable<int> tibPlantletsAvailable { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        //[DisplayName("Tubers available")]
        [DisplayName("TA")]
        [Range(0, (int)Int32.MaxValue, ErrorMessage = "Tubers available must be greater than zero.")]
        public Nullable<int> tubersAvailable { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        //[DisplayName("Seedlings available")]
        [DisplayName("SA")]
        [Range(0, (int)Int32.MaxValue, ErrorMessage = "Seedlings available must be greater than zero.")]
        public Nullable<int> seedsAvailable { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        //[DisplayName("Seedlings given")]
        [DisplayName("SG")]
        [Range(0, (int)Int32.MaxValue, ErrorMessage = "Seedlings given must be greater than zero.")]
        public Nullable<int> seedsGiven { get; set; }

        //This property line is for the user that is in-charge of this partner activity
        //[DisplayName("Officer-In-Charge")]
        [ForeignKey("userId")]
        public virtual ApplicationUser oic { get; set; }

        [DisplayName("Acty Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> activityDate { get; set; }

        [ForeignKey("reagentId")]
        public virtual Reagent reagent { get; set; }

        [ForeignKey("varietyId")]
        public virtual Variety variety { get; set; }

        [ForeignKey("partnerId")]
        public virtual Partner partner { get; set; }

        [ForeignKey("giverId")]
        public virtual Partner giver { get; set; }

        public virtual ICollection<PartnerReporting> partnerReportings { get; set; }

        [NotMapped]
        [DisplayName("OiC")]
        public string OiCFullName
        {
            get
            {
                return oic.FirstName + " " + oic.LastName;
            }
        }
    }
}
