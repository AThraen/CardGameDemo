using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace CardGameLib
{

    /// <summary>
    /// Extremely simple version of 31. This time without the "Call" element, for simplicity. Simple: Only action possible: replace card - either from Table or from Deck. First to get 31 wins.
    /// </summary>

    public class Game
    {
        public Deck Deck { get; set; }

        public List<Card> Table { get; set; }

        public Player[] Players { get; set; }

        public int CurrentTurn { get; set; }

        public bool GameOver { get; set; }

        public Player Winner { get; set; }

        public Game()
        {
        }

        public string SerializeGame()
        {
            return JsonConvert.SerializeObject(this);
        }
        
        public void NextTurn()
        {
            Players[CurrentTurn].Turn();
            if (Players[CurrentTurn].Hand.CalculateScore() == 31)
            {
                Winner = Players[CurrentTurn];
                GameOver = true;
            }

            CurrentTurn++;
            if (CurrentTurn >= Players.Length) CurrentTurn = 0;
        }

        public void InitialDeal()
        {
            GameOver = false;
            Winner = null;
            CurrentTurn = 0;
            //Deal
            foreach(var p in Players)
            {
                for (int i = 0; i < 3; i++) p.Hand.Add(Deck.DrawCard());
            }
            Table.Add(Deck.DrawCard());

        }

        public Game(Random R,params Player[] Players)
        {
            this.Players = Players;
            foreach (var p in this.Players) p.Game = this; //Back reference
            Deck = new Deck();
            Deck.Shuffle(R);
            Table = new List<Card>();
            
        }
    }
}
