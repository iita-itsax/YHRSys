using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace YHRSys.Models
{
    public class StockAlertJob : IJob
    {
        ApplicationDbContext db = new ApplicationDbContext();
        string[] groups = new string[] { "SuperAdmins" };

        public void Execute(IJobExecutionContext context)
        {
            List<ApplicationUser> groupUsers = CheckGroupAffiliate.GetGroupUsers(groups);
            string body = CheckGroupAffiliate.SendStockLevelAlert();
            //Debug.WriteLine(body);

            //Mail Server configurations from config file
            //System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/");
            System.Net.Configuration.MailSettingsSectionGroup mailSettings = (System.Net.Configuration.MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");

            if (mailSettings != null)
            {
                int port = mailSettings.Smtp.Network.Port;
                string host = mailSettings.Smtp.Network.Host;
                string password = mailSettings.Smtp.Network.Password;
                string username = mailSettings.Smtp.Network.UserName;
                string from = mailSettings.Smtp.From;
            
                if (groupUsers.Count>0)
                {
                    foreach(ApplicationUser user in groupUsers){
                        //Debug.WriteLine("SUPER ADMIN: " + user.Email);
                        //using(var message = new MailMessage("k.oraegbunam@cgiar.org", user.Email)){
                        using(var message = new MailMessage(from, user.Email)){
                            message.Subject = "YHRSys Stock Level Alert";
                            message.Body = body;
                            message.IsBodyHtml = true;
                            using (SmtpClient client = new SmtpClient
                            {
                                EnableSsl = false,
                                Host = host,//"casarray.iita.cgiarad.org",
                                Port = port,//25
                                Credentials = new NetworkCredential(username, password)
                            }) {
                                try
                                {
                                    client.Send(message);
                                }catch(Exception ex){
                                    new LogWriter(message.Subject + ", ERROR: " + ex.Message,"StockAlertJob");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}