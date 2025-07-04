using Hearthstone_Deck_Tracker.Hearthstone;

namespace HDT.Plugins.Graveyard
{
    internal class ViewConfigCard
    {
        public Card Card { get; private set; }
        public string CardId => Card.Id;
        public string CardName => Card.Name;
        public bool IsEnabled { get; internal set; }
        public ViewConfigCard(Card card, bool isEnabled)
        {
            Card = card;
            IsEnabled = isEnabled;
        }
    }
}
