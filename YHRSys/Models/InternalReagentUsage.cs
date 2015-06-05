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
    public partial class InternalReagentUsage : BaseEntity
    {
        public InternalReagentUsage() { 
        
        }

        private const int DEFAULT_VALUE = 0;

        [Key]
        public int usageId { get; set; }

        [DisplayName("Reagent")]
        public int reagentId { get; set; }

        [DisplayName("Quantity"), Required]
        [DefaultValue(DEFAULT_VALUE)]
        [Range(1, (int)Int32.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int quantity { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Dispatch Date")]
        public Nullable<System.DateTime> dispatchDate { get; set; }

        //This property line is for the user that purchased the item/product
        [DisplayName("DispatchedBy")]
        public string userId { get; set; }

        [DisplayName("ReceivedBy")]
        public string receivedBy { get; set; }

        [DisplayName("Note"), Required]
        public string note { get; set; }

        [ForeignKey("reagentId")]
        public virtual Reagent reagent { get; set; }

        [ForeignKey("userId")]
        public virtual ApplicationUser user { get; set; }

        [NotMapped]
        public string DispatcherFullName
        {
            get
            {
                return user.FirstName + " " + user.LastName;
            }
        }
    }
}