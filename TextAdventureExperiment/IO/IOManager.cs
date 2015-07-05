using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventureExperiment
{
    public interface IOManager
    {
        void Write(string output);
        string Read();
    }
}
