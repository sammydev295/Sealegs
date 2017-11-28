using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;

namespace Sealegs.Backend.Identity
{
    public class SimpleHash : ISimpleHash
    {
        private int m_SaltSize = 16;
        private int m_HashSize = 32;
        private int m_Iterations = 50000;

        public string Compute(string password, out string salt)
        {
            salt = string.Empty;
            return Compute(password, m_Iterations, out salt);
        }

        public string Compute(string password, int iterations, out string salt)
        {
            salt = string.Empty;
            using (var bytes = new Rfc2898DeriveBytes(password, m_SaltSize, iterations))
            {
                byte[] hash = bytes.GetBytes(m_HashSize);
                return CreateHashString(hash, bytes.Salt, iterations, out salt);
            }
        }

        private string ComputeHash(string password, string salt, int iterations, int hashSize)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            using (var bytes = new Rfc2898DeriveBytes(password, saltBytes, iterations))
            {
                byte[] hash = bytes.GetBytes(hashSize);

                return Convert.ToBase64String(hash);
            }
        }

        public bool Verify(string password, string passwordHashString)
        {
            var parameters = new SimpleHashParameters(passwordHashString);

            int hashSize = Convert.FromBase64String(parameters.PasswordHash).Length;

            string newPasswordHash = ComputeHash(password, parameters.Salt, parameters.Iterations, hashSize);

            return parameters.PasswordHash == newPasswordHash;
        }

        private string CreateHashString(byte[] hash, byte[] saltBytes, int iterations, out string salt)
        {
            salt = Convert.ToBase64String(saltBytes);
            string saltString = Convert.ToBase64String(saltBytes);
            string hashStringPart = Convert.ToBase64String(hash);

            return string.Join
                (
                    Constants.Splitter.ToString(),
                    Constants.Algorithm,
                    iterations.ToString(CultureInfo.InvariantCulture),
                    saltString,
                    hashStringPart
                );
        }

        public TimeSpan Estimate(string password, int iterations)
        {
            string salt = string.Empty;
            var watch = new Stopwatch();
            watch.Start();
            Compute(password, iterations, out salt);
            watch.Stop();
            return watch.Elapsed;
        }
    }
}