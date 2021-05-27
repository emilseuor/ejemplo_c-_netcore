using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Helpers;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace WebApi.Services
{
    public interface IMailService
    {
        public Task<ResponseModel> SendMail(MailModel mailobj);
    }
    public class MailService: IMailService{

        private IConfiguration _config;

        public MailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<ResponseModel> SendMail(MailModel mailobj) {
            try { 
                var client = new SmtpClient(_config["MAIL_SMTP"], int.Parse(_config["MAIL:PORT"]))
                {
                    Credentials = new NetworkCredential(_config["MAIL:USERNAME"], _config["MAIL:PASSWORD"]),
                    EnableSsl = true
                };

                client.Send(mailobj.fromemail, mailobj.toemail, mailobj.subject, mailobj.content);

                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "sent"
                };
            }
            catch(Exception e) {
                
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }

        }
    }
}
