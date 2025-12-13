//RedGuard Global function, flag and equate definition file
//functions section.
//functions defined as xxx parms y
//where xxx is an ascii name for the function and y is the number of parameters
//the function takes.

// NO LONGER speculation:
// tasks take a while to complete
// functions return immediately
// multitasks are tasks that will asynchonously, do not block script execution
using System;
using UnityEngine;

public class soupdeffcn_nimpl
{
    public static void ThrowNIMPL(string s, int[] i)
    {
        // if we throw an exception here it messes up the script readers
        // probably when in an if statement, the exception will error out and then the
        // script program counter tries to read the end of the 
        // if statement bytes as instructions
        Debug.LogError($"NIMPL: {s}({string.Join(",",i)})");
    }
    public static int PLACEHOLDER_ZERO(uint caller, bool multitask, int[] i /*4*/)
    {
        ThrowNIMPL("PLACEHOLDER_ZERO",i);
        return 0;
    }
    /*task*/
    public static int sRotate(uint caller, bool multitask, int[] i /*4*/)
    {
        ThrowNIMPL("sRotate",i);
        return 0;
    }

    /*task*/
    public static int sRotateByAxis(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("sRotateByAxis",i);
        return 0;
    }

    /*task*/
    public static int sRotateToAxis(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("sRotateToAxis",i);
        return 0;
    }

    /*task*/
    public static int sMove(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("sMove",i);
        return 0;
    }

    /*task*/
    public static int sMoveByAxis(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("sMoveByAxis",i);
        return 0;
    }

    /*task*/
    public static int sMoveAlongAxis(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("sMoveAlongAxis",i);
        return 0;
    }

    /*function*/
    public static int HeaderHook(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("HeaderHook",i);
        return 0;
    }

    /*task*/
    public static int swordAI(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("swordAI",i);
        return 0;
    }

    /*task*/
    public static int projectileAI(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("projectileAI",i);
        return 0;
    }

    /*function*/
    public static int chargeWeapon(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("chargeWeapon",i);
        return 0;
    }

    /*function*/
    public static int PlayerMain(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerMain",i);
        return 0;
    }

    /*function*/
    public static int PrintParms(uint caller, bool multitask, int[] i /*8*/)	
    {
        ThrowNIMPL("PrintParms",i);
        return 0;
    }

    /*function*/
    public static int LogParms(uint caller, bool multitask, int[] i /*8*/)	
    {
        ThrowNIMPL("LogParms",i);
        return 0;
    }

    /*function*/
    public static int PrintStringParm(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PrintStringParm",i);
        return 0;
    }

    /*function*/
    public static int PrintSingleParm(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PrintSingleParm",i);
        return 0;
    }

    /*function*/
    public static int SpacePressed(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("SpacePressed",i);
        return 0;
    }

    /*task*/
    public static int WaitOnTasks(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("WaitOnTasks",i);
        return 0;
    }

    /*function*/
    public static int updatePlayerViewAttrib(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("updatePlayerViewAttrib",i);
        return 0;
    }

    /*function*/
    public static int cameraController(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("cameraController",i);
        return 0;
    }

    /*task*/
    public static int showObjRot(uint caller, bool multitask, int[] i /*8*/)	
    {
        ThrowNIMPL("showObjRot",i);
        return 0;
    }

    /*task*/
    public static int showObj(uint caller, bool multitask, int[] i /*3*/)	
    {
        // Targets the camera to show the object, settings its position
        // i[0]: ???
        // i[1]: camera up-axis offset
        // i[2]: 
        ThrowNIMPL("showObj",i);
        return 0;
    }

    /*task*/
    public static int showObjLoc(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("showObjLoc",i);
        return 0;
    }

    /*task*/
    public static int showObjPan(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("showObjPan",i);
        return 0;
    }

    /*task*/
    public static int showObjPanLoc(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("showObjPanLoc",i);
        return 0;
    }

    /*task*/
    public static int lookObj(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("lookObj",i);
        return 0;
    }

    /*task*/
    public static int showPlayer(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("showPlayer",i);
        return 0;
    }

    /*task*/
    public static int showPlayerPan(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("showPlayerPan",i);
        return 0;
    }

    /*task*/
    public static int lookCyrus(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("lookCyrus",i);
        return 0;
    }

    /*task*/
    public static int showCyrus(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("showCyrus",i);
        return 0;
    }

    /*task*/
    public static int showCyrusLoc(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("showCyrusLoc",i);
        return 0;
    }

    /*task*/
    public static int showCyrusPan(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("showCyrusPan",i);
        return 0;
    }

    /*task*/
    public static int showCyrusPanLoc(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("showCyrusPanLoc",i);
        return 0;
    }

    /*function*/
    public static int PlayAnimation(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("PlayAnimation",i);
        return 0;
    }

    /*function*/
    public static int lockoutPlayer(uint caller, bool multitask, int[] i /*1*/)	
    {
        // Enable/disable player input
        // i[0]: 1 disables input, 0 enables it
        ThrowNIMPL("lockoutPlayer",i);
        return 0;
    }

    /*function*/
    public static int menuNew(uint caller, bool multitask, int[] i /*0*/)	
    {
        // Clears existing menu, prepares for new content
        // TODO: check this
        ThrowNIMPL("menuNew",i);
        return 0;
    }

    /*task*/
    public static int menuProc(uint caller, bool multitask, int[] i /*0*/)	
    {
        // Display the dialog menu and wait for a selection
        ThrowNIMPL("menuProc",i);
        return 0;
    }

    /*function*/
    public static int menuAddItem(uint caller, bool multitask, int[] i /*3*/)	
    {
        // add a menu item to the dialog menu
        // i[0]: string index into the RTX file
        // i[1]: 1 grays out the item
        // i[2]: the item index (? not 100% sequential, might miss some in-between)
        ThrowNIMPL("menuAddItem",i);
        return 0;
    }

    /*function*/
    public static int menuSelection(uint caller, bool multitask, int[] i /*0*/)	
    {
        // Returns the selected item from the dialog menu
        ThrowNIMPL("menuSelection",i);
        return 0;
    }

    /*task*/
    public static int RTX(uint caller, bool multitask, int[] i /*1*/)	
    {
        // Play a sound from RTX and display subtitles
        // i[0]: index of the RTX data
        ThrowNIMPL("RTX",i);
        return 0;
    }

    /*task*/
    public static int rtxAnim(uint caller, bool multitask, int[] i /*4*/)	
    {
        // Play a sound from RTX, display subtitles and play animations
        // i[0]: index of the RTX data
        // i[1]: 1st animation to play
        // i[2]: 2nd animation to play
        // i[3]: 3rd animation to play

        ThrowNIMPL("rtxAnim",i);
        return 0;
    }

    /*task*/
    public static int RTXpAnim(uint caller, bool multitask, int[] i /*4*/)	
    {
        // Play a sound from RTX, display subtitles and play animations
        // TODO: what is p?
        // i[0]: index of the RTX data
        // i[1]: 1st animation to play
        // i[2]: 2nd animation to play
        // i[3]: 3rd animation to play
        ThrowNIMPL("RTXpAnim",i);
        return 0;
    }

    /*task*/
    public static int RTXp(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("RTXp",i);
        return 0;
    }

    /*task*/
    public static int Rotate(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("Rotate",i);
        return 0;
    }

    /*task*/
    public static int RotateByAxis(uint caller, bool multitask, int[] i /*3*/)	
    {
        // i[0]: axis (0/1/2)
        // i[1]: amount
        // i[3]: time to complete
        ThrowNIMPL("RotateByAxis",i);
        return 0;
    }

    /*task*/
    public static int RotateToAxis(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("RotateToAxis",i);
        return 0;
    }

    /*task*/
    public static int WalkForward(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("WalkForward",i);
        return 0;
    }

    /*task*/
    public static int WalkBackward(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("WalkBackward",i);
        return 0;
    }

    /*task*/
    public static int MoveForward(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("MoveForward",i);
        return 0;
    }

    /*task*/
    public static int MoveBackward(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("MoveBackward",i);
        return 0;
    }

    /*task*/
    public static int MoveLeft(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("MoveLeft",i);
        return 0;
    }

    /*task*/
    public static int MoveRight(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("MoveRight",i);
        return 0;
    }

    /*task*/
    public static int Move(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("Move",i);
        return 0;
    }

    /*task*/
    public static int MoveByAxis(uint caller, bool multitask, int[] i /*3*/)	
    {
        // i[0]: axis (0/1/2)
        // i[1]: amount
        // i[3]: time to complete
        ThrowNIMPL("MoveByAxis",i);
        return 0;
    }

    /*task*/
    public static int MoveAlongAxis(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("MoveAlongAxis",i);
        return 0;
    }

    /*task*/
    public static int MoveObjectAxis(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("MoveObjectAxis",i);
        return 0;
    }

    /*task*/
    public static int MoveToLocation(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("MoveToLocation",i);
        return 0;
    }

    /*task*/
    public static int WanderToLocation(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("WanderToLocation",i);
        return 0;
    }

    /*task*/
    public static int MoveToMarker(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("MoveToMarker",i);
        return 0;
    }

    /*function*/
    public static int SetObjectLocation(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("SetObjectLocation",i);
        return 0;
    }

    /*task*/
    public static int Wait(uint caller, bool multitask, int[] i /*1*/)	
    {
        // i[0]: time to wait
        ThrowNIMPL("Wait",i);
        return 0;
    }

    /*function*/
    public static int DistanceFromStart(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("DistanceFromStart",i);
        return 0;
    }

    /*function*/
    public static int Light(uint caller, bool multitask, int[] i /*2*/)	
    {
        // Turns on a light
        // i[0]: light radius
        // i[1]: light intensity
        ThrowNIMPL("Light",i);
        return 0;
    }

    /*function*/
    public static int LightRadius(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("LightRadius",i);
        return 0;
    }

    /*function*/
    public static int LightIntensity(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("LightIntensity",i);
        return 0;
    }

    /*function*/
    public static int LightOff(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("LightOff",i);
        return 0;
    }

    /*function*/
    public static int LightOffset(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("LightOffset",i);
        return 0;
    }

    /*function*/
    public static int LightFlicker(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("LightFlicker",i);
        return 0;
    }

    /*function*/
    public static int FlickerLight(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("FlickerLight",i);
        return 0;
    }

    /*function*/
    public static int LightFlickerOff(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("LightFlickerOff",i);
        return 0;
    }

    /*function*/
    public static int LightSize(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("LightSize",i);
        return 0;
    }

    /*function*/
    public static int LightSizeOff(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("LightSizeOff",i);
        return 0;
    }

    /*multitask*/
    public static int FxPhase(uint caller, bool multitask, int[] i /*7*/)	
    {
        ThrowNIMPL("FxPhase",i);
        return 0;
    }

    /*multitask*/
    public static int FxFlickerOnOff(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("FxFlickerOnOff",i);
        return 0;
    }

    /*multitask*/
    public static int FxFlickerDim(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("FxFlickerDim",i);
        return 0;
    }

    /*multitask*/
    public static int FxLightSize(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("FxLightSize",i);
        return 0;
    }

    /*function*/
    public static int Flat(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("Flat",i);
        return 0;
    }

    /*function*/
    public static int FlatSetTexture(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("FlatSetTexture",i);
        return 0;
    }

    /*function*/
    public static int FlatOff(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("FlatOff",i);
        return 0;
    }

    /*function*/
    public static int FlatOffset(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("FlatOffset",i);
        return 0;
    }

    /*function*/
    public static int FlatLikeStatic(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("FlatLikeStatic",i);
        return 0;
    }

    /*function*/
    public static int FlatAnimate(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("FlatAnimate",i);
        return 0;
    }

    /*function*/
    public static int FlatStop(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("FlatStop",i);
        return 0;
    }

    /*function*/
    public static int SetAttribute(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("SetAttribute",i);
        return 0;
    }

    /*function*/
    public static int GetAttribute(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("GetAttribute",i);
        return 0;
    }

    /*function*/
    public static int SetGlobalFlag(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SetGlobalFlag",i);
        return 0;
    }

    /*function*/
    public static int TestGlobalFlag(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("TestGlobalFlag",i);
        return 0;
    }

    /*function*/
    public static int ResetGlobalFlag(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ResetGlobalFlag",i);
        return 0;
    }

    /*task*/
    public static int FacePlayer(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("FacePlayer",i);
        return 0;
    }

    /*task*/
    public static int FacePlayerInertia(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("FacePlayerInertia",i);
        return 0;
    }

    /*task*/
    public static int FaceAngle(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("FaceAngle",i);
        return 0;
    }

    /*task*/
    public static int FacePos(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("FacePos",i);
        return 0;
    }

    /*task*/
    public static int FaceObject(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("FaceObject",i);
        return 0;
    }

    /*function*/
    public static int Sound(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("Sound",i);
        return 0;
    }

    /*function*/
    public static int FlatSound(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("FlatSound",i);
        return 0;
    }

    /*multitask*/
    public static int AmbientSound(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("AmbientSound",i);
        return 0;
    }

    /*multitask*/
    public static int AmbientRtx(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("AmbientRtx",i);
        return 0;
    }

    /*function*/
    public static int Jump(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("Jump",i);
        return 0;
    }

    /*task*/
    public static int WaitNonMulti(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("WaitNonMulti",i);
        return 0;
    }

    /*task*/
    public static int WaitOnDialog(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("WaitOnDialog",i);
        return 0;
    }

    /*function*/
    public static int HideMe(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("HideMe",i);
        return 0;
    }

    /*function*/
    public static int ShowMe(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ShowMe",i);
        return 0;
    }

    /*function*/
    public static int Rnd(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("Rnd",i);
        return 0;
    }

    /*function*/
    public static int QuickRnd(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("QuickRnd",i);
        return 0;
    }

    /*function*/
    public static int MyId(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("MyId",i);
        return 0;
    }

    /*task*/
    public static int FaceLocation(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("FaceLocation",i);
        return 0;
    }

    /*function*/
    public static int DistanceFromLocation(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("DistanceFromLocation",i);
        return 0;
    }

    /*function*/
    public static int LoadWorld(uint caller, bool multitask, int[] i /*3*/)	
    {
        // loads a world
        // i[0]: the ID of the world to load
        // i[1]: the map marker to start the player at
        // i[2]: the player rotation when he loads in TODO: check this
        ThrowNIMPL("LoadWorld",i);
        return 0;
    }

    /*function*/
    public static int SetAiType(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SetAiType",i);
        return 0;
    }

    /*function*/
    public static int SetAiMode(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SetAiMode",i);
        return 0;
    }

    /*function*/
    public static int MyAiType(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("MyAiType",i);
        return 0;
    }

    /*function*/
    public static int MyAiMode(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("MyAiMode",i);
        return 0;
    }

    /*task*/
    public static int GotoLocation(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("GotoLocation",i);
        return 0;
    }

    /*task*/
    public static int Guard(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("Guard",i);
        return 0;
    }

    /*task*/
    public static int Animal(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("Animal",i);
        return 0;
    }

    /*function*/
    public static int TrueFunction(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("TrueFunction",i);
        return 0;
    }

    /*function*/
    public static int TurnToPos(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("TurnToPos",i);
        return 0;
    }

    /*function*/
    public static int AtPos(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("AtPos",i);
        return 0;
    }

    /*function*/
    public static int TurnToAngle(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("TurnToAngle",i);
        return 0;
    }

    /*function*/
    public static int MovePos(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("MovePos",i);
        return 0;
    }

    /*function*/
    public static int SetStartPosition(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("SetStartPosition",i);
        return 0;
    }

    /*function*/
    public static int GetMyAttr(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("GetMyAttr",i);
        return 0;
    }

    /*function*/
    public static int SetMyAttr(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("SetMyAttr",i);
        return 0;
    }

    /*function*/
    public static int GetFramePassed(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("GetFramePassed",i);
        return 0;
    }

    /*function*/
    public static int SpeedScale(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SpeedScale",i);
        return 0;
    }

    /*function*/
    public static int InKey(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("InKey",i);
        return 0;
    }

    /*function*/
    public static int PressKey(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PressKey",i);
        return 0;
    }


    /*function in all caps?: FUNCTION	ACTIVATE	PARMS 1*/
    public static int ACTIVATE(uint caller, bool multitask, int[] i /*1*/)	
    {
        // True if player activates an item
        // i[0]: Stringid shown on screen when looking at the item
        ThrowNIMPL("ACTIVATE",i);
        return 0;
    }

    /*function*/
    public static int TorchActivate(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("TorchActivate",i);
        return 0;
    }

    /*function*/
    public static int Deactivate(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("Deactivate",i);
        return 0;
    }

    /*function*/
    public static int ReleaseAnimation(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ReleaseAnimation",i);
        return 0;
    }

    /*function*/
    public static int HoldAnimation(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("HoldAnimation",i);
        return 0;
    }

    /*function*/
    public static int PauseAnimation(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PauseAnimation",i);
        return 0;
    }

    /*function*/
    public static int SetAction(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SetAction",i);
        return 0;
    }

    /*function*/
    public static int ResetAction(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ResetAction",i);
        return 0;
    }

    /*function*/
    public static int ResetAllAction(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ResetAllAction",i);
        return 0;
    }

    /*task*/
    public static int PushAnimation(uint caller, bool multitask, int[] i /*2*/)
    {
        ThrowNIMPL("PushAnimation",i);
        // i[0]: animationId
        // i[1]: firstFrame
        return 0;
    }

    /*task*/
    public static int PushControlAnimation(uint caller, bool multitask, int[] i /*2*/)
    {
        ThrowNIMPL("PushControlAnimation",i);
        return 0;
    }

    /*task*/
    public static int WaitAnimFrame(uint caller, bool multitask, int[] i /*2*/)
    {
        ThrowNIMPL("WaitAnimFrame",i);
        // i[0]: animationId
        // i[1]: frame
        return 0;
    }

    /*task*/
    public static int WaitPlayerAnimFrame(uint caller, bool multitask, int[] i /*0*/)
    {
        ThrowNIMPL("WaitPlayerAnimFrame",i);
        // i[0]: animationId
        // i[1]: frame
        return 0;
    }

    /*function*/
    public static int KillMyTasks(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("KillMyTasks",i);
        return 0;
    }

    /*task*/
    public static int ObjectLineUp(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("ObjectLineUp",i);
        return 0;
    }

    /*task*/
    public static int ObjectParallelLineUp(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("ObjectParallelLineUp",i);
        return 0;
    }

    /*task*/
    public static int ObjectPickupLineUp(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("ObjectPickupLineUp",i);
        return 0;
    }

    /*task*/
    public static int LineUp(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("LineUp",i);
        return 0;
    }

    /*task*/
    public static int WaitLineUp(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("WaitLineUp",i);
        return 0;
    }

    /*function*/
    public static int MoveAboveMe(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("MoveAboveMe",i);
        return 0;
    }

    /*function*/
    public static int KillMe(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("KillMe",i);
        return 0;
    }

    /*function*/
    public static int EndSound(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("EndSound",i);
        return 0;
    }

    /*function*/
    public static int EndMySounds(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("EndMySounds",i);
        return 0;
    }

    /*function*/
    public static int EndMySound(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("EndMySound",i);
        return 0;
    }

    /*function*/
    public static int StopAllSounds(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("StopAllSounds",i);
        return 0;
    }

    /*task*/
    public static int ViewGXA(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("ViewGXA",i);
        return 0;
    }

    /*task*/
    public static int AnimateGXA(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("AnimateGXA",i);
        return 0;
    }

    /*function*/
    public static int PlayerOffset(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("PlayerOffset",i);
        return 0;
    }

    /*multitask*/
    public static int PlayerOffsettask(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("PlayerOffsettask",i);
        return 0;
    }

    /*function*/
    public static int Offset(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("Offset",i);
        return 0;
    }

    /*function*/
    public static int UnOffset(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("UnOffset",i);
        return 0;
    }

    /*task*/
    public static int BounceObject(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("BounceObject",i);
        return 0;
    }

    /*function*/
    public static int OffsetLocation(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("OffsetLocation",i);
        return 0;
    }

    /*function*/
    public static int DistanceToLocation(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("DistanceToLocation",i);
        return 0;
    }

    /*function*/
    public static int ShowBitmap(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("ShowBitmap",i);
        return 0;
    }

    /*function*/
    public static int UnShowBitmap(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("UnShowBitmap",i);
        return 0;
    }

    /*function*/
    public static int ReplenishHealth(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ReplenishHealth",i);
        return 0;
    }

    /*function*/
    public static int InRectangle(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("InRectangle",i);
        return 0;
    }

    /*function*/
    public static int InCircle(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("InCircle",i);
        return 0;
    }

    /*function*/
    public static int ScriptParticles(uint caller, bool multitask, int[] i /*8*/)	
    {
        ThrowNIMPL("ScriptParticles",i);
        return 0;
    }

    /*function*/
    public static int LightTorch(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("LightTorch",i);
        return 0;
    }

    /*function*/
    public static int UnlightTorch(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("UnlightTorch",i);
        return 0;
    }

    /*function*/
    public static int LoadStatic(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("LoadStatic",i);
        return 0;
    }

    /*function*/
    public static int UnLoadStatic(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("UnLoadStatic",i);
        return 0;
    }

    /*function*/
    public static int PointAt(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PointAt",i);
        return 0;
    }


    //combat commands
    /*function*/
    public static int beginCombat(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("beginCombat",i);
        return 0;
    }

    /*function*/
    public static int endCombat(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("endCombat",i);
        return 0;
    }

    /*function*/
    public static int isFighting(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("isFighting",i);
        return 0;
    }

    /*function*/
    public static int isDead(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("isDead",i);
        return 0;
    }

    /*function*/
    public static int adjustHealth(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("adjustHealth",i);
        return 0;
    }

    /*function*/
    public static int setHealth(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("setHealth",i);
        return 0;
    }

    /*function*/
    public static int health(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("health",i);
        return 0;
    }

    /*function*/
    public static int adjustStrength(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("adjustStrength",i);
        return 0;
    }

    /*function*/
    public static int setStrength(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("setStrength",i);
        return 0;
    }

    /*function*/
    public static int strength(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("strength",i);
        return 0;
    }

    /*function*/
    public static int adjustArmor(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("adjustArmor",i);
        return 0;
    }

    /*function*/
    public static int setArmor(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("setArmor",i);
        return 0;
    }

    /*function*/
    public static int armor(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("armor",i);
        return 0;
    }

    /*function*/
    public static int shoot(uint caller, bool multitask, int[] i /*7*/)	
    {
        ThrowNIMPL("shoot",i);
        return 0;
    }

    /*function*/
    public static int shootPlayer(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("shootPlayer",i);
        return 0;
    }

    /*function*/
    public static int resurrect(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("resurrect",i);
        return 0;
    }


    //object script/task control
    /*function*/
    public static int EnableObject(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("EnableObject",i);
        return 0;
    }

    /*function*/
    public static int DisableObject(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DisableObject",i);
        return 0;
    }

    /*function*/
    public static int EnableTasks(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("EnableTasks",i);
        return 0;
    }

    /*function*/
    public static int DisableTasks(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DisableTasks",i);
        return 0;
    }

    /*function*/
    public static int EnableScript(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("EnableScript",i);
        return 0;
    }

    /*function*/
    public static int DisableScript(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DisableScript",i);
        return 0;
    }

    /*function*/
    public static int EnableHook(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("EnableHook",i);
        return 0;
    }

    /*function*/
    public static int DisableHook(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DisableHook",i);
        return 0;
    }

    /*function*/
    public static int EnableCombat(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("EnableCombat",i);
        return 0;
    }

    /*function*/
    public static int DisableCombat(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DisableCombat",i);
        return 0;
    }


    //rotational and positional reset commands

    /*function*/
    public static int SetRotation(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("SetRotation",i);
        return 0;
    }

    /*function*/
    public static int SetPosition(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("SetPosition",i);
        return 0;
    }

    /*function*/
    public static int ResetRotation(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("ResetRotation",i);
        return 0;
    }

    /*function*/
    public static int ResetPosition(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("ResetPosition",i);
        return 0;
    }

    /*function*/
    public static int ReloadRotation(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ReloadRotation",i);
        return 0;
    }

    /*function*/
    public static int ReloadPosition(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ReloadPosition",i);
        return 0;
    }

    /*function*/
    public static int SetLocation(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SetLocation",i);
        return 0;
    }


    //master and slave commands
    /*function*/
    public static int AttachMe(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("AttachMe",i);
        return 0;
    }

    /*function*/
    public static int DetachMe(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DetachMe",i);
        return 0;
    }

    /*function*/
    public static int DetachSlaves(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DetachSlaves",i);
        return 0;
    }

    /*function*/
    public static int KillSlaves(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("KillSlaves",i);
        return 0;
    }

    /*function*/
    public static int HideSlaves(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("HideSlaves",i);
        return 0;
    }

    /*function*/
    public static int ShowSlaves(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ShowSlaves",i);
        return 0;
    }

    /*function*/
    public static int EnableSlaves(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("EnableSlaves",i);
        return 0;
    }

    /*function*/
    public static int DisableSlaves(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DisableSlaves",i);
        return 0;
    }

    /*function*/
    public static int IsSlave(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsSlave",i);
        return 0;
    }

    /*function*/
    public static int IsMaster(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsMaster",i);
        return 0;
    }

    /*function*/
    public static int FixSlaveAngle(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("FixSlaveAngle",i);
        return 0;
    }


    //player commands
    /*function*/
    public static int PlayerMoved(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerMoved",i);
        return 0;
    }

    /*function*/
    public static int PlayerTurned(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerTurned",i);
        return 0;
    }

    /*function*/
    public static int PlayerTurnedDirection(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerTurnedDirection",i);
        return 0;
    }

    /*function*/
    public static int PlayerMovementSpeed(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerMovementSpeed",i);
        return 0;
    }

    /*function*/
    public static int GetMovementSpeed(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("GetMovementSpeed",i);
        return 0;
    }

    /*function*/
    public static int PlayerHealth(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerHealth",i);
        return 0;
    }

    /*function*/
    public static int PlayerStatus(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerStatus",i);
        return 0;
    }

    /*function*/
    public static int PlayerCollide(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerCollide",i);
        return 0;
    }

    /*function*/
    public static int PlayerStand(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerStand",i);
        return 0;
    }

    /*function*/
    public static int PlayerArmed(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerArmed",i);
        return 0;
    }

    /*function*/
    public static int PlayerHanging(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerHanging",i);
        return 0;
    }

    /*function*/
    public static int PlayerInSight(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerInSight",i);
        return 0;
    }

    /*function*/
    public static int PlayerDistance(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerDistance",i);
        return 0;
    }

    /*function*/
    public static int AcuratePlayerDistance(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("AcuratePlayerDistance",i);
        return 0;
    }

    /*function*/
    public static int PlayerFacing(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("PlayerFacing",i);
        return 0;
    }

    /*function*/
    public static int PlayerArcTan(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerArcTan",i);
        return 0;
    }

    /*function*/
    public static int PlayerArc(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("PlayerArc",i);
        return 0;
    }

    /*function*/
    public static int PlayerLooking(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PlayerLooking",i);
        return 0;
    }

    /*multitask*/
    public static int PlayerTilt(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("PlayerTilt",i);
        return 0;
    }

    /*multitask*/
    public static int PlayerTiltRevx(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("PlayerTiltRevx",i);
        return 0;
    }

    /*function*/
    public static int PlayerSnap(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("PlayerSnap",i);
        return 0;
    }

    /*task*/
    public static int PlayerLineUp(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("PlayerLineUp",i);
        return 0;
    }

    /*task*/
    public static int PlayerParallelLineUp(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("PlayerParallelLineUp",i);
        return 0;
    }

    /*task*/
    public static int PlayerPickupLineUp(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("PlayerPickupLineUp",i);
        return 0;
    }

    /*function*/
    public static int MovePlayerAboveMe(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("MovePlayerAboveMe",i);
        return 0;
    }

    /*function*/
    public static int PushPlayer(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PushPlayer",i);
        return 0;
    }

    /*function*/
    public static int PopPlayer(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PopPlayer",i);
        return 0;
    }

    /*task*/
    public static int PlayerFaceObj(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerFaceObj",i);
        return 0;
    }

    /*function*/
    public static int PlayerOnScape(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerOnScape",i);
        return 0;
    }

    /*function*/
    public static int PlayerCollideStatic(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerCollideStatic",i);
        return 0;
    }

    /*function*/
    public static int WonGame(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("WonGame",i);
        return 0;
    }

    /*function*/
    public static int MainloopExit(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("MainloopExit",i);
        return 0;
    }

    /*task*/
    public static int PlayerFaceTalk(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PlayerFaceTalk",i);
        return 0;
    }

    /*function*/
    public static int PlayerForceTalk(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PlayerForceTalk",i);
        return 0;
    }

    /*function*/
    public static int CollidePlayerWeapon(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("CollidePlayerWeapon",i);
        return 0;
    }


    //object and player health
    /*function*/
    public static int ZapPlayer(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ZapPlayer",i);
        return 0;
    }

    /*function*/
    public static int ZapPlayerNoKill(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ZapPlayerNoKill",i);
        return 0;
    }

    /*function*/
    public static int ZapObject(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ZapObject",i);
        return 0;
    }

    /*function*/
    public static int ZapObjectNoKill(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ZapObjectNoKill",i);
        return 0;
    }


    //object system messaging
    /*function*/
    public static int SendMessage(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SendMessage",i);
        return 0;
    }

    /*function*/
    public static int ReceiveMessage(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ReceiveMessage",i);
        return 0;
    }

    /*function*/
    public static int IsMessageWaiting(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsMessageWaiting",i);
        return 0;
    }

    /*function*/
    public static int GetMessageSender(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("GetMessageSender",i);
        return 0;
    }

    /*function*/
    public static int GetMessageSenderId(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("GetMessageSenderId",i);
        return 0;
    }

    /*function*/
    public static int ReplySender(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ReplySender",i);
        return 0;
    }

    /*function*/
    public static int DeleteMessage(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DeleteMessage",i);
        return 0;
    }

    /*function*/
    public static int SendBroadcast(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SendBroadcast",i);
        return 0;
    }

    /*function*/
    public static int ReceiveBroadcast(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ReceiveBroadcast",i);
        return 0;
    }

    /*function*/
    public static int DeleteBroadcast(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DeleteBroadcast",i);
        return 0;
    }

    /*function*/
    public static int IsBroadcastWaiting(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsBroadcastWaiting",i);
        return 0;
    }

    /*function*/
    public static int GetBroadcastSender(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("GetBroadcastSender",i);
        return 0;
    }

    /*function*/
    public static int GetBroadcastSenderId(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("GetBroadcastSenderId",i);
        return 0;
    }

    /*function*/
    public static int ReplyBroadcastSender(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ReplyBroadcastSender",i);
        return 0;
    }


    //sync tasks and functions
    /*function*/
    public static int ResetGroupSync(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ResetGroupSync",i);
        return 0;
    }

    /*function*/
    public static int ResetUserSync(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ResetUserSync",i);
        return 0;
    }

    /*task*/
    public static int SyncWithGroup(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SyncWithGroup",i);
        // i[0]: sync group
        return 0;
    }

    /*task*/
    public static int SyncWithUser(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SyncWithUser",i);
        return 0;
    }

    /*function*/
    public static int ChangeGroup(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ChangeGroup",i);
        return 0;
    }

    /*function*/
    public static int ChangeUser(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ChangeUser",i);
        return 0;
    }


    //pipelining functions
    /*function*/
    public static int PipeMyGroup(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PipeMyGroup",i);
        return 0;
    }

    /*function*/
    public static int PipeGroup(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PipeGroup",i);
        return 0;
    }

    /*function*/
    public static int PipeUser(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PipeUser",i);
        return 0;
    }

    /*function*/
    public static int PipeMasters(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PipeMasters",i);
        return 0;
    }

    /*function*/
    public static int PipeSlaves(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PipeSlaves",i);
        return 0;
    }

    /*function*/
    public static int PipeAroundMe(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PipeAroundMe",i);
        return 0;
    }

    /*function*/
    public static int PipeAroundPos(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("PipeAroundPos",i);
        return 0;
    }

    /*function*/
    public static int PipeHidden(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PipeHidden",i);
        return 0;
    }


    //node map negotiation
    /*task*/
    public static int MoveNodeMarker(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("MoveNodeMarker",i);
        return 0;
    }

    /*task*/
    public static int MoveNodeLocation(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("MoveNodeLocation",i);
        return 0;
    }

    /*task*/
    public static int MoveNodePosition(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("MoveNodePosition",i);
        return 0;
    }

    /*function*/
    public static int AtNodeMarker(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("AtNodeMarker",i);
        return 0;
    }

    /*function*/
    public static int AtNodeLocation(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("AtNodeLocation",i);
        return 0;
    }

    /*function*/
    public static int AtNodePosition(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("AtNodePosition",i);
        return 0;
    }

    /*function*/
    public static int Wander(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("Wander",i);
        return 0;
    }

    /*function*/
    public static int DisableNodeMaps(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DisableNodeMaps",i);
        return 0;
    }

    /*function*/
    public static int EnableNodeMaps(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("EnableNodeMaps",i);
        return 0;
    }


    //node marker negotiation
    /*task*/
    public static int MoveMarker(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("MoveMarker",i);
        return 0;
    }

    /*task*/
    public static int MoveLocation(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("MoveLocation",i);
        return 0;
    }

    /*task*/
    public static int MovePosition(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("MovePosition",i);
        return 0;
    }


    //weapons and hand-objects
    /*function*/
    public static int DrawSword(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DrawSword",i);
        return 0;
    }

    /*function*/
    public static int SheathSword(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("SheathSword",i);
        return 0;
    }

    /*function*/
    public static int CheckPlayerWeapon(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("CheckPlayerWeapon",i);
        return 0;
    }

    /*function*/
    public static int DisplayHandModel(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("DisplayHandModel",i);
        return 0;
    }

    /*function*/
    public static int DisplayHandItem(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("DisplayHandItem",i);
        return 0;
    }

    /*function*/
    public static int DisplayHiltModel(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("DisplayHiltModel",i);
        return 0;
    }

    /*function*/
    public static int DisplayHiltItem(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("DisplayHiltItem",i);
        return 0;
    }


    //item's and the inventory
    /*function*/
    public static int HandItem(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("HandItem",i);
        return 0;
    }

    /*function*/
    public static int HaveItem(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("HaveItem",i);
        return 0;
    }

    /*function*/
    public static int RemoveItem(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("RemoveItem",i);
        return 0;
    }

    /*function*/
    public static int ItemUsed(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ItemUsed",i);
        return 0;
    }

    /*function*/
    public static int PushItem(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("PushItem",i);
        return 0;
    }

    /*function*/
    public static int PopItem(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PopItem",i);
        return 0;
    }

    /*function*/
    public static int AddItem(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("AddItem",i);
        return 0;
    }

    /*function*/
    public static int SubItem(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("SubItem",i);
        return 0;
    }

    /*function*/
    public static int DropItem(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("DropItem",i);
        return 0;
    }

    /*function*/
    public static int UseItem(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("UseItem",i);
        return 0;
    }

    /*function*/
    public static int ShowItem(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("ShowItem",i);
        return 0;
    }

    /*function*/
    public static int ShowItemNoRtx(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("ShowItemNoRtx",i);
        return 0;
    }

    /*function*/
    public static int ActiveItem(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ActiveItem",i);
        return 0;
    }

    /*function*/
    public static int PushAllItems(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PushAllItems",i);
        return 0;
    }

    /*function*/
    public static int PopAllItems(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PopAllItems",i);
        return 0;
    }

    /*function*/
    public static int DropAllItems(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DropAllItems",i);
        return 0;
    }

    /*function*/
    public static int SelectItem(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SelectItem",i);
        return 0;
    }


    //general object queries
    /*function*/
    public static int IsOnEdge(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsOnEdge",i);
        return 0;
    }

    /*function*/
    public static int IsHoldingWeapon(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsHoldingWeapon",i);
        return 0;
    }

    /*function*/
    public static int IsSheathingSword(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsSheathingSword",i);
        return 0;
    }

    /*function*/
    public static int IsDrawingSword(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsDrawingSword",i);
        return 0;
    }

    /*function*/
    public static int IsDrawingOrSheathing(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsDrawingOrSheathing",i);
        return 0;
    }

    /*function*/
    public static int IsCarryingWeapon(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsCarryingWeapon",i);
        return 0;
    }

    /*function*/
    public static int IsCombatCapable(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsCombatCapable",i);
        return 0;
    }

    /*function*/
    public static int IsShellScript(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsShellScript",i);
        return 0;
    }

    /*function*/
    public static int IsWithinView(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsWithinView",i);
        return 0;
    }

    /*function*/
    public static int IsWithinMap(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsWithinMap",i);
        return 0;
    }

    /*function*/
    public static int IsInAir(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsInAir",i);
        return 0;
    }

    /*function*/
    public static int IsLightingOrUnlighting(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsLightingOrUnlighting",i);
        return 0;
    }

    /*function*/
    public static int IsUnlightingTorch(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsUnlightingTorch",i);
        return 0;
    }

    /*function*/
    public static int IsLightingTorch(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsLightingTorch",i);
        return 0;
    }

    /*function*/
    public static int IsInLava(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsInLava",i);
        return 0;
    }

    /*function*/
    public static int IsInDeepWater(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsInDeepWater",i);
        return 0;
    }

    /*function*/
    public static int IsInWater(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsInWater",i);
        return 0;
    }

    /*function*/
    public static int IsBouncing(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsBouncing",i);
        return 0;
    }


    //shell AI functions
    /*function*/
    public static int OpenShell(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("OpenShell",i);
        return 0;
    }

    /*function*/
    public static int CountAiShells(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("CountAiShells",i);
        return 0;
    }

    /*function*/
    public static int FindAiShell(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("FindAiShell",i);
        return 0;
    }


    //effects system
    /*function*/
    public static int AddFlatEffect(uint caller, bool multitask, int[] i /*6*/)	
    {
        ThrowNIMPL("AddFlatEffect",i);
        return 0;
    }

    /*function*/
    public static int AddLightEffect(uint caller, bool multitask, int[] i /*8*/)	
    {
        ThrowNIMPL("AddLightEffect",i);
        return 0;
    }

    /*function*/
    public static int GolemSteam(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("GolemSteam",i);
        return 0;
    }

    /*function*/
    public static int DragonBreath(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DragonBreath",i);
        return 0;
    }

    /*multitask*/
    public static int DragonBreathTask(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DragonBreathTask",i);
        return 0;
    }


    //Smacker functions
    /*function*/
    public static int QueueMovie(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("QueueMovie",i);
        return 0;
    }


    /*task*/
    public static int TestTask(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("TestTask",i);
        return 0;
    }

    /*function*/
    public static int XResetCamera(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("XResetCamera",i);
        return 0;
    }


    //Potion functions
    /*function*/
    public static int SetPotion(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("SetPotion",i);
        return 0;
    }

    /*function*/
    public static int GetPotion(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("GetPotion",i);
        return 0;
    }


    //Log Book functions
    /*function*/
    public static int AddLog(uint caller, bool multitask, int[] i /*1*/)	
    {
        // Adds text to the logbook
        // TODO: check this
        // i[0]: index to RTX item to log
        ThrowNIMPL("AddLog",i);
        return 0;
    }


    //spare expansion functions, (do nothing at the moment)
    /*function*/
    public static int Spare0Parm0(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("Spare0Parm0",i);
        return 0;
    }
    public static int Spare1Parm0(uint caller, bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("Spare1Parm0",i);
        return 0;
    }

    /*function*/
    public static int Spare0Parm1(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("Spare0Parm1",i);
        return 0;
    }

    /*function*/
    public static int Spare1Parm1(uint caller, bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("Spare1Parm1",i);
        return 0;
    }

    /*function*/
    public static int Spare0Parm2(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("Spare0Parm2",i);
        return 0;
    }

    /*function*/
    public static int Spare1Parm2(uint caller, bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("Spare1Parm2",i);
        return 0;
    }

    /*function*/
    public static int Spare0Parm3(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("Spare0Parm3",i);
        return 0;
    }

    /*function*/
    public static int Spare1Parm3(uint caller, bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("Spare1Parm3",i);
        return 0;
    }

    /*function*/
    public static int Spare0Parm4(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("Spare0Parm4",i);
        return 0;
    }

    /*function*/
    public static int Spare1Parm4(uint caller, bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("Spare1Parm4",i);
        return 0;
    }

    /*function*/
    public static int Spare0Parm5(uint caller, bool multitask, int[] i /*5*/)	
    {
        ThrowNIMPL("Spare0Parm5",i);
        return 0;
    }

    /*function*/
    public static int Spare1Parm5(uint caller, bool multitask, int[] i /*5*/)	
    {
        ThrowNIMPL("Spare1Parm5",i);
        return 0;
    }

    /*function*/
    public static int Spare0Parm6(uint caller, bool multitask, int[] i /*6*/)	
    {
        ThrowNIMPL("Spare0Parm6",i);
        return 0;
    }

    /*function*/
    public static int Spare1Parm6(uint caller, bool multitask, int[] i /*6*/)	
    {
        ThrowNIMPL("Spare1Parm6",i);
        return 0;
    }

    /*function*/
    public static int Spare0Parm7(uint caller, bool multitask, int[] i /*7*/)	
    {
        ThrowNIMPL("Spare0Parm7",i);
        return 0;
    }

    /*function*/
    public static int Spare1Parm7(uint caller, bool multitask, int[] i /*8*/)	
    {
        ThrowNIMPL("Spare1Parm7",i);
        return 0;
    }
    static Func<uint, bool, int[], int>[] soupdefFcn;
    public static Func<uint, bool, int[], int>[] getNIMPLS()
    {
        if(soupdefFcn == null)
        {
            soupdefFcn = new Func<uint, bool, int[], int>[367];
            soupdefFcn[0]=PLACEHOLDER_ZERO;
            soupdefFcn[1]=sRotate;
            soupdefFcn[2]=sRotateByAxis;
            soupdefFcn[3]=sRotateToAxis;
            soupdefFcn[4]=sMove;
            soupdefFcn[5]=sMoveByAxis;
            soupdefFcn[6]=sMoveAlongAxis;
            soupdefFcn[7]=HeaderHook;
            soupdefFcn[8]=swordAI;
            soupdefFcn[9]=projectileAI;
            soupdefFcn[10]=chargeWeapon;
            soupdefFcn[11]=PlayerMain;
            soupdefFcn[12]=PrintParms;
            soupdefFcn[13]=LogParms;
            soupdefFcn[14]=PrintStringParm;
            soupdefFcn[15]=PrintSingleParm;
            soupdefFcn[16]=SpacePressed;
            soupdefFcn[17]=WaitOnTasks;
            soupdefFcn[18]=updatePlayerViewAttrib;
            soupdefFcn[19]=cameraController;
            soupdefFcn[20]=showObjRot;
            soupdefFcn[21]=showObj;
            soupdefFcn[22]=showObjLoc;
            soupdefFcn[23]=showObjPan;
            soupdefFcn[24]=showObjPanLoc;
            soupdefFcn[25]=lookObj;
            soupdefFcn[26]=showPlayer;
            soupdefFcn[27]=showPlayerPan;
            soupdefFcn[28]=lookCyrus;
            soupdefFcn[29]=showCyrus;
            soupdefFcn[30]=showCyrusLoc;
            soupdefFcn[31]=showCyrusPan;
            soupdefFcn[32]=showCyrusPanLoc;
            soupdefFcn[33]=PlayAnimation;
            soupdefFcn[34]=lockoutPlayer;
            soupdefFcn[35]=menuNew;
            soupdefFcn[36]=menuProc;
            soupdefFcn[37]=menuAddItem;
            soupdefFcn[38]=menuSelection;
            soupdefFcn[39]=RTX;
            soupdefFcn[40]=rtxAnim;
            soupdefFcn[41]=RTXpAnim;
            soupdefFcn[42]=RTXp;
            soupdefFcn[43]=Rotate;
            soupdefFcn[44]=RotateByAxis;
            soupdefFcn[45]=RotateToAxis;
            soupdefFcn[46]=WalkForward;
            soupdefFcn[47]=WalkBackward;
            soupdefFcn[48]=MoveForward;
            soupdefFcn[49]=MoveBackward;
            soupdefFcn[50]=MoveLeft;
            soupdefFcn[51]=MoveRight;
            soupdefFcn[52]=Move;
            soupdefFcn[53]=MoveByAxis;
            soupdefFcn[54]=MoveAlongAxis;
            soupdefFcn[55]=MoveObjectAxis;
            soupdefFcn[56]=MoveToLocation;
            soupdefFcn[57]=WanderToLocation;
            soupdefFcn[58]=MoveToMarker;
            soupdefFcn[59]=SetObjectLocation;
            soupdefFcn[60]=Wait;
            soupdefFcn[61]=DistanceFromStart;
            soupdefFcn[62]=Light;
            soupdefFcn[63]=LightRadius;
            soupdefFcn[64]=LightIntensity;
            soupdefFcn[65]=LightOff;
            soupdefFcn[66]=LightOffset;
            soupdefFcn[67]=LightFlicker;
            soupdefFcn[68]=FlickerLight;
            soupdefFcn[69]=LightFlickerOff;
            soupdefFcn[70]=LightSize;
            soupdefFcn[71]=LightSizeOff;
            soupdefFcn[72]=FxPhase;
            soupdefFcn[73]=FxFlickerOnOff;
            soupdefFcn[74]=FxFlickerDim;
            soupdefFcn[75]=FxLightSize;
            soupdefFcn[76]=Flat;
            soupdefFcn[77]=FlatSetTexture;
            soupdefFcn[78]=FlatOff;
            soupdefFcn[79]=FlatOffset;
            soupdefFcn[80]=FlatLikeStatic;
            soupdefFcn[81]=FlatAnimate;
            soupdefFcn[82]=FlatStop;
            soupdefFcn[83]=SetAttribute;
            soupdefFcn[84]=GetAttribute;
            soupdefFcn[85]=SetGlobalFlag;
            soupdefFcn[86]=TestGlobalFlag;
            soupdefFcn[87]=ResetGlobalFlag;
            soupdefFcn[88]=FacePlayer;
            soupdefFcn[89]=FacePlayerInertia;
            soupdefFcn[90]=FaceAngle;
            soupdefFcn[91]=FacePos;
            soupdefFcn[92]=FaceObject;
            soupdefFcn[93]=Sound;
            soupdefFcn[94]=FlatSound;
            soupdefFcn[95]=AmbientSound;
            soupdefFcn[96]=AmbientRtx;
            soupdefFcn[97]=Jump;
            soupdefFcn[98]=WaitNonMulti;
            soupdefFcn[99]=WaitOnDialog;
            soupdefFcn[100]=HideMe;
            soupdefFcn[101]=ShowMe;
            soupdefFcn[102]=Rnd;
            soupdefFcn[103]=QuickRnd;
            soupdefFcn[104]=MyId;
            soupdefFcn[105]=FaceLocation;
            soupdefFcn[106]=DistanceFromLocation;
            soupdefFcn[107]=LoadWorld;
            soupdefFcn[108]=SetAiType;
            soupdefFcn[109]=SetAiMode;
            soupdefFcn[110]=MyAiType;
            soupdefFcn[111]=MyAiMode;
            soupdefFcn[112]=GotoLocation;
            soupdefFcn[113]=Guard;
            soupdefFcn[114]=Animal;
            soupdefFcn[115]=TrueFunction;
            soupdefFcn[116]=TurnToPos;
            soupdefFcn[117]=AtPos;
            soupdefFcn[118]=TurnToAngle;
            soupdefFcn[119]=MovePos;
            soupdefFcn[120]=SetStartPosition;
            soupdefFcn[121]=GetMyAttr;
            soupdefFcn[122]=SetMyAttr;
            soupdefFcn[123]=GetFramePassed;
            soupdefFcn[124]=SpeedScale;
            soupdefFcn[125]=InKey;
            soupdefFcn[126]=PressKey;
            soupdefFcn[127]=ACTIVATE;
            soupdefFcn[128]=TorchActivate;
            soupdefFcn[129]=Deactivate;
            soupdefFcn[130]=ReleaseAnimation;
            soupdefFcn[131]=HoldAnimation;
            soupdefFcn[132]=PauseAnimation;
            soupdefFcn[133]=SetAction;
            soupdefFcn[134]=ResetAction;
            soupdefFcn[135]=ResetAllAction;
            soupdefFcn[136]=PushAnimation;
            soupdefFcn[137]=PushControlAnimation;
            soupdefFcn[138]=WaitAnimFrame;
            soupdefFcn[139]=WaitPlayerAnimFrame;
            soupdefFcn[140]=KillMyTasks;
            soupdefFcn[141]=ObjectLineUp;
            soupdefFcn[142]=ObjectParallelLineUp;
            soupdefFcn[143]=ObjectPickupLineUp;
            soupdefFcn[144]=LineUp;
            soupdefFcn[145]=WaitLineUp;
            soupdefFcn[146]=MoveAboveMe;
            soupdefFcn[147]=KillMe;
            soupdefFcn[148]=EndSound;
            soupdefFcn[149]=EndMySounds;
            soupdefFcn[150]=EndMySound;
            soupdefFcn[151]=StopAllSounds;
            soupdefFcn[152]=ViewGXA;
            soupdefFcn[153]=AnimateGXA;
            soupdefFcn[154]=PlayerOffset;
            soupdefFcn[155]=PlayerOffsettask;
            soupdefFcn[156]=Offset;
            soupdefFcn[157]=UnOffset;
            soupdefFcn[158]=BounceObject;
            soupdefFcn[159]=OffsetLocation;
            soupdefFcn[160]=DistanceToLocation;
            soupdefFcn[161]=ShowBitmap;
            soupdefFcn[162]=UnShowBitmap;
            soupdefFcn[163]=ReplenishHealth;
            soupdefFcn[164]=InRectangle;
            soupdefFcn[165]=InCircle;
            soupdefFcn[166]=ScriptParticles;
            soupdefFcn[167]=LightTorch;
            soupdefFcn[168]=UnlightTorch;
            soupdefFcn[169]=LoadStatic;
            soupdefFcn[170]=UnLoadStatic;
            soupdefFcn[171]=PointAt;
            soupdefFcn[172]=beginCombat;
            soupdefFcn[173]=endCombat;
            soupdefFcn[174]=isFighting;
            soupdefFcn[175]=isDead;
            soupdefFcn[176]=adjustHealth;
            soupdefFcn[177]=setHealth;
            soupdefFcn[178]=health;
            soupdefFcn[179]=adjustStrength;
            soupdefFcn[180]=setStrength;
            soupdefFcn[181]=strength;
            soupdefFcn[182]=adjustArmor;
            soupdefFcn[183]=setArmor;
            soupdefFcn[184]=armor;
            soupdefFcn[185]=shoot;
            soupdefFcn[186]=shootPlayer;
            soupdefFcn[187]=resurrect;
            soupdefFcn[188]=EnableObject;
            soupdefFcn[189]=DisableObject;
            soupdefFcn[190]=EnableTasks;
            soupdefFcn[191]=DisableTasks;
            soupdefFcn[192]=EnableScript;
            soupdefFcn[193]=DisableScript;
            soupdefFcn[194]=EnableHook;
            soupdefFcn[195]=DisableHook;
            soupdefFcn[196]=EnableCombat;
            soupdefFcn[197]=DisableCombat;
            soupdefFcn[198]=SetRotation;
            soupdefFcn[199]=SetPosition;
            soupdefFcn[200]=ResetRotation;
            soupdefFcn[201]=ResetPosition;
            soupdefFcn[202]=ReloadRotation;
            soupdefFcn[203]=ReloadPosition;
            soupdefFcn[204]=SetLocation;
            soupdefFcn[205]=AttachMe;
            soupdefFcn[206]=DetachMe;
            soupdefFcn[207]=DetachSlaves;
            soupdefFcn[208]=KillSlaves;
            soupdefFcn[209]=HideSlaves;
            soupdefFcn[210]=ShowSlaves;
            soupdefFcn[211]=EnableSlaves;
            soupdefFcn[212]=DisableSlaves;
            soupdefFcn[213]=IsSlave;
            soupdefFcn[214]=IsMaster;
            soupdefFcn[215]=FixSlaveAngle;
            soupdefFcn[216]=PlayerMoved;
            soupdefFcn[217]=PlayerTurned;
            soupdefFcn[218]=PlayerTurnedDirection;
            soupdefFcn[219]=PlayerMovementSpeed;
            soupdefFcn[220]=GetMovementSpeed;
            soupdefFcn[221]=PlayerHealth;
            soupdefFcn[222]=PlayerStatus;
            soupdefFcn[223]=PlayerCollide;
            soupdefFcn[224]=PlayerStand;
            soupdefFcn[225]=PlayerArmed;
            soupdefFcn[226]=PlayerHanging;
            soupdefFcn[227]=PlayerInSight;
            soupdefFcn[228]=PlayerDistance;
            soupdefFcn[229]=AcuratePlayerDistance;
            soupdefFcn[230]=PlayerFacing;
            soupdefFcn[231]=PlayerArcTan;
            soupdefFcn[232]=PlayerArc;
            soupdefFcn[233]=PlayerLooking;
            soupdefFcn[234]=PlayerTilt;
            soupdefFcn[235]=PlayerTiltRevx;
            soupdefFcn[236]=PlayerSnap;
            soupdefFcn[237]=PlayerLineUp;
            soupdefFcn[238]=PlayerParallelLineUp;
            soupdefFcn[239]=PlayerPickupLineUp;
            soupdefFcn[240]=MovePlayerAboveMe;
            soupdefFcn[241]=PushPlayer;
            soupdefFcn[242]=PopPlayer;
            soupdefFcn[243]=PlayerFaceObj;
            soupdefFcn[244]=PlayerOnScape;
            soupdefFcn[245]=PlayerCollideStatic;
            soupdefFcn[246]=WonGame;
            soupdefFcn[247]=MainloopExit;
            soupdefFcn[248]=PlayerFaceTalk;
            soupdefFcn[249]=PlayerForceTalk;
            soupdefFcn[250]=CollidePlayerWeapon;
            soupdefFcn[251]=ZapPlayer;
            soupdefFcn[252]=ZapPlayerNoKill;
            soupdefFcn[253]=ZapObject;
            soupdefFcn[254]=ZapObjectNoKill;
            soupdefFcn[255]=SendMessage;
            soupdefFcn[256]=ReceiveMessage;
            soupdefFcn[257]=IsMessageWaiting;
            soupdefFcn[258]=GetMessageSender;
            soupdefFcn[259]=GetMessageSenderId;
            soupdefFcn[260]=ReplySender;
            soupdefFcn[261]=DeleteMessage;
            soupdefFcn[262]=SendBroadcast;
            soupdefFcn[263]=ReceiveBroadcast;
            soupdefFcn[264]=DeleteBroadcast;
            soupdefFcn[265]=IsBroadcastWaiting;
            soupdefFcn[266]=GetBroadcastSender;
            soupdefFcn[267]=GetBroadcastSenderId;
            soupdefFcn[268]=ReplyBroadcastSender;
            soupdefFcn[269]=ResetGroupSync;
            soupdefFcn[270]=ResetUserSync;
            soupdefFcn[271]=SyncWithGroup;
            soupdefFcn[272]=SyncWithUser;
            soupdefFcn[273]=ChangeGroup;
            soupdefFcn[274]=ChangeUser;
            soupdefFcn[275]=PipeMyGroup;
            soupdefFcn[276]=PipeGroup;
            soupdefFcn[277]=PipeUser;
            soupdefFcn[278]=PipeMasters;
            soupdefFcn[279]=PipeSlaves;
            soupdefFcn[280]=PipeAroundMe;
            soupdefFcn[281]=PipeAroundPos;
            soupdefFcn[282]=PipeHidden;
            soupdefFcn[283]=MoveNodeMarker;
            soupdefFcn[284]=MoveNodeLocation;
            soupdefFcn[285]=MoveNodePosition;
            soupdefFcn[286]=AtNodeMarker;
            soupdefFcn[287]=AtNodeLocation;
            soupdefFcn[288]=AtNodePosition;
            soupdefFcn[289]=Wander;
            soupdefFcn[290]=DisableNodeMaps;
            soupdefFcn[291]=EnableNodeMaps;
            soupdefFcn[292]=MoveMarker;
            soupdefFcn[293]=MoveLocation;
            soupdefFcn[294]=MovePosition;
            soupdefFcn[295]=DrawSword;
            soupdefFcn[296]=SheathSword;
            soupdefFcn[297]=CheckPlayerWeapon;
            soupdefFcn[298]=DisplayHandModel;
            soupdefFcn[299]=DisplayHandItem;
            soupdefFcn[300]=DisplayHiltModel;
            soupdefFcn[301]=DisplayHiltItem;
            soupdefFcn[302]=HandItem;
            soupdefFcn[303]=HaveItem;
            soupdefFcn[304]=RemoveItem;
            soupdefFcn[305]=ItemUsed;
            soupdefFcn[306]=PushItem;
            soupdefFcn[307]=PopItem;
            soupdefFcn[308]=AddItem;
            soupdefFcn[309]=SubItem;
            soupdefFcn[310]=DropItem;
            soupdefFcn[311]=UseItem;
            soupdefFcn[312]=ShowItem;
            soupdefFcn[313]=ShowItemNoRtx;
            soupdefFcn[314]=ActiveItem;
            soupdefFcn[315]=PushAllItems;
            soupdefFcn[316]=PopAllItems;
            soupdefFcn[317]=DropAllItems;
            soupdefFcn[318]=SelectItem;
            soupdefFcn[319]=IsOnEdge;
            soupdefFcn[320]=IsHoldingWeapon;
            soupdefFcn[321]=IsSheathingSword;
            soupdefFcn[322]=IsDrawingSword;
            soupdefFcn[323]=IsDrawingOrSheathing;
            soupdefFcn[324]=IsCarryingWeapon;
            soupdefFcn[325]=IsCombatCapable;
            soupdefFcn[326]=IsShellScript;
            soupdefFcn[327]=IsWithinView;
            soupdefFcn[328]=IsWithinMap;
            soupdefFcn[329]=IsInAir;
            soupdefFcn[330]=IsLightingOrUnlighting;
            soupdefFcn[331]=IsUnlightingTorch;
            soupdefFcn[332]=IsLightingTorch;
            soupdefFcn[333]=IsInLava;
            soupdefFcn[334]=IsInDeepWater;
            soupdefFcn[335]=IsInWater;
            soupdefFcn[336]=IsBouncing;
            soupdefFcn[337]=OpenShell;
            soupdefFcn[338]=CountAiShells;
            soupdefFcn[339]=FindAiShell;
            soupdefFcn[340]=AddFlatEffect;
            soupdefFcn[341]=AddLightEffect;
            soupdefFcn[342]=GolemSteam;
            soupdefFcn[343]=DragonBreath;
            soupdefFcn[344]=DragonBreathTask;
            soupdefFcn[345]=QueueMovie;
            soupdefFcn[346]=TestTask;
            soupdefFcn[347]=XResetCamera;
            soupdefFcn[348]=SetPotion;
            soupdefFcn[349]=GetPotion;
            soupdefFcn[350]=AddLog;
            soupdefFcn[351]=Spare0Parm0;
            soupdefFcn[352]=Spare1Parm0;
            soupdefFcn[353]=Spare0Parm1;
            soupdefFcn[354]=Spare1Parm1;
            soupdefFcn[355]=Spare0Parm2;
            soupdefFcn[356]=Spare1Parm2;
            soupdefFcn[357]=Spare0Parm3;
            soupdefFcn[358]=Spare1Parm3;
            soupdefFcn[359]=Spare0Parm4;
            soupdefFcn[360]=Spare1Parm4;
            soupdefFcn[361]=Spare0Parm5;
            soupdefFcn[362]=Spare1Parm5;
            soupdefFcn[363]=Spare0Parm6;
            soupdefFcn[364]=Spare1Parm6;
            soupdefFcn[365]=Spare0Parm7;
            soupdefFcn[366]=Spare1Parm7;
        }
        return soupdefFcn;
    }

    public static Func<uint, bool, int[], int> getNIMPL(int i)
    {
        if(soupdefFcn == null)
        {
            getNIMPLS();
        }
        return soupdefFcn[i];
    }

}
