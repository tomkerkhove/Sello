using System;
using System.Configuration;
using Sello.Common.Configuration.Interfaces;
using Sello.Common.Exceptions;

namespace Sello.Common.Configuration
{
    public class LocalConfigurationProvider : IConfigurationProvider
    {
        public Environment GetCurrentEnvironment()
        {
            var rawEnvironment = GetSetting("Common.Environment");
            if (Enum.TryParse<Environment>(rawEnvironment, out var environment) == false)
            {
                throw new Exception($"Unable to determine the current environment.('{rawEnvironment}')");
            }

            return environment;
        }

        public string GetSetting(string settingName, bool isMandatory)
        {
            var rawSetting = ConfigurationManager.AppSettings[settingName];
            if (string.IsNullOrWhiteSpace(rawSetting) && isMandatory)
            {
                throw new SettingNotFoundException(settingName);
            }

            return rawSetting;
        }

        public string GetSetting(string settingName)
        {
            return GetSetting(settingName, isMandatory: true);
        }

        public bool IsCurrentEnvironment(Environment environment)
        {
            var currentEnvironment = GetCurrentEnvironment();
            return currentEnvironment == environment;
        }
    }
}