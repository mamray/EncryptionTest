using System.Threading.Tasks;

namespace EncryptionService.Core.Interfaces
{
    public interface IKeyStorage
    {
        string ActiveKey { get; }
        string this[int index] { get; }        
        int Count { get; }
        Task RotateKey();
    }
}
