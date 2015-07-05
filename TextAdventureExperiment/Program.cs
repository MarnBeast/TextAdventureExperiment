using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAdventureExperiment.Actions;
using TextAdventureExperiment.IO;

namespace TextAdventureExperiment
{
    class Program
    {
        static void Main(string[] args)
        {
            IOManager io = new ConsoleIOManager();
            while (true)
            {
                String text = io.Read();
                
                Action group = ActionFactory.GetAction(io, text);

                group.Execute();

                io.Read();
            }
        }
    }
}
