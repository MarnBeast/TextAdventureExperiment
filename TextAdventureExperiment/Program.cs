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
            item.AddCustomAction("KILL", "GO 'Dead'");
            adventure.Add(item);


            /* LOCATIONS */


            item = new Item("Cottage", true);
            adventure.Add(item);
            item.AddCustomAction("DESC",
                @"IF HAS 'Took the fishing pole' THEN SAY 'You are standing in a cottage. Exits are: out.'
                  ELSE SAY 'You are standing in a cottage. There is a fishing pole here. Exits are: out.'");
            item.AddCommand("(go |exit )out", "GO 'Garden Path' DESC");
            item.AddCommand("(examine |look at )fishing (pole|rod)", "SAY 'The fishing pole is a simple fishing pole.'");
            item.AddCommand("(take |steal |grab )fishing (pole|rod)", 
                @"IF HAS 'Took the fishing pole' THEN SAY 'There is no fishing pole here, you already took it!'
                  ELSE GIVE 'Fishing Pole' GIVE 'Took the fishing pole' SAY 'You take the fishing pole.'");


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
            item.AddCustomAction("DESC", 
                @"IF HAS 'Took a branch' THEN SAY 'You are at the top of the tall tree. Exits are: Down' 
                  ELSE SAY 'You are at the top of the tall tree. There is a stout, dead branch here. Exits are: Down'");
            item.AddCommand("(go |exit |climb )down|climb(|(| down)(| the)(| tall) tree)", "GO 'Winding Path' DESC");
            item.AddCommand("(examine |look at )(|the )(|stout(|,) )(|dead )branch", "SAY 'The branch looks thick, but dried and cracked, barely hanging on to the limb of the tree.'");
            item.AddCommand("(take |steal |grab )(|the )(|stout(|,) )(|dead )branch", 
                @"IF HAS 'Took a branch' THEN SAY 'There are no more branches that can be taken from this tree.'
                  ELSE SAY 'With a crack the branch snaps off of the tree.' GIVE 'Dead Branch' GIVE 'Took a branch'");


            item = new Item("Drawbridge", true);
            adventure.Add(item);
            item.AddCustomAction("DESC",
                @"IF HAS 'Bribed a troll' THEN SAY 'You are standing on one side of a drawbridge leading to ACTION CASTLE. Exits are West, East'
                  ELSE SAY 'You are standing on one side of a drawbridge leading to ACTION CASTLE. There is a mean troll here. Exits are West, East' TROLL");
            item.AddCustomAction("TROLL",
                @"IF HAS 'Anger troll 1' THEN GIVE 'Anger troll 2' TAKE 'Anger troll 1' SAY 'The troll stands in your path and grimmaces intimidatingly. ""SNARRRRL WHO DAYRS CROZ ME BRIJ \\'ERE! AINT NOBUDY GETS OVUR \\'ERE BRIJ WITOUT PAYUN DE TOLL!""'
                  ELSE IF HAS 'Anger troll 2' THEN GIVE 'Anger troll 3' TAKE 'Anger troll 2' SAY 'The troll lurches closer. ""AY YOO FINK YOO SMART OR SUMMUT? BEST MOVE ON OR GET A BOOT! IT\\'S LUNSH TIME AN YER LOOKIN MITY TASTY!""'
                  ELSE IF HAS 'Anger troll 3' THEN GIVE 'Anger troll 4' TAKE 'Anger troll 3' SAY 'The troll grabs you by your shirt! ""I SED GET LOOOST! LAST CHANS IDYOT! MY BELLY RUMBLES AN I AIN GOT TIM FOR YUR JUNK!""'
                  ELSE IF HAS 'Anger troll 4' THEN SAY 'The troll rushes towards you! ""GET OFF MY BRIJ!"" He swings is club at your head and everything goes black.' KILL
                  ELSE GIVE 'Anger troll 1'");
            item.AddCommand("(go |exit )west", "GO 'Winding Path' DESC");
            item.AddCommand("(go |exit )east", 
                @"IF HAS 'Bribed a troll' THEN GO 'Courtyard' DESC
                  ELSE TROLL");
            item.AddCommand("(examine |look at )(|the )troll", "TROLL");


            item = new Item("Courtyard", false);
            adventure.Add(item);
            item.AddCustomAction("DESC", "SAY 'You\\'ve reached the courtyard! This is as far as I\\'ve written. Goodbye!");


            /* ITEMS */

            item = new Item("Fishing Pole", false);
            adventure.Add(item);
            item.AddCustomAction("EXAM", "SAY 'The fishing pole is a simple fishing pole.'");
            item.AddCommand("(examine |look at )fishing (pole|rod)", "SAY 'The fishing pole is a simple fishing pole.'");
            item.AddCommand("(go fish(|ing))|catch fish|use (|fishing )(pole|rod)",
                @"IF HAS 'Fishing Pond' THEN 
                    IF HAS 'Caught a fish' THEN SAY 'The fish don\\'t seem to be biting anymore.'
                    ELSE SAY 'With a flick of the wrist you cast your line out towards the middle of the pond, and wait. After some time, you feel a slight tug on your line, then another. A fish! You reel in your line, and a small fish is hooked.' 
                    GIVE 'Raw Fish'
                    GIVE 'Caught a fish'
                    ENDIF
                ELSE 
                    SAY 'You better be careful, you could hook someone with that thing!'");

            item = new Item("Took the fishing pole", true);
            adventure.Add(item);


            item = new Item("Raw Fish", false);
            adventure.Add(item);
            item.AddCommand("(eat |consume |lick |taste )(|the )(|raw )fish",
                "SAY 'Yuck! You can't do that, it\'s raw! Weirdo.'");
            item.AddCommand("(give |offer |hand )(|the )(|raw )fish(| to (|the )troll)", 
                @"IF HAS 'Drawbridge' AND NOT HAS 'Bribed a troll' THEN SAY '""WUT? FUR ME?? AWWWW SHUKS, YOU SHUDUNT AV."" The troll slurps up the fish and dissappears under the bridge.' GIVE 'Bribed a troll' TAKE 'Raw Fish'
                  ELSE SAY 'No one wants that nasty fish!'");

            item = new Item("Caught a fish", true);
            adventure.Add(item);


            item = new Item("Rose", false);
            item.AddCommand("(examine |look at )(|the )((|red )rose)", "A beautiful red rose.");
            item.AddCommand("smell (|the )((|red )rose)", "It smells quite good!");
            adventure.Add(item);

            item = new Item("Took a rose", true);
            adventure.Add(item);


            item = new Item("Dead Branch", false);
            item.AddCommand("(examine |look at )(|the )((|red )rose)", "A beautiful red rose.");
            item.AddCommand("smell (|the )((|red )rose)", "It smells quite good!");
            adventure.Add(item);

            item = new Item("Took a branch", true);
            adventure.Add(item);

            item = new Item("Bribed a troll", true);
            adventure.Add(item);

            item = new Item("Anger troll 1", true);
            adventure.Add(item);
            item = new Item("Anger troll 2", true);
            adventure.Add(item);
            item = new Item("Anger troll 3", true);
            adventure.Add(item);
            item = new Item("Anger troll 4", true);
            adventure.Add(item);


            /* KILL */

            item = new Item("Dead", false);
            adventure.Add(item);
            item.AddCustomAction("DESC", "SAY 'You are dead'");
            item.AddCommand(".*", "DESC");

            /* START SCRIPT */

            adventure.StartScript = "GIVE 'Basics' GIVE 'Raw Fish' GIVE 'Caught a fish' GO 'Drawbridge' DESC";

            adventure.Start(player);
        }
    }
}
