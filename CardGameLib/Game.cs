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
        public int GameId { get; set; }

        public Deck Deck { get; set; }

        public List<Card> Table { get; set; }

        public Player Host =>   Players.First(); 

        public List<Player> Players { get; set; }

        public int CurrentTurn { get; set; }

        public GameState State { get; set; }


        [JsonIgnore]
        public Player Winner { get; set; }

        public Game()
        {
            State = GameState.WaitingToStart;
        }

        public string SerializeGame()
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            return JsonConvert.SerializeObject(this, jsonSerializerSettings);
        }

        public static Game DeserializeGame(string s)
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            return JsonConvert.DeserializeObject<Game>(s,jsonSerializerSettings);
        }

        [JsonIgnore]
        public Player NextPlayer
        {
            get
            {
                return Players[CurrentTurn];
            }
        }
        
        private void GameOver()
        {
            //Either the current player has 31 or he has knocked
            if (NextPlayer.HasKnocked)
            {
                //Identify winner
            } else
            {
                //Winner from 31
            }
        }

        public void NextTurn()
        {
            NextPlayer.Turn(this);

            if (NextPlayer.Hand.CalculateScore() == 31)
            {
                Winner = Players[CurrentTurn];
                GameOver();
            }


            CurrentTurn++;
            if (CurrentTurn >= Players.Count) CurrentTurn = 0;
            if (NextPlayer.HasKnocked)
            {
                //Back to the player that had knocked. Let's evalutae scores.
                GameOver();
            }

            if (Deck.CardsLeft == 0)
            {
                //If there's no more cards in the deck, let's take those from the table
                Deck.Cards.AddRange(Table);
                Table.Clear();
            }


        }

        public void InitialDeal()
        {
            Winner = null;
            CurrentTurn = 0;
            //Deal
            foreach(var p in Players)
            {
                for (int i = 0; i < 3; i++) p.Hand.Add(Deck.DrawCard());
            }
            Table.Add(Deck.DrawCard());

            State = GameState.InProgress;

        }

        public Game(Random R,params Player[] Players)
        {
            this.Players = Players.ToList();
            Deck = new Deck();
            Deck.Initialize();
            Deck.Shuffle(R);
            Table = new List<Card>();
            State = GameState.WaitingToStart;
            
        }
    }
}
