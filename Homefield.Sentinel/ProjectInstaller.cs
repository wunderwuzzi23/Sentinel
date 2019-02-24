using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Security;
using System.Configuration;

namespace Homefield.Sentinel
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        static string SentinelExeLocation = null;

        public ProjectInstaller()
        { 
            InitializeComponent();

            // Customization of Installation Experience
            SentinelExeLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Console.WriteLine("*** Path: " + SentinelExeLocation);

            if (!File.Exists(SentinelExeLocation + ".config"))
            {
                throw new InstallException("Sentinel Configuration Homefield.Sentinel.exe.config not found.");
            }

            this.Committed += ProjectInstaller_Committed;
        }

        // Customization of Installation Experience
        // Asking user for info and updating the configuration file
        private void ProjectInstaller_Committed(object sender, InstallEventArgs e)
        {

            Console.WriteLine("\n\n");
            Console.WriteLine("**********************************************");
            Console.WriteLine("*** Homefield Sentinel Setup Configuration ***");
            Console.WriteLine("**********************************************");

            Console.Write("Smtp Server (Default smtp-mail.outlook.com): ");
            string smtpServer = Console.ReadLine();
            if (smtpServer == string.Empty) smtpServer = "smtp-mail.outlook.com";

            Console.Write("Smtp Server Port (Default 587): ");
            string smtpServerPort = Console.ReadLine();
            if (smtpServerPort == string.Empty) smtpServerPort = "587";

            Console.Write("Email Account for Notifications: ");
            string email = Console.ReadLine();

            Console.Write("Password: ");
            string entropy = System.Guid.NewGuid().ToString() + System.Guid.NewGuid().ToString();
          
            byte[] entropyBytes = System.Text.UTF8Encoding.Unicode.GetBytes(entropy);
            byte[] pwdBytes = readPasswordShortLivedProcess();
            byte[] content = ProtectedData.Protect(pwdBytes, entropyBytes, DataProtectionScope.LocalMachine);


            UpdateConfigKey("SmtpServer", smtpServer);
            UpdateConfigKey("SmtpServerPort", smtpServerPort);
            UpdateConfigKey("EmailAccount", email);
            UpdateConfigKey("EmailAccountEntropy", entropy);
            UpdateConfigKey("EmailAccountPassword", Convert.ToBase64String(content));
                             
            Console.WriteLine("\n\n");
            Console.WriteLine("**********************************************");
            Console.WriteLine("*** Install and Configuration Complete     ***");
            Console.WriteLine("*** Run: net start \"Homefield Sentinel\"    ***");
            Console.WriteLine("**********************************************");
            Console.WriteLine("");
        }


        /// <summary>
        /// Overwrite the configuration settings with the new value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void UpdateConfigKey(string key, string value)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(SentinelExeLocation);

            var settings = config.AppSettings.Settings;

            //If the key doesn't exist, then add it - otherwise update the existing key
            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);      
        }

        /// <summary>
        /// Read password in and hide input
        /// </summary>
        /// <returns></returns>
        private byte[] readPasswordShortLivedProcess()
        {
            string input = string.Empty;
            var key = Console.ReadKey(true);
            while (key.Key != ConsoleKey.Enter)
            {
                if (!char.IsControl(key.KeyChar))
                {
                    input += key.KeyChar;
                }

                key = Console.ReadKey(true);
            }

            return System.Text.UTF8Encoding.Unicode.GetBytes(input);
        }
    }
}
