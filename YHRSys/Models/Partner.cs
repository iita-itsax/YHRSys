using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YHRSys.Models
{
    public partial class Partner : BaseEntity
    {
        public Partner()
        {
           // this.partnerActivities = new HashSet<PartnerActivity>();
            this.partnerContacts = new HashSet<PartnerContact>();
        }

        [Key]
        public int partnerId { get; set; }

        [DisplayName("Name"), Required]
        [MaxLength(255)]
        public string name { get; set; }

        [DisplayName("Contact Address"), Required]
        [MaxLength(255)]
        public string contactAddress { get; set; }

        [DisplayName("City"), Required]
        [MaxLength(255)]
        public string contactCity { get; set; }

        [DisplayName("State"), Required]
        [MaxLength(255)]
        public string contactState { get; set; }

        [DisplayName("Country"), Required]
        [MaxLength(255)]
        public string contactCountry { get; set; }

        [DisplayName("Phone No")]
        [RegularExpression(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}", ErrorMessage = "Invalid Phone Number! Format should be xxx-xxx-xxxx")]
        public string phoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email. Format: yourname@yoursite.com")]
        [DisplayName("Email Address"), Required]
        public string emailAddress { get; set; }

        [DisplayName("Web Address")]
        [Url(ErrorMessage = "Invalid URL! Format: http://www.yoursite.com")]
        public string webAddress { get; set; }

        [DisplayName("Geo Long.")]
        public string geoLongitude { get; set; }

        [DisplayName("Geo Lat.")]
        public string geoLatitude { get; set; }

        //public virtual ICollection<PartnerActivity> partnerActivities { get; set; }
        public virtual ICollection<PartnerContact> partnerContacts { get; set; }
    }
}
