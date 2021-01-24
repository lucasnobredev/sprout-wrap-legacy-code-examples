using System;
using System.Collections.Generic;
using System.Text;

namespace SproutWrapClassAndMethodsExamples.WrapClass
{
    public class EmailAndSmsNotificationStrategy : INotification
    {
        private readonly INotification _emailNotificationDecorator;
        public EmailAndSmsNotificationStrategy(INotification emailNotificationDecorator)
        {
            _emailNotificationDecorator = emailNotificationDecorator;
        }

        public void Send(User user)
        {
            _emailNotificationDecorator.Send(user);

            // ... code to send sms to User
        }
    }
}
