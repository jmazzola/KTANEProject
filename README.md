# KTANEProject v1.0
[WIP] A hacked version of Keep Talking and Nobody Explodes to allow for more customization, information and enhancements until the developers possibly implement said things.

Includes:

* A modified Assembly-CSharp.dll to use/replace instead of the stock .dll given upon purchase of the game. Reason for this being, creating custom seeds, and printing out certain information requires changing class permissions as well as adding a whole entire interface within the stock .dll. 

[Note: Don't worry if that looks scary. I'll make everything user-friendly after the majority of the code is actually made.]
* .dll to inject to communicate with the modified .dll features:
  *   Bomb information - Print out Serial Number, Ports, Batteries, Lit Indicators, Timer, Strikes, and list of modules.
  *   Module Solutions - List out in text how each module is solved (Used for self-training in speedruns or plain out cheat)
  *   Allow custom seeds for bomb generation (allowing for time-attack/competition with the same bomb)
  *   GUI to customize and pick what modules the user wants in Free-Play bombs (Checkboxes)
