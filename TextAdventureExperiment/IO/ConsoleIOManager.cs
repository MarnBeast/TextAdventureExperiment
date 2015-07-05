using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment.IO
{
    class ConsoleIOManager : IOManager
    {

        public void Write(string output)
        {
            Console.WriteLine(output);
        }

        public string Read()
        {
            return Console.ReadLine();
        }
    }
}
