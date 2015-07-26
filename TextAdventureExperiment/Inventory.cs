using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TextAdventureExperiment.Actions;

namespace TextAdventureExperiment
{
    public class Inventory
    {
        private SortedSet<Item> m_items;
        private Player m_player;
        
        public Inventory(Player player)
            : base()
        {
            m_items = new SortedSet<Item>();
            m_player = player;
        }

        public void Add(Item item)
        {
            m_items.Add(item);
        }

        public bool Remove(Item item)
        {
            return m_items.Remove(item);
        }

        public bool Add(string itemName)
        {
            Item item = m_player.Adventure.Get(itemName);
            if (item != null)
            {
                m_items.Add(item);
                return true;
            }
            return false;
        }

        public int Remove(string itemName)
        {
            return m_items.RemoveWhere(item => item.Name == itemName);
        }

        public List<Item> Items
        {
            get { return m_items.ToList<Item>(); }
        }

        public bool Has(string itemName)
        {
            return m_items.Any(item => item.Name == itemName);
        }

        public bool Has(string itemName, int itemCount)
        {
            return m_items.Count(item => item.Name == itemName) >= itemCount;
        }

        public bool Has(int itemCount)
        {
            return m_items.Count() >= itemCount;
        }


        public static IEnumerable<Item> GetAllAndHeldItems(IEnumerable<Item> items)
        {
            IEnumerable<Item> allAndHeld = items            // This takes our list of items and adds all of each item's held items to it.
                .SelectMany(x => x.HeldItems)               // I call Distinct to ensure we only have 1 of each item (since the same item
                .Concat(items)                              // may be held my multiple items potentially, or we could get circular references).
                .Distinct();                                

            IEnumerable<Item> newItems = allAndHeld.Except(items);  // Only continue recursing if we got new items added to our list
            if (newItems.Count() != 0)                              
            {
                allAndHeld = GetAllAndHeldItems(allAndHeld);        // If we do have new items, we need to get their held items too!
            }

            return allAndHeld;
        }


        public bool DoCommandAction(string commandString)
        {
            bool ret = false;
            IEnumerable<Item> items = GetAllAndHeldItems(m_items);  // Available commands should be all items the player has AND all items IN items that the player has.

            Command command =
                items
                .OrderBy(x => x)
                .SelectMany(x => x.Commands)
                .Where(x => x.Match(commandString))
                .FirstOrDefault();

            if(command != null)
            {
                ret = DoActions(command.ActionText);
            }

            return ret;
        }

        /// <summary>
        /// Builds an Action based on the scripted actionText.
        /// </summary>
        /// <param name="player">
        /// The player trying to execute the action.</param>
        /// <param name="caller">The item that called this method. This allows the method to execute from that items perspective,
        /// only getting actions from items below it in the inventory. Passing null uses the entire inventory.</param>
        /// <param name="actionScript">A string containing the action script. This should contain quoted strings, custom action handles, 
        /// and or base action handles (SAY, GIVE, IF, etc). It may also include {#} if replacements are provided, where # is a number
        /// indicating the order in which the the replacements should be used.</param>
        /// <param name="replacements">Replacement strings that should be used in place of the {#} markers.</param>
        /// <returns>The action built from the action script.</returns>
        public bool DoActions(string actionScript, Item caller = null, params string[] replacements)
        {

            Regex regex = new Regex("('.*?[^\\\\]'|\".*?[^\\\\]\"|\\S+)");        // splits on spaces except within quotes (' or ") and allows for quotes to be escaped       // ('.*?[^\\]'|".*?[^\\]"|\S+)   unescaped regex
            string[] commands = regex.Split(actionScript).Where(s => !String.IsNullOrWhiteSpace(s)).ToArray();
            
            commands = Item.ReplaceParamMarkers(commands, replacements);

            return DoActions(commands, caller);
        }

        /// <summary>
        /// Builds an Action based on the scripted actionText.
        /// </summary>
        /// <param name="player">
        /// The player trying to execute the action.</param>
        /// <param name="caller">The item that called this method. This allows the method to execute from that items perspective,
        /// only getting actions from items below it in the inventory. Passing null uses the entire inventory.</param>
        /// <param name="commands">A string of action commands to be parsed in order. This should contain quoted strings, custom action handles, 
        /// and or base action handles (SAY, GIVE, IF, etc). It may also include {#} if replacements are provided, where # is a number
        /// indicating the order in which the the replacements should be used.</param>
        /// <returns>The action built from the commands array.</returns>
        public bool DoActions(string[] commands, Item caller = null)
        {
            bool lastAction = true;

            if (commands != null && commands.Length > 0)
            {
                int count = 0;
                while (commands.Length > 0)
                {
                    DoAction(ref commands, ref lastAction, caller);

                    if (count++ > 1000) break;  // prevent runaway while loop
                }
            }

            return lastAction;
        }

        
        public bool DoAction(ref string[] commands, ref bool lastAction, Item caller = null)
        {
            List<Item> items = m_items.ToList<Item>();

            bool actionPerformed = false;                           // Available custom actions should only be items the player has and items in the caller item.
            if (caller != null)                                     // Items held by items in the players inventory that are not the caller item should not be included.
            {
                actionPerformed = caller.DoAction(m_player, ref commands, ref lastAction);
                if (!actionPerformed)
                {
                    foreach(Item item in caller.HeldItems)
                    {
                        actionPerformed = caller.DoAction(m_player, ref commands, ref lastAction);
                        if(actionPerformed)
                        {
                            break;
                        }
                    }
                    if(!actionPerformed)
                    {
                        items = items.Skip(items.IndexOf(caller) + 1).ToList<Item>();           // if caller is index 2 (meaning 0, 1, 2 so the third in line) skip 3 will skip the caller.
                    }
                }
            }

            if (!actionPerformed)
            {
                foreach (Item item in items)
                {
                    actionPerformed = item.DoAction(m_player, ref commands, ref lastAction);
                    if (actionPerformed)
                    {
                        break;
                    }
                }
            }

            return actionPerformed;
        }

        
        
    }
}
