using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment.Actions
{
    abstract class TwoOpAction : Action
    {
        public new Player Player
        {
            get
            {
                return base.Player;
            }
            set
            {
                base.Player = value;
                if (Op1 != null) Op1.Player = value;
                if (Op2 != null) Op2.Player = value;
            }
        }


        public TwoOpAction(Player player) : base(player) { }

        public Action Op1 { get; set; }
        public Action Op2 { get; set; }
    }
}
