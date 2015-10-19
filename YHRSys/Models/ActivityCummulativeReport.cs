using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YHRSys.Models
{
    public class ActivityCummulativeReport
    {
        public PartnerActivity Activity { set; get; }
        public Partner Partner { set; get; }
        public Reagent Reagent { set; get; }

        public String ActivityDescr { set; get; }
        public DateTime? ActivityDate { set; get; }
        public String PartnerName { set; get; }

        public ApplicationUser oic { set; get; }
        public String ActivityOicName { set; get; }
        public String UploadedBy { set; get; }

        public String ReagentName { set; get; }
        public Int32? ReagentQtyGiven { set; get; }

        public Variety Variety { set; get; }
        public String VarietyName { set; get; }
        public Int32? VarietyQtyGiven { set; get; }

        public Int32? SPQtyAvailable { set; get; }
        public Int32? BioRPQtyAvailable { set; get; }
        public Int32? TCPQtyAvailable { set; get; }
        public Int32? TPQtyAvailable { set; get; }
        
        public DateTime? ReportDate { set; get; }
        public String ReportComment { set; get; }

        //Available
    }
}