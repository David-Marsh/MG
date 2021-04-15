# MG
Monogame projects is a journey to discover what can be done with Monogame. This involves discovering problems and finding solutions.

There are a few things in the Void Control project that set it apart. 
##PID 
Motion control starts from position velocity and acceleration. Starting with the player only having control over accelaration, it seems fair for the non-players to these same constraints. 
In Void Control the non-players approachs weapons firing range while of the target while at the same time avoiding other non-players. 
While the control implements P, I and D the I term is not really helpfull.
This results in control that is computationaly easy while having some profound depth.