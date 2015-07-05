using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment.Actions
{
    class SayAction : Action
    {
        public const string CMD = "SAY";

        private string m_message = String.Empty;

        public string Message
        {
            get { return m_message; }
            set 
            {
                if(value.StartsWith("'") || value.StartsWith("\""))
                {
                    value = value.Substring(1);
                }
                if(value.EndsWith("'") || value.EndsWith("\""))
                {
                    value = value.Substring(0, value.Length - 1);
                }
                m_message = value; 
            }
        }

        public SayAction(Player player)
            : base(player)
        {
        }

        public SayAction(Player player, string message)
            : base(player)
        {
            m_message = message;
        }

        override public bool Execute()
        {
            Player.IO.Write(m_message);
            return true;
        }
    }
}
