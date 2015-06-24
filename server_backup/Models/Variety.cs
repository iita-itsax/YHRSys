using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public partial class Variety : BaseEntity
    {
        public Variety()
        {
            this.varietyProcessFlows = new HashSet<VarietyProcessFlow>();
            this.activities = new HashSet<Activity>();
        }

        [Key]
        public int varietyId { get; set; }

        [Required(ErrorMessage="Variety name should not be empty")]
        [DisplayName("Name")]
        public int varietyDefinitionId { get; set; }

        [Required(ErrorMessage = "Sample number should not be empty")]
        [DisplayName("Sample No")]
        public string sampleNumber { get; set; }

       // [Required(ErrorMessage = "Officer-in-Charge should not be empty")]
        [DisplayName("Officer-In-Charge")]
        public string userId { get; set; }

        //This is the user initiated this variety process flow
        [ForeignKey("userId")]
        public virtual ApplicationUser user { get; set; }

        [DisplayName("Test Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> testDate { get; set; }

        [DisplayName("Release status")]
        public string releaseStatus { get; set; }

        [DisplayName("Resistance")]
        public string resistance { get; set; }

        [DisplayName("Storability")]
        public string storability { get; set; }

        [DisplayName("Poundability")]
        public string poundability { get; set; }

        [DisplayName("Uniformity")]
        public string uniformity { get; set; }

        [DisplayName("Stability")]
        public string stability { get; set; }

        [DisplayName("Distinctness")]
        public string distinctness { get; set; }

        [DisplayName("Value for Cultivation")]
        public Nullable<int> value { get; set; }

        [DisplayName("Location")]
        public Nullable<int> locationId { get; set; }

        [ForeignKey("locationId")]
        public virtual Location location { get; set; }
        
        [DisplayName("Species")]
        public Nullable<int> specieId { get; set; }

        [ForeignKey("specieId")]
        public virtual Species species { get; set; }

        [ForeignKey("varietyDefinitionId")]
        public virtual VarietyDefinition varietyDefinition { get; set; }

        public virtual ICollection<VarietyProcessFlow> varietyProcessFlows { get; set; }

        public virtual ICollection<Activity> activities { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                if(user!=null)
                    return user.FirstName + " " + user.LastName;
                return null;
            }
        }

        [NotMapped]
        public string FullDescription
        {
            get
            {
                if (varietyDefinition != null)
                    return varietyDefinition.name + " - " + sampleNumber;
                else if (species != null)
                    return sampleNumber  + " - {specie: " + species.name + "}";
                else
                    return sampleNumber;
            }
        }
    }
}
