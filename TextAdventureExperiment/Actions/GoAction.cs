using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment.Actions
{
    class GoAction : Action
    {

        private string m_giveItemName = String.Empty;

        public string GiveItemName
        {
            get { return m_giveItemName; }
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
                m_giveItemName = value;
            }
        }

        private string m_takeItemName = String.Empty;

        public string TakeItemName
        {
            get { return m_takeItemName; }
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
                m_takeItemName = value;
            }
        }

        public GoAction(Player player) : base(player) { }




        // GIVE and TAKE must be executed while getting actions, otherwise subsequent actions performed by the given item
        // won't be found.
        private bool executedWhenCreated = false;

        public bool ExecuteWhenCreated()
        {
            Item item = Player.Adventure.Get(GiveItemName);
            if (item != null)
            {
                Player.Inventory.Add(item); 
                Player.Inventory.Remove(TakeItemName);
                executedWhenCreated = true;
            }
            else
            {
                executedWhenCreated = false;
            }

            return executedWhenCreated;
        }

        public override bool Execute()
        {
            return executedWhenCreated;
        }
    }
}
