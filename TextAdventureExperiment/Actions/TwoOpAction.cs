using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment.Actions
{
    abstract class TwoOpAction : Action
    {
        public TwoOpAction(IOManager op) : base(op) { }

        public Action Op1 { get; set; }
        public Action Op2 { get; set; }
    }
}
