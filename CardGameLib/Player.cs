using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace CardGameLib
{


    public abstract class Player
    {
        public List<Card> Hand { get; set; }
        public string Name { get; set; }

        public bool HasKnocked { get; set; }

        public abstract void Turn(Game g); 
        

        public Player()
        {
            Hand = new List<Card>();
            HasKnocked = false;
        }

        public Player(string Name) : this()
        {
            this.Name = Name;
        }

        public void DrawFromDeck(Game g)
        {
            Hand.Add(g.Deck.DrawCard());
        }

        public void DrawFromTable(Game g)
        {
            Hand.Add(g.Table[g.Table.Count - 1]);
            g.Table.RemoveAt(g.Table.Count - 1);
        }

        public void DropCard(Game g,int idx)
        {
            g.Table.Add(Hand[idx]);
            Hand.RemoveAt(idx);
        }
    }



    public class ComputerPlayer : Player
    {
        private Random R;

        public event Action<Card> Done;

        public ComputerPlayer(Random R, string Name) : base(Name)
        {
            this.R = R;
        }

        public override void Turn(Game g)
        {
            DrawFromDeck(g); //Unless drawing from table will give a higher score than currently

            //Drop card that'll give highest score
            List<Tuple<Card, int>> lst = new List<Tuple<Card, int>>();
            foreach(var c in Hand)
            {
                lst.Add(new Tuple<Card, int>(c,Hand.Except(new Card[] { c }).CalculateScore()));
            }
            int idx = Hand.IndexOf(lst.OrderByDescending(l => l.Item2).First().Item1);
            DropCard(g,idx);
            //Invoking Done event with the card we dropped
            Done?.Invoke(lst.OrderByDescending(l => l.Item2).First().Item1);
        }
    }
}
