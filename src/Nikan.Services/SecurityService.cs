using System;
using System.Security.Cryptography;
using System.Text;

namespace Nikan.Services
{
    public interface ISecurityService
    {
        string GetSha256Hash(string input);
        Guid CreateCryptographicallySecureGuid();
        string GetShaMD5Hash(string input);
    }

    public class SecurityService : ISecurityService
    {
        private readonly RandomNumberGenerator _rand = RandomNumberGenerator.Create();

        public string GetSha256Hash(string input)
        {
              var hashAlgorithm = new SHA256CryptoServiceProvider();
            var byteValue = Encoding.UTF8.GetBytes(input);
            var byteHash = hashAlgorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }

        public string GetShaMD5Hash(string input)
        {
            var x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var data = Encoding.ASCII.GetBytes(input);
            data = x.ComputeHash(data);
            return Encoding.ASCII.GetString(data);
        }



        public Guid CreateCryptographicallySecureGuid()
        {
            var bytes = new byte[16];
            _rand.GetBytes(bytes);
            return new Guid(bytes);
        }
    }
}