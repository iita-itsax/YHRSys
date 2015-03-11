using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public partial class PartnerContactPerson
    {
        [Key]
        public int contactId { get; set; }

        public int partnerId { get; set; }

        [Required]
        public string firstName { get; set; }

        public string otherNames { get; set; }

        [Required]
        public string lastName { get; set; }

        public string phoneNumber { get; set; }
        public string emailAddress { get; set; }
        public string contactAddress { get; set; }
        public string webAddress { get; set; }

        public decimal geoLongitude { get; set; }
        public decimal geoLatitude { get; set; }

        public BaseDateEntity baseDateEntity { get; set; }
        public VersionedEntity version { get; set; }

        public BaseUserEntity baseUserEntity { get; set; }

        [ForeignKey("partnerId")]
        public virtual Partner partner { get; set; }
    }
}
