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
    public class ActivityAchievement : BaseEntity
    {
        private const int DEFAULT_VALUE = 0;

        public ActivityAchievement()
        {
        }

        [Key]
        public int achievementId { get; set; }

        [DisplayName("Weekly Activity")]
        public int activityLogId { get; set; }

        [DisplayName("Achievement Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> achievementDate { get; set; }

        [MaxLength]
        [DisplayName("Achievement Description")]
        public string description { get; set; }

        [ForeignKey("activityLogId")]
        public virtual WeeklyActivityLog WeeklyActivityLog { get; set; }
    }
}