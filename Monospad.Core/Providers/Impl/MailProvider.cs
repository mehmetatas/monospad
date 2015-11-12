using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Monospad.Core.Providers.Impl
{
    public class MailProvider: IMailProvider
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
                _mailQueue.Enqueue(new Mail
                {
                    ToAddress = email,
                    Subject = "recover your password",
                    Body = @"hi,

we have just received a request telling that you want to recover your password and prepared a unique link for you immediately. 

you can simply click it or paste it into your browser: http://monospad.com/recover/{token}. 

if you do not use this link your password will remain the same. 

the link will be valid for next 3 days.

"
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

            Send(mail);
        }

        private void Send(Mail mail)
        {
            var msg = new MailMessage
            {
                Body = mail.Body,
                Subject = mail.Subject
            };
            msg.To.Add(mail.ToAddress);
        }

        class Mail
        {
            public string ToAddress { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
        }
    }
}
