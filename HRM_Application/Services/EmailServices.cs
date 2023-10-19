using HRM_Application.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Application.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _senderEmail;

        public EmailServices(IConfiguration configuration)
        {
            var smtpConfig = configuration.GetSection("SmtpConfig");
            _smtpServer = smtpConfig["SmtpServer"];
            _smtpPort = Convert.ToInt32(smtpConfig["SmtpPort"].ToString());
            _smtpUsername = smtpConfig["SmtpUsername"];
            _smtpPassword = smtpConfig["SmtpPassword"];
            _senderEmail = smtpConfig["SmtpSenderEmail"];
        }

        public void SendExceptionEmail(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("qasimkhan", _senderEmail));
            message.To.Add(new MailboxAddress("athariqbal", to));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            client.Connect(_smtpServer, _smtpPort, useSsl: false);
            client.Authenticate(_smtpUsername, _smtpPassword);
            client.Send(message);
            client.Disconnect(true);
        }
    }
}
