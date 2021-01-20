using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WebMaterial.BLL
{
    public class MailService
    {
        private readonly IRepository _repository;
        private readonly IConfiguration _configuration;
        public MailService(IRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }
        public void SendMail(string bodyMessage)
        {
            var users = _repository.GetAllUsers();
            //var fromAddress = new MailAddress("elvir69@mail.ru");
            //var fromPassword = "password";
            var fromAddress = new MailAddress(_configuration["Secret:Email"]);
            var fromPassword = _configuration["Secret:Password"];

            string subject = "Рассылка";
            string body = bodyMessage;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.mail.ru",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            foreach (var user in users)
            {
                var toAddress = new MailAddress(user.Email);
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                smtp.Send(message);
            }
        }
    }
}
