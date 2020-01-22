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
            if (g.GameOver) return View("GameOver", g.Winner);
            return View(gvm);
        }

        [HttpPost]
        public IActionResult PickUp(string GameState, string Source)
        {
            Game g = Game.DeserializeGame(GameState);
            var p = g.NextPlayer as WebPlayer;
            if (Source == "Table") p.DrawFromTable(g);
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
            if (g.GameOver) return View("GameOver", g.Winner);
            g.NextTurn();
            if (g.GameOver) return View("GameOver", g.Winner);

            GameViewModel gvm = new GameViewModel() { Game = g };
            return View("Index", gvm);
        }

        public IActionResult New()
        {
            return View();
        }

        public IActionResult Join()
        {
            return View();
        }

    }
}