using System.Security.Cryptography;
using System.Text;

public class JwtSecretKeyGenerator
{
    public static string GenerateSecretKey(int size = 64)
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] secretKey = new byte[size];
            rng.GetBytes(secretKey);
            return Convert.ToBase64String(secretKey);
        }
    }
}
