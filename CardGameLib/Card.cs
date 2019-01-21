using System;
using System.Collections.Generic;
using System.Text;

namespace CardGameLib
{
    public class Card
    {
        public Suits Suit { get; set; }
        public int Rank { get; set; } //1-14
        public int Value {
            get
            {
                return (Rank == 14) ? 11 : (Rank >= 10 && Rank < 14) ? 10 : Rank;
            }
        }

        public override string ToString()
        {
            return Rank.ToString() + " of " + Suit.ToString();
        }
    }



    public enum Suits
    {
        Spades,
        Hearts,
        Clubs,
        Diamonds
    }
}
