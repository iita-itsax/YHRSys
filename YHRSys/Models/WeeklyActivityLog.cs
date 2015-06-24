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
            this.ActivityAchievements = new HashSet<ActivityAchievement>();
        }

        [Key]
        public int activityLogId { get; set; }

        [DisplayName("Activity Workplan")]
        public int workplanId { get; set; }

        [DisplayName("Staff")]
        public string userId { get; set; }

        [DisplayName("Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> startDate { get; set; }

        [DisplayName("End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> endDate { get; set; }

        [MaxLength]
        [DisplayName("Description")]
        public string description { get; set; }

        [DisplayName("Status")]
        public string status { get; set; }

        [ForeignKey("userId")]
        public virtual ApplicationUser staff { get; set; }

        [ForeignKey("workplanId")]
        public virtual ActivityWorkplan activityWorkplan { get; set; }

        public ICollection<ActivityAchievement> ActivityAchievements { get; set; }

        [NotMapped]
        [DisplayName("OiC")]
        public string StaffFullName
        {
            get
            {
                return staff.FirstName + " " + staff.LastName;
            }
        }

        [NotMapped]
        public string FullDescription
        {
            get
            {
                return description + " (" + startDate.Value.ToString("dd/MM/yyyy") + " - " + endDate.Value.ToString("dd/MM/yyyy") +")";
            }
        }
    }
}