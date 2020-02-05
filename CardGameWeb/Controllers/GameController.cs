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

            ViewBag.PlayerId = PlayerId;
            
            

            if (g.State == GameState.WaitingToStart)
            {
                //Check if the current player is the host
                if ((g.Host as HumanPlayer).Guid.ToString() == PlayerId)
                {
                    ViewBag.IsHost = true;
                }
                else ViewBag.Ishost = false;
                return View("GameWaitingToStart", g);
            }

            GameViewModel gvm = new GameViewModel() { Game = g, You=g.Players.Where(p => p is HumanPlayer && (p as HumanPlayer).Guid.ToString()==PlayerId).Cast<HumanPlayer>().First()};


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
            HumanPlayer hp = new HumanPlayer(options.PlayerName);
 
            hp = new HumanPlayer(options.PlayerName);
            g.Players.Add(hp);
            
            if (options.ComputerPlayer)
            {
                g.Players.Add(new ComputerPlayer(new Random(), "Computer"));
            }

            GameService gs = new GameService();
            gs.SaveGame(g);

            return RedirectToAction("Index", new { Id = g.GameId, PlayerId = hp.Guid.ToString() });
        }

        [HttpPost]
        public IActionResult Join(int GameId, string Name)
        {
            GameService gs = new GameService();
            if (!gs.GameExist(GameId))
            {
                //Game does not exist, redirect
                return RedirectToAction("Index", "Home");
            }
            Game g = gs.LoadGame(GameId);

            if (g != null && g.State == GameState.WaitingToStart)
            {
                //Add player to game
                var Player = new HumanPlayer(Name);
                g.Players.Add(Player);

                gs.SaveGame(g);
                return RedirectToAction("Index", new { Id = g.GameId, PlayerId = Player.Guid.ToString() });
            }
            return View();
            
        }

        public IActionResult StartGame(int Id, string PlayerId)
        {
            GameService gs = new GameService();
            if (!gs.GameExist(Id))
            {
                //Game does not exist, redirect
                return RedirectToAction("Index", "Home");
            }
            Game g = gs.LoadGame(Id);

            g.StartGame();
            g.NextTurn();
            //TODO: Check state / winner

            gs.SaveGame(g);


            return RedirectToAction("Index", new { Id=Id, PlayerId=PlayerId});
        }

    }
}