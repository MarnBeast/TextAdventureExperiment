using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment
{
    public abstract class Action
    {
        public IOManager IO { get; set; }

        public Action(IOManager io)
        {
            IO = io;
        }

        public abstract bool Execute();
    }
}
