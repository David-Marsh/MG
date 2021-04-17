# MG
Monogame projects is a journey to discover what can be done with Monogame. This involves discovering problems and finding solutions. Having a finished product is not a expectation, its all about the journey and not the destination.

There are a few things in the Void Control project that set it apart. While this is not a finished game it is playable and has some interesting elements.
Each void ship has 6 ship systems.
- Cloak reduces detection range by opponants
- Reactor generate energy for ship systems
- Sensor increase detection range of opponants
- Shield protects from damage from colisions with opponants and bullet 
- Thruster increases maximum acceleration and ammount of power used to accelarate
- Weapons increases amount of power and speed in bullets
## PID 
Motion control starts from position velocity and acceleration. Starting with the player only having control over accelaration, it seems fair for the non-players to have these same constraints. In Void Control the non-players approachs weapons firing range of the target while at the same time avoiding other non-players. This results in control that is computationaly easy while having some profound depth. The non-players naturaly flank the player and avoid colision with each other. While the control implements P, I and D the I term is not really helpfull in this application.
## Hash
The starfield background places stars based on a hash of visable points on a grid. The results in a non-repeating background that looks like a random distribution but it is completely deterministic. Each star uses the hash to apply an offset, change the color and size. The hash is also used to modulate the color of the stars causing them to flicker.

Non-players are also placed into world space in a simular way of hashing points on a grid.
## UI
A user interface is implemented to provide a heads up display of information while playing. This is all done in a manner that follows consepts of controls that are placed on forms in visual studio. Extending a base control provides a flexiable solution with a low cost to develop with fast performance. Even the minimap is an extention of the base control.
