using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YHRSys.Models
{
    public class Measurements
    {
        [Key]
        public int measurementId { get; set; }
        [Required]
        public string name { get; set; }

        [DisplayName("UoM"), Required]
        public string uom { get; set; }

        public BaseDateEntity baseDateEntity { get; set; }
        public VersionedEntity version { get; set; }

        public BaseUserEntity baseUserEntity { get; set; }
    }
}