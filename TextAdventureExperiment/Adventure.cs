using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TextAdventureExperiment.Items;

namespace TextAdventureExperiment
{
    [XmlRoot]
    public class Adventure
    {
        [XmlElement]
        private List<Item> m_items;         // Maybe we should change this to dictionary in the future and watch for Item name changes. For now, list works.
        private Item m_coreItem;

        public Player Player { get; set; }

        public Adventure()
        {
            m_items = new List<Item>();
            m_coreItem = new CoreItem();
            Add(m_coreItem);
        }

        [XmlAttribute]
        public string StartScript { get; set; }

        
        public void Start(Player player)
        {
            Player = player;
            var core = Get(CoreItem.NAME);
            player.Inventory.Add(core);
            player.Adventure = this;

            player.Inventory.DoActions(StartScript);
            while(true)
            {
                string command = player.IO.Read();
                if(command.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                player.Inventory.DoCommandAction(command);
            }
        }


        public void Add(Item item)
        {
            if(m_items.Contains(item))
            {
                Remove(item);
            }

            m_items.Add(item);

            int index = m_items.IndexOf(item);
            item.AdventureID = index;
        }


        public void Insert(int index, Item item)
        {
            if(m_items.Contains(item))
            {
                Remove(item);
            }

            m_items.Insert(index, item);
            UpdateAdventureIDs();
        }


        public bool Remove(Item item)
        {
            var ret = false;
            if (item != m_coreItem)
            {
                ret = m_items.Remove(item);
                UpdateAdventureIDs();
            }

            return ret;
        }


        public Item Get(string itemName)
        {
            return m_items.FirstOrDefault(x => x.Name == itemName);
        }


        private void UpdateAdventureIDs()
        {
            foreach(Item item in m_items)
            {
                int index = m_items.IndexOf(item);
                item.AdventureID = index;
            }
        }
    }
}
