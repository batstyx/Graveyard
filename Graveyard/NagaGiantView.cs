using Hearthstone_Deck_Tracker.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HearthDb.CardIds.Collectible;

namespace HDT.Plugins.Graveyard
{
    internal class NagaGiantView
    {
		private static ViewConfig _Config;
		internal static ViewConfig Config
		{
			get => _Config ?? (_Config = new ViewConfig(Neutral.NagaGiant)
			{
				Name = "NagaGiant",
				ShowFirst = () => true,
				CreateView = () => new ManaSpentView(Neutral.NagaGiant),
				UpdateOn = GameEvents.OnPlayerPlay,
				Condition = card => ManaSpentView.SharedCondition(card),
			});
		}
	}
}
