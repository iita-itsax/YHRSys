using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

        public static List<ApplicationUser> GetGroupUsers(string[] groupType)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Group group = db.Groups.Where(r => groupType.Contains(r.Name)).FirstOrDefault();
            if (group != null)
            {
                List<ApplicationUser> groupUsers = db.Users.Where(u => u.Groups.Any(g => g.GroupId == group.Id)).ToList();

                if (groupUsers == null)
                    return null;
                else
                    return groupUsers;
            }
            else
            {
                return null;
            }
        }

        public static string SendStockLevelAlert()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var reagentReOrderStatus = (from rea in db.Reagents
                                        join st in db.Stocks on rea.reagentId equals st.reagentId into reagentInfo
                                        from stocks in reagentInfo.AsEnumerable()
                                        .DefaultIfEmpty()
                                        where rea.reOrderLevel >= stocks.totalIn || stocks.totalIn == null || stocks.totalIn == 0
                                        orderby rea.name ascending
                                        select new
                                        {
                                            rName = rea.name,
                                            qtySum = (int?)stocks.totalIn ?? 0
                                        }).ToList();

            StringBuilder sb = new StringBuilder("");
            sb.Append("<tr><td><strong>Material/Item</strong></td><td><strong>Qty</strong></td></tr>");
            foreach (var lev in reagentReOrderStatus)
            {
                sb.Append("<tr><td>").Append(lev.rName).Append(":</td><td>").Append(lev.qtySum).Append("</td></tr>");
            }
            if (sb.Length > 0)
            {
                sb.Insert(0, "<h4>Re-Order Now!</h4><table style='width:100%'><colgroup><col width='30%' /><col width='70%' /></colgroup>");
                sb.Append("<tr><td>&nbsp;</td><td>&nbsp;</td></tr><tr><td colspan='2'><strong>Hint:</strong> Above info signifies stock level of corresponding reagents. Place an order now!</td></tr></table>");
            }

            new LogWriter(sb.ToString(), "StockAlertLog");

            return sb.ToString();
        }

        public static void CloseActivityLog() {
            ApplicationDbContext db = new ApplicationDbContext();
            DateTime today = new DateTime().Date;
            string status = "OPEN";

            /*var logs = (from rea in db.WeeklyActivityLogs.AsEnumerable()
                                        .DefaultIfEmpty()
                                        where DbFunctions.DiffDays(rea.endDate,today)>3 && rea.status==status orderby rea.endDate descending
                                        select new
                                        {
                                            logId = rea.activityLogId,
                                            description = rea.description,
                                            status = rea.status
                                        }).ToList();*/
            var logs = db.WeeklyActivityLogs.AsEnumerable().Where(x => DateTime.Now.Subtract(Convert.ToDateTime(x.endDate.Value)).Days > 3 && x.status == status).OrderBy(c => c.endDate).Select(s => new
                        {
                            logId = s.activityLogId,
                            description = s.description,
                            status = s.status
                        }).ToList();
            //UPDATE STATUS NOW
            foreach(var log in logs){
                //Debug.WriteLine("LOGID: " + log.logId + ", LOG DESCRIPTION: " + log.description + ", LOG STATUS: " + log.status);
                try
                {
                    WeeklyActivityLog act = (WeeklyActivityLog)db.WeeklyActivityLogs.Where(c => c.activityLogId == log.logId).FirstOrDefault();

                    act.status = "CLOSED";
                    act.updatedBy = "AutoSystem";
                    act.updatedDate = DateTime.Now;

                    db.SaveChanges();
                    string msgLog = string.Format("ACTIVITY LOGID: {0}, DATE UPDATED: {1}", log.logId, DateTime.Now);
                    new LogWriter(msgLog, "CloseActivityLog");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    new LogWriter(ex.Message, "CloseActivityLog");
                }
            }
        }
    }
}