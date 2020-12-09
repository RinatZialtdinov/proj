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
        public static void SendMail(string bodyMessage)
        {
            //// отправитель - устанавливаем адрес и отображаемое в письме имя
            //MailAddress from = new MailAddress("thebestteampi@gmail.com", "Tom");
            //// кому отправляем
            //MailAddress to = new MailAddress("elvir69@mail.ru");
            //// создаем объект сообщения
            //MailMessage m = new MailMessage(from, to);
            //// тема письма
            //m.Subject = "Тест";
            //// текст письма
            //m.Body = $"<h2>Письмо-тест работы smtp-клиента {message}</h2>";
            //// письмо представляет код html
            //m.IsBodyHtml = true;
            //// адрес smtp-сервера и порт, с которого будем отправлять письмо
            //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            //// логин и пароль
            //smtp.UseDefaultCredentials = false;
            //smtp.Credentials = new NetworkCredential("thebestteampi@gmail.com", "BestTeam");
            //smtp.EnableSsl = true;
            //smtp.UseDefaultCredentials = false;
            //smtp.Send(m);
            var fromAddress = new MailAddress("elvir69@mail.ru");
            var fromPassword = "password";
            var toAddress = new MailAddress("zialtdinov.rinat@bk.ru");

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
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })

                smtp.Send(message);
        }
    }
}
