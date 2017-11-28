using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

using Sealegs.DataObjects;
using Sealegs.Backend.Models;

namespace Sealegs.Backend.Identity
{
    public sealed class SealegsAuthClient : IDisposable
    {
        #region Fields

        SealegsContext db = new SealegsContext();

        #endregion

        #region CTOR 

        public SealegsAuthClient()
        {
        }

        #endregion 

        #region Dispose

        ~SealegsAuthClient()
        {
            db.Dispose();
            Dispose(false);
        }

        public void Dispose()
        {
            db.Dispose();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        #endregion 

        #region EncodePassword

        /// <summary>
        /// The default aspnet membership hashing is SHA1 but they also salt it and base64 it
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public string EncodePassword(string pass, string salt)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            byte[] src = Convert.FromBase64String(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }

        #endregion

        #region EncryptPassword

        // This is copied from the existing SQL providers and is provided only for back-compat.
        private static string EncryptPassword(string pass, int passwordFormat, string salt, HashAlgorithm algorithm)
        {
            if (passwordFormat == 0) // MembershipPasswordFormat.Clear
                return pass;

            byte[] bIn = Encoding.Unicode.GetBytes(pass);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bRet = null;

            if (passwordFormat == 1) // MembershipPasswordFormat.Hashed 
            {
                HashAlgorithm hm = algorithm;
                if (hm is KeyedHashAlgorithm)
                {
                    KeyedHashAlgorithm kha = (KeyedHashAlgorithm)hm;
                    if (kha.Key.Length == bSalt.Length)
                        kha.Key = bSalt;
                    else if (kha.Key.Length < bSalt.Length)
                    {
                        byte[] bKey = new byte[kha.Key.Length];
                        Buffer.BlockCopy(bSalt, 0, bKey, 0, bKey.Length);
                        kha.Key = bKey;
                    }
                    else
                    {
                        byte[] bKey = new byte[kha.Key.Length];
                        for (int iter = 0; iter < bKey.Length;)
                        {
                            int len = Math.Min(bSalt.Length, bKey.Length - iter);
                            Buffer.BlockCopy(bSalt, 0, bKey, iter, len);
                            iter += len;
                        }
                        kha.Key = bKey;
                    }
                    bRet = kha.ComputeHash(bIn);
                }
                else
                {
                    byte[] bAll = new byte[bSalt.Length + bIn.Length];
                    Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
                    Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
                    bRet = hm.ComputeHash(bAll);
                }
            }

            return Convert.ToBase64String(bRet);
        }

        #endregion

        #region GetAuthenticationToken

        public SealegsAuthResponse GetAuthenticationToken(SealegsCredentials credentials)
        {
            SealegsContext Context = new SealegsContext();
            // Validate user against database using email and password. 
            IList<SealegsUser> users = Context.SealegsUser.Include("Role").Where(u => u.Email.ToLower() == credentials.Email.ToLower()).ToList();
            SealegsUser user = (users.Count() > 0) ? users.First() : null;

            // Other possible algorithms - HashAlgorithm.Create("SHA256"), new HMACSHA1(), new HMACSHA256()
            string password = String.Empty; bool ret = false;
            if (user.PasswordFormat == 1)
                password = EncryptPassword(credentials.Password, user.PasswordFormat, user.PasswordSalt, HashAlgorithm.Create("SHA1"));
            else
            {
                SimpleHash hash = new SimpleHash();
                string salt = string.Empty;
                ret = hash.Verify(credentials.Password, user.Password);
            }

            return (user != null) ? new SealegsAuthResponse() { Success = (password == user.Password || "ns+" + password == user.Password || ret), User = user } : new SealegsAuthResponse() { Success = false, User = null };
        }

        #endregion 

        #region GetUser

        public SealegsUser GetUser(string sid)
        {
            // Validate user against database using email and password. 
            var user = db.SealegsUser.Include("Role").Where(u => u.Id.ToString() == sid).ToList();
            return user.Count() > 0 ? user.First() : null;
        }

        #endregion 
    }
}
