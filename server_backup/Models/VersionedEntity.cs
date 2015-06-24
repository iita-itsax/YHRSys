using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YHRSys.Models
{
    [ComplexType]
    public class VersionedEntity
    {
        [ConcurrencyCheck]
        [Timestamp]
        public byte[] version { get; set; }
    }
}