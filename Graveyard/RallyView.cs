﻿using Hearthstone_Deck_Tracker.API;
using static HearthDb.CardIds.Collectible;

namespace HDT.Plugins.Graveyard
{
    public class RallyView : ChancesView
	{
		private static ViewConfig _Config;
		internal static ViewConfig Config
		{
			get => _Config ?? (_Config = new ViewConfig(Neutral.Rally)
            {
				Name = Strings.GetLocalized("Rally"),
				Enabled = () => Settings.Default.RallyEnabled,
				CreateView = () => new RallyView(),
				WatchFor = GameEvents.OnPlayerPlayToGraveyard,
				Condition = card => card.Type == "Minion" && card.Cost >= 1 && card.Cost <= 3,
			});
		}
		
		public static bool IsAlwaysSeparate => Settings.Default.RallyEnabled && Settings.Default.AlwaysRallySeparately;
	}
}
