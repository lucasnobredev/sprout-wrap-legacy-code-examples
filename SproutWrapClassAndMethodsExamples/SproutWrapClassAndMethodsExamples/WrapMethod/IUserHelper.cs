using System;
using System.Collections.Generic;
using System.Text;

namespace SproutWrapClassAndMethodsExamples.WrapMethod
{
    public interface IUserHelper
    {
        void HandleUserCreated(User user);
        void HandleUserCreatedSendingSms(User user);
    }
}
