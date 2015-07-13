using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TextAdventureExperiment
{
    public class Item : IComparable<Item>
    {
        private const string PARAM = "\\{[0-9]+\\}";

        [XmlAttribute]
        private string m_name;
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        private string m_displayName;
        [XmlAttribute]
        public string DisplayName
        {
            get 
            {
                if (m_displayName != null)
                    return m_displayName;
                else return Name;
            }
            set { m_displayName = value; }
        }

        public int AdventureID { get; set; }

        [XmlAttribute]
        public bool Hidden { get; set; }
        


        [XmlElement]
        public List<Command> Commands { get; private set; }
        [XmlElement]
        public List<CustomAction> CustomActions { get; private set; }


        public Item() 
        {
            Commands = new List<Command>();
            CustomActions = new List<CustomAction>();
        }

        public Item(string name, bool hidden) : this()
        {
            Name = name;
            Hidden = hidden;
        }

        /// <summary>
        /// The item looks at the command array and try's to build an action from the first command strings
        /// in the array. If it succeeds, it skips the commands array past the commands that it parsed and
        /// returns the built action.
        /// </summary>
        /// <param name="player">
        /// The player trying to execute the action.</param>
        /// <param name="commands">A string of action commands to be parsed in order. This should contain quoted strings, custom action handles, 
        /// and or base action handles (SAY, GIVE, IF, etc).</param>
        /// <returns>The action built from the commands array.</returns>
        virtual public bool DoAction(Player player, ref string[] commands, ref bool lastAction)
        {
            bool ret = false;

            if (commands != null && commands.Length > 0)
            {
                foreach(CustomAction customAction in CustomActions)
                {
                    if (customAction.Handle.Equals(commands[0], StringComparison.OrdinalIgnoreCase))
                    {
                        int paramCount = CountParamMarkers(customAction.ActionText);
                        commands = commands.Skip(1).ToArray();
                        lastAction = player.Inventory.ParseActions(
                            customAction.ActionText, 
                            this, 
                            commands.Take(paramCount).ToArray());
                        ret = true;
                        break;
                    }
                }
            }

            return ret;
        }

        public void AddCommand(string command, string actionText)
        {
            Commands.Add(
                new Command()
                {
                    CommandText = command, 
                    ActionText = actionText
                });
        }

        public void AddCustomAction(string handle, string actionText)
        {
            CustomActions.Add(
                new CustomAction()
                {
                    Handle = handle,
                    ActionText = actionText
                });
        }


        public int CompareTo(Item other)
        {
            return other.AdventureID - this.AdventureID;
        }


        private static int CountParamMarkers(string actionText)
        {
            var regex = new Regex(PARAM);        // look for things matching {0} {1} etc.
            var matches = regex.Matches(actionText);
            List<string> uniqueParamMarkers = new List<string>();

            foreach(Match match in matches)
            {
                string paramMarker = match.Value;
                if(!uniqueParamMarkers.Contains(paramMarker))
                {
                    uniqueParamMarkers.Add(paramMarker);
                }
            }

            return uniqueParamMarkers.Count;
        }

        public static string[] ReplaceParamMarkers(string[] commands, string[] replacements)
        {
            Regex regex = new Regex(PARAM);        // look for things matching {0} {1} etc.
            List<KeyValuePair<int, int>> indexes = new List<KeyValuePair<int, int>>();     // int, int => commands index, replacements index
            for (int i = 0; i < commands.Length; i++)
            {
                var match = regex.Match(commands[i]);
                if (match.Success)
                {
                    int repIndex;
                    if (Int32.TryParse(match.Result("$1"), out repIndex))
                    {
                        indexes.Add(new KeyValuePair<int, int>(i, repIndex));
                    }
                }
            }

            int currentRepIndex = 0;
            int lastRepIndex = 0;
            int actualRepIndex = 0;
            bool firstIndex = true;
            foreach (KeyValuePair<int, int> kvp in indexes.OrderBy(x => x.Value))
            {
                currentRepIndex = kvp.Value;
                if (lastRepIndex != currentRepIndex && !firstIndex)
                {
                    actualRepIndex++;
                }
                firstIndex = false;
                commands[kvp.Key] = replacements[actualRepIndex];
            }

            return commands;
        }
    }
}
