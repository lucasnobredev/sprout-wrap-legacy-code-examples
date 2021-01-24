using System;
using System.Collections.Generic;
using System.Text;

namespace SproutWrapClassAndMethodsExamples.SproutClass
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotification _notification;

        public UserService(
            IUserRepository userRepository,
            INotification notification)
        {
            _userRepository = userRepository;
            _notification = notification;
        }

        public void CadastrarUsuario(UserRequest request)
        {
            var user = new User(request);
            if (user.IsValid() == false)
                return;

            _userRepository.Insert(user);

            _notification.Send(user);
        }
    }
}
