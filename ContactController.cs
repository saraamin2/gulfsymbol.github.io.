using GulfSymbolProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace GulfSymbolProject.Controllers
{
    public class ContactController : Controller
    {
       

        public void SendEmail(string toAddress, string fromAddress,
                       string subject, string body)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    const string email = "gulfsymbol@gmail.com";
                    const string password = "@naser224466";

                    var loginInfo = new NetworkCredential(email, password);


                    mail.From = new MailAddress(fromAddress);
                    mail.To.Add(new MailAddress(toAddress));
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    try
                    {
                        using (var smtpClient = new SmtpClient(
                                                         "smtp.gmail.com", 587))
                        {
                            smtpClient.EnableSsl = true;
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtpClient.Credentials = loginInfo;
                            smtpClient.Send(mail);
                        }

                    }

                    finally
                    {
                        //dispose the client
                        mail.Dispose();
                    }

                }
            }
            catch (SmtpFailedRecipientsException ex)
            {
                foreach (SmtpFailedRecipientException t in ex.InnerExceptions)
                {
                    var status = t.StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        Response.Write("Delivery failed - retrying in 5 seconds.");
                        System.Threading.Thread.Sleep(5000);
                        //resend
                        //smtpClient.Send(message);
                    }
                    else
                    {
                        Response.Write("Failed to deliver message to {0}");
                    }
                }
            }
            catch (SmtpException Se)
            {
                // handle exception here
                Response.Write(Se.ToString());
            }

            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

        }
        // GET: Contact
        public ActionResult Contact()
        {
            ViewBag.NavClassContact = "active";

            return View();
        }
        [HttpPost]
        public ActionResult Contact(ContactViewModel c)
        {
            if (ModelState.IsValid)
            {

                //prepare email
                var toAddress = "sarah.amin55@gmail.com";
                var fromAddress = c.Email.ToString();
                var subject = "Test enquiry from " + c.FirstName;
                var body = new StringBuilder();
                body.Append("Name: " + c.FirstName + "\n");
                body.Append("Email: " + c.Email + "\n");
                body.Append("Telephone: " + c.MobileNumber + "\n\n");
                body.Append(c.Message);

                //start email Thread
                var tEmail = new Thread(() =>
               SendEmail(toAddress, fromAddress, subject,body.ToString()));
                tEmail.Start();
           
                    return RedirectToAction("Contact");
               
            }

            ViewBag.NavClassContact = "active";
            return View();
        }

    }
}