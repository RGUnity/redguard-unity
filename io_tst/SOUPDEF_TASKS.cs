//RedGuard Global function, flag and equate definition file
//functions section.
//functions defined as xxx parms y
//where xxx is an ascii name for the function and y is the number of parameters
//the function takes.

// 100% speculation:
// tasks take a while to complete
// functions return immediately
using System;
namespace TST
{
	public class funcs
	{
		public void Log(string s, int[] i)
		{
			Console.WriteLine($"{s}({string.Join(",",i)})");
		}
		public int PLACEHOLDER_ZERO(int[] i /*4*/)
		{
			Log("PLACEHOLDER_ZERO",i);
			return 0;
		}
		/*task*/
		public int sRotate(int[] i /*4*/)
		{
			Log("sRotate",i);
			return 0;
		}

		/*task*/
		public int sRotateByAxis(int[] i /*3*/)	
		{
			Log("sRotateByAxis",i);
			return 0;
		}

		/*task*/
		public int sRotateToAxis(int[] i /*3*/)	
		{
			Log("sRotateToAxis",i);
			return 0;
		}

		/*task*/
		public int sMove(int[] i /*4*/)	
		{
			Log("sMove",i);
			return 0;
		}

		/*task*/
		public int sMoveByAxis(int[] i /*3*/)	
		{
			Log("sMoveByAxis",i);
			return 0;
		}

		/*task*/
		public int sMoveAlongAxis(int[] i /*3*/)	
		{
			Log("sMoveAlongAxis",i);
			return 0;
		}

		/*function*/
		public int HeaderHook(int[] i /*0*/)	
		{
			Log("HeaderHook",i);
			return 0;
		}

		/*task*/
		public int swordAI(int[] i /*0*/)	
		{
			Log("swordAI",i);
			return 0;
		}

		/*task*/
		public int projectileAI(int[] i /*0*/)	
		{
			Log("projectileAI",i);
			return 0;
		}

		/*function*/
		public int chargeWeapon(int[] i /*1*/)	
		{
			Log("chargeWeapon",i);
			return 0;
		}

		/*function*/
		public int PlayerMain(int[] i /*0*/)	
		{
			Log("PlayerMain",i);
			return 0;
		}

		/*function*/
		public int PrintParms(int[] i /*8*/)	
		{
			Log("PrintParms",i);
			return 0;
		}

		/*function*/
		public int LogParms(int[] i /*8*/)	
		{
			Log("LogParms",i);
			return 0;
		}

		/*function*/
		public int PrintStringParm(int[] i /*1*/)	
		{
			Log("PrintStringParm",i);
			return 0;
		}

		/*function*/
		public int PrintSingleParm(int[] i /*1*/)	
		{
			Log("PrintSingleParm",i);
			return 0;
		}

		/*function*/
		public int SpacePressed(int[] i /*0*/)	
		{
			Log("SpacePressed",i);
			return 0;
		}

		/*task*/
		public int WaitOnTasks(int[] i /*0*/)	
		{
			Log("WaitOnTasks",i);
			return 0;
		}

		/*function*/
		public int updatePlayerViewAttrib(int[] i /*0*/)	
		{
			Log("updatePlayerViewAttrib",i);
			return 0;
		}

		/*function*/
		public int cameraController(int[] i /*0*/)	
		{
			Log("cameraController",i);
			return 0;
		}

		/*task*/
		public int showObjRot(int[] i /*8*/)	
		{
			Log("showObjRot",i);
			return 0;
		}

		/*task*/
		public int showObj(int[] i /*3*/)	
		{
			Log("showObj",i);
			return 0;
		}

		/*task*/
		public int showObjLoc(int[] i /*3*/)	
		{
			Log("showObjLoc",i);
			return 0;
		}

		/*task*/
		public int showObjPan(int[] i /*4*/)	
		{
			Log("showObjPan",i);
			return 0;
		}

		/*task*/
		public int showObjPanLoc(int[] i /*4*/)	
		{
			Log("showObjPanLoc",i);
			return 0;
		}

		/*task*/
		public int lookObj(int[] i /*1*/)	
		{
			Log("lookObj",i);
			return 0;
		}

		/*task*/
		public int showPlayer(int[] i /*0*/)	
		{
			Log("showPlayer",i);
			return 0;
		}

		/*task*/
		public int showPlayerPan(int[] i /*1*/)	
		{
			Log("showPlayerPan",i);
			return 0;
		}

		/*task*/
		public int lookCyrus(int[] i /*1*/)	
		{
			Log("lookCyrus",i);
			return 0;
		}

		/*task*/
		public int showCyrus(int[] i /*3*/)	
		{
			Log("showCyrus",i);
			return 0;
		}

		/*task*/
		public int showCyrusLoc(int[] i /*3*/)	
		{
			Log("showCyrusLoc",i);
			return 0;
		}

		/*task*/
		public int showCyrusPan(int[] i /*4*/)	
		{
			Log("showCyrusPan",i);
			return 0;
		}

		/*task*/
		public int showCyrusPanLoc(int[] i /*4*/)	
		{
			Log("showCyrusPanLoc",i);
			return 0;
		}

		/*function*/
		public int PlayAnimation(int[] i /*2*/)	
		{
			Log("PlayAnimation",i);
			return 0;
		}

		/*function*/
		public int lockoutPlayer(int[] i /*1*/)	
		{
            // Enable/disable player input
            // i[0]: 1 disables input, 0 enables it
			Log("lockoutPlayer",i);
			return 0;
		}

		/*function*/
		public int menuNew(int[] i /*0*/)	
		{
			Log("menuNew",i);
			return 0;
		}

		/*task*/
		public int menuProc(int[] i /*0*/)	
		{
			Log("menuProc",i);
			return 0;
		}

		/*function*/
		public int menuAddItem(int[] i /*3*/)	
		{
			Log("menuAddItem",i);
			return 0;
		}

		/*function*/
		public int menuSelection(int[] i /*0*/)	
		{
			Log("menuSelection",i);
			return 0;
		}

		/*task*/
		public int RTX(int[] i /*1*/)	
		{
			Log("RTX",i);
			return 0;
		}

		/*task*/
		public int rtxAnim(int[] i /*4*/)	
		{
			Log("rtxAnim",i);
			return 0;
		}

		/*task*/
		public int RTXpAnim(int[] i /*4*/)	
		{
			Log("RTXpAnim",i);
			return 0;
		}

		/*task*/
		public int RTXp(int[] i /*1*/)	
		{
			Log("RTXp",i);
			return 0;
		}

		/*task*/
		public int Rotate(int[] i /*4*/)	
		{
			Log("Rotate",i);
			return 0;
		}

		/*task*/
		public int RotateByAxis(int[] i /*3*/)	
		{
			// i[0]: axis (0/1/2)
			// i[1]: amount
			// i[3]: time to complete
			Log("RotateByAxis",i);
			return 0;
		}

		/*task*/
		public int RotateToAxis(int[] i /*3*/)	
		{
			Log("RotateToAxis",i);
			return 0;
		}

		/*task*/
		public int WalkForward(int[] i /*1*/)	
		{
			Log("WalkForward",i);
			return 0;
		}

		/*task*/
		public int WalkBackward(int[] i /*1*/)	
		{
			Log("WalkBackward",i);
			return 0;
		}

		/*task*/
		public int MoveForward(int[] i /*1*/)	
		{
			Log("MoveForward",i);
			return 0;
		}

		/*task*/
		public int MoveBackward(int[] i /*1*/)	
		{
			Log("MoveBackward",i);
			return 0;
		}

		/*task*/
		public int MoveLeft(int[] i /*1*/)	
		{
			Log("MoveLeft",i);
			return 0;
		}

		/*task*/
		public int MoveRight(int[] i /*1*/)	
		{
			Log("MoveRight",i);
			return 0;
		}

		/*task*/
		public int Move(int[] i /*4*/)	
		{
			Log("Move",i);
			return 0;
		}

		/*task*/
		public int MoveByAxis(int[] i /*3*/)	
		{
			// i[0]: axis (0/1/2)
			// i[1]: amount
			// i[3]: time to complete
			Log("MoveByAxis",i);
			return 0;
		}

		/*task*/
		public int MoveAlongAxis(int[] i /*3*/)	
		{
			Log("MoveAlongAxis",i);
			return 0;
		}

		/*task*/
		public int MoveObjectAxis(int[] i /*3*/)	
		{
			Log("MoveObjectAxis",i);
			return 0;
		}

		/*task*/
		public int MoveToLocation(int[] i /*2*/)	
		{
			Log("MoveToLocation",i);
			return 0;
		}

		/*task*/
		public int WanderToLocation(int[] i /*2*/)	
		{
			Log("WanderToLocation",i);
			return 0;
		}

		/*task*/
		public int MoveToMarker(int[] i /*2*/)	
		{
			Log("MoveToMarker",i);
			return 0;
		}

		/*function*/
		public int SetObjectLocation(int[] i /*4*/)	
		{
			Log("SetObjectLocation",i);
			return 0;
		}

		/*task*/
		public int Wait(int[] i /*1*/)	
		{
			// i[0]: time to wait
			Log("Wait",i);
			return 0;
		}

		/*function*/
		public int DistanceFromStart(int[] i /*1*/)	
		{
			Log("DistanceFromStart",i);
			return 0;
		}

		/*function*/
		public int Light(int[] i /*2*/)	
		{
			Log("Light",i);
			return 0;
		}

		/*function*/
		public int LightRadius(int[] i /*1*/)	
		{
			Log("LightRadius",i);
			return 0;
		}

		/*function*/
		public int LightIntensity(int[] i /*1*/)	
		{
			Log("LightIntensity",i);
			return 0;
		}

		/*function*/
		public int LightOff(int[] i /*0*/)	
		{
			Log("LightOff",i);
			return 0;
		}

		/*function*/
		public int LightOffset(int[] i /*3*/)	
		{
			Log("LightOffset",i);
			return 0;
		}

		/*function*/
		public int LightFlicker(int[] i /*2*/)	
		{
			Log("LightFlicker",i);
			return 0;
		}

		/*function*/
		public int FlickerLight(int[] i /*1*/)	
		{
			Log("FlickerLight",i);
			return 0;
		}

		/*function*/
		public int LightFlickerOff(int[] i /*0*/)	
		{
			Log("LightFlickerOff",i);
			return 0;
		}

		/*function*/
		public int LightSize(int[] i /*2*/)	
		{
			Log("LightSize",i);
			return 0;
		}

		/*function*/
		public int LightSizeOff(int[] i /*0*/)	
		{
			Log("LightSizeOff",i);
			return 0;
		}

		/*multitask*/
		public int FxPhase(int[] i /*7*/)	
		{
			Log("FxPhase",i);
			return 0;
		}

		/*multitask*/
		public int FxFlickerOnOff(int[] i /*1*/)	
		{
			Log("FxFlickerOnOff",i);
			return 0;
		}

		/*multitask*/
		public int FxFlickerDim(int[] i /*1*/)	
		{
			Log("FxFlickerDim",i);
			return 0;
		}

		/*multitask*/
		public int FxLightSize(int[] i /*2*/)	
		{
			Log("FxLightSize",i);
			return 0;
		}

		/*function*/
		public int Flat(int[] i /*1*/)	
		{
			Log("Flat",i);
			return 0;
		}

		/*function*/
		public int FlatSetTexture(int[] i /*2*/)	
		{
			Log("FlatSetTexture",i);
			return 0;
		}

		/*function*/
		public int FlatOff(int[] i /*0*/)	
		{
			Log("FlatOff",i);
			return 0;
		}

		/*function*/
		public int FlatOffset(int[] i /*3*/)	
		{
			Log("FlatOffset",i);
			return 0;
		}

		/*function*/
		public int FlatLikeStatic(int[] i /*1*/)	
		{
			Log("FlatLikeStatic",i);
			return 0;
		}

		/*function*/
		public int FlatAnimate(int[] i /*3*/)	
		{
			Log("FlatAnimate",i);
			return 0;
		}

		/*function*/
		public int FlatStop(int[] i /*0*/)	
		{
			Log("FlatStop",i);
			return 0;
		}

		/*function*/
		public int SetAttribute(int[] i /*2*/)	
		{
			Log("SetAttribute",i);
			return 0;
		}

		/*function*/
		public int GetAttribute(int[] i /*1*/)	
		{
			Log("GetAttribute",i);
			return 0;
		}

		/*function*/
		public int SetGlobalFlag(int[] i /*1*/)	
		{
			Log("SetGlobalFlag",i);
			return 0;
		}

		/*function*/
		public int TestGlobalFlag(int[] i /*1*/)	
		{
			Log("TestGlobalFlag",i);
			return 0;
		}

		/*function*/
		public int ResetGlobalFlag(int[] i /*1*/)	
		{
			Log("ResetGlobalFlag",i);
			return 0;
		}

		/*task*/
		public int FacePlayer(int[] i /*1*/)	
		{
			Log("FacePlayer",i);
			return 0;
		}

		/*task*/
		public int FacePlayerInertia(int[] i /*1*/)	
		{
			Log("FacePlayerInertia",i);
			return 0;
		}

		/*task*/
		public int FaceAngle(int[] i /*1*/)	
		{
			Log("FaceAngle",i);
			return 0;
		}

		/*task*/
		public int FacePos(int[] i /*2*/)	
		{
			Log("FacePos",i);
			return 0;
		}

		/*task*/
		public int FaceObject(int[] i /*1*/)	
		{
			Log("FaceObject",i);
			return 0;
		}

		/*function*/
		public int Sound(int[] i /*3*/)	
		{
			Log("Sound",i);
			return 0;
		}

		/*function*/
		public int FlatSound(int[] i /*3*/)	
		{
			Log("FlatSound",i);
			return 0;
		}

		/*multitask*/
		public int AmbientSound(int[] i /*3*/)	
		{
			Log("AmbientSound",i);
			return 0;
		}

		/*multitask*/
		public int AmbientRtx(int[] i /*3*/)	
		{
			Log("AmbientRtx",i);
			return 0;
		}

		/*function*/
		public int Jump(int[] i /*0*/)	
		{
			Log("Jump",i);
			return 0;
		}

		/*task*/
		public int WaitNonMulti(int[] i /*0*/)	
		{
			Log("WaitNonMulti",i);
			return 0;
		}

		/*task*/
		public int WaitOnDialog(int[] i /*0*/)	
		{
			Log("WaitOnDialog",i);
			return 0;
		}

		/*function*/
		public int HideMe(int[] i /*0*/)	
		{
			Log("HideMe",i);
			return 0;
		}

		/*function*/
		public int ShowMe(int[] i /*0*/)	
		{
			Log("ShowMe",i);
			return 0;
		}

		/*function*/
		public int Rnd(int[] i /*1*/)	
		{
			Log("Rnd",i);
			return 0;
		}

		/*function*/
		public int QuickRnd(int[] i /*1*/)	
		{
			Log("QuickRnd",i);
			return 0;
		}

		/*function*/
		public int MyId(int[] i /*0*/)	
		{
			Log("MyId",i);
			return 0;
		}

		/*task*/
		public int FaceLocation(int[] i /*2*/)	
		{
			Log("FaceLocation",i);
			return 0;
		}

		/*function*/
		public int DistanceFromLocation(int[] i /*1*/)	
		{
			Log("DistanceFromLocation",i);
			return 0;
		}

		/*function*/
		public int LoadWorld(int[] i /*3*/)	
		{
			Log("LoadWorld",i);
			return 0;
		}

		/*function*/
		public int SetAiType(int[] i /*1*/)	
		{
			Log("SetAiType",i);
			return 0;
		}

		/*function*/
		public int SetAiMode(int[] i /*1*/)	
		{
			Log("SetAiMode",i);
			return 0;
		}

		/*function*/
		public int MyAiType(int[] i /*0*/)	
		{
			Log("MyAiType",i);
			return 0;
		}

		/*function*/
		public int MyAiMode(int[] i /*0*/)	
		{
			Log("MyAiMode",i);
			return 0;
		}

		/*task*/
		public int GotoLocation(int[] i /*1*/)	
		{
			Log("GotoLocation",i);
			return 0;
		}

		/*task*/
		public int Guard(int[] i /*3*/)	
		{
			Log("Guard",i);
			return 0;
		}

		/*task*/
		public int Animal(int[] i /*2*/)	
		{
			Log("Animal",i);
			return 0;
		}

		/*function*/
		public int TrueFunction(int[] i /*0*/)	
		{
			Log("TrueFunction",i);
			return 0;
		}

		/*function*/
		public int TurnToPos(int[] i /*2*/)	
		{
			Log("TurnToPos",i);
			return 0;
		}

		/*function*/
		public int AtPos(int[] i /*3*/)	
		{
			Log("AtPos",i);
			return 0;
		}

		/*function*/
		public int TurnToAngle(int[] i /*1*/)	
		{
			Log("TurnToAngle",i);
			return 0;
		}

		/*function*/
		public int MovePos(int[] i /*3*/)	
		{
			Log("MovePos",i);
			return 0;
		}

		/*function*/
		public int SetStartPosition(int[] i /*3*/)	
		{
			Log("SetStartPosition",i);
			return 0;
		}

		/*function*/
		public int GetMyAttr(int[] i /*1*/)	
		{
			Log("GetMyAttr",i);
			return 0;
		}

		/*function*/
		public int SetMyAttr(int[] i /*2*/)	
		{
			Log("SetMyAttr",i);
			return 0;
		}

		/*function*/
		public int GetFramePassed(int[] i /*0*/)	
		{
			Log("GetFramePassed",i);
			return 0;
		}

		/*function*/
		public int SpeedScale(int[] i /*1*/)	
		{
			Log("SpeedScale",i);
			return 0;
		}

		/*function*/
		public int InKey(int[] i /*1*/)	
		{
			Log("InKey",i);
			return 0;
		}

		/*function*/
		public int PressKey(int[] i /*1*/)	
		{
			Log("PressKey",i);
			return 0;
		}


		/*function in all caps?: FUNCTION	ACTIVATE	PARMS 1*/
		public int ACTIVATE(int[] i /*1*/)	
		{
            // True if player activates an item
            // i[0]: Stringid shown on screen when looking at the item
			Log("ACTIVATE",i);
			return 0;
		}

		/*function*/
		public int TorchActivate(int[] i /*1*/)	
		{
			Log("TorchActivate",i);
			return 0;
		}

		/*function*/
		public int Deactivate(int[] i /*0*/)	
		{
			Log("Deactivate",i);
			return 0;
		}

		/*function*/
		public int ReleaseAnimation(int[] i /*0*/)	
		{
			Log("ReleaseAnimation",i);
			return 0;
		}

		/*function*/
		public int HoldAnimation(int[] i /*0*/)	
		{
			Log("HoldAnimation",i);
			return 0;
		}

		/*function*/
		public int PauseAnimation(int[] i /*1*/)	
		{
			Log("PauseAnimation",i);
			return 0;
		}

		/*function*/
		public int SetAction(int[] i /*1*/)	
		{
			Log("SetAction",i);
			return 0;
		}

		/*function*/
		public int ResetAction(int[] i /*1*/)	
		{
			Log("ResetAction",i);
			return 0;
		}

		/*function*/
		public int ResetAllAction(int[] i /*0*/)	
		{
			Log("ResetAllAction",i);
			return 0;
		}

		/*task*/
		public int PushAnimation(int[] i /*2*/)
		{
			Log("PushAnimation",i);
			// i[0]: animationId
			// i[1]: firstFrame
			return 0;
		}

		/*task*/
		public int PushControlAnimation(int[] i /*2*/)
		{
			Log("PushControlAnimation",i);
			return 0;
		}

		/*task*/
		public int WaitAnimFrame(int[] i /*2*/)
		{
			Log("WaitAnimFrame",i);
			// i[0]: animationId
			// i[1]: frame
			return 0;
		}

		/*task*/
		public int WaitPlayerAnimFrame(int[] i /*0*/)
		{
			Log("WaitPlayerAnimFrame",i);
			// i[0]: animationId
			// i[1]: frame
			return 0;
		}

		/*function*/
		public int KillMyTasks(int[] i /*0*/)	
		{
			Log("KillMyTasks",i);
			return 0;
		}

		/*task*/
		public int ObjectLineUp(int[] i /*3*/)	
		{
			Log("ObjectLineUp",i);
			return 0;
		}

		/*task*/
		public int ObjectParallelLineUp(int[] i /*3*/)	
		{
			Log("ObjectParallelLineUp",i);
			return 0;
		}

		/*task*/
		public int ObjectPickupLineUp(int[] i /*3*/)	
		{
			Log("ObjectPickupLineUp",i);
			return 0;
		}

		/*task*/
		public int LineUp(int[] i /*3*/)	
		{
			Log("LineUp",i);
			return 0;
		}

		/*task*/
		public int WaitLineUp(int[] i /*1*/)	
		{
			Log("WaitLineUp",i);
			return 0;
		}

		/*function*/
		public int MoveAboveMe(int[] i /*4*/)	
		{
			Log("MoveAboveMe",i);
			return 0;
		}

		/*function*/
		public int KillMe(int[] i /*0*/)	
		{
			Log("KillMe",i);
			return 0;
		}

		/*function*/
		public int EndSound(int[] i /*1*/)	
		{
			Log("EndSound",i);
			return 0;
		}

		/*function*/
		public int EndMySounds(int[] i /*0*/)	
		{
			Log("EndMySounds",i);
			return 0;
		}

		/*function*/
		public int EndMySound(int[] i /*1*/)	
		{
			Log("EndMySound",i);
			return 0;
		}

		/*function*/
		public int StopAllSounds(int[] i /*0*/)	
		{
			Log("StopAllSounds",i);
			return 0;
		}

		/*task*/
		public int ViewGXA(int[] i /*3*/)	
		{
			Log("ViewGXA",i);
			return 0;
		}

		/*task*/
		public int AnimateGXA(int[] i /*4*/)	
		{
			Log("AnimateGXA",i);
			return 0;
		}

		/*function*/
		public int PlayerOffset(int[] i /*3*/)	
		{
			Log("PlayerOffset",i);
			return 0;
		}

		/*multitask*/
		public int PlayerOffsettask(int[] i /*3*/)	
		{
			Log("PlayerOffsettask",i);
			return 0;
		}

		/*function*/
		public int Offset(int[] i /*3*/)	
		{
			Log("Offset",i);
			return 0;
		}

		/*function*/
		public int UnOffset(int[] i /*0*/)	
		{
			Log("UnOffset",i);
			return 0;
		}

		/*task*/
		public int BounceObject(int[] i /*3*/)	
		{
			Log("BounceObject",i);
			return 0;
		}

		/*function*/
		public int OffsetLocation(int[] i /*4*/)	
		{
			Log("OffsetLocation",i);
			return 0;
		}

		/*function*/
		public int DistanceToLocation(int[] i /*1*/)	
		{
			Log("DistanceToLocation",i);
			return 0;
		}

		/*function*/
		public int ShowBitmap(int[] i /*2*/)	
		{
			Log("ShowBitmap",i);
			return 0;
		}

		/*function*/
		public int UnShowBitmap(int[] i /*1*/)	
		{
			Log("UnShowBitmap",i);
			return 0;
		}

		/*function*/
		public int ReplenishHealth(int[] i /*0*/)	
		{
			Log("ReplenishHealth",i);
			return 0;
		}

		/*function*/
		public int InRectangle(int[] i /*4*/)	
		{
			Log("InRectangle",i);
			return 0;
		}

		/*function*/
		public int InCircle(int[] i /*3*/)	
		{
			Log("InCircle",i);
			return 0;
		}

		/*function*/
		public int ScriptParticles(int[] i /*8*/)	
		{
			Log("ScriptParticles",i);
			return 0;
		}

		/*function*/
		public int LightTorch(int[] i /*0*/)	
		{
			Log("LightTorch",i);
			return 0;
		}

		/*function*/
		public int UnlightTorch(int[] i /*0*/)	
		{
			Log("UnlightTorch",i);
			return 0;
		}

		/*function*/
		public int LoadStatic(int[] i /*1*/)	
		{
			Log("LoadStatic",i);
			return 0;
		}

		/*function*/
		public int UnLoadStatic(int[] i /*0*/)	
		{
			Log("UnLoadStatic",i);
			return 0;
		}

		/*function*/
		public int PointAt(int[] i /*0*/)	
		{
			Log("PointAt",i);
			return 0;
		}


		//combat commands
		/*function*/
		public int beginCombat(int[] i /*0*/)	
		{
			Log("beginCombat",i);
			return 0;
		}

		/*function*/
		public int endCombat(int[] i /*0*/)	
		{
			Log("endCombat",i);
			return 0;
		}

		/*function*/
		public int isFighting(int[] i /*0*/)	
		{
			Log("isFighting",i);
			return 0;
		}

		/*function*/
		public int isDead(int[] i /*0*/)	
		{
			Log("isDead",i);
			return 0;
		}

		/*function*/
		public int adjustHealth(int[] i /*2*/)	
		{
			Log("adjustHealth",i);
			return 0;
		}

		/*function*/
		public int setHealth(int[] i /*1*/)	
		{
			Log("setHealth",i);
			return 0;
		}

		/*function*/
		public int health(int[] i /*0*/)	
		{
			Log("health",i);
			return 0;
		}

		/*function*/
		public int adjustStrength(int[] i /*1*/)	
		{
			Log("adjustStrength",i);
			return 0;
		}

		/*function*/
		public int setStrength(int[] i /*1*/)	
		{
			Log("setStrength",i);
			return 0;
		}

		/*function*/
		public int strength(int[] i /*0*/)	
		{
			Log("strength",i);
			return 0;
		}

		/*function*/
		public int adjustArmor(int[] i /*1*/)	
		{
			Log("adjustArmor",i);
			return 0;
		}

		/*function*/
		public int setArmor(int[] i /*1*/)	
		{
			Log("setArmor",i);
			return 0;
		}

		/*function*/
		public int armor(int[] i /*0*/)	
		{
			Log("armor",i);
			return 0;
		}

		/*function*/
		public int shoot(int[] i /*7*/)	
		{
			Log("shoot",i);
			return 0;
		}

		/*function*/
		public int shootPlayer(int[] i /*4*/)	
		{
			Log("shootPlayer",i);
			return 0;
		}

		/*function*/
		public int resurrect(int[] i /*0*/)	
		{
			Log("resurrect",i);
			return 0;
		}


		//object script/task control
		/*function*/
		public int EnableObject(int[] i /*0*/)	
		{
			Log("EnableObject",i);
			return 0;
		}

		/*function*/
		public int DisableObject(int[] i /*0*/)	
		{
			Log("DisableObject",i);
			return 0;
		}

		/*function*/
		public int EnableTasks(int[] i /*0*/)	
		{
			Log("EnableTasks",i);
			return 0;
		}

		/*function*/
		public int DisableTasks(int[] i /*0*/)	
		{
			Log("DisableTasks",i);
			return 0;
		}

		/*function*/
		public int EnableScript(int[] i /*0*/)	
		{
			Log("EnableScript",i);
			return 0;
		}

		/*function*/
		public int DisableScript(int[] i /*0*/)	
		{
			Log("DisableScript",i);
			return 0;
		}

		/*function*/
		public int EnableHook(int[] i /*0*/)	
		{
			Log("EnableHook",i);
			return 0;
		}

		/*function*/
		public int DisableHook(int[] i /*0*/)	
		{
			Log("DisableHook",i);
			return 0;
		}

		/*function*/
		public int EnableCombat(int[] i /*0*/)	
		{
			Log("EnableCombat",i);
			return 0;
		}

		/*function*/
		public int DisableCombat(int[] i /*0*/)	
		{
			Log("DisableCombat",i);
			return 0;
		}


		//rotational and positional reset commands

		/*function*/
		public int SetRotation(int[] i /*3*/)	
		{
			Log("SetRotation",i);
			return 0;
		}

		/*function*/
		public int SetPosition(int[] i /*3*/)	
		{
			Log("SetPosition",i);
			return 0;
		}

		/*function*/
		public int ResetRotation(int[] i /*3*/)	
		{
			Log("ResetRotation",i);
			return 0;
		}

		/*function*/
		public int ResetPosition(int[] i /*3*/)	
		{
			Log("ResetPosition",i);
			return 0;
		}

		/*function*/
		public int ReloadRotation(int[] i /*0*/)	
		{
			Log("ReloadRotation",i);
			return 0;
		}

		/*function*/
		public int ReloadPosition(int[] i /*0*/)	
		{
			Log("ReloadPosition",i);
			return 0;
		}

		/*function*/
		public int SetLocation(int[] i /*1*/)	
		{
			Log("SetLocation",i);
			return 0;
		}


		//master and slave commands
		/*function*/
		public int AttachMe(int[] i /*1*/)	
		{
			Log("AttachMe",i);
			return 0;
		}

		/*function*/
		public int DetachMe(int[] i /*0*/)	
		{
			Log("DetachMe",i);
			return 0;
		}

		/*function*/
		public int DetachSlaves(int[] i /*0*/)	
		{
			Log("DetachSlaves",i);
			return 0;
		}

		/*function*/
		public int KillSlaves(int[] i /*0*/)	
		{
			Log("KillSlaves",i);
			return 0;
		}

		/*function*/
		public int HideSlaves(int[] i /*0*/)	
		{
			Log("HideSlaves",i);
			return 0;
		}

		/*function*/
		public int ShowSlaves(int[] i /*0*/)	
		{
			Log("ShowSlaves",i);
			return 0;
		}

		/*function*/
		public int EnableSlaves(int[] i /*0*/)	
		{
			Log("EnableSlaves",i);
			return 0;
		}

		/*function*/
		public int DisableSlaves(int[] i /*0*/)	
		{
			Log("DisableSlaves",i);
			return 0;
		}

		/*function*/
		public int IsSlave(int[] i /*0*/)	
		{
			Log("IsSlave",i);
			return 0;
		}

		/*function*/
		public int IsMaster(int[] i /*0*/)	
		{
			Log("IsMaster",i);
			return 0;
		}

		/*function*/
		public int FixSlaveAngle(int[] i /*2*/)	
		{
			Log("FixSlaveAngle",i);
			return 0;
		}


		//player commands
		/*function*/
		public int PlayerMoved(int[] i /*0*/)	
		{
			Log("PlayerMoved",i);
			return 0;
		}

		/*function*/
		public int PlayerTurned(int[] i /*0*/)	
		{
			Log("PlayerTurned",i);
			return 0;
		}

		/*function*/
		public int PlayerTurnedDirection(int[] i /*0*/)	
		{
			Log("PlayerTurnedDirection",i);
			return 0;
		}

		/*function*/
		public int PlayerMovementSpeed(int[] i /*0*/)	
		{
			Log("PlayerMovementSpeed",i);
			return 0;
		}

		/*function*/
		public int GetMovementSpeed(int[] i /*0*/)	
		{
			Log("GetMovementSpeed",i);
			return 0;
		}

		/*function*/
		public int PlayerHealth(int[] i /*0*/)	
		{
			Log("PlayerHealth",i);
			return 0;
		}

		/*function*/
		public int PlayerStatus(int[] i /*0*/)	
		{
			Log("PlayerStatus",i);
			return 0;
		}

		/*function*/
		public int PlayerCollide(int[] i /*0*/)	
		{
			Log("PlayerCollide",i);
			return 0;
		}

		/*function*/
		public int PlayerStand(int[] i /*0*/)	
		{
			Log("PlayerStand",i);
			return 0;
		}

		/*function*/
		public int PlayerArmed(int[] i /*0*/)	
		{
			Log("PlayerArmed",i);
			return 0;
		}

		/*function*/
		public int PlayerHanging(int[] i /*0*/)	
		{
			Log("PlayerHanging",i);
			return 0;
		}

		/*function*/
		public int PlayerInSight(int[] i /*0*/)	
		{
			Log("PlayerInSight",i);
			return 0;
		}

		/*function*/
		public int PlayerDistance(int[] i /*0*/)	
		{
			Log("PlayerDistance",i);
			return 0;
		}

		/*function*/
		public int AcuratePlayerDistance(int[] i /*0*/)	
		{
			Log("AcuratePlayerDistance",i);
			return 0;
		}

		/*function*/
		public int PlayerFacing(int[] i /*2*/)	
		{
			Log("PlayerFacing",i);
			return 0;
		}

		/*function*/
		public int PlayerArcTan(int[] i /*0*/)	
		{
			Log("PlayerArcTan",i);
			return 0;
		}

		/*function*/
		public int PlayerArc(int[] i /*2*/)	
		{
			Log("PlayerArc",i);
			return 0;
		}

		/*function*/
		public int PlayerLooking(int[] i /*1*/)	
		{
			Log("PlayerLooking",i);
			return 0;
		}

		/*multitask*/
		public int PlayerTilt(int[] i /*3*/)	
		{
			Log("PlayerTilt",i);
			return 0;
		}

		/*multitask*/
		public int PlayerTiltRevx(int[] i /*3*/)	
		{
			Log("PlayerTiltRevx",i);
			return 0;
		}

		/*function*/
		public int PlayerSnap(int[] i /*3*/)	
		{
			Log("PlayerSnap",i);
			return 0;
		}

		/*task*/
		public int PlayerLineUp(int[] i /*3*/)	
		{
			Log("PlayerLineUp",i);
			return 0;
		}

		/*task*/
		public int PlayerParallelLineUp(int[] i /*3*/)	
		{
			Log("PlayerParallelLineUp",i);
			return 0;
		}

		/*task*/
		public int PlayerPickupLineUp(int[] i /*3*/)	
		{
			Log("PlayerPickupLineUp",i);
			return 0;
		}

		/*function*/
		public int MovePlayerAboveMe(int[] i /*4*/)	
		{
			Log("MovePlayerAboveMe",i);
			return 0;
		}

		/*function*/
		public int PushPlayer(int[] i /*1*/)	
		{
			Log("PushPlayer",i);
			return 0;
		}

		/*function*/
		public int PopPlayer(int[] i /*0*/)	
		{
			Log("PopPlayer",i);
			return 0;
		}

		/*task*/
		public int PlayerFaceObj(int[] i /*0*/)	
		{
			Log("PlayerFaceObj",i);
			return 0;
		}

		/*function*/
		public int PlayerOnScape(int[] i /*0*/)	
		{
			Log("PlayerOnScape",i);
			return 0;
		}

		/*function*/
		public int PlayerCollideStatic(int[] i /*0*/)	
		{
			Log("PlayerCollideStatic",i);
			return 0;
		}

		/*function*/
		public int WonGame(int[] i /*0*/)	
		{
			Log("WonGame",i);
			return 0;
		}

		/*function*/
		public int MainloopExit(int[] i /*1*/)	
		{
			Log("MainloopExit",i);
			return 0;
		}

		/*task*/
		public int PlayerFaceTalk(int[] i /*1*/)	
		{
			Log("PlayerFaceTalk",i);
			return 0;
		}

		/*function*/
		public int PlayerForceTalk(int[] i /*1*/)	
		{
			Log("PlayerForceTalk",i);
			return 0;
		}

		/*function*/
		public int CollidePlayerWeapon(int[] i /*4*/)	
		{
			Log("CollidePlayerWeapon",i);
			return 0;
		}


		//object and player health
		/*function*/
		public int ZapPlayer(int[] i /*1*/)	
		{
			Log("ZapPlayer",i);
			return 0;
		}

		/*function*/
		public int ZapPlayerNoKill(int[] i /*1*/)	
		{
			Log("ZapPlayerNoKill",i);
			return 0;
		}

		/*function*/
		public int ZapObject(int[] i /*1*/)	
		{
			Log("ZapObject",i);
			return 0;
		}

		/*function*/
		public int ZapObjectNoKill(int[] i /*1*/)	
		{
			Log("ZapObjectNoKill",i);
			return 0;
		}


		//object system messaging
		/*function*/
		public int SendMessage(int[] i /*1*/)	
		{
			Log("SendMessage",i);
			return 0;
		}

		/*function*/
		public int ReceiveMessage(int[] i /*0*/)	
		{
			Log("ReceiveMessage",i);
			return 0;
		}

		/*function*/
		public int IsMessageWaiting(int[] i /*0*/)	
		{
			Log("IsMessageWaiting",i);
			return 0;
		}

		/*function*/
		public int GetMessageSender(int[] i /*0*/)	
		{
			Log("GetMessageSender",i);
			return 0;
		}

		/*function*/
		public int GetMessageSenderId(int[] i /*0*/)	
		{
			Log("GetMessageSenderId",i);
			return 0;
		}

		/*function*/
		public int ReplySender(int[] i /*1*/)	
		{
			Log("ReplySender",i);
			return 0;
		}

		/*function*/
		public int DeleteMessage(int[] i /*0*/)	
		{
			Log("DeleteMessage",i);
			return 0;
		}

		/*function*/
		public int SendBroadcast(int[] i /*1*/)	
		{
			Log("SendBroadcast",i);
			return 0;
		}

		/*function*/
		public int ReceiveBroadcast(int[] i /*0*/)	
		{
			Log("ReceiveBroadcast",i);
			return 0;
		}

		/*function*/
		public int DeleteBroadcast(int[] i /*0*/)	
		{
			Log("DeleteBroadcast",i);
			return 0;
		}

		/*function*/
		public int IsBroadcastWaiting(int[] i /*0*/)	
		{
			Log("IsBroadcastWaiting",i);
			return 0;
		}

		/*function*/
		public int GetBroadcastSender(int[] i /*0*/)	
		{
			Log("GetBroadcastSender",i);
			return 0;
		}

		/*function*/
		public int GetBroadcastSenderId(int[] i /*0*/)	
		{
			Log("GetBroadcastSenderId",i);
			return 0;
		}

		/*function*/
		public int ReplyBroadcastSender(int[] i /*1*/)	
		{
			Log("ReplyBroadcastSender",i);
			return 0;
		}


		//sync tasks and functions
		/*function*/
		public int ResetGroupSync(int[] i /*0*/)	
		{
			Log("ResetGroupSync",i);
			return 0;
		}

		/*function*/
		public int ResetUserSync(int[] i /*0*/)	
		{
			Log("ResetUserSync",i);
			return 0;
		}

		/*task*/
		public int SyncWithGroup(int[] i /*1*/)	
		{
			Log("SyncWithGroup",i);
			// i[0]: sync group
			return 0;
		}

		/*task*/
		public int SyncWithUser(int[] i /*1*/)	
		{
			Log("SyncWithUser",i);
			return 0;
		}

		/*function*/
		public int ChangeGroup(int[] i /*1*/)	
		{
			Log("ChangeGroup",i);
			return 0;
		}

		/*function*/
		public int ChangeUser(int[] i /*1*/)	
		{
			Log("ChangeUser",i);
			return 0;
		}


		//pipelining functions
		/*function*/
		public int PipeMyGroup(int[] i /*0*/)	
		{
			Log("PipeMyGroup",i);
			return 0;
		}

		/*function*/
		public int PipeGroup(int[] i /*1*/)	
		{
			Log("PipeGroup",i);
			return 0;
		}

		/*function*/
		public int PipeUser(int[] i /*1*/)	
		{
			Log("PipeUser",i);
			return 0;
		}

		/*function*/
		public int PipeMasters(int[] i /*0*/)	
		{
			Log("PipeMasters",i);
			return 0;
		}

		/*function*/
		public int PipeSlaves(int[] i /*0*/)	
		{
			Log("PipeSlaves",i);
			return 0;
		}

		/*function*/
		public int PipeAroundMe(int[] i /*1*/)	
		{
			Log("PipeAroundMe",i);
			return 0;
		}

		/*function*/
		public int PipeAroundPos(int[] i /*4*/)	
		{
			Log("PipeAroundPos",i);
			return 0;
		}

		/*function*/
		public int PipeHidden(int[] i /*0*/)	
		{
			Log("PipeHidden",i);
			return 0;
		}


		//node map negotiation
		/*task*/
		public int MoveNodeMarker(int[] i /*2*/)	
		{
			Log("MoveNodeMarker",i);
			return 0;
		}

		/*task*/
		public int MoveNodeLocation(int[] i /*2*/)	
		{
			Log("MoveNodeLocation",i);
			return 0;
		}

		/*task*/
		public int MoveNodePosition(int[] i /*4*/)	
		{
			Log("MoveNodePosition",i);
			return 0;
		}

		/*function*/
		public int AtNodeMarker(int[] i /*2*/)	
		{
			Log("AtNodeMarker",i);
			return 0;
		}

		/*function*/
		public int AtNodeLocation(int[] i /*2*/)	
		{
			Log("AtNodeLocation",i);
			return 0;
		}

		/*function*/
		public int AtNodePosition(int[] i /*4*/)	
		{
			Log("AtNodePosition",i);
			return 0;
		}

		/*function*/
		public int Wander(int[] i /*2*/)	
		{
			Log("Wander",i);
			return 0;
		}

		/*function*/
		public int DisableNodeMaps(int[] i /*0*/)	
		{
			Log("DisableNodeMaps",i);
			return 0;
		}

		/*function*/
		public int EnableNodeMaps(int[] i /*0*/)	
		{
			Log("EnableNodeMaps",i);
			return 0;
		}


		//node marker negotiation
		/*task*/
		public int MoveMarker(int[] i /*2*/)	
		{
			Log("MoveMarker",i);
			return 0;
		}

		/*task*/
		public int MoveLocation(int[] i /*2*/)	
		{
			Log("MoveLocation",i);
			return 0;
		}

		/*task*/
		public int MovePosition(int[] i /*4*/)	
		{
			Log("MovePosition",i);
			return 0;
		}


		//weapons and hand-objects
		/*function*/
		public int DrawSword(int[] i /*0*/)	
		{
			Log("DrawSword",i);
			return 0;
		}

		/*function*/
		public int SheathSword(int[] i /*0*/)	
		{
			Log("SheathSword",i);
			return 0;
		}

		/*function*/
		public int CheckPlayerWeapon(int[] i /*0*/)	
		{
			Log("CheckPlayerWeapon",i);
			return 0;
		}

		/*function*/
		public int DisplayHandModel(int[] i /*4*/)	
		{
			Log("DisplayHandModel",i);
			return 0;
		}

		/*function*/
		public int DisplayHandItem(int[] i /*4*/)	
		{
			Log("DisplayHandItem",i);
			return 0;
		}

		/*function*/
		public int DisplayHiltModel(int[] i /*4*/)	
		{
			Log("DisplayHiltModel",i);
			return 0;
		}

		/*function*/
		public int DisplayHiltItem(int[] i /*4*/)	
		{
			Log("DisplayHiltItem",i);
			return 0;
		}


		//item's and the inventory
		/*function*/
		public int HandItem(int[] i /*1*/)	
		{
			Log("HandItem",i);
			return 0;
		}

		/*function*/
		public int HaveItem(int[] i /*1*/)	
		{
			Log("HaveItem",i);
			return 0;
		}

		/*function*/
		public int RemoveItem(int[] i /*1*/)	
		{
			Log("RemoveItem",i);
			return 0;
		}

		/*function*/
		public int ItemUsed(int[] i /*1*/)	
		{
			Log("ItemUsed",i);
			return 0;
		}

		/*function*/
		public int PushItem(int[] i /*2*/)	
		{
			Log("PushItem",i);
			return 0;
		}

		/*function*/
		public int PopItem(int[] i /*1*/)	
		{
			Log("PopItem",i);
			return 0;
		}

		/*function*/
		public int AddItem(int[] i /*2*/)	
		{
			Log("AddItem",i);
			return 0;
		}

		/*function*/
		public int SubItem(int[] i /*2*/)	
		{
			Log("SubItem",i);
			return 0;
		}

		/*function*/
		public int DropItem(int[] i /*2*/)	
		{
			Log("DropItem",i);
			return 0;
		}

		/*function*/
		public int UseItem(int[] i /*1*/)	
		{
			Log("UseItem",i);
			return 0;
		}

		/*function*/
		public int ShowItem(int[] i /*4*/)	
		{
			Log("ShowItem",i);
			return 0;
		}

		/*function*/
		public int ShowItemNoRtx(int[] i /*3*/)	
		{
			Log("ShowItemNoRtx",i);
			return 0;
		}

		/*function*/
		public int ActiveItem(int[] i /*0*/)	
		{
			Log("ActiveItem",i);
			return 0;
		}

		/*function*/
		public int PushAllItems(int[] i /*1*/)	
		{
			Log("PushAllItems",i);
			return 0;
		}

		/*function*/
		public int PopAllItems(int[] i /*0*/)	
		{
			Log("PopAllItems",i);
			return 0;
		}

		/*function*/
		public int DropAllItems(int[] i /*0*/)	
		{
			Log("DropAllItems",i);
			return 0;
		}

		/*function*/
		public int SelectItem(int[] i /*1*/)	
		{
			Log("SelectItem",i);
			return 0;
		}


		//general object queries
		/*function*/
		public int IsOnEdge(int[] i /*0*/)	
		{
			Log("IsOnEdge",i);
			return 0;
		}

		/*function*/
		public int IsHoldingWeapon(int[] i /*0*/)	
		{
			Log("IsHoldingWeapon",i);
			return 0;
		}

		/*function*/
		public int IsSheathingSword(int[] i /*0*/)	
		{
			Log("IsSheathingSword",i);
			return 0;
		}

		/*function*/
		public int IsDrawingSword(int[] i /*0*/)	
		{
			Log("IsDrawingSword",i);
			return 0;
		}

		/*function*/
		public int IsDrawingOrSheathing(int[] i /*0*/)	
		{
			Log("IsDrawingOrSheathing",i);
			return 0;
		}

		/*function*/
		public int IsCarryingWeapon(int[] i /*0*/)	
		{
			Log("IsCarryingWeapon",i);
			return 0;
		}

		/*function*/
		public int IsCombatCapable(int[] i /*0*/)	
		{
			Log("IsCombatCapable",i);
			return 0;
		}

		/*function*/
		public int IsShellScript(int[] i /*0*/)	
		{
			Log("IsShellScript",i);
			return 0;
		}

		/*function*/
		public int IsWithinView(int[] i /*0*/)	
		{
			Log("IsWithinView",i);
			return 0;
		}

		/*function*/
		public int IsWithinMap(int[] i /*0*/)	
		{
			Log("IsWithinMap",i);
			return 0;
		}

		/*function*/
		public int IsInAir(int[] i /*0*/)	
		{
			Log("IsInAir",i);
			return 0;
		}

		/*function*/
		public int IsLightingOrUnlighting(int[] i /*0*/)	
		{
			Log("IsLightingOrUnlighting",i);
			return 0;
		}

		/*function*/
		public int IsUnlightingTorch(int[] i /*0*/)	
		{
			Log("IsUnlightingTorch",i);
			return 0;
		}

		/*function*/
		public int IsLightingTorch(int[] i /*0*/)	
		{
			Log("IsLightingTorch",i);
			return 0;
		}

		/*function*/
		public int IsInLava(int[] i /*0*/)	
		{
			Log("IsInLava",i);
			return 0;
		}

		/*function*/
		public int IsInDeepWater(int[] i /*0*/)	
		{
			Log("IsInDeepWater",i);
			return 0;
		}

		/*function*/
		public int IsInWater(int[] i /*0*/)	
		{
			Log("IsInWater",i);
			return 0;
		}

		/*function*/
		public int IsBouncing(int[] i /*0*/)	
		{
			Log("IsBouncing",i);
			return 0;
		}


		//shell AI functions
		/*function*/
		public int OpenShell(int[] i /*1*/)	
		{
			Log("OpenShell",i);
			return 0;
		}

		/*function*/
		public int CountAiShells(int[] i /*1*/)	
		{
			Log("CountAiShells",i);
			return 0;
		}

		/*function*/
		public int FindAiShell(int[] i /*1*/)	
		{
			Log("FindAiShell",i);
			return 0;
		}


		//effects system
		/*function*/
		public int AddFlatEffect(int[] i /*6*/)	
		{
			Log("AddFlatEffect",i);
			return 0;
		}

		/*function*/
		public int AddLightEffect(int[] i /*8*/)	
		{
			Log("AddLightEffect",i);
			return 0;
		}

		/*function*/
		public int GolemSteam(int[] i /*0*/)	
		{
			Log("GolemSteam",i);
			return 0;
		}

		/*function*/
		public int DragonBreath(int[] i /*0*/)	
		{
			Log("DragonBreath",i);
			return 0;
		}

		/*multitask*/
		public int DragonBreathTask(int[] i /*0*/)	
		{
			Log("DragonBreathTask",i);
			return 0;
		}


		//Smacker functions
		/*function*/
		public int QueueMovie(int[] i /*1*/)	
		{
			Log("QueueMovie",i);
			return 0;
		}


		/*task*/
		public int TestTask(int[] i /*1*/)	
		{
			Log("TestTask",i);
			return 0;
		}

		/*function*/
		public int XResetCamera(int[] i /*0*/)	
		{
			Log("XResetCamera",i);
			return 0;
		}


		//Potion functions
		/*function*/
		public int SetPotion(int[] i /*2*/)	
		{
			Log("SetPotion",i);
			return 0;
		}

		/*function*/
		public int GetPotion(int[] i /*1*/)	
		{
			Log("GetPotion",i);
			return 0;
		}


		//Log Book functions
		/*function*/
		public int AddLog(int[] i /*1*/)	
		{
			Log("AddLog",i);
			return 0;
		}


		//spare expansion functions, (do nothing at the moment)
		/*function*/
		public int Spare0Parm0(int[] i /*0*/)	
		{
			Log("Spare0Parm0",i);
			return 0;
		}
		public int Spare1Parm0(int[] i /*0*/)	
		{
			Log("Spare1Parm0",i);
			return 0;
		}

		/*function*/
		public int Spare0Parm1(int[] i /*1*/)	
		{
			Log("Spare0Parm1",i);
			return 0;
		}

		/*function*/
		public int Spare1Parm1(int[] i /*1*/)	
		{
			Log("Spare1Parm1",i);
			return 0;
		}

		/*function*/
		public int Spare0Parm2(int[] i /*2*/)	
		{
			Log("Spare0Parm2",i);
			return 0;
		}

		/*function*/
		public int Spare1Parm2(int[] i /*2*/)	
		{
			Log("Spare1Parm2",i);
			return 0;
		}

		/*function*/
		public int Spare0Parm3(int[] i /*3*/)	
		{
			Log("Spare0Parm3",i);
			return 0;
		}

		/*function*/
		public int Spare1Parm3(int[] i /*3*/)	
		{
			Log("Spare1Parm3",i);
			return 0;
		}

		/*function*/
		public int Spare0Parm4(int[] i /*4*/)	
		{
			Log("Spare0Parm4",i);
			return 0;
		}

		/*function*/
		public int Spare1Parm4(int[] i /*4*/)	
		{
			Log("Spare1Parm4",i);
			return 0;
		}

		/*function*/
		public int Spare0Parm5(int[] i /*5*/)	
		{
			Log("Spare0Parm5",i);
			return 0;
		}

		/*function*/
		public int Spare1Parm5(int[] i /*5*/)	
		{
			Log("Spare1Parm5",i);
			return 0;
		}

		/*function*/
		public int Spare0Parm6(int[] i /*6*/)	
		{
			Log("Spare0Parm6",i);
			return 0;
		}

		/*function*/
		public int Spare1Parm6(int[] i /*6*/)	
		{
			Log("Spare1Parm6",i);
			return 0;
		}

		/*function*/
		public int Spare0Parm7(int[] i /*7*/)	
		{
			Log("Spare0Parm7",i);
			return 0;
		}

		/*function*/
		public int Spare1Parm7(int[] i /*8*/)	
		{
			Log("Spare1Parm7",i);
			return 0;
		}

		public static void Main(string[] args)
		{
			//setup();
		}
		public void setup()
		{
			Func<int[], int>[] soupdefFcn = new Func<int[], int>[367];
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
			return;
		}

	}

}

