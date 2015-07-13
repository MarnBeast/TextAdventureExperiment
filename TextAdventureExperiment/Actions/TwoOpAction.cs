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
            }
        }

        public bool Op1 { get; set; }
        public bool Op2 { get; set; }


        public TwoOpAction(Player player) : base(player) { }
    }
}
