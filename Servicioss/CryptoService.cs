using System;
using System.Security.Cryptography;
using System.Text;

namespace Servicioss
{
    public class CryptoService
    {
        private const int SaltSize = 32;
        private const int HashSize = 32;
        private const int Iterations = 100_000;

        public byte[] GenerarSalt()
        {
            byte[] salt = new byte[SaltSize];

            using (RandomNumberGenerator randomGenerator = RandomNumberGenerator.Create())
            {
                randomGenerator.GetBytes(salt);
            }

            return salt;
        }

        public byte[] CalcularHash(string password, byte[] salt)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (salt == null)
            {
                throw new ArgumentNullException(nameof(salt));
            }

            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(HashSize);
            }
        }

        public bool VerificarPassword(string password, byte[] salt, byte[] hash)
        {
            byte[] hashCalculado = CalcularHash(password, salt);
            return AreEqual(hashCalculado, hash);
        }

        public long CalcularDVH(string cadena)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(cadena ?? string.Empty));
                long dvh = BitConverter.ToInt64(hash, 0);

                if (dvh < 0)
                {
                    return -dvh;
                }
                else
                {
                    return dvh;
                }
            }
        }

        public long CalcularDVV(string[] dvhStringsOrdenados)
        {
            string cadena = string.Join("|", dvhStringsOrdenados ?? Array.Empty<string>());

            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(cadena));
                long dvv = BitConverter.ToInt64(hash, 0);

                if (dvv < 0)
                {
                    return -dvv;
                }
                else
                {
                    return dvv;
                }
            }
        }

        private bool AreEqual(byte[] firstBytes, byte[] secondBytes)
        {
            if (firstBytes == null || secondBytes == null || firstBytes.Length != secondBytes.Length)
            {
                return false;
            }

            int difference = 0;

            for (int index = 0; index < firstBytes.Length; index++)
            {
                difference |= firstBytes[index] ^ secondBytes[index];
            }

            return difference == 0;
        }
    }
}
