using System;

namespace Sello.Common.Exceptions
{
    public class SettingNotFoundException : Exception
    {
        public SettingNotFoundException(string settingName) : base($"Setting with name '{settingName}' was not found")
        {
            SettingName = settingName;
        }

        public string SettingName { get; }
    }
}