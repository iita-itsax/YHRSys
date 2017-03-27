using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public partial class Stock
    {
        [Key]
        public int stockId { get; set; }

        [Required]
        public int reagentId { get; set; }

        public int totalIn { get; set; }

        public int totalOut { get; set; }

        [ForeignKey("reagentId")]
        public Reagent reagent { get; set; }

        /*[NotMapped]
        public string NameQty { 
            get 
            { 
                return reagent.name + " - " + (totalIn - totalOut); 
            } 
        }*/
    }
}