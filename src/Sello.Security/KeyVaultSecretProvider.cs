using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace Sello.Security
{
    public class KeyVaultSecretProvider:ISecretProvider
    {
        private readonly AzureServiceTokenProvider _azureServiceTokenProvider = new AzureServiceTokenProvider();
        private readonly KeyVaultClient _keyVaultClient;

        public KeyVaultSecretProvider(string vaultUri)
        {
            VaultUri = vaultUri;

            _keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(_azureServiceTokenProvider.KeyVaultTokenCallback));
        }

        public string VaultUri { get; }

        public async Task<string> GetSecretAsync(string secretName)
        {
            var secret = await _keyVaultClient.GetSecretAsync(VaultUri, secretName);
            return secret.Value;
        }
    }

    public interface ISecretProvider
    {
        Task<string> GetSecretAsync(string secretName);
    }
}