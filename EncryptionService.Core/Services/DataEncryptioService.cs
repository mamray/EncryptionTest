using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Services
{
    public class DataEncryptioService : IDataEncryptionService
    {
        private IKeyStorage _keyStorage;
        public DataEncryptioService(IKeyStorage keyStorage)
        {
            _keyStorage = keyStorage;
        }

        public async Task<string> EncryptData(string data)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_keyStorage.ActiveKey);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            await streamWriter.WriteAsync(data);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }

        public async Task<string> DecryptData(string data, int keysBack = 0)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(data);
            var activeKey = _keyStorage[keysBack];
            if (activeKey == null)
                throw new InvalidOperationException("Trying to decrypt with unavailable key.");

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(activeKey);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return await streamReader.ReadToEndAsync();
                        }
                    }
                }
            }
        }
        
        public async Task RotateKey()
        {
            await _keyStorage.RotateKey();
        }         
    }
}
