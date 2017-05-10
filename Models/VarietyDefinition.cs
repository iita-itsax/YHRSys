using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YHRSys.Models
{
    public partial class VarietyDefinition : BaseEntity
    {
        public VarietyDefinition()
        {
            this.varieties = new HashSet<Variety>();
        }

        [Key]
        public int varietyDefinitionId { get; set; }

        [DisplayName("Def. Name"), Required]
        public string name { get; set; }

        public virtual ICollection<Variety> varieties { get; set; }
    }
}