using CardGameLib;
using CardGameWeb2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CardGameWeb2.Controllers
{
    public class GameController : Controller
    {
        
        public ActionResult Index()
        {
            Game g = (Game)Session["Game"];
            if (g == null)
            {
                //Start a new game
                Random r = new Random();
                g = new Game(r, new ComputerPlayer(r, "Computer"));
                Session["Game"] = g;
            }

            GameViewModel gvm = new GameViewModel() { CurrentGame = g };
            return View(gvm);
        }
    }
}