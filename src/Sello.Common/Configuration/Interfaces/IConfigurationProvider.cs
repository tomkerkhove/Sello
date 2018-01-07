namespace Sello.Common.Configuration.Interfaces
{
    public interface IConfigurationProvider
    {
        Environment GetCurrentEnvironment();
        string GetSetting(string settingName);
        bool IsCurrentEnvironment(Environment environment);
    }
}