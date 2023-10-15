/*
	This file is part of TakeCommand /L Unleashed
		© 2018-2023 LisiasT : http://lisias.net <support@lisias.net>
		© 2016-2018 LinuxGuruGamer
		© 2015 Sean McDougall

	TakeCommand is licensed as follows:
		* GPL 3.0 : https://www.gnu.org/licenses/gpl-3.0.txt

	TakeCommand /L Unleashed is distributed in the hope that it will be
	useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

	You should have received a copy of the GNU General Public License 3.0
	along with TakeCommand /L Unleashed. If not, see <https://www.gnu.org/licenses/>.

*/
using System.Collections.Generic;
using System.Linq;
using CommNet;
using UnityEngine;
using Experience.Effects;


namespace TakeCommand
{

    public class TakeCommand : ModuleCommand
    {
        [KSPField]
        int CrewCapacity = 1;

        // Keep track of all the command seats that need to be emptied (shared across all instances)
        public static List<Part> allCommandSeats = new List<Part>();

        // Variables to store the escape hatch and collider
        GameObject escapeHatch = null;
        BoxCollider escapeHatchCollider = null;

        // Name of the Kerbal who belongs in this seat
        private string myKerbal;
        private string myFemaleKerbal;

        // Whether or not the Kerbal has been ejected and should now be boarded
        private bool boardKerbal = false;

        private bool error = false;
        private bool editorStart = true;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                // Need to set the capacity to 0 herre so that KSP doesn't try to transfer crew into and out of an external seat
                this.CrewCapacity = this.part.CrewCapacity;
                this.part.CrewCapacity = 0;

                if (escapeHatch == null)
                {
                    escapeHatch = new GameObject("EscapeHatch");
                    escapeHatch.tag = "Airlock";
                    escapeHatch.layer = 21;
                    escapeHatch.transform.parent = this.part.transform;
                    escapeHatch.transform.localEulerAngles = new Vector3(0, 0, 0);
                    escapeHatch.transform.localPosition = new Vector3(0, 0, 0);

                    escapeHatch.AddComponent<BoxCollider>();
                    escapeHatchCollider = escapeHatch.GetComponent<BoxCollider>();
                    escapeHatchCollider.size = new Vector3(0.25f, 0.25f, 0.25f);
                    escapeHatchCollider.isTrigger = true;

                    this.part.airlock = escapeHatch.transform;
                    Log.info("added escape hatch to {0} ({1})", this.part.name, this.part.GetInstanceID());

                    // Disable it for now until we need it
                    escapeHatch.GetComponent<Collider>().enabled = false;
                }

            }
            base.OnStart(state);
        }

        List<Part> getModulePartParent(string moduleToFind, PartModule pm)
        {
            List<Part> lastPart = new List<Part>();

            List<Part> plist = this.vessel.parts;
            if (plist != null)
            {
                for (int i = plist.Count - 1; i >= 0; --i)
                {
                    PartModuleList pmoduleList = plist[i].Modules;
                    for (int i1 = pmoduleList.Count - 1; i1 >= 0; --i1)
                    {
                        if (pmoduleList[i1].moduleName == moduleToFind)
                        {
                            lastPart.Add(plist[i]);

                        }
                    }
                }
            }
            return lastPart;
        }

        public override VesselControlState UpdateControlSourceState()
        {
            this.commCapable = false;
            bool isHibernating = this.IsHibernating;

            var ksList = this.part.Modules.OfType<KerbalSeat>();
            KerbalSeat ks = this.part.Modules.OfType<KerbalSeat>().First();
            //  KerbalEVA kev;
            // Part seatParent = null;

            if (ks == null)
            {
                Log.error("Can't find KerbalSeat in part: {0}", this.part.partInfo.title);
                this.controlSrcStatusText = "No Crew";
                this.moduleState = ModuleCommand.ModuleControlState.NotEnoughCrew;
                return VesselControlState.Kerbal;
            }
            this.pilots = (this.crewCount = (this.totalCrewCount = 0));
            // Kerbal kerbal = null;
            ProtoCrewMember pcm = null;
#if false
            foreach (var k in ksList)
            {
                if (k.Occupant != null)
                {
                    foreach (var k2 in k.Occupant.Modules.OfType<KerbalEVA>())
                    {
                        if (k2 != null)
                        {
                            seatParent = getModulePartParent("KerbalEVA", keva);

                        }
                    }
                }
            }
#endif

            if (ks.Occupant != null)
            {
                KerbalEVA keva = ks.Occupant.Modules.OfType<KerbalEVA>().FirstOrDefault();

                if (keva != null)
                {

                    var seatParentList = getModulePartParent("KerbalEVA", keva);

                    if (seatParentList != null)
                    {
                        foreach (var seatParent in seatParentList)
                        {
                            foreach (var p in seatParent.protoModuleCrew)
                            Log.detail(
                                    "seatParent: {0}  seatParent.protoModuleCrew.count: {1}",
                                    seatParent.partInfo.title, seatParent.protoModuleCrew.Count()
                                );
                            {
                                Log.detail(
                                        "Looking for: {0}      p.name: {1}",
                                        ks.Occupant.partInfo.title, p.name
                                    );
                                if (p.name == ks.Occupant.partInfo.title)
                                {
                                    // Look for a crew member, if possible
                                    if (pcm == null || (pcm != null && pcm.type == ProtoCrewMember.KerbalType.Tourist && p.type == ProtoCrewMember.KerbalType.Crew))
                                        pcm = p;
                                    // If not a pilot, keep looking
                                    if (p.type == ProtoCrewMember.KerbalType.Crew && p.HasEffect<FullVesselControlSkill>() && !p.inactive)
                                    {
                                        pcm = p;
                                        break;
                                    }
                                    //if (pcm.type == ProtoCrewMember.KerbalType.Crew && pcm.HasEffect<FullVesselControlSkill>() && !pcm.inactive)
                                    //break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Log.warn(
                        "No occupant in any seat! this.vessel.name: {0), FlightGlobals.ActiveVessel.name: {1}",
                        this.vessel.name, FlightGlobals.ActiveVessel.name
                    );

                this.controlSrcStatusText = "No Crew";
                this.moduleState = ModuleCommand.ModuleControlState.NotEnoughCrew;
                return VesselControlState.Kerbal;
            }


            if (pcm != null)
            {
                if (pcm.HasEffect<FullVesselControlSkill>() && !pcm.inactive)
                    this.pilots = 1;
                if (pcm.type != ProtoCrewMember.KerbalType.Tourist)
                {
                    this.totalCrewCount = 1;
                    if (!pcm.inactive)
                        this.crewCount = 1;
                }
            }
            else
            {
                if (ks.Occupant != null)
                    Log.error("Unable to find Kerbal: {0} in crew list", ks.Occupant.partInfo.title);
                else
                    Log.error("Unable to find Kerbal in crew list");
                this.controlSrcStatusText = "No Crew";
                this.moduleState = ModuleCommand.ModuleControlState.NotEnoughCrew;
                return VesselControlState.Kerbal;
            }

            if (this.crewCount == 0)
            {
                if (pcm.type == ProtoCrewMember.KerbalType.Tourist)
                {
                    this.controlSrcStatusText = "Tourists Need Crew";
                    this.moduleState = ModuleCommand.ModuleControlState.TouristCrew;
                    return VesselControlState.Kerbal;
                }
                this.controlSrcStatusText = "No Crew";
                this.moduleState = ModuleCommand.ModuleControlState.NotEnoughCrew;
                return VesselControlState.Kerbal;
            }

            bool pilotNeededNotAvail = this.requiresPilot && this.pilots == 0;
            this.commCapable = true;
            if (CommNetScenario.CommNetEnabled && CommNetScenario.Instance != null)
            {
                bool connectionNeededAndConnected = this.Connection != null && this.Connection.IsConnected;

                if (!this.remoteControl && !connectionNeededAndConnected && pilotNeededNotAvail)
                {
                    if (this.controlSrcStatusText != "No Pilot")
                        this.controlSrcStatusText = "No Pilot";

                    this.moduleState = ModuleCommand.ModuleControlState.PartialManned;
                    return VesselControlState.KerbalPartial;
                }
                if (!connectionNeededAndConnected)
                {
                    if (this.totalCrewCount == 0)
                    {
                        if (this.SignalRequired || !this.remoteControl)
                        {
                            if (this.controlSrcStatusText != "No Telemetry")
                                this.controlSrcStatusText = "No Telemetry";

                            this.moduleState = ModuleCommand.ModuleControlState.NoControlPoint;
                            return VesselControlState.Probe;
                        }
                        if (this.requiresTelemetry)
                        {
                            if (this.controlSrcStatusText != "Partial Control")
                                this.controlSrcStatusText = "Partial Control";

                            this.moduleState = ModuleCommand.ModuleControlState.PartialProbe;
                            return VesselControlState.ProbePartial;
                        }
                    }
                }
            }

            if (isHibernating)
            {
                if (this.controlSrcStatusText != "Hibernating")
                    this.controlSrcStatusText = "Hibernating";

                if (this.minimumCrew > 0)
                {
                    this.moduleState = ModuleCommand.ModuleControlState.PartialManned;
                    return VesselControlState.KerbalPartial;
                }
                this.moduleState = ModuleCommand.ModuleControlState.PartialProbe;
                return VesselControlState.ProbePartial;
            }
            else
            {
                if (this.controlSrcStatusText != "Operational")
                    this.controlSrcStatusText = "Operational";
                this.moduleState = ModuleCommand.ModuleControlState.Nominal;
                if (this.minimumCrew > 0)
                    return VesselControlState.KerbalFull;
                return VesselControlState.ProbeFull;
            }
        }
        void Update()
        {
            if (HighLogic.LoadedSceneIsEditor)
            {
                if (error || editorStart)
                {
                    error = false;
                    editorStart = false;

                    allCommandSeats.Clear();
                }
            }
            else editorStart = true;

        }
        public override void OnUpdate()
        {
            Log.dbg("OnUpdate 1");
            if (HighLogic.LoadedSceneIsFlight) // && vessel.HoldPhysics == true)
            {
                // Make sure controls are unlocked (workaround for compatibility issue with Kerbal Joint Reinforcement)
                if (InputLockManager.GetControlLock("KJRLoadLock") != ControlTypes.ALL_SHIP_CONTROLS)
                {
                    if (this.part.protoModuleCrew.Count > 0 && allCommandSeats.Count == 0)
                    {

                        Log.detail("populating seat list");
                        foreach (Part p in vessel.parts)
                        {
                            if (p.Modules.OfType<TakeCommand>().Any())
                            {
                                if (p.protoModuleCrew.Count > 0)
                                {
                                    allCommandSeats.Add(p);
                                }
                            }
                        }
                        Log.detail("[TakeCommand] found {0} occupied seats", allCommandSeats.Count);
                    }

                    Log.detail("this.part.protoModuleCrew.Count: {0}", this.part.protoModuleCrew.Count());
#if true
                    if (!error)
                    {
                        Log.dbg("OnUpdate 2");

                        if (FlightEVA.hatchInsideFairing(this.part))
                        {
                            ScreenMessages.PostScreenMessage(part.partInfo.title + " is inside a fairing (not allowed)", 15.0f, ScreenMessageStyle.UPPER_CENTER);
                            ScreenMessages.PostScreenMessage("Revert and try again", 15.0f, ScreenMessageStyle.UPPER_CENTER);
                            error = true;
                        }
                        else
                        {
                            Log.dbg("OnUpdate 3");

                            if (boardKerbal == false)
                            {
                                Log.dbg("OnUpdate 4");

                                Log.detail("boardKerbal");
                                if (this.part.protoModuleCrew.Count > 0 && allCommandSeats.First().GetInstanceID() == this.part.GetInstanceID())
                                {
                                    // Time to eject this crew member
                                    ProtoCrewMember kerbal;
                                    while (this.part.protoModuleCrew.Count() > 0)
                                    {

                                        kerbal = this.part.protoModuleCrew[0];
                                        //ProtoCrewMember kerbal = this.part.protoModuleCrew.First();
                                        Log.detail("ejecting {0} from {1}", kerbal.name, this.part.GetInstanceID());
                                        escapeHatch.GetComponent<Collider>().enabled = true;
                                        if (FlightEVA.fetch.spawnEVA(kerbal, this.part, escapeHatch.transform))
                                        {
                                            myKerbal = "kerbalEVA (" + kerbal.name + ")";
                                            myFemaleKerbal = "kerbalEVAfemale (" + kerbal.name + ")";

                                            boardKerbal = true;
                                            escapeHatch.GetComponent<Collider>().enabled = false;
                                        }
                                        else
                                        {
                                            Log.detail("error ejecting {0}", kerbal.name);
                                            ScreenMessages.PostScreenMessage("Unable to put kerbal: " + kerbal.name + " into the external seat", 5.0f, ScreenMessageStyle.UPPER_CENTER);
                                            ScreenMessages.PostScreenMessage("Revert and try again", 5.0f, ScreenMessageStyle.UPPER_CENTER);

                                            error = true;
                                            Log.dbg("Error set true");
                                            break;
                                            //    this.part.protoModuleCrew.Remove(kerbal);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                Log.dbg("OnUpdate 5");

                                // Check and wait until the ejected Kerbal is the active vessel before proceeding
                                Log.detail("this.vessel.name: {0}", this.vessel.name);
                                Log.detail("FlightGlobals.ActiveVessel.name: {0}", FlightGlobals.ActiveVessel.name);
                                if (this.vessel == FlightGlobals.ActiveVessel)
                                    Log.detail("this.vessel is activevessel, myKerbal: {0}", myKerbal);
                                if (FlightGlobals.ActiveVessel.name == myKerbal || FlightGlobals.ActiveVessel.name == myFemaleKerbal)
                                {
                                    KerbalEVA kerbal = FlightGlobals.ActiveVessel.GetComponent<KerbalEVA>();
                                    Log.detail("kerbal.fsm.currentStateName: {0}", kerbal.fsm.currentStateName);
                                    if (kerbal.fsm.Started == true)
                                    {
                                        allCommandSeats.Remove(allCommandSeats.First());
                                        //allCommandSeats.Remove(this.part);
                                        boardKerbal = false;

                                        if (kerbal.flagItems == 0)
                                            kerbal.AddFlag();

                                        var freeModule = this.part.Modules.OfType<KerbalSeat>().First(t => t.Occupant == null);
                                        Log.info("seating {0} in {1}", kerbal.name, this.part.GetInstanceID());
										// Board in first unoccupied seat

                                        freeModule.BoardSeat();

                                    }
                                }
                            }
                        }
                    }
                    else
                        Log.dbg("error is true");
#endif
                }
            }
            base.OnUpdate();
        }

    }

}