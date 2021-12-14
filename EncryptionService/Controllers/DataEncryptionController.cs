using System;
using System.Threading.Tasks;

using EncryptionService.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
                var alarm = await _encryptor.EncryptData(data);
                return Ok(alarm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Decrypts data
        /// </summary>
        [HttpGet("decrypt")]
        public async Task<IActionResult> GetDecryptedDataAsync([FromQuery] string data)
        {
            try
            {
                var alarm = await _encryptor.DecryptData(data);
                return Ok(alarm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
