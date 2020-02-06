using CardGameLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardGameWeb.Models
{
    public class GameViewModel
    {
        public Game Game { get; set; }

        public HumanPlayer You { get; set; }

        public string Message { get; set; }

    }
}
