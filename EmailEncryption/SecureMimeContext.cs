using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using MimeKit.Cryptography;
using System.IO;
using MimeKit;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using MailKit.Net.Smtp;

/*****************************************************
1. Change the SQL Lite to a specified path
2. For importing your own p12 certificate, change the path P12Stream function
3. To add the public certificate of recipients, change the path in getAllCerts function
/*****************************************************/
namespace EmailEncryption
{
    public class SecureMimeContext:DefaultSecureMimeContext
    {
       static  IX509CertificateDatabase _database;
        public SecureMimeContext()
        : base(_database=OpenDatabase("C:\\CodeBase\\EmailEncryption\\certdb.sqlite"))
        {
            CryptographyContext.Register(typeof(SecureMimeContext));
            Import(P12Stream(), "<PWD>"); //Import your own Certificate;     
            AddCertificate();
        }

     

        
        public static  bool SendEncryptMessage(MimeMessage message)
        {
            bool isSuccess = false;
            try
            {
                using (var ctx = new SecureMimeContext())
                {
                    message.Body = ApplicationPkcs7Mime.Encrypt(ctx, message.To.Mailboxes, message.Body);
                    SendEmailByMailServer(message);
                    isSuccess=true;
                }
                return isSuccess;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.InnerException.ToString());
                return isSuccess;
            }
        }

        public  MimeEntity Decrypt(MimeMessage message)
        {
            var pkcs7 = message.Body as ApplicationPkcs7Mime;

            if (pkcs7 != null && pkcs7.SecureMimeType == SecureMimeType.EnvelopedData)
            {               
              
                return pkcs7.Decrypt();
            }
            else
            {
               
                return message.Body;
            }
        }


      

        public void AddCertificate()
        {

            FileInfo[] files= this.getAllCerts();

            foreach (var file in files)
            {
                System.Security.Cryptography.X509Certificates.X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(file.FullName);

                byte[] hashBytes = cert.GetRawCertData();
                X509CertificateStore certstore = new X509CertificateStore();
                certstore.Import(hashBytes);

                IEnumerable<X509Certificate> ix509Cert = certstore.Certificates;
                foreach (var item in ix509Cert)
                {
                    X509Certificate cert2 = item;

                    X509CertificateRecord certrecord = _database.Find(cert2, X509CertificateRecordFields.Certificate);
                    if (certrecord == null)
                    {
                        certrecord = new X509CertificateRecord(cert2);
                        _database.Add(certrecord);
                    }
                }
            }
         }
              

        public static bool SendSignedMessage(MimeMessage message)
        {
            bool IsMailSigned = false;
            // digitally sign our message body using our custom S/MIME cryptography context
            try
            {
                using (var ctx = new SecureMimeContext())
                {
                    var signer = new CmsSigner(P12Stream(), "<PWD>")
                    {
                        DigestAlgorithm = DigestAlgorithm.Sha1
                    };

                    message.Body = MultipartSigned.Create(ctx, signer, message.Body);
                }
                SendEmailByMailServer(message);
                IsMailSigned = true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.InnerException.ToString());
                IsMailSigned = false;
            }
      
            return IsMailSigned;
        }

        public void VerifyMultipartSigned(MimeMessage message)
        {
            if (message.Body is MultipartSigned)
            {
                var signed = (MultipartSigned)message.Body;
                
                foreach (var signature in signed.Verify())
                {
                    try
                    {
                        PublicKeyAlgorithm pubAlg = signature.PublicKeyAlgorithm;

                        bool valid = signature.Verify();
                    }
                    catch (DigitalSignatureVerifyException e)
                    {
                        Console.WriteLine(e.InnerException.ToString());
                    }
                }
            }
        }

        public  void SendEmailByGoogleMail(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate("<gmailID>", "<gmailPwd>");
                client.Send(message);
                client.Disconnect(true);
            }
        }

        public static void SendEmailByMailServer(MimeMessage message)
        {
            using (var emailClient = new SmtpClient())
            {              
                emailClient.Connect("xxxxxxx",25);              
                emailClient.Send(message);
                emailClient.Disconnect(true);
            }

        }


        private static IX509CertificateDatabase OpenDatabase(string fileName)
        {
            var builder = new SQLiteConnectionStringBuilder();
            builder.DateTimeFormat = SQLiteDateFormats.Ticks;
            builder.DataSource = fileName;

            if (!File.Exists(fileName))
                SQLiteConnection.CreateFile(fileName);

            var sqlite = new SQLiteConnection(builder.ConnectionString);
            sqlite.Open();

            return new SqliteCertificateDatabase(sqlite, "<pwd>");
        }

       

        private static Stream P12Stream()
        {
            Stream fs = File.OpenRead(@"C:\CodeBase\EmailEncryption\rrrrr.p12");
            return fs;
        }

        private FileInfo[] getAllCerts()
        {
            FileInfo[] files = null;
            DirectoryInfo folder = new DirectoryInfo(@"C:\CodeBase\EmailEncryption\certs\");
            if (folder.Exists)
            {
                files = folder.GetFiles("*.cer");
            }
            return files;
        }

        //private static Stream CertStream(string filename)
        //{
        //    Stream fs = File.OpenRead(filename);
        //    return fs;
        //}

    }
}
