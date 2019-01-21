using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CardGameLib
{

    /// <summary>
    /// Extremely simple version of 31. This time without the "Knock" element, for simplicity. Simple: Only action possible: replace card. First to get 31 wins.
    /// </summary>

    public class Game
    {
        public Deck Deck { get; set; }

        public List<Card> Table { get; set; }

        public Player[] Players { get; set; }

        public int CurrentTurn { get; set; }

        public Game()
        {
        }

        public void Play()
        {
            CurrentTurn = 0;
            //Deal
            foreach(var p in Players)
            {
                for (int i = 0; i < 3; i++) p.Hand.Add(Deck.DrawCard());
            }
            Table.Add(Deck.DrawCard());

            //Play the game!
            string winner = null;
            while (winner == null)
            {
                Console.WriteLine($"{Players[CurrentTurn].Name} turn!");
                Players[CurrentTurn].Turn();
                if (Players[CurrentTurn].Hand.CalculateScore() == 31)
                {
                    winner = Players[CurrentTurn].Name;
                }
                CurrentTurn++;
                if (CurrentTurn >= Players.Length) CurrentTurn = 0;
            }
            Console.WriteLine($"--- GAME OVER, {winner} WON! ---");
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
