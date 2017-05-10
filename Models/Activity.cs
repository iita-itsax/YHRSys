using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public enum ACTIVITYSTATUS
    {
        NEW, ASSIGNED, PENDING, COMPLETED, UNFINISHED
    }

    public partial class Activity : BaseEntity
    {
        public Activity()
        {
            this.activityAssignments = new HashSet<ActivityAssignment>();
        }
        private const int DEFAULT_VALUE = 0;

        [Key]
        public int activityId { get; set; }

        [Required]
        [DisplayName("Name")]
        public int activityDefinitionId { get; set; }

        [DisplayName("Location")]
        public int locationId { get; set; }

        [DisplayName("Type")]
        public int typeId { get; set; }

        [MaxLength]
        [DisplayName("Description")]
        public string description { get; set; }

        [DisplayName("Variety")]
        public Nullable<int> varietyId { get; set; }

        [DisplayName("OiC")]
        public string userId { get; set; }

        [DisplayName("Quantity"), Required]
        [DefaultValue(DEFAULT_VALUE)]
        [Range(1, (int)Int32.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int quantity { get; set; }

        [DisplayName("Quality"), Required]
        [DefaultValue(DEFAULT_VALUE)]
        [Range(1, 100, ErrorMessage = "Quality must be greater than zero.")]
        public int quality { get; set; }

        [DisplayName("Activity Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> activityDate { get; set; }

        [DisplayName("Status")]
        public string status { get; set; }

        [ForeignKey("varietyId")]
        public virtual Variety variety { get; set; }
        
        [ForeignKey("locationId")]
        public virtual Location location { get; set; }

        [ForeignKey("typeId")]
        public virtual MediumPrepType mediumPrepType { get; set; }

        [ForeignKey("activityDefinitionId")]
        public virtual ActivityDefinition activityDefinition { get; set; }

        [ForeignKey("userId")]
        public virtual ApplicationUser user { get; set; }

        public virtual ICollection<ActivityAssignment> activityAssignments { get; set; }
        
    }
}
