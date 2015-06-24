using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YHRSys.Models
{
    public class CheckGroupAffiliate
    {
        public static bool checkGroupExist(ApplicationUser currentUser, string[] groupType) {
            ApplicationDbContext db = new ApplicationDbContext();
            Group group = db.Groups.Where(r => groupType.Contains(r.Name)).FirstOrDefault();
            if (group != null)
            {
                ApplicationUser groupUsers = db.Users.Where(u => u.Groups.Any(g => g.GroupId == group.Id) && u.Id == currentUser.Id).FirstOrDefault();

                if (groupUsers == null)
                    return false;
                else
                    return true;
            }
            else {
                return false;
            }
        }
    }
}