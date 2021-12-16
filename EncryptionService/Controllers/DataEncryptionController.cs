using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using EncryptionService.Core.Interfaces;

namespace EncryptionService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataEncryptionController : Controller
    {
        private readonly ILogger<DataEncryptionController> _logger;
        private readonly IDataEncryptionService _encryptor;

        public DataEncryptionController(IDataEncryptionService encryptor, ILogger<DataEncryptionController> logger)
        {
            _encryptor = encryptor;
            _logger = logger;
        }

        /// <summary>
        /// Encrypts data
        /// </summary>
        [HttpGet("encrypt")]
        public async Task<IActionResult> GetEncryptedDataAsync([FromQuery] string data)
        {
            try
            {
                var result = await _encryptor.EncryptData(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Encryption failed");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Decrypts data
        /// </summary>
        [HttpGet("decrypt")]
        public async Task<IActionResult> GetDecryptedDataAsync([FromQuery] string data, [FromQuery] int keyDraining = 0)
        {
            try
            {
                var result = await _encryptor.DecryptData(data, keyDraining);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Decryption failed");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Decrypts data
        /// </summary>
        [HttpGet("rotate-key")]
        public async Task<IActionResult> RotateKey()
        {
            try
            {
                await _encryptor.RotateKey();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Key rotation failed");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
