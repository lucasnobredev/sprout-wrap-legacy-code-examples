using System;
using System.Collections.Generic;
using System.Text;

namespace SproutWrapClassAndMethodsExamples.WrapMethod
{
    public interface INotification
    {
        void Send(User user);
        void SendEmailAndSms(User user);
    }
}
