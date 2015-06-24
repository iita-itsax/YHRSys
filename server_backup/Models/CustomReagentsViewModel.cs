using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YHRSys.Models
{
    public class CustomReagentsViewModel
    {
        public String rName { set; get; }
        public DateTime? createdDate { set; get; }
        public Int32? quantity { set; get; }
        public Int32? qtySum { set; get; }
    }
}