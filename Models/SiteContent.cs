using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace YHRSys.Models
{
    public enum PUBLISHSTATUS {
        New, Pending, Published, Unpublished
    }

    public class SiteContent : BaseEntity
    {
        public SiteContent() { }

        [Key]
        public int id { get; set; }

        [DisplayName("Caption"), Required]
        public string caption { get; set; }

        [DisplayName("Summary")]
        public string summary { get; set; }

        [DisplayName("Full Article")]
        [AllowHtml]
        public string fullArticle { get; set; }

        [DisplayName("Status")]
        public PUBLISHSTATUS status { get; set; }

        [DisplayName("Publisher")]
        public string userId { get; set; }

        [ForeignKey("userId")]
        public virtual ApplicationUser user { get; set; }
    }
}