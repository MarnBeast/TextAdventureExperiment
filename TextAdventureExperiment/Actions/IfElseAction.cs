using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment.Actions
{
    class IfElseAction : Action
    {
        public Action Condition { get; set; }
        public Action IfAction { get; set; }
        public Action ElseAction { get; set; }

        public new Player Player
        {
            get
            {
                return base.Player;
            }
            set
            {
                base.Player = value;
                if (Condition != null)  Condition.Player = value;
                if (IfAction != null)   IfAction.Player = value;
                if (ElseAction != null) ElseAction.Player = value;
            }
        }

        public IfElseAction(Player player) : base(player) { }

        public override bool Execute()
        {
            if(Condition.Execute())
            {
                return IfAction.Execute();
            }
            else
            {
                return ElseAction.Execute();
            }
        }

        public int Parse(String[] commands)
        {
            int ret = 0;
            bool elseDetected = false;

            Condition = new GroupAction(Player);      // empty group actions as default (execute will do nothing)
            IfAction = new GroupAction(Player);
            ElseAction = new GroupAction(Player);

            for (int i = ret; i < commands.Length; i++ )
            {
                if (commands[i].ToUpper() == "THEN")
                {
                    Condition = Player.Inventory.GetAction(commands.Take(i).ToArray());
                    ret = i+1;
                    break;
                }
            }

            for (int i = ret; i < commands.Length; i++)
            {
                if (commands[i].ToUpper() == "ELSE")
                {
                    IfAction = Player.Inventory.GetAction(commands.Take(i).Skip(ret).ToArray());
                    ret = i + 1;
                    elseDetected = true;
                    break;
                }
            }

            for (int i = ret; i < commands.Length; i++)
            {
                if (commands[i].ToUpper() == "ENDIF")
                {
                    if(elseDetected)
                    {
                        ElseAction = Player.Inventory.GetAction(commands.Take(i).Skip(ret).ToArray());
                    }
                    else
                    {
                        IfAction = Player.Inventory.GetAction(commands.Take(i).Skip(ret).ToArray());
                    }
                    ret = i + 1;
                    elseDetected = true;
                    break;
                }
                else if (i == commands.Length - 1)
                {
                    if (elseDetected)
                    {
                        ElseAction = Player.Inventory.GetAction(commands.Skip(ret).ToArray());
                    }
                    else
                    {
                        IfAction = Player.Inventory.GetAction(commands.Skip(ret).ToArray());
                    }
                    ret = i + 1;
                    elseDetected = true;
                    break;
                }
            }
            
            return ret;
        }
    }
}
