using System.Threading.Tasks;
using Sello.Common.Configuration.Interfaces;
using Sello.Common.Exceptions;
using Sello.Common.Security.Interfaces;

namespace Sello.Common.Security
{
    public class LocalSecretProvider : ISecretProvider
    {
        private readonly IConfigurationProvider configurationProvider;

        public LocalSecretProvider(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        public Task<string> GetSecretAsync(string secretName)
        {
            var setting = configurationProvider.GetSetting(secretName);
            if (string.IsNullOrWhiteSpace(setting))
            {
                throw new SecretNotFoundException(secretName);
            }

            return Task.FromResult(setting);
        }
    }
}