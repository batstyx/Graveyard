using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Hearthstone;
using static HearthDb.CardIds.Collectible;

namespace HDT.Plugins.Graveyard
{
    public class HedraView : NormalView
    {
		private static ViewConfig _Config;
		internal static ViewConfig Config
		{
			get => _Config ?? (_Config = new HedraViewConfig()
			{
				Name = "Hedra",
				CreateView = () => new HedraView(),
				UpdateOn = GameEvents.OnPlayerPlay,
				Condition = card => card.Type == "Spell",
			});
		}

		internal InHandAssistant InHand = new InHandAssistant(Druid.HedraTheHeretic);

		public override bool Update(Card card)
        {			
			return InHand.Check(card) && base.Update(card);
        }

        internal class HedraViewConfig : ViewConfig
		{
			public HedraViewConfig() : base(Druid.HedraTheHeretic)
			{

			}

			public override void RegisterView(ViewBase view, bool isDefault = false)
			{
				base.RegisterView(view, isDefault);
                if (view is HedraView hedraview)
                {
					RegisterForCardEvent(GameEvents.OnPlayerPlayToHand, hedraview.InHand.Incoming);
				}
				
			}
		}
	}
}
