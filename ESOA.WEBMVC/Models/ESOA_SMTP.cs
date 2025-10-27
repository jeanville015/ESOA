using ESOA.Model;
using ESOA.Model.Constant;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ESOA.Model.Constants;

namespace ESOA.Model
{
    /// <summary>
    ///  
    /// </summary>
    public static class ESOA_SMTP
    {

        /// <summary>
        ///  
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<SmtpClient> SetSmtpClient()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
            SmtpClient client = new SmtpClient("mailx.lbcmail.com", 587);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Timeout = 100000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //string senderEmail = "noreply-appalert@lbcexpress.com";
            //string senderPassword = "Qr!3mHL=7bb";
            client.Credentials = new NetworkCredential(DefaultValues.smtpClientSenderEmail, DefaultValues.smtpClientSenderPassword);

            return client;
        }

        private static bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            //Console.WriteLine(certificate);
            return true;
        }
    }
}
