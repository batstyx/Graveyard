using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.API;
using Core = Hearthstone_Deck_Tracker.API.Core;
using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;

namespace HDT.Plugins.Graveyard
{
	public class Graveyard
	{
		// The views
		public NormalView Normal;
		public NormalView Enemy;

		public ResurrectView Resurrect;
		public AnyfinView Anyfin;
		public NZothView NZoth;
		public HadronoxView Hadronox;
		public DiscardView Discard;
		public GuldanView Guldan;

		private StackPanel _friendlyPanel;
		private StackPanel _enemyPanel;

		public static InputManager Input;

		public Graveyard()
		{

			// Create container
			_enemyPanel = new StackPanel();
			_enemyPanel.Orientation = Orientation.Vertical;
			Core.OverlayCanvas.Children.Add(_enemyPanel);
			Canvas.SetTop(_enemyPanel, Settings.Default.EnemyTop);
			Canvas.SetLeft(_enemyPanel, Settings.Default.EnemyLeft);

			// Create container
			_friendlyPanel = new StackPanel();
			_friendlyPanel.Orientation = Orientation.Vertical;
			Core.OverlayCanvas.Children.Add(_friendlyPanel);
			Canvas.SetTop(_friendlyPanel, Settings.Default.PlayerTop);
			Canvas.SetLeft(_friendlyPanel, Settings.Default.PlayerLeft);

			Input = new InputManager(_friendlyPanel, _enemyPanel);

			Settings.Default.PropertyChanged += SettingsChanged;
			SettingsChanged(null, null);

			// Connect events
			GameEvents.OnGameStart.Add(Reset);
			GameEvents.OnGameEnd.Add(Reset);
			DeckManagerEvents.OnDeckSelected.Add(d => Reset());

			GameEvents.OnPlayerPlayToGraveyard.Add(PlayerGraveyardUpdate);
			GameEvents.OnOpponentPlayToGraveyard.Add(EnemyGraveyardUpdate);

			GameEvents.OnPlayerPlay.Add(c => Anyfin?.UpdateDamage());
			GameEvents.OnOpponentPlay.Add(c => Anyfin?.UpdateDamage());

			GameEvents.OnPlayerHandDiscard.Add(PlayerDiscardUpdate);
		}

		//on year change clear out the grid and update the data
		private void SettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			_friendlyPanel.RenderTransform = new ScaleTransform(Settings.Default.FriendlyScale / 100, Settings.Default.FriendlyScale / 100);
			_friendlyPanel.Opacity = Settings.Default.FriendlyOpacity / 100;
			_enemyPanel.RenderTransform = new ScaleTransform(Settings.Default.EnemyScale / 100, Settings.Default.EnemyScale / 100);
			_enemyPanel.Opacity = Settings.Default.EnemyOpacity / 100;
		}

		public void Dispose()
		{
			Core.OverlayCanvas.Children.Remove(_friendlyPanel);
			Core.OverlayCanvas.Children.Remove(_enemyPanel);
			Input.Dispose();
		}

		/**
		* Clear then recreate all Views.
		*/
		public void Reset()
		{
			_friendlyPanel.Children.Clear();
			_enemyPanel.Children.Clear();

			if (Settings.Default.EnemyEnabled)
			{
				Enemy = new NormalView();
				_enemyPanel.Children.Add(Enemy);
			}
			else
			{
				Enemy = null;
			}

			if (Settings.Default.ResurrectEnabled && ResurrectView.isValid())
			{
				Resurrect = new ResurrectView();
				_friendlyPanel.Children.Add(Resurrect);
				Normal = null;
			}
			else if (Settings.Default.NormalEnabled)
			{
				Normal = new NormalView();
				_friendlyPanel.Children.Add(Normal);
				Resurrect = null;
			}
			else
			{
				Normal = null;
				Resurrect = null;
			}

			if (Settings.Default.AnyfinEnabled && AnyfinView.isValid())
			{
				Anyfin = new AnyfinView();
				_friendlyPanel.Children.Add(Anyfin);
			}
			else
			{
				Anyfin = null;
			}

			if (Settings.Default.NZothEnabled && NZothView.isValid())
			{
				NZoth = new NZothView();
				_friendlyPanel.Children.Add(NZoth);
			}
			else
			{
				NZoth = null;
			}

			if (Settings.Default.HadronoxEnabled && HadronoxView.isValid())
			{
				Hadronox = new HadronoxView();
				_friendlyPanel.Children.Add(Hadronox);
			}
			else
			{
				Hadronox = null;
			}

			if (Settings.Default.DiscardEnabled && DiscardView.isValid())
			{
				Discard = new DiscardView();
				_friendlyPanel.Children.Add(Discard);
			}
			else
			{
				Discard = null;
			}

			if (Settings.Default.GuldanEnabled && GuldanView.isValid())
			{
				Guldan = new GuldanView();
				_friendlyPanel.Children.Add(Guldan);
			}
			else
			{
				Guldan = null;
			}
		}

		public void PlayerGraveyardUpdate(Card card)
		{
			var any = Anyfin?.Update(card) ?? false;
			var nzoth = NZoth?.Update(card) ?? false;
			var hadr = Hadronox?.Update(card) ?? false;
			var guldan = Guldan?.Update(card) ?? false;
			var rez = Resurrect?.Update(card) ?? false;
			if (!(any || nzoth || hadr || guldan || rez))
			{
				Normal?.Update(card);
			}
		}

		public void EnemyGraveyardUpdate(Card card)
		{
			Anyfin?.Update(card);
			Enemy?.Update(card);
		}

		public void PlayerDiscardUpdate(Card card)
		{
			Discard?.Update(card);
		}
	}
}
