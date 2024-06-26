using HDT.Plugins.Graveyard.Properties;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Plugins;
using System;
using System.Reflection;
using System.Windows.Controls;

namespace HDT.Plugins.Graveyard
{
    public class Plugin : IPlugin
	{
        private Settings Settings;
        internal static EventManager Events { get; private set; }
        internal static Graveyard Graveyard { get; private set; }
        public string Author => "RedHatter";
        public string ButtonText => Strings.GetLocalized(nameof(Resources.Strings.Settings));

        public string Description => Strings.GetLocalized(nameof(Resources.Strings.GraveyardDescription));

        public MenuItem MenuItem { get; set; }
        public string Name => LibraryInfo.Name;

        public void OnButtonPress() => SettingsView.Flyout.IsOpen = true;
        public void OnLoad()
        {
            Settings = Settings.Default;

            MenuItem = new MenuItem { Header = Strings.GetLocalized(nameof(Resources.Strings.Graveyard)) };
            MenuItem.Click += (sender, args) => OnButtonPress();

            Events = new EventManager();

            ViewConfigCards.Instance = new ViewConfigCards(Settings);

            Graveyard = new Graveyard();

            Settings.Upgrade();

            GameEvents.OnGameStart.Add(Graveyard.Reset);
            DeckManagerEvents.OnDeckSelected.Add(UpdateSelectedDeck);

            GameEvents.OnPlayerPlayToGraveyard.Add(Events.OnPlayerPlayToGraveyard.Poll);
            GameEvents.OnOpponentPlayToGraveyard.Add(Events.OnOpponentPlayToGraveyard.Poll);

            GameEvents.OnPlayerHandDiscard.Add(Events.OnPlayerHandDiscard.Poll);
            
            GameEvents.OnPlayerPlay.Add(Events.OnPlayerPlay.Poll);
            GameEvents.OnOpponentPlay.Add(Events.OnOpponentPlay.Poll);

            GameEvents.OnPlayerCreateInPlay.Add(Events.OnPlayerCreateInPlay.Poll);
            GameEvents.OnOpponentCreateInPlay.Add(Events.OnOpponentCreateInPlay.Poll);
           
            GameEvents.OnPlayerDraw.Add(Events.OnPlayerDraw.Poll);

            GameEvents.OnPlayerMulligan.Add(Events.OnPlayerMulligan.Poll);

            GameEvents.OnPlayerGet.Add(Events.OnPlayerGet.Poll);

            GameEvents.OnTurnStart.Add(Events.OnOpponentTurnStart.Poll);

            UpdateSelectedDeck(DeckList.Instance.ActiveDeck);
        }

        private Deck SelectedDeck;
        private void UpdateSelectedDeck(Deck deck)
        {
            if (deck == DeckList.Instance.ActiveDeck && deck != SelectedDeck)
            {
                SelectedDeck = deck;
                Graveyard.Reset();
            }
        }

        public void OnUnload()
        {
            if (Settings?.HasChanges ?? false) Settings?.Save();
            Settings = null;

            Graveyard?.Dispose();
            Graveyard = null;

            ViewConfigCards.Instance = null;

            Events?.Clear();
            Events = null;
        }

        public void OnUpdate() 
        {
            Graveyard?.Update();
        }

        public Version Version => LibraryInfo.Version;
    }
}
