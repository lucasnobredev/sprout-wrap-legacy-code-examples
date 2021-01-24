using System;
using System.Collections.Generic;
using System.Text;

namespace SproutWrapClassAndMethodsExamples.WrapMethod
{
    public class NotificationService : WrapMethod.INotification
    {
        public void Send(User user)
        {
            //... code to send email to User
        }

        public void SendEmailAndSms(User user)
        {
            Send(user);

            //... code to send sms to User
        }
    }
}
