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
    public class BaseDateEntity
    {

        [DisplayName("CreatedOn")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] 
        public System.DateTime createdDate { get; set; }

        [DisplayName("UpdatedOn")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] 
        public Nullable<System.DateTime> updatedDate { get; set; }
    }
}