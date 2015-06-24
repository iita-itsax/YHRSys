using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace YHRSys.Models
{
    
    public class CustomActivityExport
    {
        //ApplicationDbContext db = new ApplicationDbContext();

        public string Activity { set; get; }
        public string Location { set; get; }
        public string Type { set; get; }
        public string Description { set; get; }
        public string Variety { set; get; }
        public string OiC { set; get; }
        public string ActionPerformed { set; get; }
        public int Quantity { set; get; }
        public int Quality { set; get; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ActivityDate { set; get; }
        public string Status { set; get; }
    }
}
