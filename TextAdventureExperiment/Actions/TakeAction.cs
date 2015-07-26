using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment.Actions
{
    class TakeAction : Action
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

        private string m_fromItemName = String.Empty;

        public string FromItemName
        {
            get { return m_fromItemName; }
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
                m_fromItemName = value;
            }
        }
        
        public TakeAction(Player player) : base(player) { }


        public override bool Execute()
        {
            bool ret;
            if (String.IsNullOrWhiteSpace(FromItemName))
            {
                ret = TakeFromPlayer();
            }
            else
            {
                ret = TakeFromItem();
            }

            return ret;
        }

        public bool TakeFromPlayer()
        {
            bool ret = false;
            int removed = Player.Inventory.Remove(ItemName);
            if (removed > 0) ret = true;
            else ret = false;
            return ret;
        }

        public bool TakeFromItem()
        {
            bool ret = false;
            Item fromItem = Player.Adventure.Get(FromItemName);
            if (fromItem != null)
            {
                int removed = fromItem.HeldItems.RemoveWhere(item => item.Name == ItemName);
                if (removed > 0) ret = true;
                else ret = false;
            }
            return ret;
        }
    }
}
