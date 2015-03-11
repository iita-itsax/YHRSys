using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public partial class Reagent
    {
        public Reagent()
        {
            this.inventories = new HashSet<Inventory>();
            this.partnerActivities = new HashSet<PartnerActivity>();
        }

        [Key]
        public int reagentId { get; set; }

        [DisplayName("Name"), Required]
        public string name { get; set; }

        [Required]
        public int measurementId { get; set; }

        public BaseDateEntity baseDateEntity { get; set; }
        public VersionedEntity version { get; set; }

        public BaseUserEntity baseUserEntity { get; set; }

        public virtual ICollection<Inventory> inventories { get; set; }
        public virtual ICollection<PartnerActivity> partnerActivities { get; set; }

        [ForeignKey("measurementId")]
        public virtual Measurements measurements { get; set; }
    }
}
