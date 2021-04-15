# MG
Monogame projects is a journey to discover what can be done with Monogame. This involves discovering problems and finding solutions.

There are a few things in the Void Control project that set it apart. 

## PID 
Motion control starts from position velocity and acceleration. Starting with the player only having control over accelaration, it seems fair for the non-players to have these same constraints. In Void Control the non-players approachs weapons firing range while of the target while at the same time avoiding other non-players. This results in control that is computationaly easy while having some profound depth. The non-players naturaly flank the player and avoid colision with each other. While the control implements P, I and D the I term is not really helpfull in this application.
## Hashing
The starfield background places stars based on a hash of visable points on a grid. This results in a non-repeating background that looks like a random distribution but it is completely deterministic. Each star uses the hash to apply an offset, change the color and size. The hash is also used to modulate the color of the stars causing them to flicker.
