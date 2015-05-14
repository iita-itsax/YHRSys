using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    public partial class PartnerContact : BaseEntity
    {
        [Key]
        public int contactId { get; set; }

        [DisplayName("Partner")]
        [Required]
        public int partnerId { get; set; }

        [DisplayName("Title")]
        public string personTitle { get; set; }

        [DisplayName("Gender")]
        public string gender { get; set; }
         
        [Required]
        [DisplayName("First Name")]
        public string firstName { get; set; }

        [DisplayName("Other Name")]
        public string otherNames { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string lastName { get; set; }

        [DisplayName("Phone no")]
        //[RegularExpression(@"((\(\d{3}\)?)|(\d{3}))?\d{3}\d{4}", ErrorMessage = "Invalid Phone Number! Format should be (xxx)-xxx-xxx-xxxx")]
        public string phoneNumber { get; set; }

        [DisplayName("Email address")]
        [EmailAddress(ErrorMessage = "Invalid Email. Format: yourname@yoursite.com")]
        public string emailAddress { get; set; }

        [DisplayName("Contact address")]
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

        [DisplayName("Web address")]
        [Url(ErrorMessage = "Invalid URL! Format: http://www.yoursite.com")]
        public string webAddress { get; set; }

        [DisplayName("Geo long")]
        public Nullable<decimal> geoLongitude { get; set; }

        [DisplayName("Geo lat")]
        public Nullable<decimal> geoLatitude { get; set; }

        [ForeignKey("partnerId")]
        public virtual Partner partner { get; set; }
    }
}
