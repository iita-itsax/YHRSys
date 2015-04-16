using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YHRSys.Models
{
    public partial class Species : BaseEntity
    {
        public Species()
        {
            this.varieties = new HashSet<Variety>();
        }

        [Key]
        public int specieId { get; set; }

        [DisplayName("Specie Name"), Required]
        public string name { get; set; }

        public virtual ICollection<Variety> varieties { get; set; }
    }
}
