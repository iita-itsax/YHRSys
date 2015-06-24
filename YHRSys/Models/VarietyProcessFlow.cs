using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public partial class VarietyProcessFlow : BaseEntity
    {
        private const int DEFAULT_VALUE = 0;

        [Key]
        public int processId { get; set; }

        [DisplayName("Variety")]
        [Required(ErrorMessage = "Variety should not be empty")]
        public int varietyId { get; set; }

        [DisplayName("Officer-In-Charge")]
        public string userId { get; set; }

        [DisplayName("Form")]
        [Required(ErrorMessage = "Form should not be empty")]
        public string form { get; set; }

        [DisplayName("Process Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> processDate { get; set; }

        //This is the user initiated this variety process flow
        [ForeignKey("userId")]
        public virtual ApplicationUser user { get; set; }

        [DisplayName("Rank")]
        [Required(ErrorMessage = "Rank should not be empty")]
        public string rank { get; set; }

        [DisplayName("Barcode")]
        public string barcode { get; set; }

        [DisplayName("BarcodeImageUrl")]
        public byte[] barcodeImageUrl { get; set; }

        [MaxLength]
        [DisplayName("Description")]
        public string description { get; set; }

        [DisplayName("Quality"), Required]
        [DefaultValue(DEFAULT_VALUE)]
        [Range(1, 100, ErrorMessage = "Quality must be greater than zero.")]
        public int quality { get; set; }

        [ForeignKey("varietyId")]
        public virtual Variety variety { get; set; }

        [NotMapped]
        public string FullIdentifier {
            get {
                string identifier = null;
                if(variety.varietyDefinition.name != null)
                    identifier = variety.varietyDefinition.name;

                if (identifier == null)
                {
                    if (barcode != null)
                        identifier = barcode;
                    else if (variety.sampleNumber != null)
                        identifier = variety.sampleNumber;
                }
                else {
                    if (barcode != null)
                        identifier += "(" + barcode + ")";
                    else if (variety.sampleNumber != null)
                        identifier += "(" + variety.sampleNumber + ")";
                }

                return identifier; 
            }
        }
    }
}
