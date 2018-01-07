using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Sello.Common.Configuration.Interfaces;
using Sello.Common.Security.Interfaces;

namespace Sello.Common.Security
{
    public class KeyVaultSecretProvider : ISecretProvider
    {
        private readonly AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
        private readonly KeyVaultClient keyVaultClient;

        public KeyVaultSecretProvider(IConfigurationProvider configurationProvider)
        {
            VaultUri = configurationProvider.GetSetting("Security.KeyVault.Uri");

            keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
        }

        public string VaultUri { get; }

        public async Task<string> GetSecretAsync(string secretName)
        {
            // Remove '.' since Key Vault can't handle it
            secretName = secretName.Replace(".", "-");

            var secret = await keyVaultClient.GetSecretAsync(VaultUri, secretName);
            return secret.Value;
        }
    }
}