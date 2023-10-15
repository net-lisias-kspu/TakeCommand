# TakeCommand :: Change log

* 2023-1015: 1.4.12.4 (lisias) for KSP 1.4.x
	+ Updates the thing to the latest KSPe
	+ Declutters a bit the `KSP.log`
	+ Moves it into the `net.lisias.ksp` hierarchy
* 2018-0808: 1.4.12.3 (lisias) for KSP 1.4.x
	+ Merging upstream changes:
		- Added OpenCockpit rear gunner seat  
* 2018-0801: 1.4.12.2 (lisias) for KSP 1.4.x
	+ Preventing Kerbals being "poofed" when more than one seat is being populated on a cockpit.
		- As by SXT's EAS-316 Meadowlark
	+ The second Kerbal drops to the ground, however. A proper fix is WiP at the moment.
* 2018-0629: 1.4.12.1 (linuxgurugamer) for KSP 1.4.1
	+ Added fix so returning to editor won't prevent spawning again
	+ Added check for female kerbal 
* 2018-0317: 1.4.12 (linuxgurugamer) for KSP 1.4.1
	+ Updated for 1.4.1
	+ Updated build scripts
	+ Updated version file
* 2018-0120: 1.4.11.1 (linuxgurugamer) for KSP 1.3.1
	+ Added line for the Stockalike Mk1 Open Cockpit in the patch to add a vesselType line
* 2017-1126: 1.4.11 (linuxgurugamer) for KSP 1.3.1
	+ Added check for external seat being inside fairing
	+ Added Jenkins build
* 2017-1009: 1.4.10.1 (linuxgurugamer) for KSP 1.3.1
	+ Updated MM dll
* 2017-1008: 1.4.10 (linuxgurugamer) for KSP 1.3.1
	+ Updated for 1.3.1 
* 2017-0925: 1.4.9.1 (linuxgurugamer) for KSP 1.3
	+ Added MM patch to add mod AutoAction function to External Command Seat-like parts. 
* 2017-0528: 1.4.9 (linuxgurugamer) for KSP 1.3
	+ Updated for 1.3 
* 2017-0116: 1.4.8 (linuxgurugamer) for KSP 1.2.2
	+ Now finds the best kerbal in the seats 
* 2017-0115: 1.4.7 (linuxgurugamer) for KSP 1.2.2
	+ Added Akita rover seat from USI Konstruction/MKS, thanks Aelfhe1m
	+ Added code to override UpdateControlSourceState, to report correct state for external seats
* 2017-0111: 1.4.6 (linuxgurugamer) for KSP 1.2.2
	+ Updated all patches to add EVA Parachute if that mod is installed
	+ removed BetterCrewAssignment (can't do internal transfer to external seat)
	+ Moved cfg for stock external command seat to ModuleManager directory
	+ Fixed code so that KSP won't try to transfer a kerbal into/outof an external seat
	+ Added AssemblyVersion code
* 2016-1226: 1.4.5 (linuxgurugamer) for KSP 1.2.2
	+ Added patch for Omicron
	+ Added patch form Kerbonov-KN2
* 2016-1226: 1.4.4 (linuxgurugamer) for KSP 1.2.2
	+ Added patch for WildBlueIndustries
	+ Added patch for USI Exploration
	+ Added patch for NESDparts
* 2016-1225: 1.4.3 (linuxgurugamer) for KSP 1.2.2
	+ Patch added for BetterCrewAssignment
		- Fixed issue when a part has multiple external seats
		- Patch added for SXTContinued, for the Lark
* 2016-1218: 1.4.2.1 (linuxgurugamer) for KSP 1.2.2
	+ Fixed build scripts (paths were wrong)
	+ Now contains correct .version fil
* 2016-1213: 1.4.2 (linuxgurugamer) for KSP 1.2.2
	+ Updated for 1.2.2
* 2016-0506: 1.4.1 (seanmcdougall) for KSP 1.1.2
	+ recompiled for KSP 1.1.2
	+ updated ModuleManager to 2.6.24
* 2016-0424: 1.4 (seanmcdougall) for KSP 1.1
	+ fixed for KSP 1.1
	+ updated ModuleManager to 2.6.23 
* 2015-1206: 1.3 (seanmcdougall) for KSP 1.0.5
	+ now allows spawning Kerbals into command seats after launch (i.e. * 2015-1130: 1.3 beta (seanmcdougall) for KSP 1.0.5
	+ now allows spawning Kerbals into command seats after launch (i.e. on a crew transfer)
* 2015-1114: 1.2.1 (seanmcdougall) for KSP 1.0.5
	+ Recompiled for KSP 1.0.5
	+ Updated to ModuleManager 2.6.13
* 2015-0904: 1.2 (seanmcdougall) for KSP 1.0.3
	+ fixes compatibility issue with Action Groups Extended (and likely other mods as well)
	+ updated to use ModuleManager 2.6.8
* 2015-0606: 1.1.4 (seanmcdougall) for KSP 1.0.3
	+ recompiled for KSP 1.0.3/1.0.4
	+ updated to latest version of ModuleManager
* 2015-0521: 1.1.3
	+ fixes compatibility issue with Kerbal Inventory System (KIS)
	+ fixes compatibility issue with Kerbal Joint Reinforcement (KJR)
* 2015-0515: 1.1.2
	+ tweaked MM config again to add ":for" tag
* 2015-0512: 1.1.1
	+ tweaked MM config to remove ":final" tag
	+ module is now just disabled after processing is complete instead of being removed from the part
* 2015-0512: 1.1
	+ fixed "No Control" warning
	+ ModuleManager is now required for more reliable PartModule manipulation (latest version is included in release package)
	+ general code cleanup and commenting
* 2015-0510): 1.0
	+ initial release
