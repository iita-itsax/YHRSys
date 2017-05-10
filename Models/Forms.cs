using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YHRSys.Models
{
    public class Forms : BaseEntity
    {
        [Key]
        public int formId { get; set; }

        [DisplayName("Name"), Required]
        public string name { get; set; }
    }
}