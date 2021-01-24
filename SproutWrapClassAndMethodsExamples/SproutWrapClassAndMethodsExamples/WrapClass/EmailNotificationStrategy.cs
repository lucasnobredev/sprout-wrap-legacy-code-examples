using System;
using System.Collections.Generic;
using System.Text;

namespace SproutWrapClassAndMethodsExamples.WrapClass
{
    public class EmailNotificationStrategy : INotification
    {
        public void Send(User user)
        {
            //... code to send email to User
        }
    }
}
