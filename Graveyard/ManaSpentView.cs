using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Hearthstone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static HearthDb.CardIds.Collectible;
using static Hearthstone_Deck_Tracker.API.Core;


namespace HDT.Plugins.Graveyard
{   
    public class ManaSpentView : NormalView
    {
        internal static readonly Predicate<Card> SharedCondition = card =>
        card.Type == "Spell" && ManaSpentView.ActualCardCost(card) > 0;

        private readonly Card Card;
        public ManaSpentView(string cardId)
        {
            Card = Database.GetCardFromId(cardId);
            Cards.Add(Card);
        }

        public override bool Update(Card card)
        {
            if (Condition(card))
            {
                Card.Cost = Math.Max(Card.Cost - ActualCardCost(card), 0);
                Card.Count = Card.Cost;

                View.Update(Cards, false);

                Visibility = Visibility.Visible;

                return true;
            }
            return false;
        }

        private static int ActualCardCost(Card card)
        {
            switch (card.Id)
            {
                case Paladin.LibramOfHope:
                case Paladin.LibramOfJudgment:
                case Paladin.LibramOfJustice:
                case Paladin.LibramOfWisdom:
                    return card.Cost - Game.Player.LibramReductionCount;
                default:
                    return card.Cost;
            }
        }
    }
}
