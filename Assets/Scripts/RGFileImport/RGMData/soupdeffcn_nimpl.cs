//RedGuard Global function, flag and equate definition file
//functions section.
//functions defined as xxx parms y
//where xxx is an ascii name for the function and y is the number of parameters
//the function takes.

// 100% speculation:
// tasks take a while to complete
// functions return immediately
using System;

public class soupdeffcn_nimpl
{
    public static void ThrowNIMPL(string s, int[] i)
    {
        throw new Exception($"NIMPL: {s}({string.Join(",",i)})");
    }
    public static int PLACEHOLDER_ZERO(bool multitask, int[] i /*4*/)
    {
        ThrowNIMPL("PLACEHOLDER_ZERO",i);
        return 0;
    }
    /*task*/
    public static int sRotate(bool multitask, int[] i /*4*/)
    {
        ThrowNIMPL("sRotate",i);
        return 0;
    }

    /*task*/
    public static int sRotateByAxis(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("sRotateByAxis",i);
        return 0;
    }

    /*task*/
    public static int sRotateToAxis(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("sRotateToAxis",i);
        return 0;
    }

    /*task*/
    public static int sMove(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("sMove",i);
        return 0;
    }

    /*task*/
    public static int sMoveByAxis(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("sMoveByAxis",i);
        return 0;
    }

    /*task*/
    public static int sMoveAlongAxis(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("sMoveAlongAxis",i);
        return 0;
    }

    /*function*/
    public static int HeaderHook(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("HeaderHook",i);
        return 0;
    }

    /*task*/
    public static int swordAI(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("swordAI",i);
        return 0;
    }

    /*task*/
    public static int projectileAI(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("projectileAI",i);
        return 0;
    }

    /*function*/
    public static int chargeWeapon(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("chargeWeapon",i);
        return 0;
    }

    /*function*/
    public static int PlayerMain(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerMain",i);
        return 0;
    }

    /*function*/
    public static int PrintParms(bool multitask, int[] i /*8*/)	
    {
        ThrowNIMPL("PrintParms",i);
        return 0;
    }

    /*function*/
    public static int LogParms(bool multitask, int[] i /*8*/)	
    {
        ThrowNIMPL("LogParms",i);
        return 0;
    }

    /*function*/
    public static int PrintStringParm(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PrintStringParm",i);
        return 0;
    }

    /*function*/
    public static int PrintSingleParm(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PrintSingleParm",i);
        return 0;
    }

    /*function*/
    public static int SpacePressed(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("SpacePressed",i);
        return 0;
    }

    /*task*/
    public static int WaitOnTasks(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("WaitOnTasks",i);
        return 0;
    }

    /*function*/
    public static int updatePlayerViewAttrib(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("updatePlayerViewAttrib",i);
        return 0;
    }

    /*function*/
    public static int cameraController(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("cameraController",i);
        return 0;
    }

    /*task*/
    public static int showObjRot(bool multitask, int[] i /*8*/)	
    {
        ThrowNIMPL("showObjRot",i);
        return 0;
    }

    /*task*/
    public static int showObj(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("showObj",i);
        return 0;
    }

    /*task*/
    public static int showObjLoc(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("showObjLoc",i);
        return 0;
    }

    /*task*/
    public static int showObjPan(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("showObjPan",i);
        return 0;
    }

    /*task*/
    public static int showObjPanLoc(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("showObjPanLoc",i);
        return 0;
    }

    /*task*/
    public static int lookObj(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("lookObj",i);
        return 0;
    }

    /*task*/
    public static int showPlayer(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("showPlayer",i);
        return 0;
    }

    /*task*/
    public static int showPlayerPan(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("showPlayerPan",i);
        return 0;
    }

    /*task*/
    public static int lookCyrus(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("lookCyrus",i);
        return 0;
    }

    /*task*/
    public static int showCyrus(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("showCyrus",i);
        return 0;
    }

    /*task*/
    public static int showCyrusLoc(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("showCyrusLoc",i);
        return 0;
    }

    /*task*/
    public static int showCyrusPan(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("showCyrusPan",i);
        return 0;
    }

    /*task*/
    public static int showCyrusPanLoc(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("showCyrusPanLoc",i);
        return 0;
    }

    /*function*/
    public static int PlayAnimation(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("PlayAnimation",i);
        return 0;
    }

    /*function*/
    public static int lockoutPlayer(bool multitask, int[] i /*1*/)	
    {
        // Enable/disable player input
        // i[0]: 1 disables input, 0 enables it
        ThrowNIMPL("lockoutPlayer",i);
        return 0;
    }

    /*function*/
    public static int menuNew(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("menuNew",i);
        return 0;
    }

    /*task*/
    public static int menuProc(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("menuProc",i);
        return 0;
    }

    /*function*/
    public static int menuAddItem(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("menuAddItem",i);
        return 0;
    }

    /*function*/
    public static int menuSelection(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("menuSelection",i);
        return 0;
    }

    /*task*/
    public static int RTX(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("RTX",i);
        return 0;
    }

    /*task*/
    public static int rtxAnim(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("rtxAnim",i);
        return 0;
    }

    /*task*/
    public static int RTXpAnim(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("RTXpAnim",i);
        return 0;
    }

    /*task*/
    public static int RTXp(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("RTXp",i);
        return 0;
    }

    /*task*/
    public static int Rotate(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("Rotate",i);
        return 0;
    }

    /*task*/
    public static int RotateByAxis(bool multitask, int[] i /*3*/)	
    {
        // i[0]: axis (0/1/2)
        // i[1]: amount
        // i[3]: time to complete
        ThrowNIMPL("RotateByAxis",i);
        return 0;
    }

    /*task*/
    public static int RotateToAxis(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("RotateToAxis",i);
        return 0;
    }

    /*task*/
    public static int WalkForward(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("WalkForward",i);
        return 0;
    }

    /*task*/
    public static int WalkBackward(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("WalkBackward",i);
        return 0;
    }

    /*task*/
    public static int MoveForward(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("MoveForward",i);
        return 0;
    }

    /*task*/
    public static int MoveBackward(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("MoveBackward",i);
        return 0;
    }

    /*task*/
    public static int MoveLeft(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("MoveLeft",i);
        return 0;
    }

    /*task*/
    public static int MoveRight(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("MoveRight",i);
        return 0;
    }

    /*task*/
    public static int Move(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("Move",i);
        return 0;
    }

    /*task*/
    public static int MoveByAxis(bool multitask, int[] i /*3*/)	
    {
        // i[0]: axis (0/1/2)
        // i[1]: amount
        // i[3]: time to complete
        ThrowNIMPL("MoveByAxis",i);
        return 0;
    }

    /*task*/
    public static int MoveAlongAxis(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("MoveAlongAxis",i);
        return 0;
    }

    /*task*/
    public static int MoveObjectAxis(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("MoveObjectAxis",i);
        return 0;
    }

    /*task*/
    public static int MoveToLocation(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("MoveToLocation",i);
        return 0;
    }

    /*task*/
    public static int WanderToLocation(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("WanderToLocation",i);
        return 0;
    }

    /*task*/
    public static int MoveToMarker(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("MoveToMarker",i);
        return 0;
    }

    /*function*/
    public static int SetObjectLocation(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("SetObjectLocation",i);
        return 0;
    }

    /*task*/
    public static int Wait(bool multitask, int[] i /*1*/)	
    {
        // i[0]: time to wait
        ThrowNIMPL("Wait",i);
        return 0;
    }

    /*function*/
    public static int DistanceFromStart(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("DistanceFromStart",i);
        return 0;
    }

    /*function*/
    public static int Light(bool multitask, int[] i /*2*/)	
    {
        // Turns on a light
        // i[0]: light radius
        // i[1]: light intensity
        ThrowNIMPL("Light",i);
        return 0;
    }

    /*function*/
    public static int LightRadius(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("LightRadius",i);
        return 0;
    }

    /*function*/
    public static int LightIntensity(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("LightIntensity",i);
        return 0;
    }

    /*function*/
    public static int LightOff(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("LightOff",i);
        return 0;
    }

    /*function*/
    public static int LightOffset(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("LightOffset",i);
        return 0;
    }

    /*function*/
    public static int LightFlicker(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("LightFlicker",i);
        return 0;
    }

    /*function*/
    public static int FlickerLight(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("FlickerLight",i);
        return 0;
    }

    /*function*/
    public static int LightFlickerOff(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("LightFlickerOff",i);
        return 0;
    }

    /*function*/
    public static int LightSize(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("LightSize",i);
        return 0;
    }

    /*function*/
    public static int LightSizeOff(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("LightSizeOff",i);
        return 0;
    }

    /*multitask*/
    public static int FxPhase(bool multitask, int[] i /*7*/)	
    {
        ThrowNIMPL("FxPhase",i);
        return 0;
    }

    /*multitask*/
    public static int FxFlickerOnOff(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("FxFlickerOnOff",i);
        return 0;
    }

    /*multitask*/
    public static int FxFlickerDim(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("FxFlickerDim",i);
        return 0;
    }

    /*multitask*/
    public static int FxLightSize(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("FxLightSize",i);
        return 0;
    }

    /*function*/
    public static int Flat(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("Flat",i);
        return 0;
    }

    /*function*/
    public static int FlatSetTexture(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("FlatSetTexture",i);
        return 0;
    }

    /*function*/
    public static int FlatOff(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("FlatOff",i);
        return 0;
    }

    /*function*/
    public static int FlatOffset(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("FlatOffset",i);
        return 0;
    }

    /*function*/
    public static int FlatLikeStatic(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("FlatLikeStatic",i);
        return 0;
    }

    /*function*/
    public static int FlatAnimate(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("FlatAnimate",i);
        return 0;
    }

    /*function*/
    public static int FlatStop(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("FlatStop",i);
        return 0;
    }

    /*function*/
    public static int SetAttribute(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("SetAttribute",i);
        return 0;
    }

    /*function*/
    public static int GetAttribute(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("GetAttribute",i);
        return 0;
    }

    /*function*/
    public static int SetGlobalFlag(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SetGlobalFlag",i);
        return 0;
    }

    /*function*/
    public static int TestGlobalFlag(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("TestGlobalFlag",i);
        return 0;
    }

    /*function*/
    public static int ResetGlobalFlag(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ResetGlobalFlag",i);
        return 0;
    }

    /*task*/
    public static int FacePlayer(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("FacePlayer",i);
        return 0;
    }

    /*task*/
    public static int FacePlayerInertia(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("FacePlayerInertia",i);
        return 0;
    }

    /*task*/
    public static int FaceAngle(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("FaceAngle",i);
        return 0;
    }

    /*task*/
    public static int FacePos(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("FacePos",i);
        return 0;
    }

    /*task*/
    public static int FaceObject(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("FaceObject",i);
        return 0;
    }

    /*function*/
    public static int Sound(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("Sound",i);
        return 0;
    }

    /*function*/
    public static int FlatSound(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("FlatSound",i);
        return 0;
    }

    /*multitask*/
    public static int AmbientSound(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("AmbientSound",i);
        return 0;
    }

    /*multitask*/
    public static int AmbientRtx(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("AmbientRtx",i);
        return 0;
    }

    /*function*/
    public static int Jump(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("Jump",i);
        return 0;
    }

    /*task*/
    public static int WaitNonMulti(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("WaitNonMulti",i);
        return 0;
    }

    /*task*/
    public static int WaitOnDialog(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("WaitOnDialog",i);
        return 0;
    }

    /*function*/
    public static int HideMe(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("HideMe",i);
        return 0;
    }

    /*function*/
    public static int ShowMe(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ShowMe",i);
        return 0;
    }

    /*function*/
    public static int Rnd(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("Rnd",i);
        return 0;
    }

    /*function*/
    public static int QuickRnd(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("QuickRnd",i);
        return 0;
    }

    /*function*/
    public static int MyId(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("MyId",i);
        return 0;
    }

    /*task*/
    public static int FaceLocation(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("FaceLocation",i);
        return 0;
    }

    /*function*/
    public static int DistanceFromLocation(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("DistanceFromLocation",i);
        return 0;
    }

    /*function*/
    public static int LoadWorld(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("LoadWorld",i);
        return 0;
    }

    /*function*/
    public static int SetAiType(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SetAiType",i);
        return 0;
    }

    /*function*/
    public static int SetAiMode(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SetAiMode",i);
        return 0;
    }

    /*function*/
    public static int MyAiType(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("MyAiType",i);
        return 0;
    }

    /*function*/
    public static int MyAiMode(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("MyAiMode",i);
        return 0;
    }

    /*task*/
    public static int GotoLocation(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("GotoLocation",i);
        return 0;
    }

    /*task*/
    public static int Guard(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("Guard",i);
        return 0;
    }

    /*task*/
    public static int Animal(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("Animal",i);
        return 0;
    }

    /*function*/
    public static int TrueFunction(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("TrueFunction",i);
        return 0;
    }

    /*function*/
    public static int TurnToPos(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("TurnToPos",i);
        return 0;
    }

    /*function*/
    public static int AtPos(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("AtPos",i);
        return 0;
    }

    /*function*/
    public static int TurnToAngle(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("TurnToAngle",i);
        return 0;
    }

    /*function*/
    public static int MovePos(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("MovePos",i);
        return 0;
    }

    /*function*/
    public static int SetStartPosition(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("SetStartPosition",i);
        return 0;
    }

    /*function*/
    public static int GetMyAttr(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("GetMyAttr",i);
        return 0;
    }

    /*function*/
    public static int SetMyAttr(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("SetMyAttr",i);
        return 0;
    }

    /*function*/
    public static int GetFramePassed(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("GetFramePassed",i);
        return 0;
    }

    /*function*/
    public static int SpeedScale(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SpeedScale",i);
        return 0;
    }

    /*function*/
    public static int InKey(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("InKey",i);
        return 0;
    }

    /*function*/
    public static int PressKey(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PressKey",i);
        return 0;
    }


    /*function in all caps?: FUNCTION	ACTIVATE	PARMS 1*/
    public static int ACTIVATE(bool multitask, int[] i /*1*/)	
    {
        // True if player activates an item
        // i[0]: Stringid shown on screen when looking at the item
        ThrowNIMPL("ACTIVATE",i);
        return 0;
    }

    /*function*/
    public static int TorchActivate(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("TorchActivate",i);
        return 0;
    }

    /*function*/
    public static int Deactivate(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("Deactivate",i);
        return 0;
    }

    /*function*/
    public static int ReleaseAnimation(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ReleaseAnimation",i);
        return 0;
    }

    /*function*/
    public static int HoldAnimation(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("HoldAnimation",i);
        return 0;
    }

    /*function*/
    public static int PauseAnimation(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PauseAnimation",i);
        return 0;
    }

    /*function*/
    public static int SetAction(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SetAction",i);
        return 0;
    }

    /*function*/
    public static int ResetAction(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ResetAction",i);
        return 0;
    }

    /*function*/
    public static int ResetAllAction(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ResetAllAction",i);
        return 0;
    }

    /*task*/
    public static int PushAnimation(bool multitask, int[] i /*2*/)
    {
        ThrowNIMPL("PushAnimation",i);
        // i[0]: animationId
        // i[1]: firstFrame
        return 0;
    }

    /*task*/
    public static int PushControlAnimation(bool multitask, int[] i /*2*/)
    {
        ThrowNIMPL("PushControlAnimation",i);
        return 0;
    }

    /*task*/
    public static int WaitAnimFrame(bool multitask, int[] i /*2*/)
    {
        ThrowNIMPL("WaitAnimFrame",i);
        // i[0]: animationId
        // i[1]: frame
        return 0;
    }

    /*task*/
    public static int WaitPlayerAnimFrame(bool multitask, int[] i /*0*/)
    {
        ThrowNIMPL("WaitPlayerAnimFrame",i);
        // i[0]: animationId
        // i[1]: frame
        return 0;
    }

    /*function*/
    public static int KillMyTasks(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("KillMyTasks",i);
        return 0;
    }

    /*task*/
    public static int ObjectLineUp(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("ObjectLineUp",i);
        return 0;
    }

    /*task*/
    public static int ObjectParallelLineUp(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("ObjectParallelLineUp",i);
        return 0;
    }

    /*task*/
    public static int ObjectPickupLineUp(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("ObjectPickupLineUp",i);
        return 0;
    }

    /*task*/
    public static int LineUp(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("LineUp",i);
        return 0;
    }

    /*task*/
    public static int WaitLineUp(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("WaitLineUp",i);
        return 0;
    }

    /*function*/
    public static int MoveAboveMe(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("MoveAboveMe",i);
        return 0;
    }

    /*function*/
    public static int KillMe(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("KillMe",i);
        return 0;
    }

    /*function*/
    public static int EndSound(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("EndSound",i);
        return 0;
    }

    /*function*/
    public static int EndMySounds(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("EndMySounds",i);
        return 0;
    }

    /*function*/
    public static int EndMySound(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("EndMySound",i);
        return 0;
    }

    /*function*/
    public static int StopAllSounds(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("StopAllSounds",i);
        return 0;
    }

    /*task*/
    public static int ViewGXA(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("ViewGXA",i);
        return 0;
    }

    /*task*/
    public static int AnimateGXA(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("AnimateGXA",i);
        return 0;
    }

    /*function*/
    public static int PlayerOffset(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("PlayerOffset",i);
        return 0;
    }

    /*multitask*/
    public static int PlayerOffsettask(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("PlayerOffsettask",i);
        return 0;
    }

    /*function*/
    public static int Offset(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("Offset",i);
        return 0;
    }

    /*function*/
    public static int UnOffset(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("UnOffset",i);
        return 0;
    }

    /*task*/
    public static int BounceObject(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("BounceObject",i);
        return 0;
    }

    /*function*/
    public static int OffsetLocation(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("OffsetLocation",i);
        return 0;
    }

    /*function*/
    public static int DistanceToLocation(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("DistanceToLocation",i);
        return 0;
    }

    /*function*/
    public static int ShowBitmap(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("ShowBitmap",i);
        return 0;
    }

    /*function*/
    public static int UnShowBitmap(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("UnShowBitmap",i);
        return 0;
    }

    /*function*/
    public static int ReplenishHealth(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ReplenishHealth",i);
        return 0;
    }

    /*function*/
    public static int InRectangle(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("InRectangle",i);
        return 0;
    }

    /*function*/
    public static int InCircle(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("InCircle",i);
        return 0;
    }

    /*function*/
    public static int ScriptParticles(bool multitask, int[] i /*8*/)	
    {
        ThrowNIMPL("ScriptParticles",i);
        return 0;
    }

    /*function*/
    public static int LightTorch(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("LightTorch",i);
        return 0;
    }

    /*function*/
    public static int UnlightTorch(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("UnlightTorch",i);
        return 0;
    }

    /*function*/
    public static int LoadStatic(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("LoadStatic",i);
        return 0;
    }

    /*function*/
    public static int UnLoadStatic(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("UnLoadStatic",i);
        return 0;
    }

    /*function*/
    public static int PointAt(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PointAt",i);
        return 0;
    }


    //combat commands
    /*function*/
    public static int beginCombat(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("beginCombat",i);
        return 0;
    }

    /*function*/
    public static int endCombat(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("endCombat",i);
        return 0;
    }

    /*function*/
    public static int isFighting(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("isFighting",i);
        return 0;
    }

    /*function*/
    public static int isDead(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("isDead",i);
        return 0;
    }

    /*function*/
    public static int adjustHealth(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("adjustHealth",i);
        return 0;
    }

    /*function*/
    public static int setHealth(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("setHealth",i);
        return 0;
    }

    /*function*/
    public static int health(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("health",i);
        return 0;
    }

    /*function*/
    public static int adjustStrength(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("adjustStrength",i);
        return 0;
    }

    /*function*/
    public static int setStrength(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("setStrength",i);
        return 0;
    }

    /*function*/
    public static int strength(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("strength",i);
        return 0;
    }

    /*function*/
    public static int adjustArmor(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("adjustArmor",i);
        return 0;
    }

    /*function*/
    public static int setArmor(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("setArmor",i);
        return 0;
    }

    /*function*/
    public static int armor(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("armor",i);
        return 0;
    }

    /*function*/
    public static int shoot(bool multitask, int[] i /*7*/)	
    {
        ThrowNIMPL("shoot",i);
        return 0;
    }

    /*function*/
    public static int shootPlayer(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("shootPlayer",i);
        return 0;
    }

    /*function*/
    public static int resurrect(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("resurrect",i);
        return 0;
    }


    //object script/task control
    /*function*/
    public static int EnableObject(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("EnableObject",i);
        return 0;
    }

    /*function*/
    public static int DisableObject(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DisableObject",i);
        return 0;
    }

    /*function*/
    public static int EnableTasks(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("EnableTasks",i);
        return 0;
    }

    /*function*/
    public static int DisableTasks(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DisableTasks",i);
        return 0;
    }

    /*function*/
    public static int EnableScript(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("EnableScript",i);
        return 0;
    }

    /*function*/
    public static int DisableScript(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DisableScript",i);
        return 0;
    }

    /*function*/
    public static int EnableHook(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("EnableHook",i);
        return 0;
    }

    /*function*/
    public static int DisableHook(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DisableHook",i);
        return 0;
    }

    /*function*/
    public static int EnableCombat(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("EnableCombat",i);
        return 0;
    }

    /*function*/
    public static int DisableCombat(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DisableCombat",i);
        return 0;
    }


    //rotational and positional reset commands

    /*function*/
    public static int SetRotation(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("SetRotation",i);
        return 0;
    }

    /*function*/
    public static int SetPosition(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("SetPosition",i);
        return 0;
    }

    /*function*/
    public static int ResetRotation(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("ResetRotation",i);
        return 0;
    }

    /*function*/
    public static int ResetPosition(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("ResetPosition",i);
        return 0;
    }

    /*function*/
    public static int ReloadRotation(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ReloadRotation",i);
        return 0;
    }

    /*function*/
    public static int ReloadPosition(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ReloadPosition",i);
        return 0;
    }

    /*function*/
    public static int SetLocation(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SetLocation",i);
        return 0;
    }


    //master and slave commands
    /*function*/
    public static int AttachMe(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("AttachMe",i);
        return 0;
    }

    /*function*/
    public static int DetachMe(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DetachMe",i);
        return 0;
    }

    /*function*/
    public static int DetachSlaves(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DetachSlaves",i);
        return 0;
    }

    /*function*/
    public static int KillSlaves(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("KillSlaves",i);
        return 0;
    }

    /*function*/
    public static int HideSlaves(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("HideSlaves",i);
        return 0;
    }

    /*function*/
    public static int ShowSlaves(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ShowSlaves",i);
        return 0;
    }

    /*function*/
    public static int EnableSlaves(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("EnableSlaves",i);
        return 0;
    }

    /*function*/
    public static int DisableSlaves(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DisableSlaves",i);
        return 0;
    }

    /*function*/
    public static int IsSlave(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsSlave",i);
        return 0;
    }

    /*function*/
    public static int IsMaster(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsMaster",i);
        return 0;
    }

    /*function*/
    public static int FixSlaveAngle(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("FixSlaveAngle",i);
        return 0;
    }


    //player commands
    /*function*/
    public static int PlayerMoved(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerMoved",i);
        return 0;
    }

    /*function*/
    public static int PlayerTurned(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerTurned",i);
        return 0;
    }

    /*function*/
    public static int PlayerTurnedDirection(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerTurnedDirection",i);
        return 0;
    }

    /*function*/
    public static int PlayerMovementSpeed(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerMovementSpeed",i);
        return 0;
    }

    /*function*/
    public static int GetMovementSpeed(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("GetMovementSpeed",i);
        return 0;
    }

    /*function*/
    public static int PlayerHealth(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerHealth",i);
        return 0;
    }

    /*function*/
    public static int PlayerStatus(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerStatus",i);
        return 0;
    }

    /*function*/
    public static int PlayerCollide(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerCollide",i);
        return 0;
    }

    /*function*/
    public static int PlayerStand(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerStand",i);
        return 0;
    }

    /*function*/
    public static int PlayerArmed(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerArmed",i);
        return 0;
    }

    /*function*/
    public static int PlayerHanging(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerHanging",i);
        return 0;
    }

    /*function*/
    public static int PlayerInSight(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerInSight",i);
        return 0;
    }

    /*function*/
    public static int PlayerDistance(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerDistance",i);
        return 0;
    }

    /*function*/
    public static int AcuratePlayerDistance(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("AcuratePlayerDistance",i);
        return 0;
    }

    /*function*/
    public static int PlayerFacing(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("PlayerFacing",i);
        return 0;
    }

    /*function*/
    public static int PlayerArcTan(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerArcTan",i);
        return 0;
    }

    /*function*/
    public static int PlayerArc(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("PlayerArc",i);
        return 0;
    }

    /*function*/
    public static int PlayerLooking(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PlayerLooking",i);
        return 0;
    }

    /*multitask*/
    public static int PlayerTilt(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("PlayerTilt",i);
        return 0;
    }

    /*multitask*/
    public static int PlayerTiltRevx(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("PlayerTiltRevx",i);
        return 0;
    }

    /*function*/
    public static int PlayerSnap(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("PlayerSnap",i);
        return 0;
    }

    /*task*/
    public static int PlayerLineUp(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("PlayerLineUp",i);
        return 0;
    }

    /*task*/
    public static int PlayerParallelLineUp(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("PlayerParallelLineUp",i);
        return 0;
    }

    /*task*/
    public static int PlayerPickupLineUp(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("PlayerPickupLineUp",i);
        return 0;
    }

    /*function*/
    public static int MovePlayerAboveMe(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("MovePlayerAboveMe",i);
        return 0;
    }

    /*function*/
    public static int PushPlayer(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PushPlayer",i);
        return 0;
    }

    /*function*/
    public static int PopPlayer(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PopPlayer",i);
        return 0;
    }

    /*task*/
    public static int PlayerFaceObj(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerFaceObj",i);
        return 0;
    }

    /*function*/
    public static int PlayerOnScape(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerOnScape",i);
        return 0;
    }

    /*function*/
    public static int PlayerCollideStatic(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PlayerCollideStatic",i);
        return 0;
    }

    /*function*/
    public static int WonGame(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("WonGame",i);
        return 0;
    }

    /*function*/
    public static int MainloopExit(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("MainloopExit",i);
        return 0;
    }

    /*task*/
    public static int PlayerFaceTalk(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PlayerFaceTalk",i);
        return 0;
    }

    /*function*/
    public static int PlayerForceTalk(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PlayerForceTalk",i);
        return 0;
    }

    /*function*/
    public static int CollidePlayerWeapon(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("CollidePlayerWeapon",i);
        return 0;
    }


    //object and player health
    /*function*/
    public static int ZapPlayer(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ZapPlayer",i);
        return 0;
    }

    /*function*/
    public static int ZapPlayerNoKill(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ZapPlayerNoKill",i);
        return 0;
    }

    /*function*/
    public static int ZapObject(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ZapObject",i);
        return 0;
    }

    /*function*/
    public static int ZapObjectNoKill(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ZapObjectNoKill",i);
        return 0;
    }


    //object system messaging
    /*function*/
    public static int SendMessage(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SendMessage",i);
        return 0;
    }

    /*function*/
    public static int ReceiveMessage(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ReceiveMessage",i);
        return 0;
    }

    /*function*/
    public static int IsMessageWaiting(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsMessageWaiting",i);
        return 0;
    }

    /*function*/
    public static int GetMessageSender(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("GetMessageSender",i);
        return 0;
    }

    /*function*/
    public static int GetMessageSenderId(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("GetMessageSenderId",i);
        return 0;
    }

    /*function*/
    public static int ReplySender(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ReplySender",i);
        return 0;
    }

    /*function*/
    public static int DeleteMessage(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DeleteMessage",i);
        return 0;
    }

    /*function*/
    public static int SendBroadcast(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SendBroadcast",i);
        return 0;
    }

    /*function*/
    public static int ReceiveBroadcast(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ReceiveBroadcast",i);
        return 0;
    }

    /*function*/
    public static int DeleteBroadcast(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DeleteBroadcast",i);
        return 0;
    }

    /*function*/
    public static int IsBroadcastWaiting(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsBroadcastWaiting",i);
        return 0;
    }

    /*function*/
    public static int GetBroadcastSender(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("GetBroadcastSender",i);
        return 0;
    }

    /*function*/
    public static int GetBroadcastSenderId(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("GetBroadcastSenderId",i);
        return 0;
    }

    /*function*/
    public static int ReplyBroadcastSender(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ReplyBroadcastSender",i);
        return 0;
    }


    //sync tasks and functions
    /*function*/
    public static int ResetGroupSync(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ResetGroupSync",i);
        return 0;
    }

    /*function*/
    public static int ResetUserSync(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ResetUserSync",i);
        return 0;
    }

    /*task*/
    public static int SyncWithGroup(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SyncWithGroup",i);
        // i[0]: sync group
        return 0;
    }

    /*task*/
    public static int SyncWithUser(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SyncWithUser",i);
        return 0;
    }

    /*function*/
    public static int ChangeGroup(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ChangeGroup",i);
        return 0;
    }

    /*function*/
    public static int ChangeUser(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ChangeUser",i);
        return 0;
    }


    //pipelining functions
    /*function*/
    public static int PipeMyGroup(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PipeMyGroup",i);
        return 0;
    }

    /*function*/
    public static int PipeGroup(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PipeGroup",i);
        return 0;
    }

    /*function*/
    public static int PipeUser(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PipeUser",i);
        return 0;
    }

    /*function*/
    public static int PipeMasters(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PipeMasters",i);
        return 0;
    }

    /*function*/
    public static int PipeSlaves(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PipeSlaves",i);
        return 0;
    }

    /*function*/
    public static int PipeAroundMe(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PipeAroundMe",i);
        return 0;
    }

    /*function*/
    public static int PipeAroundPos(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("PipeAroundPos",i);
        return 0;
    }

    /*function*/
    public static int PipeHidden(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PipeHidden",i);
        return 0;
    }


    //node map negotiation
    /*task*/
    public static int MoveNodeMarker(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("MoveNodeMarker",i);
        return 0;
    }

    /*task*/
    public static int MoveNodeLocation(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("MoveNodeLocation",i);
        return 0;
    }

    /*task*/
    public static int MoveNodePosition(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("MoveNodePosition",i);
        return 0;
    }

    /*function*/
    public static int AtNodeMarker(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("AtNodeMarker",i);
        return 0;
    }

    /*function*/
    public static int AtNodeLocation(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("AtNodeLocation",i);
        return 0;
    }

    /*function*/
    public static int AtNodePosition(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("AtNodePosition",i);
        return 0;
    }

    /*function*/
    public static int Wander(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("Wander",i);
        return 0;
    }

    /*function*/
    public static int DisableNodeMaps(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DisableNodeMaps",i);
        return 0;
    }

    /*function*/
    public static int EnableNodeMaps(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("EnableNodeMaps",i);
        return 0;
    }


    //node marker negotiation
    /*task*/
    public static int MoveMarker(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("MoveMarker",i);
        return 0;
    }

    /*task*/
    public static int MoveLocation(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("MoveLocation",i);
        return 0;
    }

    /*task*/
    public static int MovePosition(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("MovePosition",i);
        return 0;
    }


    //weapons and hand-objects
    /*function*/
    public static int DrawSword(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DrawSword",i);
        return 0;
    }

    /*function*/
    public static int SheathSword(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("SheathSword",i);
        return 0;
    }

    /*function*/
    public static int CheckPlayerWeapon(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("CheckPlayerWeapon",i);
        return 0;
    }

    /*function*/
    public static int DisplayHandModel(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("DisplayHandModel",i);
        return 0;
    }

    /*function*/
    public static int DisplayHandItem(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("DisplayHandItem",i);
        return 0;
    }

    /*function*/
    public static int DisplayHiltModel(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("DisplayHiltModel",i);
        return 0;
    }

    /*function*/
    public static int DisplayHiltItem(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("DisplayHiltItem",i);
        return 0;
    }


    //item's and the inventory
    /*function*/
    public static int HandItem(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("HandItem",i);
        return 0;
    }

    /*function*/
    public static int HaveItem(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("HaveItem",i);
        return 0;
    }

    /*function*/
    public static int RemoveItem(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("RemoveItem",i);
        return 0;
    }

    /*function*/
    public static int ItemUsed(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("ItemUsed",i);
        return 0;
    }

    /*function*/
    public static int PushItem(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("PushItem",i);
        return 0;
    }

    /*function*/
    public static int PopItem(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PopItem",i);
        return 0;
    }

    /*function*/
    public static int AddItem(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("AddItem",i);
        return 0;
    }

    /*function*/
    public static int SubItem(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("SubItem",i);
        return 0;
    }

    /*function*/
    public static int DropItem(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("DropItem",i);
        return 0;
    }

    /*function*/
    public static int UseItem(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("UseItem",i);
        return 0;
    }

    /*function*/
    public static int ShowItem(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("ShowItem",i);
        return 0;
    }

    /*function*/
    public static int ShowItemNoRtx(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("ShowItemNoRtx",i);
        return 0;
    }

    /*function*/
    public static int ActiveItem(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("ActiveItem",i);
        return 0;
    }

    /*function*/
    public static int PushAllItems(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("PushAllItems",i);
        return 0;
    }

    /*function*/
    public static int PopAllItems(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("PopAllItems",i);
        return 0;
    }

    /*function*/
    public static int DropAllItems(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DropAllItems",i);
        return 0;
    }

    /*function*/
    public static int SelectItem(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("SelectItem",i);
        return 0;
    }


    //general object queries
    /*function*/
    public static int IsOnEdge(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsOnEdge",i);
        return 0;
    }

    /*function*/
    public static int IsHoldingWeapon(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsHoldingWeapon",i);
        return 0;
    }

    /*function*/
    public static int IsSheathingSword(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsSheathingSword",i);
        return 0;
    }

    /*function*/
    public static int IsDrawingSword(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsDrawingSword",i);
        return 0;
    }

    /*function*/
    public static int IsDrawingOrSheathing(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsDrawingOrSheathing",i);
        return 0;
    }

    /*function*/
    public static int IsCarryingWeapon(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsCarryingWeapon",i);
        return 0;
    }

    /*function*/
    public static int IsCombatCapable(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsCombatCapable",i);
        return 0;
    }

    /*function*/
    public static int IsShellScript(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsShellScript",i);
        return 0;
    }

    /*function*/
    public static int IsWithinView(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsWithinView",i);
        return 0;
    }

    /*function*/
    public static int IsWithinMap(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsWithinMap",i);
        return 0;
    }

    /*function*/
    public static int IsInAir(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsInAir",i);
        return 0;
    }

    /*function*/
    public static int IsLightingOrUnlighting(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsLightingOrUnlighting",i);
        return 0;
    }

    /*function*/
    public static int IsUnlightingTorch(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsUnlightingTorch",i);
        return 0;
    }

    /*function*/
    public static int IsLightingTorch(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsLightingTorch",i);
        return 0;
    }

    /*function*/
    public static int IsInLava(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsInLava",i);
        return 0;
    }

    /*function*/
    public static int IsInDeepWater(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsInDeepWater",i);
        return 0;
    }

    /*function*/
    public static int IsInWater(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsInWater",i);
        return 0;
    }

    /*function*/
    public static int IsBouncing(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("IsBouncing",i);
        return 0;
    }


    //shell AI functions
    /*function*/
    public static int OpenShell(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("OpenShell",i);
        return 0;
    }

    /*function*/
    public static int CountAiShells(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("CountAiShells",i);
        return 0;
    }

    /*function*/
    public static int FindAiShell(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("FindAiShell",i);
        return 0;
    }


    //effects system
    /*function*/
    public static int AddFlatEffect(bool multitask, int[] i /*6*/)	
    {
        ThrowNIMPL("AddFlatEffect",i);
        return 0;
    }

    /*function*/
    public static int AddLightEffect(bool multitask, int[] i /*8*/)	
    {
        ThrowNIMPL("AddLightEffect",i);
        return 0;
    }

    /*function*/
    public static int GolemSteam(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("GolemSteam",i);
        return 0;
    }

    /*function*/
    public static int DragonBreath(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DragonBreath",i);
        return 0;
    }

    /*multitask*/
    public static int DragonBreathTask(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("DragonBreathTask",i);
        return 0;
    }


    //Smacker functions
    /*function*/
    public static int QueueMovie(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("QueueMovie",i);
        return 0;
    }


    /*task*/
    public static int TestTask(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("TestTask",i);
        return 0;
    }

    /*function*/
    public static int XResetCamera(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("XResetCamera",i);
        return 0;
    }


    //Potion functions
    /*function*/
    public static int SetPotion(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("SetPotion",i);
        return 0;
    }

    /*function*/
    public static int GetPotion(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("GetPotion",i);
        return 0;
    }


    //Log Book functions
    /*function*/
    public static int AddLog(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("AddLog",i);
        return 0;
    }


    //spare expansion functions, (do nothing at the moment)
    /*function*/
    public static int Spare0Parm0(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("Spare0Parm0",i);
        return 0;
    }
    public static int Spare1Parm0(bool multitask, int[] i /*0*/)	
    {
        ThrowNIMPL("Spare1Parm0",i);
        return 0;
    }

    /*function*/
    public static int Spare0Parm1(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("Spare0Parm1",i);
        return 0;
    }

    /*function*/
    public static int Spare1Parm1(bool multitask, int[] i /*1*/)	
    {
        ThrowNIMPL("Spare1Parm1",i);
        return 0;
    }

    /*function*/
    public static int Spare0Parm2(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("Spare0Parm2",i);
        return 0;
    }

    /*function*/
    public static int Spare1Parm2(bool multitask, int[] i /*2*/)	
    {
        ThrowNIMPL("Spare1Parm2",i);
        return 0;
    }

    /*function*/
    public static int Spare0Parm3(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("Spare0Parm3",i);
        return 0;
    }

    /*function*/
    public static int Spare1Parm3(bool multitask, int[] i /*3*/)	
    {
        ThrowNIMPL("Spare1Parm3",i);
        return 0;
    }

    /*function*/
    public static int Spare0Parm4(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("Spare0Parm4",i);
        return 0;
    }

    /*function*/
    public static int Spare1Parm4(bool multitask, int[] i /*4*/)	
    {
        ThrowNIMPL("Spare1Parm4",i);
        return 0;
    }

    /*function*/
    public static int Spare0Parm5(bool multitask, int[] i /*5*/)	
    {
        ThrowNIMPL("Spare0Parm5",i);
        return 0;
    }

    /*function*/
    public static int Spare1Parm5(bool multitask, int[] i /*5*/)	
    {
        ThrowNIMPL("Spare1Parm5",i);
        return 0;
    }

    /*function*/
    public static int Spare0Parm6(bool multitask, int[] i /*6*/)	
    {
        ThrowNIMPL("Spare0Parm6",i);
        return 0;
    }

    /*function*/
    public static int Spare1Parm6(bool multitask, int[] i /*6*/)	
    {
        ThrowNIMPL("Spare1Parm6",i);
        return 0;
    }

    /*function*/
    public static int Spare0Parm7(bool multitask, int[] i /*7*/)	
    {
        ThrowNIMPL("Spare0Parm7",i);
        return 0;
    }

    /*function*/
    public static int Spare1Parm7(bool multitask, int[] i /*8*/)	
    {
        ThrowNIMPL("Spare1Parm7",i);
        return 0;
    }
    static Func<bool, int[], int>[] soupdefFcn;
    public static Func<bool, int[], int>[] getNIMPLS()
    {
        if(soupdefFcn == null)
        {
            soupdefFcn = new Func<bool, int[], int>[367];
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

    public static Func<bool, int[], int> getNIMPL(int i)
    {
        if(soupdefFcn == null)
        {
            getNIMPLS();
        }
        return soupdefFcn[i];
    }

}
