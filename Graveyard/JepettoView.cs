using Hearthstone_Deck_Tracker.API;
using static HearthDb.CardIds.Collectible;

namespace HDT.Plugins.Graveyard
{
    internal class JepettoView
    {
        private static ViewConfig _Config;
        internal static ViewConfig Config
        {
            get => _Config ?? (_Config = new ViewConfig(Neutral.JoymancerJepetto)
            {
                CreateView = () => new NormalView(),
                UpdateOn = GameEvents.OnPlayerPlay,
                Condition = card => card.Type == "Minion" 
                    && (card.Health == 1 
                    || card.Attack == 1)
            });
        }
    }
}
