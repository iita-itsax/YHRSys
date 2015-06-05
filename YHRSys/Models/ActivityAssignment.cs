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
    public class ActivityAssignment : BaseEntity
    {
        private const int DEFAULT_VALUE = 0;

        public ActivityAssignment()
        {
        }

        [Key]
        public int assignmentId { get; set; }

        [DisplayName("Activity"), Required]
        public int activityId { get; set; }

        [DisplayName("Assigned To")]
        public string userId { get; set; }

        [DisplayName("Todo")]
        public string todo { get; set; }

        [ForeignKey("userId")]
        public virtual ApplicationUser assignee { get; set; }

        [ForeignKey("activityId")]
        public virtual Activity activity { get; set; }

        [NotMapped]
        [DisplayName("FullName")]
        public string AssigneeFullName
        {
            get
            {
                return assignee.FirstName + " " + assignee.LastName;
            }
        }
    }
}