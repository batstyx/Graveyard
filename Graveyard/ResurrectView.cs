using Hearthstone_Deck_Tracker.API;
using static HearthDb.CardIds.Collectible;

namespace HDT.Plugins.Graveyard
{
    public class ResurrectView
	{
		private static ViewConfig _Config;
		internal static ViewConfig Config
		{
			get => _Config ?? (_Config = new ViewConfig(
				Warlock.HabeasCorpses,
				Priest.RaiseDead, 
				Priest.Psychopomp, 
				Neutral.BodyWrapper, 
				Priest.MassResurrection, 
				Priest.Resurrect,
				Priest.OnyxBishopKARA,
                Priest.EternalServitude,
                Priest.LesserDiamondSpellstone,
				Neutral.Kazakus)
            {
				Name = "Resurrect",
				Enabled = "ResurrectEnabled",
				CreateView = () => new ChancesView(),
				UpdateOn = GameEvents.OnPlayerPlayToGraveyard,
				Condition = card => card.Type == "Minion",
			});
		}
	}
}
