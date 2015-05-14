using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YHRSys.Models
{
    public class ReagentStockModel
    {
        public int reagentId { set; get; }
        public string reagentType { set; get; }    
        public string reagentName { set; get; }
        public string measurementName { set; get; }
        public int reOrderLevel { set; get; }
        public string description { set; get; }
        public int stockBalance { set; get; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string updatedBy { get; set; }
        public DateTime? updatedDate { get; set; }
    }
}