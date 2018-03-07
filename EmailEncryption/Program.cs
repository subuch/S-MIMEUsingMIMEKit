using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Cryptography;
using System;
using System.Net;


namespace EmailEncryption
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Started S/MIME Encryption Process...");          
            Console.WriteLine("Started drating Email...");
            var mimeMessage =MailMessage.SimpleMessage();
            Console.WriteLine("Completed drating Email...");
            /*****************************Encryption******************************/
            Console.WriteLine("Started sending encrypted email ...");
            var encryptedmsg = SecureMimeContext.SendEncryptMessage(mimeMessage);
            Console.WriteLine("Completed sending encrypted email ...");
            Console.WriteLine("Completed S/MIME Encryption Process...");
            /*****************************Sign******************************/
            //Console.WriteLine("Started mail Signing ...");
            //var signmail = SecureMimeContext.SendSignedMessage(mimeMessage);
            //Console.WriteLine("Completed mail Signing ...");
            Console.ReadLine();
        }
    }
}
