using Application.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Security
{
    public class Sha256Hasher : IHasher
    {
        public string Hash(string str)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(str);
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hashedBytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }
    }
}
