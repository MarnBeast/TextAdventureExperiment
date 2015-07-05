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

        public Action Op { get; set; }

        public override bool Execute()
        {
            //if (Op == null) return false;
            return !Op.Execute();
        }
    }
}
