using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment.Actions
{
    class GroupAction : Action
    {

        public new IOManager IO
        { 
            get
            {
                return base.IO;
            }
            set
            {
                base.IO = value;
                foreach(Action action in Actions)
                {
                    action.IO = value;
                }
            }
        }

        public List<Action> Actions
        {
            get;
            private set;
        }

        public GroupAction(IOManager io) : base(io)
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
