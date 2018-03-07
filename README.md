# S/MIME Using MIMEKit& SQL Lite DB

This project helps  to send S/MIME encrypted email using Google Mail Provider or custom mail provider by storing the certificate in the SQLLite Database. If you are targetting any of the Xamarin platforms (or Linux), you won't need to do anything (although you certainly can if you want to) because, by default, MimeKit will automatically use the Mono.Data.Sqlite binding to SQLite

Why S/MIME:

S/MIME, or Secure/Multipurpose Internet Mail Extensions, is a technology that allows you to encrypt your emails. S/MIME is based on asymmetric cryptography to protect your emails from unwanted access. It also allows you to digitally sign your emails to verify you as the legitimate sender of the message, making it an effective weapon against many phishing attacks out there

Reference: https://www.globalsign.com/en/blog/what-is-s-mime/

Config Setup:

Under SecureMimeContext.cs:
1. Change the SQL Lite to a specified path 
2. For importing your own p12 certificate, change the path P12Stream function
3. To add the public certificate of recipients, change the path in getAllCerts function
4. Ensure <PWD> as been replaced with the correct password
	
Under MailMessage.cs

1. Draft an email and specify the From and TO recipient
2. Ensure the TO recipient Public key is available with you. If not, export it and save in the folder

