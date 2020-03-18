using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace MyFace.Services
{
    public interface IHashService
    {
        byte[] GenerateSalt();
        string HashPassword(byte[] salt, string password);
    }
    
    public class HashService : IHashService
    {
        public byte[] GenerateSalt()
        {
            var salt = new byte[16];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(salt);
            return salt;
        }

        public string HashPassword(byte[] salt, string password)
        {
            var hashedPassword = KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 32
                );
            return Convert.ToBase64String(hashedPassword);
        }
    }
}