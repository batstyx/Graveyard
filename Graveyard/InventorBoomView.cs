using Hearthstone_Deck_Tracker.API;
using static HearthDb.CardIds.Collectible;

namespace HDT.Plugins.Graveyard
{
    internal class InventorBoomView
    {
        private static ViewConfig _Config;
        internal static ViewConfig Config
        {
            get => _Config ?? (_Config = new ViewConfig(Warrior.InventorBoom)
            {
                CreateView = () => new NormalView(),
                UpdateOn = GameEvents.OnPlayerPlayToGraveyard,
                Condition = card => {

                    if (card.Name == "Zilliax Deluxe 3000") 
                        return true;
                    if (card.Race == "Mech" || card.Race == "All")
                        if (card.Cost >= 5)
                            return true;
                    return false;
                },
            });
        }
    }
}
