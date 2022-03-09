using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Models.Database;
using MyFace.Repositories;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;

namespace MyFace.Helpers
{
    public static class PasswordHelper
    {
        public static byte[] CreateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            return salt;
        }
        public static string CreateHashValue(string password, byte[] salt)
        {
            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

            return hashed;
        }

        public static string GetPasswordFromHeader(string AuthHeader)
        {
            var authHeaderSplit = AuthHeader.Split(' ');
            var authType = authHeaderSplit[0];
            var encodedUsernamePassword = authHeaderSplit[1];

            var usernamePassword = System.Text.Encoding.UTF8.GetString(
                Convert.FromBase64String(encodedUsernamePassword)
            );

            var usernamePasswordArray = usernamePassword.Split(':');

            var password = usernamePasswordArray[1];

            return password;

        }

        public static string GetUsernameFromHeader(string authHeader)
        {
            var authHeaderSplit = authHeader.Split(' ');
            var authType = authHeaderSplit[0];
            var encodedUsernamePassword = authHeaderSplit[1];

            var usernamePassword = System.Text.Encoding.UTF8.GetString(
                Convert.FromBase64String(encodedUsernamePassword)
            );

            var usernamePasswordArray = usernamePassword.Split(':');

            var username = usernamePasswordArray[0];

            return username;

        }
    }
}