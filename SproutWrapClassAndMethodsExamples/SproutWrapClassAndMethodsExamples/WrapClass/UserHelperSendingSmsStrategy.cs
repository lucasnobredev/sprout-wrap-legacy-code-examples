using System;
using System.Collections.Generic;
using System.Text;

namespace SproutWrapClassAndMethodsExamples.WrapClass
{
    public class UserHelperSendingSmsStrategy : IUserHelper
    {
        private readonly IUserHelper _emailNotificationDecorator;
        public UserHelperSendingSmsStrategy(IUserHelper emailNotificationDecorator)
        {
            _emailNotificationDecorator = emailNotificationDecorator;
        }

        public void HandleUserCreated(User user)
        {
            _emailNotificationDecorator.HandleUserCreated(user);

            // ... code to send sms to User
        }
    }
}
