using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardGameLib;
using CardGameWeb.Business;
using CardGameWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CardGameWeb.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Index()
        {
            //Start new game
            Random R = new Random();
            Game g = new Game(R, new ComputerPlayer(R, "Computer"), new WebPlayer() { Name = "You" });
            g.InitialDeal();
            if (g.NextPlayer is ComputerPlayer) g.NextTurn();

            GameViewModel gvm = new GameViewModel() { Game = g };
            if (g.State==GameState.GameOver) return View("GameOver", g.Winner);
            return View(gvm);
        }

        [HttpPost]
        public IActionResult PickUp(string GameState, string Source)
        {
            Game g = Game.DeserializeGame(GameState);
            var p = g.NextPlayer as WebPlayer;
            if (Source == "Table") p.DrawFromTable(g);
            else if (Source == "Knock") {
                p.HasKnocked = true;
                g.NextTurn();

                //Compare state
            } 
            else p.DrawFromDeck(g);

            GameViewModel gvm = new GameViewModel() { Game = g };
            return View(gvm);
        }

        [HttpPost]
        public IActionResult Drop(string GameState, int Selection)
        {
            Game g = Game.DeserializeGame(GameState);
            var p = g.NextPlayer as WebPlayer;
            p.DropCard(g, Selection);
            g.NextTurn();
            if (g.State==CardGameLib.GameState.GameOver) return View("GameOver", g.Winner);
            g.NextTurn();
            if (g.State==CardGameLib.GameState.GameOver) return View("GameOver", g.Winner);

            GameViewModel gvm = new GameViewModel() { Game = g };
            return View("Index", gvm);
        }

        /// <summary>
        /// Starts a new game
        /// </summary>
        /// <returns></returns>
        public IActionResult New()
        {
            //Shows Options screen before start. Your name, Cast to screen, Add computer player

            return View(g);
        }

        [HttpPost]
        public IActionResult SetupNewGame(string PlayerName, bool ComputerPlayer)
        {

            Game g = new Game();
            if (!string.IsNullOrEmpty(PlayerName))
            {
                g.Players.Add(new HumanPlayer(PlayerName));
            }
            if (ComputerPlayer)
            {
                g.Players.Add(new ComputerPlayer(new Random(), "Computer"));
            }
            return View("GameWaitingToStart",g);
        }

        public IActionResult Join(int GameId, string Name)
        {
            return View();
        }

    }
}