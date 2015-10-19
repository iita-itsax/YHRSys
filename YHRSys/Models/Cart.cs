using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YHRSys.Models
{
    public class Cart
    {
        [Key]
        public int RecordId { get; set; }
        public string CartId { get; set; }
        public int VarietyId { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = " ")]
        [Range(0, 100, ErrorMessage = "Quantity must be between 0 and 100")]
        [DisplayName("Quantity")]
        public int Count { get; set; }

        public System.DateTime DateCreated { get; set; }
        public virtual Variety Variety { get; set; }
    }
}