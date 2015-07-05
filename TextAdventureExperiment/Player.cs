using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAdventureExperiment.IO;

namespace TextAdventureExperiment
{
    public class Player
    {
        public List<Item> Inventory { get; set; }

        public IOManager IO { get; set; }


        public Player() : this(null) { }

        public Player(IOManager io)
        {
            IO = io;
            if(IO == null)
            {
                IO = new ConsoleIOManager();
            }
        }

        /// <summary>
        /// When passed an item, this method returns all custom actions included within and below this item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public List<KeyValuePair<String, Action>> GetCustomActions(Item item)
        {
            return null;
        }
    }
}
