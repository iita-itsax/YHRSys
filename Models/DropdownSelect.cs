using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DropdownSelect.Models
{
    public class SelectedValue
    {
        public bool checkForSelectedValue(string cnt, string maxCnt)
        {
            return cnt == maxCnt;
        }

        public bool checkForSelectedValue(int cnt, int maxCnt)
        {
            return cnt == maxCnt;
        }
    }
}