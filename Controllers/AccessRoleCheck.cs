using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YHRSys.Models;

namespace YHRSys.Controllers
{
    class AccessRoleCheck
    {
        public Boolean checkPerm(string[] groups, ApplicationUser user, string perms)
        {
            List<ApplicationUser> groupUsers = CheckGroupAffiliate.GetGroupUsers(groups);
            if (groupUsers != null)
            {
                foreach (ApplicationUser userG in groupUsers)
                {
                    if (userG.Id == user.Id)
                    {
                        foreach (var r in userG.Roles)
                        {
                            if (perms.Contains(r.Role.Name))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            else return false;
        }

        public Boolean checkOwnPerm(string[] groups, ApplicationUser user, string perms)
        {
            List<ApplicationUser> groupOwnUsers = CheckGroupAffiliate.GetGroupUsers(groups);
            if (groupOwnUsers != null)
            {
                foreach (ApplicationUser userG in groupOwnUsers)
                {
                    if (userG.Id == user.Id)
                    {
                        foreach (var r in userG.Roles)
                        {
                            if (perms.Contains(r.Role.Name))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            else return false;
        }
    }
}
