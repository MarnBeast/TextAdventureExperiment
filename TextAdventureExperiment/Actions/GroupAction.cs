using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment.Actions
{
    class GroupAction : Action
    {

        public new Player Player
        { 
            get
            {
                return base.Player;
            }
            set
            {
                base.Player = value;
                foreach(Action action in Actions)
                {
                    action.Player = value;
                }
            }
        }

        public List<Action> Actions
        {
            get;
            private set;
        }

        public GroupAction(Player player)
            : base(player)
        {
            Actions = new List<Action>();
        }

        public void Add(Action action)
        {
            Actions.Add(action);
        }

        public void Remove(Action action)
        {
            Actions.Remove(action);
        }



        public override bool Execute()
        {
            foreach (Action action in Actions)
            {
                bool ret = action.Execute();
                if (!ret) return false;
            }
            return true;
        }

        
    }
}
