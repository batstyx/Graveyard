﻿using Hearthstone_Deck_Tracker.API;
using static HearthDb.CardIds.Collectible;
using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
using Core = Hearthstone_Deck_Tracker.Core;

namespace HDT.Plugins.Graveyard
{
    public class AntonidasView : MultiTurnView
    {
		private static ViewConfig _Config;
		internal static ViewConfig Config
        {
			get => _Config ?? (_Config = new ViewConfig(Mage.GrandMagusAntonidas)
			{
				Name = Strings.GetLocalized("Antonidas"),
				Enabled = () => Settings.Default.AntonidasEnabled,
				Condition = card => card.GetSchool() == School.Fire,
				WatchFor = GameEvents.OnPlayerPlay,
				CreateView = () => new MultiTurnView(Strings.GetLocalized("Antonidas"), 3),
			});
        }

		public static bool isValid()
		{
			return Core.Game.Player.PlayerCardList.FindIndex(card => Config.ShowOn.Contains(card.Id)) > -1;
		}

		public AntonidasView() 
			: base(Config.Name, 3)
		{
		}

		public override bool Update(Card card)
		{
			if (Config.Condition(card))
			{
				return base.Update(card);
			}
			return false;
		}
	}
}
