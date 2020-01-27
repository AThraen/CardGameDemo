using CardGameLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardGameWeb.Models
{
    public class HumanPlayer : Player
    {

        public Guid Guid { get; set; }

        public override void Turn(Game g)
        {
            //Do nothing
        }

        public HumanPlayer(string Name): base(Name)
        {
            this.Guid = Guid.NewGuid();
        }
    }
}
