using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TextAdventureExperiment.Actions;

namespace TextAdventureExperiment
{
    class ActionFactory
    {
        private List<Type> m_ActionTypes;

        public List<Type> ActionTypes
        {
            get { return m_ActionTypes; }
            set { m_ActionTypes = value; }
        }

        public static Action GetAction(IOManager io, string actionText)
        {
            //Regex regex = new Regex("[^\\s\"']+|\"([^\"]*)\"|'([^']*)'");   // splits on spaces except within quot
            Regex regex = new Regex("('.*?'|\".*?\"|\\S+)");
            string[] commands = regex.Split(actionText).Where(s => !String.IsNullOrWhiteSpace(s)).ToArray();

            return GetAction(io, commands);
        }

        public static Action GetAction(IOManager io, string[] commands)
        {
            GroupAction root = new GroupAction(io);

            if (commands != null && commands.Length > 0)
            {
                int count = 0;
                while (commands.Length > 0)
                {
                    Action action = GetAction(io, ref commands);
                    if (action != null)
                    {
                        // If we just parsed a two op action, set the Op1 value
                        if(action is TwoOpAction && root.Actions.Count > 0)
                        {
                            Action op1 = root.Actions.Last();
                            root.Remove(op1);
                            TwoOpAction twoAction = action as TwoOpAction;
                            twoAction.Op1 = op1;
                        }
                        root.Add(action);
                    }
                    if (count++ > 1000) break;  // prevent runaway while loop
                }
            }

            return root;
        }

        public static Action GetAction(IOManager io, ref string[] commands)
        {
            Action ret = null;

            if (commands != null && commands.Length > 0)
            {
                int index = 0;
                switch (commands[0].ToUpper())
                {
                    case "SAY":
                        {
                            SayAction sayAction = new SayAction(io);
                            index++;
                            if (commands.Length > 1)
                            {
                                sayAction.Message = commands[1];
                                index++;
                            }
                            ret = sayAction;
                        }
                        break;

                    case "IF":
                        {
                            IfElseAction ifAction = new IfElseAction(io);
                            index++;
                            if (commands.Length > 1)
                            {
                                index = ifAction.Parse(commands.Skip(1).ToArray());
                            }
                            ret = ifAction;
                        }
                        break;

                    case "NOT":
                        {
                            NotAction notAction = new NotAction(io);
                            commands = commands.Skip(1).ToArray();  // skip to next action
                            Action opAction = GetAction(io, ref commands);
                            notAction.Op = opAction;
                            // no index increment, the ref is our increment
                            ret = notAction;
                        }
                        break;

                    case "AND":
                        {
                            AndAction andAction = new AndAction(io);
                            commands = commands.Skip(1).ToArray();
                            Action op2Action = GetAction(io, ref commands);
                            andAction.Op2 = op2Action;
                            // no index increment, the ref is our increment
                            ret = andAction;
                        }
                        break;

                    case "OR":
                        {
                            OrAction orAction = new OrAction(io);
                            commands = commands.Skip(1).ToArray();
                            Action op2Action = GetAction(io, ref commands);
                            orAction.Op2 = op2Action;
                            // no index increment, the ref is our increment
                            ret = orAction;
                        }
                        break;

                    case "GIVE":
                        index++;
                        break;

                    case "TAKE":
                        index++;
                        break;

                    default:    // unrecognized? ignore it.
                        index++;
                        break;

                }

                commands = commands.Skip(index).ToArray();
            }

            return ret;
        }
        
    }
}
