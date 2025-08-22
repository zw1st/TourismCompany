using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.Infrastructure;

public static class PasswordHelper
{
    public static string GenerateSalt()
    {
        var buffer = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(buffer);
        }
        return Convert.ToBase64String(buffer);
    }

    public static string HashPassword(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha256.ComputeHash(saltedPassword);
            return Convert.ToBase64String(hash);
        }
    }

    public static void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 7)
            throw new ArgumentException("Пароль должен содержать минимум 7 символов");
    }
}