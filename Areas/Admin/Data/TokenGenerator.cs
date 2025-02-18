using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
namespace QLDaoTao.Areas.Admin.Data
{
    public class TokenGenerator
    {
        public static string GenerateToken(int length = 32)
        {
            // Khởi tạo salt ngẫu nhiên
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Tạo token từ salt và độ dài mong muốn
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: Guid.NewGuid().ToString(), // Sử dụng một giá trị ngẫu nhiên làm password
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: length));

            return hashed;
        }
    }
}
