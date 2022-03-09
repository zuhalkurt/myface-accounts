using MyFace.Repositories;
using MyFace.Models.Database;
using System;
using MyFace.Helpers;
using MyFace.Models.Response;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;



namespace MyFace.Services
{
    public interface IAuthService
    {
        public bool IsValidUsernameAndPassword(string username, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly IUsersRepo _users;

        public AuthService(IUsersRepo users)
        {
            _users = users;
        }

        public bool IsValidUsernameAndPassword(string username, string password)
        {
            User user;
            try
            {
                user = _users.GetByUsername(username);
            }
            catch (InvalidOperationException)
            {

                return false;

            }

            string hashed = PasswordHelper.CreateHashValue(
                password,
                Convert.FromBase64String(user.Salt)
            );

            if (hashed != user.HashedPassword)
            {
                return false;
            }

            return true;
        }
    }
}