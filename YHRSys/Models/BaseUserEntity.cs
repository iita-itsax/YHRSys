using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    [ComplexType]
    public class BaseUserEntity
    {
        [DisplayName("CreatedBy")]
        [Required]
        public string createdBy { get; set; }

        [DisplayName("UpdatedBy")]
        public string updatedBy { get; set; }
    }
}