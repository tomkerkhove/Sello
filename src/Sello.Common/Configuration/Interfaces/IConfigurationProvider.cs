namespace Sello.Common.Configuration.Interfaces
{
    public interface IConfigurationProvider
    {
        Environment GetCurrentEnvironment();
        string GetSetting(string settingName);
        string GetSetting(string settingName, bool isMandatory);
        bool IsCurrentEnvironment(Environment environment);
    }
}