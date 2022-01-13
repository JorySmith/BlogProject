using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.ViewModels
{
    public class MailSettings
    {
        // Configure/use a SMTP server, e.g. Google's Gmail
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }  // SMTP server
        public int Port { get; set; }
    }
}
