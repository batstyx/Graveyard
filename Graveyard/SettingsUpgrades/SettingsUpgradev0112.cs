using Hearthstone_Deck_Tracker.Utility.Logging;
using System;

namespace HDT.Plugins.Graveyard.SettingsUpgrades
{
    internal class SettingsUpgradev0112 : ISettingsUpgrade
    {
        private Settings Settings;

        public Version Version { get; } = new Version(1, 12);

        public bool Upgrade(Settings settings)
        {
            Settings = settings;

            if (Version.CompareTo(new Version(Settings.Version)) <= 0) return false;

            return TryUpdateConfig(nameof(Settings.FriendlyQuestlineEnabled))
                & TryUpdateConfig(nameof(Settings.EnemyQuestlineEnabled));
        }

        bool TryUpdateConfig(string setting, bool value = false, bool updateSetting = true)
        {
            try
            {
                if (updateSetting) Settings[setting] = value;
                Log.Info($"Upgraded {setting} setting to {Version}");
                return updateSetting;
            }
            catch (Exception ex)
            {
                Log.Warn($"Upgrade {setting} setting to {Version} failed");
                Log.Error(ex);
                return false;
            }
        }
    }
}
