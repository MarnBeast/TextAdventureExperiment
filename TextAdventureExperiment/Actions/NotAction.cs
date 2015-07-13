using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment.Actions
{
    class NotAction : Action
    {
        public NotAction(Player player)
            : base(player)
        { }

        public bool Op { get; set; }

        public override bool Execute()
        {
            return !Op;
        }
    }
}
