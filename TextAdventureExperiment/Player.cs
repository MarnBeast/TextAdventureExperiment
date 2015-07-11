using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAdventureExperiment.Actions;
using TextAdventureExperiment.IO;

namespace TextAdventureExperiment
{
    public class Player
    {
        public Inventory Inventory { get; set; }

        public IOManager IO { get; set; }


        public Player() : this(null) { }

        public Player(IOManager io)
        {
            Inventory = new Inventory(this);
            IO = io;
            if(IO == null)
            {
                IO = new ConsoleIOManager();
            }
        }


        public Adventure Adventure { get; set; }

        public void BeginAdventure(Adventure adventure)
        {
            Adventure = adventure;
            adventure.Start(this);
        }
    }
}
