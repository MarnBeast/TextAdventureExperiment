# TextAdventureExperiment

This is a library I am creating that will allow for custom text adventures, similar to the old telnet adventures and parsely games.

The entire concept is distilled down entirely to a player with items. Users can create items that contain commands, and those commands will only be accessible when the player has that them in his inventory. Thus, if you make a book with a "read book" command, the player will be able to type "read book" while the book is in their inventory and have a scripted response. This works for locations as well, as a location can just be a hidden item (meaning that the item is in the players inventory, but they player does not see it when viewing their inventory). A minimal scripting language exists for the command actions.

This should allow for a very flexible system that people could really get creative with.
