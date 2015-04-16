using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YHRSys.Models
{
    public class Measurements : BaseEntity
    {
        [Key]
        public int measurementId { get; set; }

        [DisplayName("Name"), Required]
        public string name { get; set; }

        [DisplayName("Abbreviation"), Required]
        public string uom { get; set; }
    }
}