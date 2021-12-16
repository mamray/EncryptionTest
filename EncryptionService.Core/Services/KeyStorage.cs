using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Services
{
    public class KeyStorage : IKeyStorage
    {
        private List<string> _keys = new List<string>();
        private int _maxKeyCount = 256;

        public KeyStorage() { }

        public KeyStorage(int maxKeyCount)
        {
            _maxKeyCount = maxKeyCount;
        }

        /// <summary>
        /// Returns current active key
        /// </summary>
        public string ActiveKey 
        { 
            get
            {
                if (!_keys.Any())
                {
                    PushKey();
                }                    

                return _keys[_keys.Count - 1];
            }
        }

        /// <summary>
        /// Returns any of the available keys by index
        /// </summary>
        /// <param name="index">index of the key, e.g. number in the stack</param>
        /// <returns></returns>
        public string this[int index]
        {
            get
            {                
                if (index < 0 || index > (_keys.Count - 1))
                    return null;

                return _keys[_keys.Count - index - 1];
            }
        }

        /// <summary>
        /// Amount of the keys already used
        /// </summary>
        public int Count
        {
            get
            {                
                return _keys.Count;
            }
        }

        /// <summary>
        /// Creates new key and makes it active by placing on the top of the stack. It's synchronous method here
        /// but can become real asynchronous if working with some external storage like DB or Azure KeyVault.
        /// </summary>
        /// <returns></returns>
        public async Task RotateKey()
        {
            PushKey();
            if (_keys.Count > _maxKeyCount)
                _keys.RemoveAt(0);
        }      
        
        /// <summary>
        /// An algorythm to calculate and add key to the top of the stack
        /// </summary>
        private void PushKey()
        {
            _keys.Add(Guid.NewGuid().ToString().Replace("-", ""));
        }
    }
}
