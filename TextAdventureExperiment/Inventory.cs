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




        public Action GetCommandAction(string commandString)
        {
            Action ret = null;

            List<Item> items = m_items.ToList<Item>();

            Command command =
                items
                .SelectMany(x => x.Commands)
                .Where(x => x.Match(commandString))
                .FirstOrDefault();

            if(command != null)
            {
                ret = GetAction(command.ActionText);
            }

            if(ret == null)
            {
                ret = new GroupAction(m_player);
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
        public Action GetAction(string actionScript, Item caller = null, params string[] replacements)
        {

            Regex regex = new Regex("('.*?[^\\\\]'|\".*?[^\\\\]\"|\\S+)");        // splits on spaces except within quotes (' or ") and allows for quotes to be escaped       // ('.*?[^\\]'|".*?[^\\]"|\S+)   unescaped regex
            string[] commands = regex.Split(actionScript).Where(s => !String.IsNullOrWhiteSpace(s)).ToArray();
            
            commands = Item.ReplaceParamMarkers(commands, replacements);

            return GetAction(commands, caller);
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
        public Action GetAction(string[] commands, Item caller = null)
        {
            GroupAction root = new GroupAction(m_player);

            if (commands != null && commands.Length > 0)
            {
                int count = 0;
                while (commands.Length > 0)
                {
                    List<Item> items = m_items.ToList<Item>();
                    if (caller != null)
                    {
                        items = items.Skip(items.IndexOf(caller) + 1).ToList<Item>();
                    }

                    foreach (Item item in items)
                    {
                        Action action = item.GetAction(m_player, ref commands);
                        if (action != null)
                        {
                            // If we just parsed a two op action, set the Op1 value
                            if (action is TwoOpAction && root.Actions.Count > 0)
                            {
                                Action op1 = root.Actions.Last();
                                root.Remove(op1);
                                TwoOpAction twoAction = action as TwoOpAction;
                                twoAction.Op1 = op1;
                            }
                            root.Add(action);
                            break;
                        }
                    }

                    if (count++ > 1000) break;  // prevent runaway while loop
                }
            }

            return root;
        }


        
        
    }
}
