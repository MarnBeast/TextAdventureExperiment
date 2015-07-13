using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment.Actions
{
    class IfElseAction : Action
    {
        public String[] Condition { get; set; }
        public String[] IfCommands { get; set; }
        public String[] ElseCommands { get; set; }

        public new Player Player
        {
            get
            {
                return base.Player;
            }
            set
            {
                base.Player = value;
            }
        }

        public IfElseAction(Player player) : base(player) { }

        public override bool Execute()
        {
            if(Player.Inventory.ParseActions(Condition))
            {
                return Player.Inventory.ParseActions(IfCommands);
            }
            else
            {
                return Player.Inventory.ParseActions(ElseCommands);
            }
        }

        public int Parse(String[] commands)
        {
            int ret = 0;
            bool elseDetected = false;
            int conditionStart = 0;
            int conditionEnd = 0;
            int ifStart = 0;
            int ifEnd = 0;
            int elseStart = 0;
            int elseEnd = 0;

            int innerIfCount = 0;
            int innerThenCount = 0;

            for (int i = ret; i < commands.Length; i++ )
            {
                if (commands[i].ToUpper() == "IF")
                {
                    innerIfCount++;
                    continue;
                }
                if (commands[i].ToUpper() == "THEN")
                {
                    if (innerIfCount != innerThenCount)
                    {
                        innerThenCount++;
                    }
                    else
                    {
                        conditionStart = 0;
                        conditionEnd = i;
                        ret = i + 1;
                        break;
                    }
                }
            }

            innerIfCount = 0;
            int innerElseCount = 0;

            for (int i = ret; i < commands.Length; i++)
            {
                if (commands[i].ToUpper() == "IF")
                {
                    innerIfCount++;
                    continue;
                }
                if (commands[i].ToUpper() == "ELSE")
                {
                    if (innerIfCount != innerElseCount)
                    {
                        innerElseCount++;
                    }
                    else
                    {
                        ifStart = ret;
                        ifEnd = i;
                        ret = i + 1;
                        elseDetected = true;
                        break;
                    }
                }
            }


            innerIfCount = 0;
            int innerEndIfCount = 0;

            for (int i = ret; i < commands.Length; i++)
            {
                if (commands[i].ToUpper() == "IF")
                {
                    innerIfCount++;
                    continue;
                }
                if (commands[i].ToUpper() == "ENDIF")
                {
                    if (innerIfCount != innerEndIfCount)
                    {
                        innerEndIfCount++;
                    }
                    else
                    {
                        if (elseDetected)
                        {
                            elseStart = ret;
                            elseEnd = i;
                        }
                        else
                        {
                            ifStart = ret;
                            ifEnd = i;
                        }
                        ret = i + 1;
                        break;
                    }
                }
                else if (i == commands.Length - 1)
                {
                    if (elseDetected)
                    {
                        elseStart = ret;
                        elseEnd = i + 1;
                    }
                    else
                    {
                        ifStart = ret;
                        ifEnd = i + 1;
                    }
                    ret = i + 1;
                    break;
                }
            }

            Condition = commands.Take(conditionEnd).Skip(conditionStart).ToArray();
            IfCommands =  commands.Take(ifEnd).Skip(ifStart).ToArray();
            ElseCommands = commands.Take(elseEnd).Skip(elseStart).ToArray();
            
            return ret;
        }
    }
}
