using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YHRSys.Models
{
    public partial class Partner
    {
        public Partner()
        {
            this.partnerActivities = new HashSet<PartnerActivity>();
            this.partnerContacts = new HashSet<PartnerContactPerson>();
        }

        [Key]
        public int partnerId { get; set; }

        [Required]
        [MaxLength(255)]
        public string name { get; set; }

        [Required]
        [MaxLength(255)]
        public string contactAddress { get; set; }

        public string phoneNumber { get; set; }

        [Required]
        public string emailAddress { get; set; }

        public string webAddress { get; set; }

        public decimal geoLongitude { get; set; }
        public decimal geoLatitude { get; set; }

        public BaseDateEntity baseDateEntity { get; set; }
        public VersionedEntity version { get; set; }

        public BaseUserEntity baseUserEntity { get; set; }

        public virtual ICollection<PartnerActivity> partnerActivities { get; set; }
        public virtual ICollection<PartnerContactPerson> partnerContacts { get; set; }
    }
}
