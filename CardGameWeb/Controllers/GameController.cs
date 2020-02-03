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

        

        public IActionResult Index(int Id, string PlayerId=null)
        {
            //Show main game page
            GameService gs = new GameService();
            if (!gs.GameExist(Id))
            {
                //Game does not exist, redirect
                return RedirectToAction("Index", "Home");
            }
            Game g = gs.LoadGame(Id);


            if (g.State == GameState.WaitingToStart)
            {
                return View("GameWaitingToStart", g);
            }

            GameViewModel gvm = new GameViewModel() { Game = g };
            if (g.State==GameState.GameOver) return View("GameOver", g.Winner);
            return View(gvm);
        }

        [HttpPost]
        public IActionResult PickUp(int Id, string PlayerId, PlayerAction Action)
        {
            GameService gs = new GameService();
            if (!gs.GameExist(Id))
            {
                //Game does not exist, redirect
                return RedirectToAction("Index", "Home");
            }
            Game g = gs.LoadGame(Id);

            //Check current player is the next player.

            var p = g.NextPlayer as WebPlayer;

            //Perform the action
            if (Action == PlayerAction.TakeFromTable) p.DrawFromTable(g);
            else if (Action == PlayerAction.Knock) {
                p.HasKnocked = true;
                g.NextTurn();
                if (g.State == CardGameLib.GameState.GameOver) return View("GameOver", g.Winner);
                //Compare state
            } 
            else p.DrawFromDeck(g);

            GameViewModel gvm = new GameViewModel() { Game = g };
            return View(gvm);
        }

        [HttpPost]
        public IActionResult Drop(int Id, string PlayerId, int Selection)
        {
            GameService gs = new GameService();
            if (!gs.GameExist(Id))
            {
                //Game does not exist, redirect
                return RedirectToAction("Index", "Home");
            }
            Game g = gs.LoadGame(Id);

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

            return View();
        }

        [HttpPost]
        public IActionResult SetupNewGame(GameStartOptions options)
        {
            Game g = new Game();
            if (!string.IsNullOrEmpty(options.PlayerName))
            {
                g.Players.Add(new HumanPlayer(options.PlayerName));
            }
            if (options.ComputerPlayer)
            {
                g.Players.Add(new ComputerPlayer(new Random(), "Computer"));
            }

            GameService gs = new GameService();
            gs.SaveGame(g);

            return View("GameWaitingToStart",g);
        }

        public IActionResult Join(int Id, string Name)
        {
            GameService gs = new GameService();
            if (!gs.GameExist(Id))
            {
                //Game does not exist, redirect
                return RedirectToAction("Index", "Home");
            }
            Game g = gs.LoadGame(Id);

            //Add player to game
            

            return View();
        }

        public IActionResult StartGame(int Id, string PlayerId)
        {
            return RedirectToAction("Index");
        }

    }
}