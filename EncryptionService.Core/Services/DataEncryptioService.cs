using System;
using System.Threading.Tasks;

using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Services
{
    public class DataEncryptioService : IDataEncryptionService
    {
        public Task<string> DecryptData(string data)
        {
            return Task.FromResult(Base64Decode(data));
        }

        public Task<string> EncryptData(string data)
        {
            return Task.FromResult(Base64Encode(data));
        }

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
