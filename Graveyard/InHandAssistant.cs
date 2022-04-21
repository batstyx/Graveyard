using Hearthstone_Deck_Tracker.Hearthstone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDT.Plugins.Graveyard
{
    internal class InHandAssistant
    {
        private readonly string CardId;

        private bool IsCard(Card card) => card.Id == CardId;

        public int Count { get; private set; } = 0;
        public bool Value => Count > 0;

        public InHandAssistant(string cardId)
        {
            CardId = cardId;
        }

        // If the card is in hand, check if the card is being played 
        public bool Check(Card card)
        {
            if (Value && IsCard(card)) Count -= 1;
            return Value;
        }

        // If the card isn't already in hand, check if the card is being played to hand
        public bool IsPlayedTo(Card card)
        {
            if (!Value && IsCard(card)) Count += 1;
            return Value;
        }
    }
}
