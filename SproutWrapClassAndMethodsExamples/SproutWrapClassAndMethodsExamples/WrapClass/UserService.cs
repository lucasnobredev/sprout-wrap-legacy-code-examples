using System;
using System.Collections.Generic;
using System.Text;

namespace SproutWrapClassAndMethodsExamples.WrapClass
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

        public void CreateUser(UserRequest request)
        {
            var user = new User(request);
            if (user.IsValid() == false)
                return;

            _userRepository.Insert(user);

            _notification.Send(user);
        }

        public void UpdateUser(UserRequest request)
        {
            var user = _userRepository.GetById(request.Id);

            _userRepository.Update(user);

            _notification.Send(user);
        }
    }
}
