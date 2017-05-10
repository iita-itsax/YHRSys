using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YHRSys.Models
{
    public class CustomVarietyProcessFlow
    {
        public Decimal processId { get; set; }
        public String variety { get; set; }
        public String OiC { get; set; }
        public String form { get; set; }
        public String processDate { get; set; }
        public String rank { get; set; }
        public String barcode { get; set; }
        public String barcodeImageUrl { get; set; }
        public String description { get; set; }
    }
}