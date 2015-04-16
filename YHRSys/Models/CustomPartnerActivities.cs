using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YHRSys.Models
{
    public class CustomPartnerActivities
    {
        public String PartnerName { set; get; }
        public String ReagentName { set; get; }
        public Decimal ReagentQty { set; get; }
        public String OiCFullName { set; get; }
        public String BackStopping { set; get; }
        public Decimal TCPG { set; get; }
        public Decimal BioRPG { set; get; }
        public Decimal TG { set; get; }
        public Decimal TCPA { set; get; }
        public Decimal TIPA { set; get; }
        public Decimal TA { set; get; }
        public String ActivityDate { set; get; }
    }
}