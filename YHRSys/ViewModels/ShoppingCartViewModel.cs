using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YHRSys.Models;

namespace YHRSys.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
        public decimal QtyTotal { get; set; }
    }
}