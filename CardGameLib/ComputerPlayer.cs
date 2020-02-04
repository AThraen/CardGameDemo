using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGameLib
{

    /// <summary>
    /// Computer AI Player
    /// </summary>
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
            foreach (var c in Hand)
            {
                lst.Add(new Tuple<Card, int>(c, Hand.Except(new Card[] { c }).CalculateScore()));
            }
            int idx = Hand.IndexOf(lst.OrderByDescending(l => l.Item2).First().Item1);
            DropCard(g, idx);

            //Invoking Done event with the card we dropped
            Done?.Invoke(lst.OrderByDescending(l => l.Item2).First().Item1);
        }
    }
}
