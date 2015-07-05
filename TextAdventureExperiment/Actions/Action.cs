using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment
{
    public abstract class Action
    {
        public Player Player { get; set; }

        public Action(Player player)
        {
            Player = player;
        }

        public abstract bool Execute();
    }
}
