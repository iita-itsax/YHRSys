using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public class WeeklyActivityLog : BaseEntity
    {
        private const int DEFAULT_VALUE = 0;

        public WeeklyActivityLog()
        {
        }

        [Key]
        public int activityLogId { get; set; }

        [DisplayName("Staff")]
        public string userId { get; set; }

        [DisplayName("Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> startDate { get; set; }

        [DisplayName("End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> endDate { get; set; }

        [DisplayName("Description")]
        public string description { get; set; }

        [ForeignKey("userId")]
        public virtual ApplicationUser staff { get; set; }

        [NotMapped]
        [DisplayName("OiC")]
        public string StaffFullName
        {
            get
            {
                return staff.FirstName + " " + staff.LastName;
            }
        }
    }
}