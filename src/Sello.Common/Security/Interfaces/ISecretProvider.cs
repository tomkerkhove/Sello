using System.Threading.Tasks;

namespace Sello.Common.Security.Interfaces
{
    public interface ISecretProvider
    {
        Task<string> GetSecretAsync(string secretName);
    }
}