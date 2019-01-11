using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using Commons.Libs;
using PassHelper;

namespace Commons.Libs
{
    public class XMail
    {
        private static PasswordObject p = new PasswordObject();

        public static string SMTPServer = WebConfigurationManager.AppSettings["SMTPServer"].ToString();
        public static int Port = Int32.Parse(WebConfigurationManager.AppSettings["Port"].ToString());
        public static string CredentialUserName = WebConfigurationManager.AppSettings["CredentialUserName"].ToString();
        public static string CredentialPassword =  WebConfigurationManager.AppSettings["CredentialPassword"].ToString();
        public static string EnableSsl = "False";
        public static bool ssl = false;
        public static string from = "itfa.ahihi@gmail.com";

        public static void Send(String to, String subject, String body)
        {

            //Send(from, to, subject, body);
            String cc = "";
            String bcc = "";
            String attachments = "";
            Thread email = new Thread(delegate ()
            {
                SendAsyncEmail(from, to, cc, bcc, subject, body, attachments);
            });
            email.IsBackground = true;
            email.Start();
        }
        public static bool Sended(String to, String subject, String body)
        {
            try
            {

                //Send(from, to, subject, body);
                String cc = "";
                String bcc = "";
                String attachments = "";
                //Thread email = new Thread(delegate ()
                //{
                //    SendAsyncEmail(from, to, cc, bcc, subject, body, attachments);
                //});
                //email.IsBackground = true;
                //email.Start();
                SendAsyncEmail(from, to, cc, bcc, subject, body, attachments);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Gửi email đơn giản thông qua tài khoản gmail
        /// </summary>
        /// <param name="from">Email người gửi</param>
        /// <param name="to">Email người nhận</param>
        /// <param name="subject">Tiêu đề mail</param>
        /// <param name="body">Nội dung mail</param>
        public static void Send(String from, String to, String subject, String body)
        {
            String cc = "";
            String bcc = "";
            String attachments = "";

            Thread email = new Thread(delegate ()
            {
                SendAsyncEmail(from, to, cc, bcc, subject, body, attachments);
            });
            email.IsBackground = true;
            email.Start();
        }

        /// <summary>
        /// Gửi email thông qua tài khoản gmail
        /// </summary>
        /// <param name="from">Email người gửi</param>
        /// <param name="to">Email người nhận</param>
        /// <param name="cc">Danh sách email những người cùng nhận phân cách bởi dấu phẩy</param>
        /// <param name="bcc">Danh sách email những người cùng nhận phân cách bởi dấu phẩy</param>
        /// <param name="subject">Tiêu đề mail</param>
        /// <param name="body">Nội dung mail</param>
        /// <param name="attachments">Danh sách file định kèm phân cách bởi phẩy hoặc chấm phẩy</param>
        /// 

        public static void Sends(String from, String to, String cc, String bcc, String subject, String body, String attachments)
        {

            if (EnableSsl == "0" || EnableSsl == "true" || EnableSsl == "True" || EnableSsl == "TRUE")
            {
                ssl = true;
            }
            else
            {
                ssl = false;
            }

            var message = new System.Net.Mail.MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(from);
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            message.Body = body;

            message.ReplyToList.Add(from);
            if (cc.Length > 0)
            {
                message.CC.Add(cc);
            }
            if (bcc.Length > 0)
            {
                message.Bcc.Add(bcc);
            }
            if (attachments.Length > 0)
            {
                String[] fileNames = attachments.Split(';', ',');
                foreach (var fileName in fileNames)
                {
                    message.Attachments.Add(new Attachment(fileName));
                }
            }

            // Kết nối GMail
            var client = new SmtpClient(SMTPServer, Port)
            {
                Credentials = new NetworkCredential(CredentialUserName, CredentialPassword),
                EnableSsl = ssl
            };
            // Gởi mail
            client.Send(message);
        }

        public static void Send(String from, String to, String cc, String bcc, String subject, String body, String attachments)
        {

            Thread email = new Thread(delegate ()
            {
                SendAsyncEmail(from, to, cc, bcc, subject, body, attachments);
            });
            email.IsBackground = true;
            email.Start();
        }


        private static void SendAsyncEmail(String from, String to, String CC, String BCC, String subject, String body, String attachments)
        {
            try
            {
                if (true)
                {
                    if (EnableSsl == "0" || EnableSsl == "true" || EnableSsl == "True" || EnableSsl == "TRUE")
                    {
                        ssl = true;
                    }
                    else
                    {
                        ssl = false;
                    }

                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(from);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;
                    message.ReplyToList.Add(from);
                    if (to != null)
                    {
                        String[] toes = to.Split(';', ',', ' ');
                        foreach (var t in toes)
                        {
                            message.To.Add(new MailAddress(t));
                        }


                    }

                    if (CC.Length > 0)
                    {
                        String[] CCs = CC.Split(';', ',', ' ');
                        foreach (string c in CCs)
                        {
                            message.CC.Add(new MailAddress(c));
                        }
                    }

                    if (BCC.Length > 0)
                    {
                        String[] BCCs = BCC.Split(';', ',', ' ');
                        foreach (string b in BCCs)
                        {
                            message.Bcc.Add(new MailAddress(b));
                        }
                    }

                    if (attachments.Length > 0)
                    {
                        String[] fileNames = attachments.Split(';', ',');
                        foreach (var fileName in fileNames)
                        {
                            message.Attachments.Add(new Attachment(fileName));
                        }
                    }

                    var client = new SmtpClient(SMTPServer, Port)
                    {
                        Credentials = new NetworkCredential(CredentialUserName, CredentialPassword),
                        EnableSsl = ssl
                    };

                    client.Send(message);
                }
             
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


    }
    public class vnMail
    {
        public static int SendGmail(string fromGmail, string fromName, string fromPassword, string toEmail, string toName, string subject, string body, string cc, string bcc)
        {
            var fromAddress = new MailAddress(fromGmail, fromName);
            var toAddress = new MailAddress(toEmail, toName);
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            var message = new System.Net.Mail.MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            if (!string.IsNullOrEmpty(cc.Trim()))
            {
                foreach (string ccm in cc.Split(",;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!string.IsNullOrEmpty(ccm.Trim()))
                    {
                        message.CC.Add(new MailAddress(ccm.Trim()));
                    }
                }

            }

            if (!string.IsNullOrEmpty(bcc.Trim()))
            {
                foreach (string bccm in bcc.Split(",;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!string.IsNullOrEmpty(bccm.Trim()))
                    {
                        message.Bcc.Add(new MailAddress(bccm.Trim()));
                    }
                }

            }
            try
            {
                smtp.Send(message);
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static int GuiMail(string title, string content, string toEmail, string ccMail, string bccMail)
        {
            
            PasswordObject p = new PasswordObject();
           // Console.WriteLine("DeCode:" + p.Decode(pass.PassWord));
            string fromEmail = WebConfigurationManager.AppSettings["CredentialUserName"].ToString();
            string fromName = "Hệ thống bán hàng trực tuyến ";
            string fromPassword =  p.Decode(WebConfigurationManager.AppSettings["CredentialPassword"].ToString());

            return vnMail.SendGmail(fromEmail, fromName, fromPassword, toEmail, toEmail.Split("@".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0], title, content, ccMail, bccMail);
        }
    }
}