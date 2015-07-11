﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAdventureExperiment.Actions;

namespace TextAdventureExperiment.Items
{
    class CoreItem : Item
    {
        public string GoItemString { get; set; }

        public const string NAME = "Core Item";
        internal CoreItem() : base() 
        {
            this.Name = NAME;
            this.Hidden = true;
            this.GoItemString = String.Empty;

            this.AddCommand("(inventory|inv)", "INV");
        }

        /// <summary>
        /// CoreItem should always be the lowest item priority in the inventory, so we can assume 
        /// that there will be no more commands that could match after this method. Thus, if we can't
        /// match the string at the beginning of commands, GetAction will still skip the commands
        /// array anyway.
        /// </summary>
        /// <param name="player">
        /// The player trying to execute the action.</param>
        /// <param name="commands">A string of action commands to be parsed in order. This should contain quoted strings and
        /// core action handles (SAY, GIVE, IF, etc) only.</param>
        /// <returns>The action built from the commands array.</returns>
        override public Action GetAction(Player player, ref string[] commands)
        {
            Action ret = null;

            if (commands != null && commands.Length > 0)
            {
                int index = 0;
                switch (commands[0].ToUpper())
                {
                    case "SAY":
                        {
                            SayAction sayAction = new SayAction(player);
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
                            IfElseAction ifAction = new IfElseAction(player);
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
                            NotAction notAction = new NotAction(player);
                            commands = commands.Skip(1).ToArray();  // skip to next action
                            Action opAction = GetAction(player, ref commands);
                            notAction.Op = opAction;
                            // no index increment, the ref is our increment
                            ret = notAction;
                        }
                        break;

                    case "AND":
                        {
                            AndAction andAction = new AndAction(player);
                            commands = commands.Skip(1).ToArray();
                            Action op2Action = GetAction(player, ref commands);
                            andAction.Op2 = op2Action;
                            // no index increment, the ref is our increment
                            ret = andAction;
                        }
                        break;

                    case "OR":
                        {
                            OrAction orAction = new OrAction(player);
                            commands = commands.Skip(1).ToArray();
                            Action op2Action = GetAction(player, ref commands);
                            orAction.Op2 = op2Action;
                            // no index increment, the ref is our increment
                            ret = orAction;
                        }
                        break;

                    case "GIVE":        
                        {
                            GiveAction giveAction = new GiveAction(player);
                            index++;
                            if (commands.Length > 1)
                            {
                                giveAction.ItemName = commands[1];
                                index++;
                            }
                            giveAction.ExecuteWhenCreated();
                            ret = giveAction;
                        }
                        break;

                    case "TAKE":
                        {
                            TakeAction takeAction = new TakeAction(player);
                            index++;
                            if (commands.Length > 1)
                            {
                                takeAction.ItemName = commands[1];
                                index++;
                            }
                            takeAction.ExecuteWhenCreated();
                            ret = takeAction;
                        }
                        break;

                    case "HAS":
                        {
                            HasAction hasAction = new HasAction(player);
                            index++;
                            if(commands.Length > 1)
                            {
                                hasAction.ItemName = commands[1];
                                index++;
                            }
                            ret = hasAction;
                        }
                        break;

                    case "GO":
                        {
                            GoAction goAction = new GoAction(player);
                            index++;
                            if (commands.Length > 1)
                            {
                                goAction.GiveItemName = commands[1];
                                goAction.TakeItemName = GoItemString;
                                index++;
                            }
                            if(goAction.ExecuteWhenCreated())
                            {
                                GoItemString = goAction.GiveItemName;
                            }
                            ret = goAction;
                        }
                        break;

                    case "INV":
                            {
                                GroupAction groupAction = new GroupAction(player);
                                index++;
                                foreach(Item item in player.Inventory.Items
                                    .Where(item => !item.Hidden))
                                {
                                    SayAction sayAction = new SayAction(player);
                                    sayAction.Message = item.DisplayName;
                                    groupAction.Add(sayAction);
                                }
                                if(groupAction.Actions.Count < 1)
                                {
                                    SayAction sayAction = new SayAction(player);
                                    sayAction.Message = "You have no items.";
                                    groupAction.Add(sayAction);
                                }
                                ret = groupAction;
                            }
                        break;

                    default:        // unrecognized? This is the base item, so there's no other items that could have matching actions.
                        index++;    // ignore it.
                        break;

                }

                commands = commands.Skip(index).ToArray();
            }

            return ret;
        }
    }
}