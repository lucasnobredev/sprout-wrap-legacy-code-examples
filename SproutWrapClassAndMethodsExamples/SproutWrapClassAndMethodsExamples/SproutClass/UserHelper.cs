using System;
using System.Collections.Generic;
using System.Text;

namespace SproutWrapClassAndMethodsExamples.SproutClass
{
    public class UserHelper : IUserHelper
    {
        public void HandleUserCreated(User user)
        {
            //... code to send notification to User
        }
    }
}
