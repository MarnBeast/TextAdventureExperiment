using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment
{
    public class CustomAction : Action
    {
        public string Handle { get; set; }
        public Action Action { get; set; }

        public CustomAction(Player player) : base(player) { }

        public override bool Execute()
        {
            if (Action == null) return false;
            else return Action.Execute();
        }
    }
}
