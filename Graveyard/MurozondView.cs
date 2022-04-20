using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Hearthstone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HearthDb.CardIds.Collectible;

namespace HDT.Plugins.Graveyard
{
    public class MurozondView : MultiTurnView
	{
		private static ViewConfig _Config;
        internal static ViewConfig Config
		{
			get => _Config ?? (_Config = new MurozondViewConfig()
			{
				Name = "Murozond",
				CreateView = () => new MurozondView(),
				UpdateOn = GameEvents.OnOpponentPlay,
				Condition = card => (card.Type == "Spell" || card.Type == "Minion"),
			});
		}
		public MurozondView() : base(1) { }

		private static bool IsMurozond(Card card) => card.Id == Priest.MurozondTheInfinite1810;
		private bool MurozondInHand = false;

        public override bool Update(Card card)
        {
            if (MurozondInHand && IsMurozond(card)) MurozondInHand = false;
            return MurozondInHand && base.Update(card);
        }

        public bool PlayerPlayToHand(Card card)
		{
			if (!MurozondInHand && IsMurozond(card)) MurozondInHand = true;
			return MurozondInHand;
		}

		internal class MurozondViewConfig : ViewConfig
		{
			public MurozondViewConfig() : base(Priest.MurozondTheInfinite1810)
			{

			}

			public override void RegisterView(ViewBase view, bool isDefault = false)
			{
				base.RegisterView(view, isDefault);
                if (view is MurozondView murozondview)
                {
                    RegisterForCardEvent(GameEvents.OnPlayerPlayToHand, murozondview.PlayerPlayToHand);
                }
            }
		}
	}


}
