using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Monospad.Core.Providers.Impl
{
    public class MailProvider : IMailProvider
    {
        private readonly Queue<Mail> _mailQueue;
        private volatile bool _running;

        public MailProvider()
        {
            _mailQueue = new Queue<Mail>();
        }

        public void SendPasswordRecoveryMail(string email, Guid token)
        {
            lock (this)
            {
                var link = $"http://monospad.com/recover/{token}";
                _mailQueue.Enqueue(new Mail
                {
                    ToAddress = email,
                    Subject = "recover your password",
                    Body = $@"<html><head></head>
<body style='font-family: ""Consolas"", ""Courier New""'>
hi,</p>

<p>we have just received a request telling that you want to recover your password and prepared a unique <a href=""{link}"" target=_blank>link</a> for you immediately.</p>

<p>you can simply click it or paste it into your browser: {link} </p>

<p>if you do not use this link your password will remain the same.</p>

<p>the link will be valid for next {Constants.PasswordRecoveryValidDays} days.</p>

<p>regards,<br>
monospad.com</p>
</body></html>"
                });

                EnsureRunning();
            }
        }

        private void EnsureRunning()
        {
            lock (this)
            {
                if (_running)
                {
                    return;
                }

                _running = true;
            }

            Task.Factory.StartNew(Run);
        }

        private void Run()
        {
            while (_running)
            {
                SendNext();
            }
        }

        private void SendNext()
        {
            Mail mail = null;

            lock (this)
            {
                if (_mailQueue.Count > 0)
                {
                    mail = _mailQueue.Dequeue();
                }
            }

            if (mail == null)
            {
                Thread.Sleep(1000);
                return;
            }

            try
            {
                Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void Send(Mail mail)
        {
            var msg = new MailMessage
            {
                Body = mail.Body,
                Subject = mail.Subject,
                IsBodyHtml = true
            };
            msg.To.Add(mail.ToAddress);

            using (var sc = new SmtpClient())
            {
                sc.Send(msg);
            }
        }

        class Mail
        {
            public string ToAddress { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
        }
    }
}
