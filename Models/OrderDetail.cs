using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YHRSys.Models
{
    public class OrderDetail : BaseEntity
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int VarietyId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual Variety Variety { get; set; }
        public virtual Order Order { get; set; }
    }
}