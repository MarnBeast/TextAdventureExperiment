using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment.Actions
{
    class AndAction : TwoOpAction
    {
        public AndAction(Player player) : base(player) { }

        public override bool Execute()
        {
            return Op1 && Op2;
        }
    }
}
