using System;
using System.Security.Cryptography;
using System.Text;

namespace Monospad.Core.Providers.Impl
{
    public class CryptoProvider : ICryptoProvider
    {
        private readonly HashAlgorithm _hashAlg = SHA256.Create();
        private readonly Encoding _encoding = Encoding.UTF8;

        public string ComputeHash(string utf8Text)
        {
            var bytes = _encoding.GetBytes(utf8Text);
            byte[] hash;
            lock(_hashAlg)
            {
                hash = _hashAlg.ComputeHash(bytes);
            }
            return BitConverter.ToString(hash);
        }
    }
}
