# poc-sending-smime-email

This project helps  to send S/MIME mail using Google Mail Provider or 13 CABS mail provider

Config Setup:

Under SecureMimeContext.cs:
		1. Change the SQL Lite to a specified path 
		2. For importing your own p12 certificate, change the path P12Stream function
		3. To add the public certificate of recipients, change the path in getAllCerts function
		4. Ensure <PWD> as been replaced with the correct password
Under MailMessage.cs

		1. Draft an email and specify the From and TO recipient
		2. Ensure the TO recipient Public key is available with you. If not, export it and save in the folder

