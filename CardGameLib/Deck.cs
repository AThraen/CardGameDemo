using System;
using System.Collections.Generic;
using System.Text;

namespace CardGameLib
{
    public class Deck
    {
        private List<Card> Cards { get; set; }

        public int CardsLeft {
            get
            {
                return Cards.Count;
            }
        }

        private void Initialize()
        {
            foreach(Suits suit in (Suits[])Enum.GetValues(typeof(Suits)))
            {
                for(int i = 1; i < 14; i++)
                {
                    Card c = new Card() { Rank = i, Suit = suit };
                    Cards.Add(c);
                }
            }
        }

        public Card DrawCard()
        {
            if (Cards.Count == 0) return null;
            Card c = Cards[0];
            Cards.RemoveAt(0);
            return c;
        }

        public void Shuffle(Random R)
        {
            for(int i = 0; i < 20000; i++)
            {
                int from = R.Next(Cards.Count);
                int to = R.Next(Cards.Count);
                Card c = Cards[to];
                Cards[to] = Cards[from];
                Cards[from] = c;
            }
        }

        public Deck()
        {
            Cards = new List<Card>();
            Initialize();
        }
    }
}
