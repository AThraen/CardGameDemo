using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CardGameLib
{


    public abstract class Player
    {
        public List<Card> Hand { get; set; }
        public string Name { get; set; }

        public abstract void Turn(); //0,1,2 = replace card

        public Game Game { get; set; }

        public Player()
        {
            Hand = new List<Card>();
        }

        public Player(string Name) : this()
        {
            this.Name = Name;
        }

        protected void DrawFromDeck()
        {
            Hand.Add(Game.Deck.DrawCard());
        }

        protected void DrawFromTable()
        {
            Hand.Add(Game.Table[Game.Table.Count - 1]);
            Game.Table.RemoveAt(Game.Table.Count - 1);
        }

        protected void DropCard(int idx)
        {
            Game.Table.Add(Hand[idx]);
            Hand.RemoveAt(idx);
        }
    }



    public class ComputerPlayer : Player
    {
        private Random R;


        public ComputerPlayer(Random R, string Name) : base(Name)
        {
            this.R = R;
        }

        public override void Turn()
        {
            DrawFromDeck();
            //Drop card that'll give highest score
            List<Tuple<Card, int>> lst = new List<Tuple<Card, int>>();
            foreach(var c in Hand)
            {
                lst.Add(new Tuple<Card, int>(c,Hand.Except(new Card[] { c }).CalculateScore()));
            }
            int idx = Hand.IndexOf(lst.OrderByDescending(l => l.Item2).First().Item1);
            DropCard(idx);
        }
    }
}
