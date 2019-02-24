using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Eventing.Reader;
using System.Net.Mail;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Net;
using System.Configuration;


namespace Homefield.Sentinel
{
   class TheSentinel
    { 
        SmtpClient      smtpClient;
        EventLogWatcher logWatcher;
        Logger          log;

        //event log query to retrieve event id 4656 (Audit ACLs)
        EventLogQuery   logQuery = new EventLogQuery("Security", PathType.LogName, "*[System/EventID=4656]");
        
        public void StartWatching()
        { 
            try
            {
                SentinelConfiguration.Load();
                log = new Logger(SentinelConfiguration.Logfile);
                log.WriteLine("Starting...");

                this.smtpClient = new SmtpClient(SentinelConfiguration.SmtpServer, SentinelConfiguration.SmtpServerPort);
                this.logWatcher = new EventLogWatcher(logQuery);
                this.logWatcher.EventRecordWritten += this.logWatcher_EventRecordWritten;
                this.logWatcher.Enabled = true;

                this.smtpClient.EnableSsl = true;
                this.smtpClient.Credentials = SentinelConfiguration.EmailCredentials;
                log.WriteLine("Started.");
            }
            catch (Exception e)
            {
                log.WriteLine("Unexpected Error during startup: " + e.ToString());
            }
        }


        /// <summary>
        /// Event Handler for the watcher
        /// Double check Event ID and see if the access is related
        /// to the passwords.txt file we have setup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logWatcher_EventRecordWritten(object sender, EventRecordWrittenEventArgs e)
        {
            if (e.EventRecord.Id == 4656)
            {
                //Check if the audit Evvent is for a file we setup (avoiding false positves)
                if (e.EventRecord.FormatDescription().Contains("passwords.txt"))
                {
                    try
                    { 
                        lock(SentinelConfiguration.Logfile)
                        {
                            //Write to logfile
                            log.WriteLine("Honeypot file accesssed");
                            log.WriteLine(e.EventRecord.FormatDescription());
                            log.WriteLine("*******************************************");
                        }

                        //Send Mail
                        string email = ((NetworkCredential)this.smtpClient.Credentials).UserName;
                        MailMessage mail = new MailMessage(email, email);
                        mail.Subject = "[Sentinel Notification] Honeypot file accessed.";
                        mail.Body = e.EventRecord.FormatDescription();
                        mail.Priority = MailPriority.High;
                        mail.IsBodyHtml = false;

                        smtpClient.Send(mail);
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("Unexpected Error during OnEventWritten: " + ex.ToString());
                    }
                }
            }

        }

        public void StopWatching()
        {
            this.logWatcher.Enabled = false;
            log.WriteLine("Stopped.");

        }
    }
}
