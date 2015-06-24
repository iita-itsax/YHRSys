using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public class BaseEntity
    {
        [DisplayName("CreatedOn")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Nullable<System.DateTime> createdDate { get; set; }

        [DisplayName("UpdatedOn")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)] 
        public Nullable<System.DateTime> updatedDate { get; set; }

        [ConcurrencyCheck]
        [Timestamp]
        public Byte[] Timestamp { get; set; }

        [DisplayName("CreatedBy")]
        //[Required(ErrorMessage = "{0} should not be blank")]
        public string createdBy { get; set; }

        [DisplayName("UpdatedBy")]
        public string updatedBy { get; set; }

        [NotMapped]
        public string extendCreatedBy
        {
            get
            {
                if (createdBy != null)
                    return createdBy.ToUpperFirstLetter();
                else
                    return createdBy;
            }
        }

        [NotMapped]
        public string extendUpdatedBy
        {
            get
            {
                if (updatedBy != null)
                    return updatedBy.ToUpperFirstLetter();
                else
                    return updatedBy;
            }
        }
    }
}