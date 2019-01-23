using CardGameLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardGameWeb.Business
{
    public class WebPlayer : Player
    {
        public bool PickFromDeck { get; set; }

        public int WhichToDrop { get; set; }

        public override void Turn(Game g)
        {
            //Do nothing
        }
    }
}
