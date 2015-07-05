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

        public new IOManager IO
        {
            get
            {
                return base.IO;
            }
            set
            {
                base.IO = value;
                if (Condition != null)  Condition.IO = value;
                if (IfAction != null)   IfAction.IO = value;
                if (ElseAction != null) ElseAction.IO = value;
            }
        }

        public IfElseAction(IOManager io) : base(io) { }

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

            Condition = new GroupAction(IO);      // empty group actions as default (execute will do nothing)
            IfAction = new GroupAction(IO);
            ElseAction = new GroupAction(IO);

            for (int i = ret; i < commands.Length; i++ )
            {
                if (commands[i].ToUpper() == "THEN")
                {
                    Condition = ActionFactory.GetAction(IO, commands.Take(i).ToArray());
                    ret = i+1;
                    break;
                }
            }

            for (int i = ret; i < commands.Length; i++)
            {
                if (commands[i].ToUpper() == "ELSE")
                {
                    IfAction = ActionFactory.GetAction(IO, commands.Take(i).Skip(ret).ToArray());
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
                        ElseAction = ActionFactory.GetAction(IO, commands.Take(i).Skip(ret).ToArray());
                    }
                    else
                    {
                        IfAction = ActionFactory.GetAction(IO, commands.Take(i).Skip(ret).ToArray());
                    }
                    ret = i + 1;
                    elseDetected = true;
                    break;
                }
                else if (i == commands.Length - 1)
                {
                    if (elseDetected)
                    {
                        ElseAction = ActionFactory.GetAction(IO, commands.Skip(ret).ToArray());
                    }
                    else
                    {
                        IfAction = ActionFactory.GetAction(IO, commands.Skip(ret).ToArray());
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
