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

        private readonly GameService _gameService;

        public GameController()
        {
            _gameService = new GameService();
        }

        public IActionResult Index(int Id, string PlayerId=null)
        {
            //Show main game page
            if (!_gameService.GameExist(Id))
            {
                //Game does not exist, redirect
                return RedirectToAction("Index", "Home");
            }
            Game g = _gameService.LoadGame(Id);

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
            } else if (g.State == GameState.GameOver)
            {
                return RedirectToAction("GameOver", new { Id = Id });
            }

            //Check if we are in the middle of a step
            HumanPlayer you = g.Players.Where(p => p is HumanPlayer && (p as HumanPlayer).Guid.ToString() == PlayerId).Cast<HumanPlayer>().First();
            GameViewModel gvm = new GameViewModel() { Game = g, You = you };
            if (g.CurrentPlayer.Hand.Count>3 && g.CurrentPlayer == you)
            {
                return View("PickUp", gvm);
            }

            return View(gvm);
        }



        public IActionResult PickUp(int Id, string PlayerId, PlayerAction PlayerAction)
        {
            if (!_gameService.GameExist(Id))
            {
                //Game does not exist, redirect
                return RedirectToAction("Index", "Home");
            }
            Game g = _gameService.LoadGame(Id);

            //Check current player is the next player.
            var p = g.CurrentPlayer as HumanPlayer;

            GameViewModel gvm = new GameViewModel() { Game = g, You = g.Players.Where(pp => pp is HumanPlayer && (pp as HumanPlayer).Guid.ToString() == PlayerId).First() as HumanPlayer };

            //Perform the action
            if (PlayerAction == PlayerAction.TakeFromTable)
            {
                p.DrawFromTable(g);
                p.LastAction = $"picked up {p.Hand.Last().ToString()} from the table";
            }
            else if (PlayerAction == PlayerAction.Knock)
            {
                p.HasKnocked = true;
                p.LastAction = "knocked / called";
                if(g.NextTurn()) return View("GameOver", g);
                //Compare state
                _gameService.SaveGame(g);
                return RedirectToAction("Index", new { Id = g.GameId, PlayerId = PlayerId });
            }
            else
            {
                p.DrawFromDeck(g);
                p.LastAction = " picked up a card from the deck ";
            }

            _gameService.SaveGame(g);
            return View(gvm);
        }

        /// <summary>
        /// Dropping a card typically completes the players turn
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="PlayerId"></param>
        /// <param name="Selection"></param>
        /// <returns></returns>
        public IActionResult Drop(int Id, string PlayerId, int Selection)
        {
            if (!_gameService.GameExist(Id))
            {
                //Game does not exist, redirect
                return RedirectToAction("Index", "Home");
            }
            Game g = _gameService.LoadGame(Id);

            var p = g.CurrentPlayer as HumanPlayer;
            p.LastAction = $"dropped {p.Hand[Selection].ToString()}";
            p.DropCard(g, Selection);

            //Done with the turn - complete the turn
            if (g.NextTurn())
            {
                _gameService.SaveGame(g);
                return RedirectToAction("GameOver", new { Id = Id });
            }

            _gameService.SaveGame(g);
            GameViewModel gvm = new GameViewModel() { Game = g };
            return RedirectToAction("Index", new { Id = g.GameId, PlayerId = PlayerId });
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
            //Verify game ID is unique

            HumanPlayer hp = new HumanPlayer(options.PlayerName);
 
            hp = new HumanPlayer(options.PlayerName);
            g.Players.Add(hp);
            
            if (options.ComputerPlayer)
            {
                g.Players.Add(new ComputerPlayer("Computer"));
            }

            _gameService.SaveGame(g);

            return RedirectToAction("Index", new { Id = g.GameId, PlayerId = hp.Guid.ToString() });
        }


        public IActionResult Join(int GameId, string Name)
        {
            if (!_gameService.GameExist(GameId))
            {
                //Game does not exist, redirect
                return RedirectToAction("Index", "Home");
            }
            Game g = _gameService.LoadGame(GameId);

            if (g != null && g.State == GameState.WaitingToStart)
            {
                //Add player to game
                var Player = new HumanPlayer(Name);
                g.Players.Add(Player);

                _gameService.SaveGame(g);
                return RedirectToAction("Index", new { Id = g.GameId, PlayerId = Player.Guid.ToString() });
            }
            return View();
            
        }

        public IActionResult GameOver(int Id)
        {
            if (!_gameService.GameExist(Id))
            {
                //Game does not exist, redirect
                return RedirectToAction("Index", "Home");
            }
            Game g = _gameService.LoadGame(Id);

            return View(g);
        }

        public IActionResult StartGame(int Id, string PlayerId)
        {
            if (!_gameService.GameExist(Id))
            {
                //Game does not exist, redirect
                return RedirectToAction("Index", "Home");
            }
            Game g = _gameService.LoadGame(Id);

            g.StartGame();
            //TODO: Check state / winner

            _gameService.SaveGame(g);


            return RedirectToAction("Index", new { Id=Id, PlayerId=PlayerId});
        }

    }
}