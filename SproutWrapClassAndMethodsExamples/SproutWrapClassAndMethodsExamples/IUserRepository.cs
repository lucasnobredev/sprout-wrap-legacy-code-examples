﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SproutWrapClassAndMethodsExamples
{
    public interface IUserRepository
    {
        void Insert(User user);
        void Update(User user);
        User GetById(int id);
    }
}
