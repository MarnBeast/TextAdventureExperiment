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
            Player player = new Player(io);

            Item item;
            Adventure adventure = new Adventure();

            /* BASICS */
            item = new Item("Basics", true);
            item.AddCommand("where(| am I(|\\?))|look(| around)", "DESC");
            adventure.Add(item);


            /* LOCATIONS */

            item = new Item("Cottage", true);
            adventure.Add(item);
            item.AddCustomAction("DESC", "SAY 'You are standing in a cottage. There is a fishing pole here. Exits are: out.'");
            item.AddCommand("(go |exit )out", "GO 'Garden Path' DESC");
            item.AddCommand("(examine |look at )fishing (pole|rod)", "SAY 'The fishing pole is a simple fishing pole.'");
            item.AddCommand("(take |steal |grab )fishing (pole|rod)", "GIVE 'Fishing Pole' SAY 'You take the fishing pole.");


            item = new Item("Garden Path", true);
            adventure.Add(item);
            item.AddCustomAction("DESC", "SAY 'You are standing on a lush garden path. There is a rosebush here. There is a cottage here. Exits are: North, South, In'");
            item.AddCommand("(go |exit )north", "GO 'Winding Path' DESC");
            item.AddCommand("(go |exit )south", "GO 'Fishing Pond' DESC");
            item.AddCommand("(take |steal |grab )(|a |the )((|red )rose|rosebush)", 
                @"IF HAS 'Took a rose' THEN SAY 'Let\'s not get too greedy, hmm?'
                  ELSE SAY 'You take a rose.' GIVE 'Rose' GIVE 'Took a rose'");
            item.AddCommand("(examine |look at |)(|the )rose(|bush)", "SAY 'The rosebush has many beautiful red roses.'");
            item.AddCommand("(go |exit )in(| (the |)cottage)", "GO 'Cottage' DESC");       // putting this last because "in" matches "examine"


            item = new Item("Fishing Pond", true);
            adventure.Add(item);
            item.AddCustomAction("DESC", "SAY 'You are at the edge of a small fishing pond. Exits are: North'");
            item.AddCommand("(go |exit )north", "GO 'Garden Path' DESC");


            item = new Item("Winding Path", true);
            adventure.Add(item);
            item.AddCustomAction("DESC", "SAY 'You are walking along a winding path. There is a tall tree here. Exits are: South, East, Up'");
            item.AddCommand("(go |exit )south", "GO 'Garden Path' DESC");
            item.AddCommand("(go |exit )east", "GO 'Drawbridge' DESC");
            item.AddCommand("(go |exit |climb )up|climb(|(| up)(| the)(| tall) tree)", "GO 'Top of the Tall Tree' DESC");
            item.AddCommand("(examine |look at )(|the )(|tall )tree", "The tree towers above you. It is very tall and it\'s many branches splinter out near the top of the tree.");


            item = new Item("Top of the Tall Tree", true);
            adventure.Add(item);
            item.AddCustomAction("DESC", "SAY 'You are at the top of the tall tree. There is a stout, dead branch here. Exits are: Down'");
            item.AddCommand("(go |exit |climb )down|climb(|(| down)(| the)(| tall) tree)", "GO 'Winding Path' DESC");
            item.AddCommand("(examine |look at )(|the )(|stout(|,) )(|dead )branch", "");


            /* ITEMS */

            item = new Item("Fishing Pole", false);
            adventure.Add(item);
            item.AddCustomAction("EXAM", "SAY 'The fishing pole is a simple fishing pole.'");
            item.AddCommand("(examine |look at )fishing (pole|rod)", "SAY 'The fishing pole is a simple fishing pole.'");
            item.AddCommand("(go fish(|ing))|catch fish|use (|fishing )(pole|rod)",
                @"IF HAS 'Fishing Pond' THEN 
                    IF HAS 'Caught a fish' THEN SAY 'The fish don't seem to be biting anymore.'
                    ELSE SAY 
'With a flick of the wrist you cast your line out towards the middle of the pond, and wait.
After some time, you feel a slight tug on your line, then another. A fish! You reel in your line, and a small fish is hooked.' 
                    GIVE 'Raw Fish'
                    GIVE 'Caught a fish'
                    ENDIF
                ELSE 
                    SAY 'You better be careful, you could hook someone with that thing!'");


            item = new Item("Raw Fish", false);
            adventure.Add(item);
            item.AddCommand("(eat |consume |lick |taste )(|the )(|raw )fish",
                "SAY 'Yuck! You can't do that, it\'s raw! Weirdo.'");

            item = new Item("Caught a fish", true);
            adventure.Add(item);


            item = new Item("Rose", false);
            item.AddCommand("(examine |look at )(|the )((|red )rose)", "A beautiful red rose.");
            item.AddCommand("smell (|the )((|red )rose)", "It smells quite good!");
            adventure.Add(item);

            item = new Item("Took a rose", true);
            adventure.Add(item);



            /* START SCRIPT */

            adventure.StartScript = "GIVE 'Basics' GO 'Cottage' DESC";

            adventure.Start(player);
        }
    }
}
