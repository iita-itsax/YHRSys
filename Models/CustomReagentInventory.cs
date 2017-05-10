using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YHRSys.Models
{
    public class CustomReagentInventory
    {
        public String reagentName { set; get; }
        public Decimal reagentQty { set; get; }
        public String stockerName { set; get; }
        public String stockDate { set; get; }
        public String note { set; get; }
    }
}