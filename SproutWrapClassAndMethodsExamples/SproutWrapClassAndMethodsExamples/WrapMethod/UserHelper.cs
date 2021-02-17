using System;
using System.Collections.Generic;
using System.Text;

namespace SproutWrapClassAndMethodsExamples.WrapMethod
{
    public class UserHelper : WrapMethod.IUserHelper
    {
        public void HandleUserCreated(User user)
        {
            //... code to send email to User
        }

        public void HandleUserCreatedSendingSms(User user)
        {
            HandleUserCreated(user);

            //... code to send sms to User
        }
    }
}
