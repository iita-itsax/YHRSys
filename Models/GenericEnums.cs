using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YHRSys.Models
{
    public class GenericEnums {
        public bool EnumValidate<T>(object o)
        {
            ACTIVITYSTATUS status;
            if (Enum.TryParse(o.ToString(), out status))
            {
                T enumVal = (T)Enum.Parse(typeof(T), o.ToString());
                return true;
            }
            else { return false; }
        }
    }
}