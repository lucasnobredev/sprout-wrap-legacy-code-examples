using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SproutWrapClassAndMethodsExamples.SproutMethod
{
    public class UserService
    {
        private readonly IConfiguration _configuration;
        public UserService(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void CreateUser(UserRequest request)
        {
            var user = new User();
            user.Name = request.Name;
            user.Email = request.Email;
            user.Password = request.Password;

            if (string.IsNullOrEmpty(user.Name) || user.Name.Length > 150)
                throw new Exception("Name invalid");

            if (string.IsNullOrEmpty(user.Email) || user.Email.Contains('@') == false || user.Email.Length > 100)
                throw new Exception("Email invalid");

            if (string.IsNullOrEmpty(user.Password) || user.Password.All(char.IsLetterOrDigit) == false || user.Email.Length > 40)
                throw new Exception("Password invalid");

            const string SQL = @"
            INSERT INTO User (Name, Email, Password)
            Values (@name, @email, @password)";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServer")))
            {
                var parameter = new DynamicParameters();
                parameter.Add("@name", user.Name, DbType.AnsiString);
                parameter.Add("@email", user.Email, DbType.AnsiString);
                parameter.Add("@password", user.Password, DbType.AnsiString);

                connection.Execute(SQL, parameter);
            }

            SendNotificationToUser(user);
        }

        private void SendNotificationToUser(User user)
        {
            //... code to send notification to User
        }
    }
}
