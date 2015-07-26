using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment.Actions
{
    class HasAction : Action
    {

        private int m_itemCount = -1;

        public int ItemCount
        {
            get { return m_itemCount; }
            set { m_itemCount = value; }
        }


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

        private string m_inItemName = String.Empty;
        public string InItemName
        {
            get { return m_inItemName; }
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
                m_inItemName = value;
            }
        }

        public HasAction(Player player) : base(player) { }


        public override bool Execute()
        {
            bool ret = false;
            if (String.IsNullOrWhiteSpace(InItemName))
            {
                PlayerHas();
                int x = 5;
                Int32 y = (Int32) 5;
            }
            else
            {
                ItemHas();
            }

            return ret;
        }

        private bool PlayerHas()
        {
            bool ret = false;
            if (ItemCount >= 0)
            {
                if (!String.IsNullOrWhiteSpace(ItemName))
                {
                    ret = Player.Inventory.Has(ItemName, ItemCount);
                }
                else
                {
                    ret = Player.Inventory.Has(ItemCount);
                }
            }
            else
            {
                ret = Player.Inventory.Has(ItemName);
            }

            return ret;
        }

        private bool ItemHas()
        {
            bool ret = false;
            Item inItem = Player.Adventure.Get(InItemName);
            if (inItem != null)
            {
                if (ItemCount >= 0)
                {
                    if (!String.IsNullOrWhiteSpace(ItemName))
                    {
                        ret = inItem.HeldItems.Count(item => item.Name == ItemName) >= ItemCount;
                    }
                    else
                    {
                        ret = inItem.HeldItems.Count() >= ItemCount;
                    }
                }
                else
                {
                    ret = inItem.HeldItems.Any(item => item.Name == ItemName);
                }
            }
            return ret;
        }
    }
}
