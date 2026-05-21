using System.Security.Cryptography;
using System.Text;

namespace AkkuAppsAi.Helpers;

public static class CryptoHelper
{
    public static string HashEmail(string email)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(email.ToLower().Trim()));
        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }
}
