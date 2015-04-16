using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public partial class Reagent : BaseEntity
    {
        private const int DEFAULT_VALUE = 0;
        public Reagent()
        {
            this.inventories = new HashSet<Inventory>();
            this.partnerActivities = new HashSet<PartnerActivity>();
            this.internalReagentUsages = new HashSet<InternalReagentUsage>();
        }

        [Key]
        public int reagentId { get; set; }

        [DisplayName("Name"), Required]
        public string name { get; set; }

        [Required]
        [DisplayName("UoM")]
        public int measurementId { get; set; }

        [DisplayName("Description")]
        public string description { get; set; }

        [Required]
        [DisplayName("ReOrder Level")]
        [DefaultValue(DEFAULT_VALUE)]
        [Range(0, (int)Int32.MaxValue, ErrorMessage = "Quantity must be not less than zero.")]
        public int reOrderLevel { get; set; }

        public virtual ICollection<Inventory> inventories { get; set; }
        public virtual ICollection<PartnerActivity> partnerActivities { get; set; }
        public virtual ICollection<InternalReagentUsage> internalReagentUsages { get; set; }

        [ForeignKey("measurementId")]
        public virtual Measurements measurements { get; set; }
    }
}
