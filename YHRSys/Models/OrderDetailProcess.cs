using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace YHRSys.Models
{
    public class OrderDetailProcess
    {
        public string BodyString(Order order) {
            string body = "";

            if (order.OrderDetails.Count() > 0)
            {
                body += string.Format("<h4>Variety Order Details</h4>");
                body += string.Format("<table>");
                body += string.Format("<colgroup>");
                body += string.Format("<col />");
                body += string.Format("<col />");
                body += string.Format("</colgroup>");
                body += string.Format("<tr><th>Variety</th><th>Qty</th></tr>");
                foreach (OrderDetail od in order.OrderDetails)
                {
                    body += string.Format("<tr><td>{0}</td><td>{1}</td></tr>", od.Variety.FullDescription, od.Quantity);
                }
                body += string.Format("</table>");
            }
            return body;
        }

        public void SendCompletedAlert(Order order, int id) {
            string body = "";

            if (order != null)
            {
                body = string.Format("<p>Order details from <strong>{0}, {1}</strong></p><p>ADDRESS:<br/>===============<br/>Email: {2}<br/>Address: {3}<br/>City: {4}<br/>State: {5}<br/>Country: {6}<br/></p><p>Contact IITA YIIFSWA for your completed Varieties Order</p>", order.LastName, order.FirstName, order.Email, order.Address, order.City, order.State, order.Country);

                if (order.OrderDetails.Count() > 0)
                {
                    OrderDetailProcess oDP = new OrderDetailProcess();

                    body += oDP.BodyString(order);
                }
            }
            else
                body = string.Format("Your Varieties Order with ID: {0} processing has been completed.", id);

            System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/");
            System.Net.Configuration.MailSettingsSectionGroup mailSettings = (System.Net.Configuration.MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");

            if (mailSettings != null)
            {
                int port = mailSettings.Smtp.Network.Port;
                string host = mailSettings.Smtp.Network.Host;
                string password = mailSettings.Smtp.Network.Password;
                string username = mailSettings.Smtp.Network.UserName;
                string from = mailSettings.Smtp.From;

                if (order.Email!=null)
                {
                    //Debug.WriteLine("SUPER ADMIN: " + user.Email);
                        //using(var message = new MailMessage("k.oraegbunam@cgiar.org", user.Email)){
                    using (var message = new MailMessage(from, order.Email))
                    {
                        message.Subject = "IITA YIIFSWA Completed Order Notification";
                        message.Body = body;
                        message.IsBodyHtml = true;
                        using (SmtpClient client = new SmtpClient
                        {
                            EnableSsl = false,
                            Host = host,//"casarray.iita.cgiarad.org",
                            Port = port,//25
                            Credentials = new NetworkCredential(username, password)
                        })
                        {
                            try
                            {
                                client.Send(message);
                            }
                            catch (Exception ex)
                            {
                                new LogWriter(message.Subject + ", ERROR: " + ex.Message, "CompletedOrderNotification");
                            }
                        }
                    }
                }
            }
        }
    }
}