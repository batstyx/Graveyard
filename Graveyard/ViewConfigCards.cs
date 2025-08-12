using Hearthstone_Deck_Tracker.Hearthstone;
using System.Collections.Generic;

namespace HDT.Plugins.Graveyard
{
    internal class ViewConfigCards
    {
        public static ViewConfigCards Instance { get; set; }

        private readonly Settings Settings;
        private readonly Dictionary<string, ViewConfigCard> Lookup = new Dictionary<string, ViewConfigCard>();
        private readonly List<string> Excluded = new List<string>();

        public ViewConfigCards(Settings settings)
        {
            Settings = settings;
            if (Settings.ExcludedCards != "")
            {
                Excluded.AddRange(Settings.ExcludedCards.Split(','));
            }
        }
        public ViewConfigCard Factory(string cardId)
        {
            var tc = new ViewConfigCard(Database.GetCardFromId(cardId), IsEnabled(cardId));
            Lookup.Add(tc.CardId, tc);
            return tc;
        }
        public IEnumerable<ViewConfigCard> Enumerable => Lookup.Values;

        public bool IsEnabled(string cardId) => !Excluded.Contains(cardId);

        public void Toggle(ViewConfigCard config)
        {
            Toggle(config.CardId, config.IsEnabled);
        }

        public void Toggle(string cardId, bool value)
        {
            if (value && Excluded.Contains(cardId))
            {
                Excluded.Remove(cardId);
                Lookup[cardId].IsEnabled = true;
                UpdateSetting();
            }
            else if (!(value && Excluded.Contains(cardId)))
            {
                Excluded.Add(cardId);
                Lookup[cardId].IsEnabled = false;
                UpdateSetting();
            }
        }

        private void UpdateSetting()
        {
            Settings.ExcludedCards = string.Join(",", Excluded);
        }
    }
}
