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
    public class ActivityWorkplan : BaseEntity
    {
        private const int DEFAULT_VALUE = 0;

        public ActivityWorkplan()
        {
            this.WeeklyActivityLogs = new HashSet<WeeklyActivityLog>();
        }

        [Key]
        public int workplanId { get; set; }

        [DisplayName("Start Period")]
        public Nullable<System.DateTime> StartPeriod { get; set; }

        [DisplayName("End Period")]
        public Nullable<System.DateTime> EndPeriod { get; set; }

        [DisplayName("Objective")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(255, ErrorMessage = "{0}'s length should not be more than {1}.")]
        //[StringLength(50, MinimumLength = 10, ErrorMessage = "{0}'s length should not be more than {2} and {1}.")]
        public string Objective { get; set; }

        [DisplayName("Performance indicator")]
        [Required(ErrorMessage="{0} is required.")]
        [StringLength(255, ErrorMessage = "{0}'s length should not be more than {1}.")]
        public string PerformanceIndicator { get; set; }

        public ICollection<WeeklyActivityLog> WeeklyActivityLogs {get; set;}

        [NotMapped]
        public string FullObjective
        {
            get
            {
                return Objective + " (" + StartPeriod.Value.ToString("dd/MM/yyyy") +" - " + EndPeriod.Value.ToString("dd/MM/yyyy") +")";
            }
        }
    }
}