using System;

namespace AccountingSystem.Desktop
{
    public class SettingsManager
    {
        public void SaveLocalSetting(string key, string value)
        {
            Console.WriteLine($"[Settings] Saving local setting: {key} = {value}");
        }
    }
}
