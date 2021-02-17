using System;
using System.Collections.Generic;
using System.Text;

namespace SproutWrapClassAndMethodsExamples.WrapClass
{
    public class UserHelperCommonStrategy : IUserHelper
    {
        public void HandleUserCreated(User user)
        {
            //... code to send email to User
        }
    }
}
