using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Hearthstone;
using System;
using System.Windows;
using static HearthDb.CardIds.Collectible;
using static Hearthstone_Deck_Tracker.API.Core;

namespace HDT.Plugins.Graveyard
{
    public class ShirvallahView
	{
		private static ViewConfig _Config;
		internal static ViewConfig Config
		{
			get => _Config ?? (_Config = new ViewConfig(Paladin.ShirvallahTheTiger)
			{
				Name = "Shirvallah",
				Enabled = "ShirvallahEnabled",
				ShowFirst = () => true,
				CreateView = () => new ManaSpentView(Paladin.ShirvallahTheTiger),
				UpdateOn = GameEvents.OnPlayerPlay,
				Condition = card => ManaSpentView.SharedCondition(card),				
			});
		}		
	}
}
