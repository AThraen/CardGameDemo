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

        [JsonIgnore]
        public Player Winner { get; set; }

        public Game()
        {
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
        
        public void NextTurn()
        {
            NextPlayer.Turn(this);
            if (NextPlayer.Hand.CalculateScore() == 31)
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
            Deck = new Deck();
            Deck.Initialize();
            Deck.Shuffle(R);
            Table = new List<Card>();
            
        }
    }
}
