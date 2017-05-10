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
    public class LocationSubordinate : BaseEntity
    {
        private const int DEFAULT_VALUE = 0;

        public LocationSubordinate()
        {
        }

        [Key]
        public int subordinateId { get; set; }

        [DisplayName("Location OiC"), Required]
        public int locationUserId { get; set; }

        [DisplayName("Subordinate")]
        public string userSubordinateId { get; set; }

        [DisplayName("Status")]
        public STATUS status { get; set; }

        [DisplayName("Work Brief")]
        public string workBrief { get; set; }

        [ForeignKey("userSubordinateId")]
        public virtual ApplicationUser subordinate { get; set; }

        [ForeignKey("locationUserId")]
        public virtual LocationUser locationUser { get; set; }

        [NotMapped]
        [DisplayName("FullName")]
        public string SubordinateFullName
        {
            get
            {
                return subordinate.FirstName + " " + subordinate.LastName;
            }
        }
    }
}