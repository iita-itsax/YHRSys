using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public enum STOCK
    {
        IN, OUT
    }

    public partial class Inventory
    {
        private const int DEFAULT_VALUE = 0;

        [Key]
        public int inventoryId { get; set; }

        [DisplayName("Reagent"), Required]
        public int reagentId { get; set; }

        [DisplayName("Quantity"), Required]
        [DefaultValue(DEFAULT_VALUE)]
        public int quantity { get; set; }

        [DisplayName("Stock Movement")]
        public STOCK stock { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Stock Date")]
        public Nullable<System.DateTime> stockDate { get; set; }

        //This property line is for the user that purchased the item/product
        [DisplayName("Stocker(Staff/User)")]
        public int stockUserId { get; set; }

        [DefaultValue(DEFAULT_VALUE)]
        public int initialStock { get; set; }

        public BaseDateEntity baseDateEntity { get; set; }
        public VersionedEntity version { get; set; }

        public BaseUserEntity baseUserEntity { get; set; }

        [ForeignKey("reagentId")]
        public virtual Reagent reagent { get; set; }
    }
}