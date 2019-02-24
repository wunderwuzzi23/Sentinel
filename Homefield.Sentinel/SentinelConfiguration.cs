using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Configuration;
using System.Security.Cryptography;

namespace Homefield.Sentinel
{
    class SentinelConfiguration
    {
        
        public static string Logfile = System.IO.Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location, "..\\sentinel.log");
        public static string SmtpServer = string.Empty;
        public static int SmtpServerPort = 587;
        public static NetworkCredential EmailCredentials;

        public static void Load()
        {
            /// read configuration settings 
            SmtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            SmtpServerPort = int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]);

            string email = ConfigurationManager.AppSettings["EmailAccount"];
            byte[] entBytes = System.Text.UTF8Encoding.Unicode.GetBytes(ConfigurationManager.AppSettings["EmailAccountEntropy"]);

            /// the email password is encrypted in the settings file
            byte[] pwdBytes = Convert.FromBase64String(ConfigurationManager.AppSettings["EmailAccountPassword"]);
            byte[] decryptedContent = ProtectedData.Unprotect(pwdBytes, entBytes, DataProtectionScope.LocalMachine);
            string pwd = System.Text.UTF8Encoding.Unicode.GetString(decryptedContent);

            EmailCredentials = new NetworkCredential(email, pwd);
        }
    }
}
