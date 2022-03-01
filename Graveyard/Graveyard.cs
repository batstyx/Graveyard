using Hearthstone_Deck_Tracker.API;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
using Core = Hearthstone_Deck_Tracker.API.Core;
using GameMode = Hearthstone_Deck_Tracker.Enums.GameMode;


namespace HDT.Plugins.Graveyard
{
    public class Graveyard
	{
		private static readonly string SharedName = Strings.GetLocalized("Graveyard");
		private static readonly Func<ViewBase> SharedCreateView = () => new NormalView();
		private static readonly Predicate<Card> SharedCondition = card => card.Type == "Minion";

		internal static ViewConfig FriendlyConfig
        {
			get => _FriendlyConfig ?? (_FriendlyConfig = new ViewConfig()
            {
				Name = SharedName,
				Enabled = () => Settings.Default.NormalEnabled,
				CreateView = SharedCreateView,
				WatchFor = GameEvents.OnPlayerPlayToGraveyard,
				Condition = SharedCondition,
			});
        }
		private static ViewConfig _FriendlyConfig;

		internal static ViewConfig EnemyConfig
        {
			get => _EnemyConfig ?? (_EnemyConfig = new ViewConfig()
            {
				Name = SharedName,
				Enabled = () => Settings.Default.EnemyEnabled,
				CreateView = SharedCreateView,
				WatchFor = GameEvents.OnOpponentPlayToGraveyard,
				Condition = SharedCondition,
            });
        }
		private static ViewConfig _EnemyConfig;

		// The views
		public NormalView Normal;
		public NormalView Enemy;

		public QuestlineView FriendlyQuestline;
		public QuestlineView EnemyQuestline;

		public ResurrectView Resurrect;
		public AnyfinView Anyfin;
        public DeathrattleView Deathrattle;
        public NZothView NZoth;
		public HadronoxView Hadronox;
		public DiscardView Discard;
		public GuldanView Guldan;
		public ShudderwockView Shudderwock;
		public DragoncallerAlannaView DragoncallerAlanna;
        public CavernsView Caverns;
        public MulchmuncherView Mulchmuncher;
        public KangorView Kangor;
        public WitchingHourView WitchingHour;
        public TessGreymaneView TessGreymane;
        public SoulwardenView Soulwarden;
		public ZuljinView Zuljin;
		public HoardPillagerView HoardPillager;
		public LadyLiadrinView LadyLiadrin;
		public NZothGotDView NZothGotD;
		public RallyView Rally;
		public SaurfangView Saurfang;
		public YShaarjView YShaarj;
		public ElwynnBoarView ElwynnBoar;
		public KargalView Kargal;
		public AntonidasView Antonidas;
		public GrandFinaleView GrandFinale;
		public LastPlayedView LastPlayed;
		public LastCardView BrilliantMacaw;
		public LastCardView GreySageParrot;
		public LastCardView MonstrousParrot;
		public LastCardView SunwingSquawker;
		public LastCardView VanessaVanCleef;
		public MulticasterView Multicaster;
		public ShirvallahView Shirvallah;

		private readonly StackPanel FriendlyPanel;
		private readonly StackPanel EnemyPanel;

		private StackPanel SinglesPanel;

		public static InputManager Input;

        public Graveyard()
		{

			// Create container
			EnemyPanel = new StackPanel();
			EnemyPanel.Orientation = Orientation.Vertical;
			Core.OverlayCanvas.Children.Add(EnemyPanel);
			Canvas.SetTop(EnemyPanel, Settings.Default.EnemyTop);
			Canvas.SetLeft(EnemyPanel, Settings.Default.EnemyLeft);

			// Create container
			FriendlyPanel = new StackPanel();
			FriendlyPanel.Orientation = Settings.Default.FriendlyOrientation;
			Core.OverlayCanvas.Children.Add(FriendlyPanel);
			Canvas.SetTop(FriendlyPanel, Settings.Default.PlayerTop);
			Canvas.SetLeft(FriendlyPanel, Settings.Default.PlayerLeft);

			Input = new InputManager(FriendlyPanel, EnemyPanel);

			Settings.Default.PropertyChanged += SettingsChanged;
			SettingsChanged(null, null);
		}

        //on year change clear out the grid and update the data
        private void SettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			FriendlyPanel.Orientation = Settings.Default.FriendlyOrientation;
			FriendlyPanel.RenderTransform = new ScaleTransform(Settings.Default.FriendlyScale / 100, Settings.Default.FriendlyScale / 100);
			FriendlyPanel.Opacity = Settings.Default.FriendlyOpacity / 100;
			EnemyPanel.RenderTransform = new ScaleTransform(Settings.Default.EnemyScale / 100, Settings.Default.EnemyScale / 100);
			EnemyPanel.Opacity = Settings.Default.EnemyOpacity / 100;
		}

		public void Dispose()
		{
			Core.OverlayCanvas.Children.Remove(FriendlyPanel);
			Core.OverlayCanvas.Children.Remove(EnemyPanel);
			Input.Dispose();
		}

		/**
		* Clear then recreate all Views.
		*/
		public void Reset()
		{
			FriendlyPanel.Children.Clear();
			EnemyPanel.Children.Clear();

			if (Core.Game.IsBattlegroundsMatch || Core.Game.IsMercenariesMatch)
			{
				// don't show graveyard for Battlegrounds or Mercenaries
				// this should include spectating
				return;
			}

			ShowEnemyView(QuestlineView.EnemyConfig, ref EnemyQuestline);
			ShowEnemyView(EnemyConfig, ref Enemy);

			ShowFriendlyView(QuestlineView.FriendlyConfig, ref FriendlyQuestline);
			SinglesPanel = new StackPanel();
			FriendlyPanel.Children.Add(SinglesPanel);
			if (!ShowFriendlyView(ResurrectView.Config, ref Resurrect))
			{
				ShowFriendlyView(FriendlyConfig, ref Normal);
			}
			ShowFriendlyView(AnyfinView.Config, ref Anyfin);
			ShowFriendlyView(DeathrattleView.Config, ref Deathrattle);
			ShowFriendlyView(NZothView.Config, ref NZoth);
			ShowFriendlyView(HadronoxView.Config, ref Hadronox);
			ShowFriendlyView(DiscardView.Config, ref Discard);
			ShowFriendlyView(ShudderwockView.Config, ref Shudderwock);
			ShowFriendlyView(GuldanView.Config, ref Guldan);
			ShowFriendlyView(DragoncallerAlannaView.Config, ref DragoncallerAlanna);
			ShowView(MulchmuncherView.Config, ref Mulchmuncher, SinglesPanel.Children);
			ShowFriendlyView(CavernsView.Config, ref Caverns);
			ShowFriendlyView(KangorView.Config, ref Kangor);
			ShowFriendlyView(WitchingHourView.Config, ref WitchingHour);
			ShowFriendlyView(SoulwardenView.Config, ref Soulwarden);
			ShowFriendlyView(TessGreymaneView.Config, ref TessGreymane);
			ShowFriendlyView(ZuljinView.Config, ref Zuljin);
			ShowFriendlyView(HoardPillagerView.Config, ref HoardPillager);
			ShowFriendlyView(LadyLiadrinView.Config, ref LadyLiadrin);
			ShowFriendlyView(NZothGotDView.Config, ref NZothGotD);
			ShowFriendlyView(RallyView.Config, ref Rally);
			ShowFriendlyView(SaurfangView.Config, ref Saurfang);
			ShowFriendlyView(YShaarjView.Config, ref YShaarj);
			ShowView(ElwynnBoarView.Config, ref ElwynnBoar, SinglesPanel.Children);
			ShowFriendlyView(KargalView.Config, ref Kargal);
			ShowFriendlyView(AntonidasView.Config, ref Antonidas);
			ShowFriendlyView(GrandFinaleView.Config, ref GrandFinale);
			ShowView(LastPlayedView.BrilliantMacawConfig, ref BrilliantMacaw, SinglesPanel.Children);
			ShowView(LastPlayedView.GreySageParrotConfig, ref GreySageParrot, SinglesPanel.Children);
			ShowView(LastPlayedView.MonstrousParrotConfig, ref MonstrousParrot, SinglesPanel.Children);
			ShowView(LastPlayedView.SunwingSquawkerConfig, ref SunwingSquawker, SinglesPanel.Children);
			ShowView(LastPlayedView.VanessaVanCleefConfig, ref VanessaVanCleef, SinglesPanel.Children);
			ShowFriendlyView(MulticasterView.Config, ref Multicaster);
			ShowFriendlyView(ShirvallahView.Config, ref Shirvallah);
		}

		private bool ShowFriendlyView<T>(ViewConfig config, ref T view) where T : ViewBase, new()
        {
			return ShowView(config, ref view, FriendlyPanel.Children);
		}

		private bool ShowEnemyView<T>(ViewConfig config, ref T view) where T : ViewBase, new()
		{
			return ShowView(config, ref view, EnemyPanel.Children);
		}

		private bool ShowView<T>(ViewConfig config, ref T view, UIElementCollection parent) where T : ViewBase, new()
		{
			view = CreateView<T>(config);
            if (view == null)
            {
				return false;
			}	
			parent.Add(view);
			return true;
		}

		private T CreateView<T>(ViewConfig config) where T : ViewBase, new()
        {
			if (config.Enabled() && (config.ShowOn == null || Core.Game.Player.PlayerCardList.FindIndex(card => config.ShowOn.Contains(card.Id)) > -1))
			{
				var view = config.CreateView() as T;
				view.Title = config.Name;
				view.Condition = config.Condition;
				return view;
			}
			return null;
		}

		public void PlayerGraveyardUpdate(Card card)
		{
			var any = Anyfin?.Update(card) ?? false;
            var deathrattle = Deathrattle?.Update(card) ?? false;
			MonstrousParrot?.Update(card);
            var nzoth = NZoth?.Update(card) ?? false;
			var hadr = Hadronox?.Update(card) ?? false;
			var guldan = Guldan?.Update(card) ?? false;
			var rez = Resurrect?.Update(card) ?? false;
            var mulch = Mulchmuncher?.Update(card) ?? false;
            var kangor = Kangor?.Update(card) ?? false;
            var witching = WitchingHour?.Update(card) ?? false;
			var hoardpillager = HoardPillager?.Update(card) ?? false;
			var nzothgotd = NZothGotD?.Update(card) ?? false;
			var rally = ((rez && RallyView.IsAlwaysSeparate) || !rez) && (Rally?.Update(card) ?? false);
			var saurfang = Saurfang?.Update(card) ?? false;
			var elwynnboar = ((deathrattle && ElwynnBoarView.IsAlwaysSeparate) || !deathrattle) && (ElwynnBoar?.Update(card) ?? false);
            if (!(any || deathrattle || nzoth || hadr || guldan || rez || mulch || kangor || witching || hoardpillager || nzothgotd || rally || saurfang || elwynnboar))
			{
				Normal?.Update(card);
			}
		}

		public void EnemyDamageUpdate(Card card)
		{
			Anyfin?.Update(card);
		}

		public void PlayerDamageUpdate(Card card)
		{
			Anyfin?.Update(card);
		}

		public void EnemyGraveyardUpdate(Card card)
		{
			Anyfin?.Update(card);
			Enemy?.Update(card);
		}

		public void PlayerDiscardUpdate(Card card)
		{
			Discard?.Update(card);
            Soulwarden?.Update(card);
		}

		public void PlayerPlayUpdate(Card card)
		{
			FriendlyQuestline?.Update(card);
			Shudderwock?.Update(card);
			BrilliantMacaw?.Update(card);
			DragoncallerAlanna?.Update(card);
			GreySageParrot?.Update(card);
            Caverns?.Update(card);
            TessGreymane?.Update(card);
			Zuljin?.Update(card);
			LadyLiadrin?.Update(card);
			SunwingSquawker?.Update(card);
			YShaarj?.Update(card);
			Kargal?.Update(card);
			Antonidas?.Update(card);
			GrandFinale?.Update(card);
			Multicaster?.Update(card);
			Shirvallah?.Update(card);
		}

		public void OpponentPlayUpdate(Card card)
        {
			EnemyQuestline?.Update(card);
			VanessaVanCleef?.Update(card);
        }

		public async void TurnStartUpdate(Hearthstone_Deck_Tracker.Enums.ActivePlayer player)
		{
			if (player == Hearthstone_Deck_Tracker.Enums.ActivePlayer.Opponent)
            {
				if (Antonidas != null) await Antonidas.TurnEnded();
				if (GrandFinale != null) await GrandFinale.TurnEnded();
			}
		}
	}
}