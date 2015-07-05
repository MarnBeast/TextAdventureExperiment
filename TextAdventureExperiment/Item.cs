using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment
{
    public class Item
    {
        public bool Hidden { get; set; }
        public List<Command> Commands { get; set; }
        public List<CustomAction> CustomActions { get; set; }

        public Item()
        {

        }


    }
}
