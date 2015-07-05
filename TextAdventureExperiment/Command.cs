using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment
{
    public class Command
    {
        public String CommandText { get; set; }
        public Action Action { get; set; }
    }
}
