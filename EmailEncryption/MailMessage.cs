using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailEncryption
{
    public class MailMessage
    {

        public static MimeMessage SimpleMessage()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("xxx", "xxx@xx.com"));
            message.To.Add(new MailboxAddress("yyy", "yyy@yyy.com"));
            message.Subject = "Encryption Test!!!";

            message.Body = new TextPart("plain")
            {
                Text = @"Hey,

                    Mail with Encryption ...!!!

                    -- Subbu
"
            };

            return message;
        }
    }
}
