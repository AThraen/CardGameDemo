using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace CardGameLib
{

    /// <summary>
    /// Cardgame Thirty-One
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Id of the game
        /// </summary>
        public int GameId { get; set; }

        public Deck Deck { get; set; }

        public List<Card> Table { get; set; }

        public Player Host =>   Players.FirstOrDefault(); 

        public List<Player> Players { get; set; }

        public int CurrentTurn { get; set; }

        public GameState State { get; set; }

        private Random _random;


        [JsonIgnore]
        public Player Winner { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Game()
        {
            State = GameState.WaitingToStart;
            _random = new Random();
            GameId = _random.Next(1000, 9999);
            Players = new List<Player>();
        }

        /// <summary>
        /// Constructor that starts game right away
        /// </summary>
        /// <param name="R"></param>
        /// <param name="Players"></param>
        public Game(Random R, params Player[] Players) : this()
        {
            this.Players = Players.ToList();
            StartGame();            
        }

        /// <summary>
        /// Serializes game into Json
        /// </summary>
        /// <returns></returns>
        public string SerializeGame()
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            return JsonConvert.SerializeObject(this, jsonSerializerSettings);
        }

        /// <summary>
        /// Deserializes game from Json
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
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

        public void StartGame()
        {
            
            Deck = new Deck();
            Deck.Initialize();
            Deck.Shuffle(_random);
            Table = new List<Card>();
            InitialDeal();
            State = GameState.InProgress;
        }


    }
}
