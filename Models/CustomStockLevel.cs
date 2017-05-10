using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YHRSys.Models
{
    public class CustomStockLevel
    {
        public String reagentName { set; get; }
        public Decimal reOrderLevel { set; get; }
        public Decimal totalIn { set; get; }
        public Decimal totalPartnerActivitiesOut { set; get; }
        public Decimal totalInHouseOut { set; get; }
        public Decimal balance { set; get; }
    }
}