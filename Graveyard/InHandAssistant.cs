using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Utility.Logging;
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

        public bool Check(Card card)
        {
            if (IsCard(card)) Count -= 1;
            if (Value) Log.Info($"{Database.GetCardFromId(CardId).Name} still in hand");
            return Value;
        }

        public bool Incoming(Card card)
        {
            if (IsCard(card)) Count += 1;
            if (Value) Log.Info($"{Database.GetCardFromId(CardId).Name} played to hand");
            return Value;
        }
    }
}
