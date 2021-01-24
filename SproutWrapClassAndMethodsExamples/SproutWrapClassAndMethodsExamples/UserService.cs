using System;
using System.Collections.Generic;
using System.Text;

namespace SproutWrapClassAndMethodsExamples
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void CadastrarUsuario(UserRequest request)
        {
            var user = new User(request);
            if (user.IsValid() == false)
                return;

            _userRepository.Insert(user);
        }
    }
}
