using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public class PartnerReporting : BaseEntity
    {
        private const int DEFAULT_VALUE = 0;

        public PartnerReporting()
        {

        }

        [Key]
        public int reportId { get; set; }

        [DisplayName("Activity"), Required]
        public int activityId { get; set; }

        //[DisplayName("Reagent")]
        //public int reagentId { get; set; }

        //[DisplayName("Variety")]
        //public int varietyId { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        [DisplayName("Reagent Qty Available")]
        public int reagentQty { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        [DisplayName("Seedlings Qty")]
        [Range(0, (int)Int32.MaxValue, ErrorMessage = "Available SP qantity must be numeric.")]
        public int spQty { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        [DisplayName("TCP Qty")]
        [Range(0, (int)Int32.MaxValue, ErrorMessage = "Available TCP qantity must be numeric.")]
        public int tcpQty { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        [DisplayName("TP Qty")]
        [Range(0, (int)Int32.MaxValue, ErrorMessage = "Available TP qantity must be numeric.")]
        public int tpQty { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        [DisplayName("BioRP Qty")]
        [Range(0, (int)Int32.MaxValue, ErrorMessage = "Available BioRP qantity must be numeric.")]
        public int bioRPQty { get; set; }

        [DisplayName("Comment")]
        public string comment { get; set; }

        [ForeignKey("activityId")]
        public virtual PartnerActivity activity { get; set; }

        [DisplayName("Report Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> reportDate { get; set; }

        //[ForeignKey("reagentId")]
        //public virtual Reagent reagent { get; set; }

        //[ForeignKey("varietyId")]
        //public virtual Variety variety { get; set; }
    }
}