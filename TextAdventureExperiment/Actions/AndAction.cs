using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment.Actions
{
    class AndAction : TwoOpAction
    {
        public AndAction(IOManager io) : base(io) { }

        public override bool Execute()
        {
            bool op1 = false;
            bool op2 = false;

            if(Op1 != null)
            {
                op1 = Op1.Execute();
            }

            if(Op2 != null)
            {
                op2 = Op2.Execute();
            }

            return op1 && op2;
        }
    }
}
