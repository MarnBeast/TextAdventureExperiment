using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment.Actions
{
    class GiveAction : Action
    {

        private string m_itemName = String.Empty;

        public string ItemName
        {
            get { return m_itemName; }
            set
            {
                if (value.StartsWith("'") || value.StartsWith("\""))
                {
                    value = value.Substring(1);
                }
                if (value.EndsWith("'") || value.EndsWith("\""))
                {
                    value = value.Substring(0, value.Length - 1);
                }
                value = value.Replace("\\", String.Empty);
                m_itemName = value;
            }
        }


        private string m_toItemName = String.Empty;
        public string ToItemName
        {
            get { return m_toItemName; }
            set
            {
                if (value.StartsWith("'") || value.StartsWith("\""))
                {
                    value = value.Substring(1);
                }
                if (value.EndsWith("'") || value.EndsWith("\""))
                {
                    value = value.Substring(0, value.Length - 1);
                }
                value = value.Replace("\\", String.Empty);
                m_toItemName = value;
            }
        }

        public GiveAction(Player player) : base(player) { }


        public override bool Execute()
        {
            bool ret;
            if(String.IsNullOrWhiteSpace(ToItemName))
            {
                ret = GiveToPlayer();
            }
            else
            {
                ret = GivetoItem();
            }

            return ret;
        }

        public bool GiveToPlayer()
        {
            bool ret = false;
            if (Player.Inventory.Add(ItemName))
            {
                ret = true;
            }
            else
            {
                ret = false;
            }
            return ret;
        }

        public bool GivetoItem()
        {
            bool ret = false;
            
            Item toItem = Player.Adventure.Get(ToItemName);
            if (toItem != null)
            {
                Item item = Player.Adventure.Get(ItemName);
                if(item != null)
                {
                    item.HeldItems.Add(item);
                    ret = true;
                }
            }

            return ret;
        }
    }
}
