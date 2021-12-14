using System.Threading.Tasks;

namespace EncryptionService.Core.Interfaces
{
    public interface IDataEncryptionService
    {
        Task<string> EncryptData(string data);
        Task<string> DecryptData(string data);
    }
}
