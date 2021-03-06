﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YHRSys.Models
{
    public class Ranks : BaseEntity
    {
        [Key]
        public int rankId { get; set; }

        [DisplayName("Name"), Required]
        public string name { get; set; }
    }
}