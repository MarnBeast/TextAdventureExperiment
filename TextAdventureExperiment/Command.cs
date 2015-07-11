using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextAdventureExperiment
{
    public class Command
    {
        public String CommandText { get; set; }
        public String ActionText { get; set; }


        public bool Match(string enteredCommand)
        {
            var regex = new Regex(CommandText, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            return regex.IsMatch(enteredCommand);
        }
    }
}
