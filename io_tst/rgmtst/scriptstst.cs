using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RGFileImport;

    public class RGObjectStore 
    {

        public static int DoObjectTask(uint objectId, string subjectName, int taskId, bool isMultiTask, int[] parameters)
        {
//            static Func<bool, int[], int>[] SetupFuncs()

Console.WriteLine($"FUNC: {taskId}");
            return 1;
        }

    }
namespace xyz
{
    public class test
	{
		public static void Main(string[] args)
		{
			RGRGMFile filergm = new RGRGMFile();
/*
			filergm.LoadFile("../../game_3dfx/maps/TAVERN.RGM");
            RGRGMScriptStore.ReadScript(filergm);
            for(int i=0;i<RGRGMScriptStore.Scripts.Count;i++)
            {
                RGRGMScriptStore.RGMScript cs = RGRGMScriptStore.Scripts.ElementAt(i).Value;
                string o = new string($"{cs.scriptName}: {cs.scriptStrings.Count}:");
                o += $"{cs.objectName}";
                o+= "\\n";
                for(int j=0;j<cs.scriptStrings.Count;j++)
                    o+= $"{cs.scriptStrings[j]},";
                Console.WriteLine(o);
            }
            return;
            */
            Func<bool, int[], int>[] funcs = SetupFuncs();

			filergm.LoadFile("../../game_3dfx/maps/OBSERVE.RGM");
            RGRGMScriptStore.ReadScript(filergm);
            
            RGRGMScriptStore.flags[201] = 1;
            ScriptData sd = new ScriptData("OB_PLT07", 0);
            for(int i=0;i<100;i++)
                Console.WriteLine($"ATTR_{i} = {sd.attributes[i]}");
            int ticks = 20;
            for(int i=0;i<ticks;i++)
            {
                Console.Write($"{i}:");
                sd.tickScript();
            }
            /*
            sd = new ScriptData("OB_ENG04", funcs,0);
            for(int i=0;i<ticks;i++)
            {
                Console.Write($"{i}:");
                sd.tickScript();
            }
*/
           /* 
            Func<bool, int[], int>[] funcs = SetupFuncs();
			filergm.LoadFile("../../game_3dfx/maps/OBSERVE.RGM");
            RGRGMScriptStore.ReadScript(filergm);
            Console.WriteLine(RGRGMScriptStore.getScript("OB_PLT04").DumpStrings());
*/
           /* 
            RGRGMScriptStore.flags[195] = 6;
            ScriptData sd = new ScriptData("OB_PLT04", 0);
            sd.runScript();
            sd.runScript();
            sd.runScript();
            sd.runScript();
            sd.runScript();
            Console.WriteLine($"FLAG195: {RGRGMScriptStore.flags[195]}");
            Console.WriteLine($"FLAG196: {RGRGMScriptStore.flags[196]}");
            Console.WriteLine($"FLAG197: {RGRGMScriptStore.flags[197]}");
           */ 

/*
            Console.WriteLine($"{filergm.RAHD.dict.Count}");
            for(int i=0;i<109;i++)
            {
                RGRGMFile.RGMRAHDItem item = filergm.RAHD.dict.ElementAt(i).Value;
                Console.WriteLine($"{i}:{item.scriptName}");
            }
*/
/*
            int cnt = RGRGMScriptStore.scripts.Count;
            int runs      = 1;
            for(int j=0;j<cnt;j++)
            {
                RGRGMScriptStore.scripts[j].TST_IFRET = true;
                RGRGMScriptStore.scripts[j].TST_IFFLIP = true;
                for(int i=0;i<runs;i++)
                    RGRGMScriptStore.scripts[j].runScript();
            }
*/

        }
		static public void Log(string s, int[] i)
		{
			Console.WriteLine($"{s}({string.Join(",",i)})");
		}
		static public int PLACEHOLDER_ZERO(bool b, int[] i /*4*/)
		{
			Log("PLACEHOLDER_ZERO",i);
			return 0;
		}
		/*task*/
		static public int sRotate(bool b, int[] i /*4*/)
		{
			Log("sRotate",i);
			return 0;
		}

		/*task*/
		static public int sRotateByAxis(bool b, int[] i /*3*/)	
		{
			Log("sRotateByAxis",i);
			return 0;
		}

		/*task*/
		static public int sRotateToAxis(bool b, int[] i /*3*/)	
		{
			Log("sRotateToAxis",i);
			return 0;
		}

		/*task*/
		static public int sMove(bool b, int[] i /*4*/)	
		{
			Log("sMove",i);
			return 0;
		}

		/*task*/
		static public int sMoveByAxis(bool b, int[] i /*3*/)	
		{
			Log("sMoveByAxis",i);
			return 0;
		}

		/*task*/
		static public int sMoveAlongAxis(bool b, int[] i /*3*/)	
		{
			Log("sMoveAlongAxis",i);
			return 0;
		}

		/*function*/
		static public int HeaderHook(bool b, int[] i /*0*/)	
		{
			Log("HeaderHook",i);
			return 0;
		}

		/*task*/
		static public int swordAI(bool b, int[] i /*0*/)	
		{
			Log("swordAI",i);
			return 0;
		}

		/*task*/
		static public int projectileAI(bool b, int[] i /*0*/)	
		{
			Log("projectileAI",i);
			return 0;
		}

		/*function*/
		static public int chargeWeapon(bool b, int[] i /*1*/)	
		{
			Log("chargeWeapon",i);
			return 0;
		}

		/*function*/
		static public int PlayerMain(bool b, int[] i /*0*/)	
		{
			Log("PlayerMain",i);
			return 0;
		}

		/*function*/
		static public int PrintParms(bool b, int[] i /*8*/)	
		{
			Log("PrintParms",i);
			return 0;
		}

		/*function*/
		static public int LogParms(bool b, int[] i /*8*/)	
		{
			Log("LogParms",i);
			return 0;
		}

		/*function*/
		static public int PrintStringParm(bool b, int[] i /*1*/)	
		{
			Log("PrintStringParm",i);
			return 0;
		}

		/*function*/
		static public int PrintSingleParm(bool b, int[] i /*1*/)	
		{
			Log("PrintSingleParm",i);
			return 0;
		}

		/*function*/
		static public int SpacePressed(bool b, int[] i /*0*/)	
		{
			Log("SpacePressed",i);
			return 0;
		}

		/*task*/
		static public int WaitOnTasks(bool b, int[] i /*0*/)	
		{
			Log("WaitOnTasks",i);
			return 0;
		}

		/*function*/
		static public int updatePlayerViewAttrib(bool b, int[] i /*0*/)	
		{
			Log("updatePlayerViewAttrib",i);
			return 0;
		}

		/*function*/
		static public int cameraController(bool b, int[] i /*0*/)	
		{
			Log("cameraController",i);
			return 0;
		}

		/*task*/
		static public int showObjRot(bool b, int[] i /*8*/)	
		{
			Log("showObjRot",i);
			return 0;
		}

		/*task*/
		static public int showObj(bool b, int[] i /*3*/)	
		{
			Log("showObj",i);
			return 0;
		}

		/*task*/
		static public int showObjLoc(bool b, int[] i /*3*/)	
		{
			Log("showObjLoc",i);
			return 0;
		}

		/*task*/
		static public int showObjPan(bool b, int[] i /*4*/)	
		{
			Log("showObjPan",i);
			return 0;
		}

		/*task*/
		static public int showObjPanLoc(bool b, int[] i /*4*/)	
		{
			Log("showObjPanLoc",i);
			return 0;
		}

		/*task*/
		static public int lookObj(bool b, int[] i /*1*/)	
		{
			Log("lookObj",i);
			return 0;
		}

		/*task*/
		static public int showPlayer(bool b, int[] i /*0*/)	
		{
			Log("showPlayer",i);
			return 0;
		}

		/*task*/
		static public int showPlayerPan(bool b, int[] i /*1*/)	
		{
			Log("showPlayerPan",i);
			return 0;
		}

		/*task*/
		static public int lookCyrus(bool b, int[] i /*1*/)	
		{
			Log("lookCyrus",i);
			return 0;
		}

		/*task*/
		static public int showCyrus(bool b, int[] i /*3*/)	
		{
			Log("showCyrus",i);
			return 0;
		}

		/*task*/
		static public int showCyrusLoc(bool b, int[] i /*3*/)	
		{
			Log("showCyrusLoc",i);
			return 0;
		}

		/*task*/
		static public int showCyrusPan(bool b, int[] i /*4*/)	
		{
			Log("showCyrusPan",i);
			return 0;
		}

		/*task*/
		static public int showCyrusPanLoc(bool b, int[] i /*4*/)	
		{
			Log("showCyrusPanLoc",i);
			return 0;
		}

		/*function*/
		static public int PlayAnimation(bool b, int[] i /*2*/)	
		{
			Log("PlayAnimation",i);
			return 0;
		}

		/*function*/
		static public int lockoutPlayer(bool b, int[] i /*1*/)	
		{
			Log("lockoutPlayer",i);
			return 0;
		}

		/*function*/
		static public int menuNew(bool b, int[] i /*0*/)	
		{
			Log("menuNew",i);
			return 0;
		}

		/*task*/
		static public int menuProc(bool b, int[] i /*0*/)	
		{
			Log("menuProc",i);
			return 0;
		}

		/*function*/
		static public int menuAddItem(bool b, int[] i /*3*/)	
		{
			Log("menuAddItem",i);
			return 0;
		}

		/*function*/
		static public int menuSelection(bool b, int[] i /*0*/)	
		{
			Log("menuSelection",i);
			return 0;
		}

		/*task*/
		static public int RTX(bool b, int[] i /*1*/)	
		{
			Log("RTX",i);
			return 0;
		}

		/*task*/
		static public int rtxAnim(bool b, int[] i /*4*/)	
		{
			Log("rtxAnim",i);
			return 0;
		}

		/*task*/
		static public int RTXpAnim(bool b, int[] i /*4*/)	
		{
			Log("RTXpAnim",i);
			return 0;
		}

		/*task*/
		static public int RTXp(bool b, int[] i /*1*/)	
		{
			Log("RTXp",i);
			return 0;
		}

		/*task*/
		static public int Rotate(bool b, int[] i /*4*/)	
		{
			Log("Rotate",i);
			return 0;
		}

		/*task*/
		static public int RotateByAxis(bool b, int[] i /*3*/)	
		{
			// i[0]: axis (0/1/2)
			// i[1]: amount
			// i[3]: time to complete
			Log("RotateByAxis",i);
			return 0;
		}

		/*task*/
		static public int RotateToAxis(bool b, int[] i /*3*/)	
		{
			Log("RotateToAxis",i);
			return 0;
		}

		/*task*/
		static public int WalkForward(bool b, int[] i /*1*/)	
		{
			Log("WalkForward",i);
			return 0;
		}

		/*task*/
		static public int WalkBackward(bool b, int[] i /*1*/)	
		{
			Log("WalkBackward",i);
			return 0;
		}

		/*task*/
		static public int MoveForward(bool b, int[] i /*1*/)	
		{
			Log("MoveForward",i);
			return 0;
		}

		/*task*/
		static public int MoveBackward(bool b, int[] i /*1*/)	
		{
			Log("MoveBackward",i);
			return 0;
		}

		/*task*/
		static public int MoveLeft(bool b, int[] i /*1*/)	
		{
			Log("MoveLeft",i);
			return 0;
		}

		/*task*/
		static public int MoveRight(bool b, int[] i /*1*/)	
		{
			Log("MoveRight",i);
			return 0;
		}

		/*task*/
		static public int Move(bool b, int[] i /*4*/)	
		{
			Log("Move",i);
			return 0;
		}

		/*task*/
		static public int MoveByAxis(bool b, int[] i /*3*/)	
		{
			// i[0]: axis (0/1/2)
			// i[1]: amount
			// i[3]: time to complete
			Log("MoveByAxis",i);
			return 0;
		}

		/*task*/
		static public int MoveAlongAxis(bool b, int[] i /*3*/)	
		{
			Log("MoveAlongAxis",i);
			return 0;
		}

		/*task*/
		static public int MoveObjectAxis(bool b, int[] i /*3*/)	
		{
			Log("MoveObjectAxis",i);
			return 0;
		}

		/*task*/
		static public int MoveToLocation(bool b, int[] i /*2*/)	
		{
			Log("MoveToLocation",i);
			return 0;
		}

		/*task*/
		static public int WanderToLocation(bool b, int[] i /*2*/)	
		{
			Log("WanderToLocation",i);
			return 0;
		}

		/*task*/
		static public int MoveToMarker(bool b, int[] i /*2*/)	
		{
			Log("MoveToMarker",i);
			return 0;
		}

		/*function*/
		static public int SetObjectLocation(bool b, int[] i /*4*/)	
		{
			Log("SetObjectLocation",i);
			return 0;
		}

		/*task*/
		static public int Wait(bool b, int[] i /*1*/)	
		{
			// i[0]: time to wait
			Log("Wait",i);
			return 0;
		}

		/*function*/
		static public int DistanceFromStart(bool b, int[] i /*1*/)	
		{
			Log("DistanceFromStart",i);
			return 0;
		}

		/*function*/
		static public int Light(bool b, int[] i /*2*/)	
		{
			Log("Light",i);
			return 0;
		}

		/*function*/
		static public int LightRadius(bool b, int[] i /*1*/)	
		{
			Log("LightRadius",i);
			return 0;
		}

		/*function*/
		static public int LightIntensity(bool b, int[] i /*1*/)	
		{
			Log("LightIntensity",i);
			return 0;
		}

		/*function*/
		static public int LightOff(bool b, int[] i /*0*/)	
		{
			Log("LightOff",i);
			return 0;
		}

		/*function*/
		static public int LightOffset(bool b, int[] i /*3*/)	
		{
			Log("LightOffset",i);
			return 0;
		}

		/*function*/
		static public int LightFlicker(bool b, int[] i /*2*/)	
		{
			Log("LightFlicker",i);
			return 0;
		}

		/*function*/
		static public int FlickerLight(bool b, int[] i /*1*/)	
		{
			Log("FlickerLight",i);
			return 0;
		}

		/*function*/
		static public int LightFlickerOff(bool b, int[] i /*0*/)	
		{
			Log("LightFlickerOff",i);
			return 0;
		}

		/*function*/
		static public int LightSize(bool b, int[] i /*2*/)	
		{
			Log("LightSize",i);
			return 0;
		}

		/*function*/
		static public int LightSizeOff(bool b, int[] i /*0*/)	
		{
			Log("LightSizeOff",i);
			return 0;
		}

		/*multitask*/
		static public int FxPhase(bool b, int[] i /*7*/)	
		{
			Log("FxPhase",i);
			return 0;
		}

		/*multitask*/
		static public int FxFlickerOnOff(bool b, int[] i /*1*/)	
		{
			Log("FxFlickerOnOff",i);
			return 0;
		}

		/*multitask*/
		static public int FxFlickerDim(bool b, int[] i /*1*/)	
		{
			Log("FxFlickerDim",i);
			return 0;
		}

		/*multitask*/
		static public int FxLightSize(bool b, int[] i /*2*/)	
		{
			Log("FxLightSize",i);
			return 0;
		}

		/*function*/
		static public int Flat(bool b, int[] i /*1*/)	
		{
			Log("Flat",i);
			return 0;
		}

		/*function*/
		static public int FlatSetTexture(bool b, int[] i /*2*/)	
		{
			Log("FlatSetTexture",i);
			return 0;
		}

		/*function*/
		static public int FlatOff(bool b, int[] i /*0*/)	
		{
			Log("FlatOff",i);
			return 0;
		}

		/*function*/
		static public int FlatOffset(bool b, int[] i /*3*/)	
		{
			Log("FlatOffset",i);
			return 0;
		}

		/*function*/
		static public int FlatLikeStatic(bool b, int[] i /*1*/)	
		{
			Log("FlatLikeStatic",i);
			return 0;
		}

		/*function*/
		static public int FlatAnimate(bool b, int[] i /*3*/)	
		{
			Log("FlatAnimate",i);
			return 0;
		}

		/*function*/
		static public int FlatStop(bool b, int[] i /*0*/)	
		{
			Log("FlatStop",i);
			return 0;
		}

		/*function*/
		static public int SetAttribute(bool b, int[] i /*2*/)	
		{
			Log("SetAttribute",i);
			return 0;
		}

		/*function*/
		static public int GetAttribute(bool b, int[] i /*1*/)	
		{
			Log("GetAttribute",i);
			return 0;
		}

		/*function*/
		static public int SetGlobalFlag(bool b, int[] i /*1*/)	
		{
			Log("SetGlobalFlag",i);
			return 0;
		}

		/*function*/
		static public int TestGlobalFlag(bool b, int[] i /*1*/)	
		{
			Log("TestGlobalFlag",i);
			return 0;
		}

		/*function*/
		static public int ResetGlobalFlag(bool b, int[] i /*1*/)	
		{
			Log("ResetGlobalFlag",i);
			return 0;
		}

		/*task*/
		static public int FacePlayer(bool b, int[] i /*1*/)	
		{
			Log("FacePlayer",i);
			return 0;
		}

		/*task*/
		static public int FacePlayerInertia(bool b, int[] i /*1*/)	
		{
			Log("FacePlayerInertia",i);
			return 0;
		}

		/*task*/
		static public int FaceAngle(bool b, int[] i /*1*/)	
		{
			Log("FaceAngle",i);
			return 0;
		}

		/*task*/
		static public int FacePos(bool b, int[] i /*2*/)	
		{
			Log("FacePos",i);
			return 0;
		}

		/*task*/
		static public int FaceObject(bool b, int[] i /*1*/)	
		{
			Log("FaceObject",i);
			return 0;
		}

		/*function*/
		static public int Sound(bool b, int[] i /*3*/)	
		{
			Log("Sound",i);
			return 0;
		}

		/*function*/
		static public int FlatSound(bool b, int[] i /*3*/)	
		{
			Log("FlatSound",i);
			return 0;
		}

		/*multitask*/
		static public int AmbientSound(bool b, int[] i /*3*/)	
		{
			Log("AmbientSound",i);
			return 0;
		}

		/*multitask*/
		static public int AmbientRtx(bool b, int[] i /*3*/)	
		{
			Log("AmbientRtx",i);
			return 0;
		}

		/*function*/
		static public int Jump(bool b, int[] i /*0*/)	
		{
			Log("Jump",i);
			return 0;
		}

		/*task*/
		static public int WaitNonMulti(bool b, int[] i /*0*/)	
		{
			Log("WaitNonMulti",i);
			return 0;
		}

		/*task*/
		static public int WaitOnDialog(bool b, int[] i /*0*/)	
		{
			Log("WaitOnDialog",i);
			return 0;
		}

		/*function*/
		static public int HideMe(bool b, int[] i /*0*/)	
		{
			Log("HideMe",i);
			return 0;
		}

		/*function*/
		static public int ShowMe(bool b, int[] i /*0*/)	
		{
			Log("ShowMe",i);
			return 0;
		}

		/*function*/
		static public int Rnd(bool b, int[] i /*1*/)	
		{
			Log("Rnd",i);
			return 0;
		}

		/*function*/
		static public int QuickRnd(bool b, int[] i /*1*/)	
		{
			Log("QuickRnd",i);
			return 0;
		}

		/*function*/
		static public int MyId(bool b, int[] i /*0*/)	
		{
			Log("MyId",i);
			return 0;
		}

		/*task*/
		static public int FaceLocation(bool b, int[] i /*2*/)	
		{
			Log("FaceLocation",i);
			return 0;
		}

		/*function*/
		static public int DistanceFromLocation(bool b, int[] i /*1*/)	
		{
			Log("DistanceFromLocation",i);
			return 0;
		}

		/*function*/
		static public int LoadWorld(bool b, int[] i /*3*/)	
		{
			Log("LoadWorld",i);
			return 0;
		}

		/*function*/
		static public int SetAiType(bool b, int[] i /*1*/)	
		{
			Log("SetAiType",i);
			return 0;
		}

		/*function*/
		static public int SetAiMode(bool b, int[] i /*1*/)	
		{
			Log("SetAiMode",i);
			return 0;
		}

		/*function*/
		static public int MyAiType(bool b, int[] i /*0*/)	
		{
			Log("MyAiType",i);
			return 0;
		}

		/*function*/
		static public int MyAiMode(bool b, int[] i /*0*/)	
		{
			Log("MyAiMode",i);
			return 0;
		}

		/*task*/
		static public int GotoLocation(bool b, int[] i /*1*/)	
		{
			Log("GotoLocation",i);
			return 0;
		}

		/*task*/
		static public int Guard(bool b, int[] i /*3*/)	
		{
			Log("Guard",i);
			return 0;
		}

		/*task*/
		static public int Animal(bool b, int[] i /*2*/)	
		{
			Log("Animal",i);
			return 0;
		}

		/*function*/
		static public int TrueFunction(bool b, int[] i /*0*/)	
		{
			Log("TrueFunction",i);
			return 0;
		}

		/*function*/
		static public int TurnToPos(bool b, int[] i /*2*/)	
		{
			Log("TurnToPos",i);
			return 0;
		}

		/*function*/
		static public int AtPos(bool b, int[] i /*3*/)	
		{
			Log("AtPos",i);
			return 0;
		}

		/*function*/
		static public int TurnToAngle(bool b, int[] i /*1*/)	
		{
			Log("TurnToAngle",i);
			return 0;
		}

		/*function*/
		static public int MovePos(bool b, int[] i /*3*/)	
		{
			Log("MovePos",i);
			return 0;
		}

		/*function*/
		static public int SetStartPosition(bool b, int[] i /*3*/)	
		{
			Log("SetStartPosition",i);
			return 0;
		}

		/*function*/
		static public int GetMyAttr(bool b, int[] i /*1*/)	
		{
			Log("GetMyAttr",i);
			return 0;
		}

		/*function*/
		static public int SetMyAttr(bool b, int[] i /*2*/)	
		{
			Log("SetMyAttr",i);
			return 0;
		}

		/*function*/
		static public int GetFramePassed(bool b, int[] i /*0*/)	
		{
			Log("GetFramePassed",i);
			return 0;
		}

		/*function*/
		static public int SpeedScale(bool b, int[] i /*1*/)	
		{
			Log("SpeedScale",i);
			return 0;
		}

		/*function*/
		static public int InKey(bool b, int[] i /*1*/)	
		{
			Log("InKey",i);
			return 0;
		}

		/*function*/
		static public int PressKey(bool b, int[] i /*1*/)	
		{
			Log("PressKey",i);
			return 0;
		}


		/*function in all caps?: FUNCTION	ACTIVATE	PARMS 1*/
		static public int ACTIVATE(bool b, int[] i /*1*/)	
		{
			Log("ACTIVATE",i);
			return 0;
		}

		/*function*/
		static public int TorchActivate(bool b, int[] i /*1*/)	
		{
			Log("TorchActivate",i);
			return 0;
		}

		/*function*/
		static public int Deactivate(bool b, int[] i /*0*/)	
		{
			Log("Deactivate",i);
			return 0;
		}

		/*function*/
		static public int ReleaseAnimation(bool b, int[] i /*0*/)	
		{
			Log("ReleaseAnimation",i);
			return 0;
		}

		/*function*/
		static public int HoldAnimation(bool b, int[] i /*0*/)	
		{
			Log("HoldAnimation",i);
			return 0;
		}

		/*function*/
		static public int PauseAnimation(bool b, int[] i /*1*/)	
		{
			Log("PauseAnimation",i);
			return 0;
		}

		/*function*/
		static public int SetAction(bool b, int[] i /*1*/)	
		{
			Log("SetAction",i);
			return 0;
		}

		/*function*/
		static public int ResetAction(bool b, int[] i /*1*/)	
		{
			Log("ResetAction",i);
			return 0;
		}

		/*function*/
		static public int ResetAllAction(bool b, int[] i /*0*/)	
		{
			Log("ResetAllAction",i);
			return 0;
		}

		/*task*/
		static public int PushAnimation(bool b, int[] i /*2*/)
		{
			Log("PushAnimation",i);
			// i[0]: animationId
			// i[1]: firstFrame
			return 0;
		}

		/*task*/
		static public int PushControlAnimation(bool b, int[] i /*2*/)
		{
			Log("PushControlAnimation",i);
			return 0;
		}

		/*task*/
		static public int WaitAnimFrame(bool b, int[] i /*2*/)
		{
			Log("WaitAnimFrame",i);
			// i[0]: animationId
			// i[1]: frame
			return 0;
		}

		/*task*/
		static public int WaitPlayerAnimFrame(bool b, int[] i /*0*/)
		{
			Log("WaitPlayerAnimFrame",i);
			// i[0]: animationId
			// i[1]: frame
			return 0;
		}

		/*function*/
		static public int KillMyTasks(bool b, int[] i /*0*/)	
		{
			Log("KillMyTasks",i);
			return 0;
		}

		/*task*/
		static public int ObjectLineUp(bool b, int[] i /*3*/)	
		{
			Log("ObjectLineUp",i);
			return 0;
		}

		/*task*/
		static public int ObjectParallelLineUp(bool b, int[] i /*3*/)	
		{
			Log("ObjectParallelLineUp",i);
			return 0;
		}

		/*task*/
		static public int ObjectPickupLineUp(bool b, int[] i /*3*/)	
		{
			Log("ObjectPickupLineUp",i);
			return 0;
		}

		/*task*/
		static public int LineUp(bool b, int[] i /*3*/)	
		{
			Log("LineUp",i);
			return 0;
		}

		/*task*/
		static public int WaitLineUp(bool b, int[] i /*1*/)	
		{
			Log("WaitLineUp",i);
			return 0;
		}

		/*function*/
		static public int MoveAboveMe(bool b, int[] i /*4*/)	
		{
			Log("MoveAboveMe",i);
			return 0;
		}

		/*function*/
		static public int KillMe(bool b, int[] i /*0*/)	
		{
			Log("KillMe",i);
			return 0;
		}

		/*function*/
		static public int EndSound(bool b, int[] i /*1*/)	
		{
			Log("EndSound",i);
			return 0;
		}

		/*function*/
		static public int EndMySounds(bool b, int[] i /*0*/)	
		{
			Log("EndMySounds",i);
			return 0;
		}

		/*function*/
		static public int EndMySound(bool b, int[] i /*1*/)	
		{
			Log("EndMySound",i);
			return 0;
		}

		/*function*/
		static public int StopAllSounds(bool b, int[] i /*0*/)	
		{
			Log("StopAllSounds",i);
			return 0;
		}

		/*task*/
		static public int ViewGXA(bool b, int[] i /*3*/)	
		{
			Log("ViewGXA",i);
			return 0;
		}

		/*task*/
		static public int AnimateGXA(bool b, int[] i /*4*/)	
		{
			Log("AnimateGXA",i);
			return 0;
		}

		/*function*/
		static public int PlayerOffset(bool b, int[] i /*3*/)	
		{
			Log("PlayerOffset",i);
			return 0;
		}

		/*multitask*/
		static public int PlayerOffsettask(bool b, int[] i /*3*/)	
		{
			Log("PlayerOffsettask",i);
			return 0;
		}

		/*function*/
		static public int Offset(bool b, int[] i /*3*/)	
		{
			Log("Offset",i);
			return 0;
		}

		/*function*/
		static public int UnOffset(bool b, int[] i /*0*/)	
		{
			Log("UnOffset",i);
			return 0;
		}

		/*task*/
		static public int BounceObject(bool b, int[] i /*3*/)	
		{
			Log("BounceObject",i);
			return 0;
		}

		/*function*/
		static public int OffsetLocation(bool b, int[] i /*4*/)	
		{
			Log("OffsetLocation",i);
			return 0;
		}

		/*function*/
		static public int DistanceToLocation(bool b, int[] i /*1*/)	
		{
			Log("DistanceToLocation",i);
			return 0;
		}

		/*function*/
		static public int ShowBitmap(bool b, int[] i /*2*/)	
		{
			Log("ShowBitmap",i);
			return 0;
		}

		/*function*/
		static public int UnShowBitmap(bool b, int[] i /*1*/)	
		{
			Log("UnShowBitmap",i);
			return 0;
		}

		/*function*/
		static public int ReplenishHealth(bool b, int[] i /*0*/)	
		{
			Log("ReplenishHealth",i);
			return 0;
		}

		/*function*/
		static public int InRectangle(bool b, int[] i /*4*/)	
		{
			Log("InRectangle",i);
			return 0;
		}

		/*function*/
		static public int InCircle(bool b, int[] i /*3*/)	
		{
			Log("InCircle",i);
			return 0;
		}

		/*function*/
		static public int ScriptParticles(bool b, int[] i /*8*/)	
		{
			Log("ScriptParticles",i);
			return 0;
		}

		/*function*/
		static public int LightTorch(bool b, int[] i /*0*/)	
		{
			Log("LightTorch",i);
			return 0;
		}

		/*function*/
		static public int UnlightTorch(bool b, int[] i /*0*/)	
		{
			Log("UnlightTorch",i);
			return 0;
		}

		/*function*/
		static public int LoadStatic(bool b, int[] i /*1*/)	
		{
			Log("LoadStatic",i);
			return 0;
		}

		/*function*/
		static public int UnLoadStatic(bool b, int[] i /*0*/)	
		{
			Log("UnLoadStatic",i);
			return 0;
		}

		/*function*/
		static public int PointAt(bool b, int[] i /*0*/)	
		{
			Log("PointAt",i);
			return 0;
		}


		//combat commands
		/*function*/
		static public int beginCombat(bool b, int[] i /*0*/)	
		{
			Log("beginCombat",i);
			return 0;
		}

		/*function*/
		static public int endCombat(bool b, int[] i /*0*/)	
		{
			Log("endCombat",i);
			return 0;
		}

		/*function*/
		static public int isFighting(bool b, int[] i /*0*/)	
		{
			Log("isFighting",i);
			return 0;
		}

		/*function*/
		static public int isDead(bool b, int[] i /*0*/)	
		{
			Log("isDead",i);
			return 0;
		}

		/*function*/
		static public int adjustHealth(bool b, int[] i /*2*/)	
		{
			Log("adjustHealth",i);
			return 0;
		}

		/*function*/
		static public int setHealth(bool b, int[] i /*1*/)	
		{
			Log("setHealth",i);
			return 0;
		}

		/*function*/
		static public int health(bool b, int[] i /*0*/)	
		{
			Log("health",i);
			return 0;
		}

		/*function*/
		static public int adjustStrength(bool b, int[] i /*1*/)	
		{
			Log("adjustStrength",i);
			return 0;
		}

		/*function*/
		static public int setStrength(bool b, int[] i /*1*/)	
		{
			Log("setStrength",i);
			return 0;
		}

		/*function*/
		static public int strength(bool b, int[] i /*0*/)	
		{
			Log("strength",i);
			return 0;
		}

		/*function*/
		static public int adjustArmor(bool b, int[] i /*1*/)	
		{
			Log("adjustArmor",i);
			return 0;
		}

		/*function*/
		static public int setArmor(bool b, int[] i /*1*/)	
		{
			Log("setArmor",i);
			return 0;
		}

		/*function*/
		static public int armor(bool b, int[] i /*0*/)	
		{
			Log("armor",i);
			return 0;
		}

		/*function*/
		static public int shoot(bool b, int[] i /*7*/)	
		{
			Log("shoot",i);
			return 0;
		}

		/*function*/
		static public int shootPlayer(bool b, int[] i /*4*/)	
		{
			Log("shootPlayer",i);
			return 0;
		}

		/*function*/
		static public int resurrect(bool b, int[] i /*0*/)	
		{
			Log("resurrect",i);
			return 0;
		}


		//object script/task control
		/*function*/
		static public int EnableObject(bool b, int[] i /*0*/)	
		{
			Log("EnableObject",i);
			return 0;
		}

		/*function*/
		static public int DisableObject(bool b, int[] i /*0*/)	
		{
			Log("DisableObject",i);
			return 0;
		}

		/*function*/
		static public int EnableTasks(bool b, int[] i /*0*/)	
		{
			Log("EnableTasks",i);
			return 0;
		}

		/*function*/
		static public int DisableTasks(bool b, int[] i /*0*/)	
		{
			Log("DisableTasks",i);
			return 0;
		}

		/*function*/
		static public int EnableScript(bool b, int[] i /*0*/)	
		{
			Log("EnableScript",i);
			return 0;
		}

		/*function*/
		static public int DisableScript(bool b, int[] i /*0*/)	
		{
			Log("DisableScript",i);
			return 0;
		}

		/*function*/
		static public int EnableHook(bool b, int[] i /*0*/)	
		{
			Log("EnableHook",i);
			return 0;
		}

		/*function*/
		static public int DisableHook(bool b, int[] i /*0*/)	
		{
			Log("DisableHook",i);
			return 0;
		}

		/*function*/
		static public int EnableCombat(bool b, int[] i /*0*/)	
		{
			Log("EnableCombat",i);
			return 0;
		}

		/*function*/
		static public int DisableCombat(bool b, int[] i /*0*/)	
		{
			Log("DisableCombat",i);
			return 0;
		}


		//rotational and positional reset commands

		/*function*/
		static public int SetRotation(bool b, int[] i /*3*/)	
		{
			Log("SetRotation",i);
			return 0;
		}

		/*function*/
		static public int SetPosition(bool b, int[] i /*3*/)	
		{
			Log("SetPosition",i);
			return 0;
		}

		/*function*/
		static public int ResetRotation(bool b, int[] i /*3*/)	
		{
			Log("ResetRotation",i);
			return 0;
		}

		/*function*/
		static public int ResetPosition(bool b, int[] i /*3*/)	
		{
			Log("ResetPosition",i);
			return 0;
		}

		/*function*/
		static public int ReloadRotation(bool b, int[] i /*0*/)	
		{
			Log("ReloadRotation",i);
			return 0;
		}

		/*function*/
		static public int ReloadPosition(bool b, int[] i /*0*/)	
		{
			Log("ReloadPosition",i);
			return 0;
		}

		/*function*/
		static public int SetLocation(bool b, int[] i /*1*/)	
		{
			Log("SetLocation",i);
			return 0;
		}


		//master and slave commands
		/*function*/
		static public int AttachMe(bool b, int[] i /*1*/)	
		{
			Log("AttachMe",i);
			return 0;
		}

		/*function*/
		static public int DetachMe(bool b, int[] i /*0*/)	
		{
			Log("DetachMe",i);
			return 0;
		}

		/*function*/
		static public int DetachSlaves(bool b, int[] i /*0*/)	
		{
			Log("DetachSlaves",i);
			return 0;
		}

		/*function*/
		static public int KillSlaves(bool b, int[] i /*0*/)	
		{
			Log("KillSlaves",i);
			return 0;
		}

		/*function*/
		static public int HideSlaves(bool b, int[] i /*0*/)	
		{
			Log("HideSlaves",i);
			return 0;
		}

		/*function*/
		static public int ShowSlaves(bool b, int[] i /*0*/)	
		{
			Log("ShowSlaves",i);
			return 0;
		}

		/*function*/
		static public int EnableSlaves(bool b, int[] i /*0*/)	
		{
			Log("EnableSlaves",i);
			return 0;
		}

		/*function*/
		static public int DisableSlaves(bool b, int[] i /*0*/)	
		{
			Log("DisableSlaves",i);
			return 0;
		}

		/*function*/
		static public int IsSlave(bool b, int[] i /*0*/)	
		{
			Log("IsSlave",i);
			return 0;
		}

		/*function*/
		static public int IsMaster(bool b, int[] i /*0*/)	
		{
			Log("IsMaster",i);
			return 0;
		}

		/*function*/
		static public int FixSlaveAngle(bool b, int[] i /*2*/)	
		{
			Log("FixSlaveAngle",i);
			return 0;
		}


		//player commands
		/*function*/
		static public int PlayerMoved(bool b, int[] i /*0*/)	
		{
			Log("PlayerMoved",i);
			return 0;
		}

		/*function*/
		static public int PlayerTurned(bool b, int[] i /*0*/)	
		{
			Log("PlayerTurned",i);
			return 0;
		}

		/*function*/
		static public int PlayerTurnedDirection(bool b, int[] i /*0*/)	
		{
			Log("PlayerTurnedDirection",i);
			return 0;
		}

		/*function*/
		static public int PlayerMovementSpeed(bool b, int[] i /*0*/)	
		{
			Log("PlayerMovementSpeed",i);
			return 0;
		}

		/*function*/
		static public int GetMovementSpeed(bool b, int[] i /*0*/)	
		{
			Log("GetMovementSpeed",i);
			return 0;
		}

		/*function*/
		static public int PlayerHealth(bool b, int[] i /*0*/)	
		{
			Log("PlayerHealth",i);
			return 0;
		}

		/*function*/
		static public int PlayerStatus(bool b, int[] i /*0*/)	
		{
			Log("PlayerStatus",i);
			return 0;
		}

		/*function*/
		static public int PlayerCollide(bool b, int[] i /*0*/)	
		{
			Log("PlayerCollide",i);
			return 0;
		}

		/*function*/
		static public int PlayerStand(bool b, int[] i /*0*/)	
		{
			Log("PlayerStand",i);
			return 1;
		}

		/*function*/
		static public int PlayerArmed(bool b, int[] i /*0*/)	
		{
			Log("PlayerArmed",i);
			return 0;
		}

		/*function*/
		static public int PlayerHanging(bool b, int[] i /*0*/)	
		{
			Log("PlayerHanging",i);
			return 0;
		}

		/*function*/
		static public int PlayerInSight(bool b, int[] i /*0*/)	
		{
			Log("PlayerInSight",i);
			return 0;
		}

		/*function*/
		static public int PlayerDistance(bool b, int[] i /*0*/)	
		{
			Log("PlayerDistance",i);
			return 0;
		}

		/*function*/
		static public int AcuratePlayerDistance(bool b, int[] i /*0*/)	
		{
			Log("AcuratePlayerDistance",i);
			return 0;
		}

		/*function*/
		static public int PlayerFacing(bool b, int[] i /*2*/)	
		{
			Log("PlayerFacing",i);
			return 0;
		}

		/*function*/
		static public int PlayerArcTan(bool b, int[] i /*0*/)	
		{
			Log("PlayerArcTan",i);
			return 0;
		}

		/*function*/
		static public int PlayerArc(bool b, int[] i /*2*/)	
		{
			Log("PlayerArc",i);
			return 0;
		}

		/*function*/
		static public int PlayerLooking(bool b, int[] i /*1*/)	
		{
			Log("PlayerLooking",i);
			return 0;
		}

		/*multitask*/
		static public int PlayerTilt(bool b, int[] i /*3*/)	
		{
			Log("PlayerTilt",i);
			return 0;
		}

		/*multitask*/
		static public int PlayerTiltRevx(bool b, int[] i /*3*/)	
		{
			Log("PlayerTiltRevx",i);
			return 0;
		}

		/*function*/
		static public int PlayerSnap(bool b, int[] i /*3*/)	
		{
			Log("PlayerSnap",i);
			return 0;
		}

		/*task*/
		static public int PlayerLineUp(bool b, int[] i /*3*/)	
		{
			Log("PlayerLineUp",i);
			return 0;
		}

		/*task*/
		static public int PlayerParallelLineUp(bool b, int[] i /*3*/)	
		{
			Log("PlayerParallelLineUp",i);
			return 0;
		}

		/*task*/
		static public int PlayerPickupLineUp(bool b, int[] i /*3*/)	
		{
			Log("PlayerPickupLineUp",i);
			return 0;
		}

		/*function*/
		static public int MovePlayerAboveMe(bool b, int[] i /*4*/)	
		{
			Log("MovePlayerAboveMe",i);
			return 0;
		}

		/*function*/
		static public int PushPlayer(bool b, int[] i /*1*/)	
		{
			Log("PushPlayer",i);
			return 0;
		}

		/*function*/
		static public int PopPlayer(bool b, int[] i /*0*/)	
		{
			Log("PopPlayer",i);
			return 0;
		}

		/*task*/
		static public int PlayerFaceObj(bool b, int[] i /*0*/)	
		{
			Log("PlayerFaceObj",i);
			return 0;
		}

		/*function*/
		static public int PlayerOnScape(bool b, int[] i /*0*/)	
		{
			Log("PlayerOnScape",i);
			return 0;
		}

		/*function*/
		static public int PlayerCollideStatic(bool b, int[] i /*0*/)	
		{
			Log("PlayerCollideStatic",i);
			return 0;
		}

		/*function*/
		static public int WonGame(bool b, int[] i /*0*/)	
		{
			Log("WonGame",i);
			return 0;
		}

		/*function*/
		static public int MainloopExit(bool b, int[] i /*1*/)	
		{
			Log("MainloopExit",i);
			return 0;
		}

		/*task*/
		static public int PlayerFaceTalk(bool b, int[] i /*1*/)	
		{
			Log("PlayerFaceTalk",i);
			return 0;
		}

		/*function*/
		static public int PlayerForceTalk(bool b, int[] i /*1*/)	
		{
			Log("PlayerForceTalk",i);
			return 0;
		}

		/*function*/
		static public int CollidePlayerWeapon(bool b, int[] i /*4*/)	
		{
			Log("CollidePlayerWeapon",i);
			return 0;
		}


		//object and player health
		/*function*/
		static public int ZapPlayer(bool b, int[] i /*1*/)	
		{
			Log("ZapPlayer",i);
			return 0;
		}

		/*function*/
		static public int ZapPlayerNoKill(bool b, int[] i /*1*/)	
		{
			Log("ZapPlayerNoKill",i);
			return 0;
		}

		/*function*/
		static public int ZapObject(bool b, int[] i /*1*/)	
		{
			Log("ZapObject",i);
			return 0;
		}

		/*function*/
		static public int ZapObjectNoKill(bool b, int[] i /*1*/)	
		{
			Log("ZapObjectNoKill",i);
			return 0;
		}


		//object system messaging
		/*function*/
		static public int SendMessage(bool b, int[] i /*1*/)	
		{
			Log("SendMessage",i);
			return 0;
		}

		/*function*/
		static public int ReceiveMessage(bool b, int[] i /*0*/)	
		{
			Log("ReceiveMessage",i);
			return 0;
		}

		/*function*/
		static public int IsMessageWaiting(bool b, int[] i /*0*/)	
		{
			Log("IsMessageWaiting",i);
			return 0;
		}

		/*function*/
		static public int GetMessageSender(bool b, int[] i /*0*/)	
		{
			Log("GetMessageSender",i);
			return 0;
		}

		/*function*/
		static public int GetMessageSenderId(bool b, int[] i /*0*/)	
		{
			Log("GetMessageSenderId",i);
			return 0;
		}

		/*function*/
		static public int ReplySender(bool b, int[] i /*1*/)	
		{
			Log("ReplySender",i);
			return 0;
		}

		/*function*/
		static public int DeleteMessage(bool b, int[] i /*0*/)	
		{
			Log("DeleteMessage",i);
			return 0;
		}

		/*function*/
		static public int SendBroadcast(bool b, int[] i /*1*/)	
		{
			Log("SendBroadcast",i);
			return 0;
		}

		/*function*/
		static public int ReceiveBroadcast(bool b, int[] i /*0*/)	
		{
			Log("ReceiveBroadcast",i);
			return 0;
		}

		/*function*/
		static public int DeleteBroadcast(bool b, int[] i /*0*/)	
		{
			Log("DeleteBroadcast",i);
			return 0;
		}

		/*function*/
		static public int IsBroadcastWaiting(bool b, int[] i /*0*/)	
		{
			Log("IsBroadcastWaiting",i);
			return 0;
		}

		/*function*/
		static public int GetBroadcastSender(bool b, int[] i /*0*/)	
		{
			Log("GetBroadcastSender",i);
			return 0;
		}

		/*function*/
		static public int GetBroadcastSenderId(bool b, int[] i /*0*/)	
		{
			Log("GetBroadcastSenderId",i);
			return 0;
		}

		/*function*/
		static public int ReplyBroadcastSender(bool b, int[] i /*1*/)	
		{
			Log("ReplyBroadcastSender",i);
			return 0;
		}


		//sync tasks and functions
		/*function*/
		static public int ResetGroupSync(bool b, int[] i /*0*/)	
		{
			Log("ResetGroupSync",i);
			return 0;
		}

		/*function*/
		static public int ResetUserSync(bool b, int[] i /*0*/)	
		{
			Log("ResetUserSync",i);
			return 0;
		}

		/*task*/
		static public int SyncWithGroup(bool b, int[] i /*1*/)	
		{
			Log("SyncWithGroup",i);
			// i[0]: sync group
			return 0;
		}

		/*task*/
		static public int SyncWithUser(bool b, int[] i /*1*/)	
		{
			Log("SyncWithUser",i);
			return 0;
		}

		/*function*/
		static public int ChangeGroup(bool b, int[] i /*1*/)	
		{
			Log("ChangeGroup",i);
			return 0;
		}

		/*function*/
		static public int ChangeUser(bool b, int[] i /*1*/)	
		{
			Log("ChangeUser",i);
			return 0;
		}


		//pipelining functions
		/*function*/
		static public int PipeMyGroup(bool b, int[] i /*0*/)	
		{
			Log("PipeMyGroup",i);
			return 0;
		}

		/*function*/
		static public int PipeGroup(bool b, int[] i /*1*/)	
		{
			Log("PipeGroup",i);
			return 0;
		}

		/*function*/
		static public int PipeUser(bool b, int[] i /*1*/)	
		{
			Log("PipeUser",i);
			return 0;
		}

		/*function*/
		static public int PipeMasters(bool b, int[] i /*0*/)	
		{
			Log("PipeMasters",i);
			return 0;
		}

		/*function*/
		static public int PipeSlaves(bool b, int[] i /*0*/)	
		{
			Log("PipeSlaves",i);
			return 0;
		}

		/*function*/
		static public int PipeAroundMe(bool b, int[] i /*1*/)	
		{
			Log("PipeAroundMe",i);
			return 0;
		}

		/*function*/
		static public int PipeAroundPos(bool b, int[] i /*4*/)	
		{
			Log("PipeAroundPos",i);
			return 0;
		}

		/*function*/
		static public int PipeHidden(bool b, int[] i /*0*/)	
		{
			Log("PipeHidden",i);
			return 0;
		}


		//node map negotiation
		/*task*/
		static public int MoveNodeMarker(bool b, int[] i /*2*/)	
		{
			Log("MoveNodeMarker",i);
			return 0;
		}

		/*task*/
		static public int MoveNodeLocation(bool b, int[] i /*2*/)	
		{
			Log("MoveNodeLocation",i);
			return 0;
		}

		/*task*/
		static public int MoveNodePosition(bool b, int[] i /*4*/)	
		{
			Log("MoveNodePosition",i);
			return 0;
		}

		/*function*/
		static public int AtNodeMarker(bool b, int[] i /*2*/)	
		{
			Log("AtNodeMarker",i);
			return 0;
		}

		/*function*/
		static public int AtNodeLocation(bool b, int[] i /*2*/)	
		{
			Log("AtNodeLocation",i);
			return 0;
		}

		/*function*/
		static public int AtNodePosition(bool b, int[] i /*4*/)	
		{
			Log("AtNodePosition",i);
			return 0;
		}

		/*function*/
		static public int Wander(bool b, int[] i /*2*/)	
		{
			Log("Wander",i);
			return 0;
		}

		/*function*/
		static public int DisableNodeMaps(bool b, int[] i /*0*/)	
		{
			Log("DisableNodeMaps",i);
			return 0;
		}

		/*function*/
		static public int EnableNodeMaps(bool b, int[] i /*0*/)	
		{
			Log("EnableNodeMaps",i);
			return 0;
		}


		//node marker negotiation
		/*task*/
		static public int MoveMarker(bool b, int[] i /*2*/)	
		{
			Log("MoveMarker",i);
			return 0;
		}

		/*task*/
		static public int MoveLocation(bool b, int[] i /*2*/)	
		{
			Log("MoveLocation",i);
			return 0;
		}

		/*task*/
		static public int MovePosition(bool b, int[] i /*4*/)	
		{
			Log("MovePosition",i);
			return 0;
		}


		//weapons and hand-objects
		/*function*/
		static public int DrawSword(bool b, int[] i /*0*/)	
		{
			Log("DrawSword",i);
			return 0;
		}

		/*function*/
		static public int SheathSword(bool b, int[] i /*0*/)	
		{
			Log("SheathSword",i);
			return 0;
		}

		/*function*/
		static public int CheckPlayerWeapon(bool b, int[] i /*0*/)	
		{
			Log("CheckPlayerWeapon",i);
			return 0;
		}

		/*function*/
		static public int DisplayHandModel(bool b, int[] i /*4*/)	
		{
			Log("DisplayHandModel",i);
			return 0;
		}

		/*function*/
		static public int DisplayHandItem(bool b, int[] i /*4*/)	
		{
			Log("DisplayHandItem",i);
			return 0;
		}

		/*function*/
		static public int DisplayHiltModel(bool b, int[] i /*4*/)	
		{
			Log("DisplayHiltModel",i);
			return 0;
		}

		/*function*/
		static public int DisplayHiltItem(bool b, int[] i /*4*/)	
		{
			Log("DisplayHiltItem",i);
			return 0;
		}


		//item's and the inventory
		/*function*/
		static public int HandItem(bool b, int[] i /*1*/)	
		{
			Log("HandItem",i);
			return 0;
		}

		/*function*/
		static public int HaveItem(bool b, int[] i /*1*/)	
		{
			Log("HaveItem",i);
			return 0;
		}

		/*function*/
		static public int RemoveItem(bool b, int[] i /*1*/)	
		{
			Log("RemoveItem",i);
			return 0;
		}

		/*function*/
		static public int ItemUsed(bool b, int[] i /*1*/)	
		{
			Log("ItemUsed",i);
			return 0;
		}

		/*function*/
		static public int PushItem(bool b, int[] i /*2*/)	
		{
			Log("PushItem",i);
			return 0;
		}

		/*function*/
		static public int PopItem(bool b, int[] i /*1*/)	
		{
			Log("PopItem",i);
			return 0;
		}

		/*function*/
		static public int AddItem(bool b, int[] i /*2*/)	
		{
			Log("AddItem",i);
			return 0;
		}

		/*function*/
		static public int SubItem(bool b, int[] i /*2*/)	
		{
			Log("SubItem",i);
			return 0;
		}

		/*function*/
		static public int DropItem(bool b, int[] i /*2*/)	
		{
			Log("DropItem",i);
			return 0;
		}

		/*function*/
		static public int UseItem(bool b, int[] i /*1*/)	
		{
			Log("UseItem",i);
			return 0;
		}

		/*function*/
		static public int ShowItem(bool b, int[] i /*4*/)	
		{
			Log("ShowItem",i);
			return 0;
		}

		/*function*/
		static public int ShowItemNoRtx(bool b, int[] i /*3*/)	
		{
			Log("ShowItemNoRtx",i);
			return 0;
		}

		/*function*/
		static public int ActiveItem(bool b, int[] i /*0*/)	
		{
			Log("ActiveItem",i);
			return 0;
		}

		/*function*/
		static public int PushAllItems(bool b, int[] i /*1*/)	
		{
			Log("PushAllItems",i);
			return 0;
		}

		/*function*/
		static public int PopAllItems(bool b, int[] i /*0*/)	
		{
			Log("PopAllItems",i);
			return 0;
		}

		/*function*/
		static public int DropAllItems(bool b, int[] i /*0*/)	
		{
			Log("DropAllItems",i);
			return 0;
		}

		/*function*/
		static public int SelectItem(bool b, int[] i /*1*/)	
		{
			Log("SelectItem",i);
			return 0;
		}


		//general object queries
		/*function*/
		static public int IsOnEdge(bool b, int[] i /*0*/)	
		{
			Log("IsOnEdge",i);
			return 0;
		}

		/*function*/
		static public int IsHoldingWeapon(bool b, int[] i /*0*/)	
		{
			Log("IsHoldingWeapon",i);
			return 0;
		}

		/*function*/
		static public int IsSheathingSword(bool b, int[] i /*0*/)	
		{
			Log("IsSheathingSword",i);
			return 0;
		}

		/*function*/
		static public int IsDrawingSword(bool b, int[] i /*0*/)	
		{
			Log("IsDrawingSword",i);
			return 0;
		}

		/*function*/
		static public int IsDrawingOrSheathing(bool b, int[] i /*0*/)	
		{
			Log("IsDrawingOrSheathing",i);
			return 0;
		}

		/*function*/
		static public int IsCarryingWeapon(bool b, int[] i /*0*/)	
		{
			Log("IsCarryingWeapon",i);
			return 0;
		}

		/*function*/
		static public int IsCombatCapable(bool b, int[] i /*0*/)	
		{
			Log("IsCombatCapable",i);
			return 0;
		}

		/*function*/
		static public int IsShellScript(bool b, int[] i /*0*/)	
		{
			Log("IsShellScript",i);
			return 0;
		}

		/*function*/
		static public int IsWithinView(bool b, int[] i /*0*/)	
		{
			Log("IsWithinView",i);
			return 0;
		}

		/*function*/
		static public int IsWithinMap(bool b, int[] i /*0*/)	
		{
			Log("IsWithinMap",i);
			return 0;
		}

		/*function*/
		static public int IsInAir(bool b, int[] i /*0*/)	
		{
			Log("IsInAir",i);
			return 0;
		}

		/*function*/
		static public int IsLightingOrUnlighting(bool b, int[] i /*0*/)	
		{
			Log("IsLightingOrUnlighting",i);
			return 0;
		}

		/*function*/
		static public int IsUnlightingTorch(bool b, int[] i /*0*/)	
		{
			Log("IsUnlightingTorch",i);
			return 0;
		}

		/*function*/
		static public int IsLightingTorch(bool b, int[] i /*0*/)	
		{
			Log("IsLightingTorch",i);
			return 0;
		}

		/*function*/
		static public int IsInLava(bool b, int[] i /*0*/)	
		{
			Log("IsInLava",i);
			return 0;
		}

		/*function*/
		static public int IsInDeepWater(bool b, int[] i /*0*/)	
		{
			Log("IsInDeepWater",i);
			return 0;
		}

		/*function*/
		static public int IsInWater(bool b, int[] i /*0*/)	
		{
			Log("IsInWater",i);
			return 0;
		}

		/*function*/
		static public int IsBouncing(bool b, int[] i /*0*/)	
		{
			Log("IsBouncing",i);
			return 0;
		}


		//shell AI functions
		/*function*/
		static public int OpenShell(bool b, int[] i /*1*/)	
		{
			Log("OpenShell",i);
			return 0;
		}

		/*function*/
		static public int CountAiShells(bool b, int[] i /*1*/)	
		{
			Log("CountAiShells",i);
			return 0;
		}

		/*function*/
		static public int FindAiShell(bool b, int[] i /*1*/)	
		{
			Log("FindAiShell",i);
			return 0;
		}


		//effects system
		/*function*/
		static public int AddFlatEffect(bool b, int[] i /*6*/)	
		{
			Log("AddFlatEffect",i);
			return 0;
		}

		/*function*/
		static public int AddLightEffect(bool b, int[] i /*8*/)	
		{
			Log("AddLightEffect",i);
			return 0;
		}

		/*function*/
		static public int GolemSteam(bool b, int[] i /*0*/)	
		{
			Log("GolemSteam",i);
			return 0;
		}

		/*function*/
		static public int DragonBreath(bool b, int[] i /*0*/)	
		{
			Log("DragonBreath",i);
			return 0;
		}

		/*multitask*/
		static public int DragonBreathTask(bool b, int[] i /*0*/)	
		{
			Log("DragonBreathTask",i);
			return 0;
		}


		//Smacker functions
		/*function*/
		static public int QueueMovie(bool b, int[] i /*1*/)	
		{
			Log("QueueMovie",i);
			return 0;
		}


		/*task*/
		static public int TestTask(bool b, int[] i /*1*/)	
		{
			Log("TestTask",i);
			return 0;
		}

		/*function*/
		static public int XResetCamera(bool b, int[] i /*0*/)	
		{
			Log("XResetCamera",i);
			return 0;
		}


		//Potion functions
		/*function*/
		static public int SetPotion(bool b, int[] i /*2*/)	
		{
			Log("SetPotion",i);
			return 0;
		}

		/*function*/
		static public int GetPotion(bool b, int[] i /*1*/)	
		{
			Log("GetPotion",i);
			return 0;
		}


		//Log Book functions
		/*function*/
		static public int AddLog(bool b, int[] i /*1*/)	
		{
			Log("AddLog",i);
			return 0;
		}


		//spare expansion functions, (do nothing at the moment)
		/*function*/
		static public int Spare0Parm0(bool b, int[] i /*0*/)	
		{
			Log("Spare0Parm0",i);
			return 0;
		}
		static public int Spare1Parm0(bool b, int[] i /*0*/)	
		{
			Log("Spare1Parm0",i);
			return 0;
		}

		/*function*/
		static public int Spare0Parm1(bool b, int[] i /*1*/)	
		{
			Log("Spare0Parm1",i);
			return 0;
		}

		/*function*/
		static public int Spare1Parm1(bool b, int[] i /*1*/)	
		{
			Log("Spare1Parm1",i);
			return 0;
		}

		/*function*/
		static public int Spare0Parm2(bool b, int[] i /*2*/)	
		{
			Log("Spare0Parm2",i);
			return 0;
		}

		/*function*/
		static public int Spare1Parm2(bool b, int[] i /*2*/)	
		{
			Log("Spare1Parm2",i);
			return 0;
		}

		/*function*/
		static public int Spare0Parm3(bool b, int[] i /*3*/)	
		{
			Log("Spare0Parm3",i);
			return 0;
		}

		/*function*/
		static public int Spare1Parm3(bool b, int[] i /*3*/)	
		{
			Log("Spare1Parm3",i);
			return 0;
		}

		/*function*/
		static public int Spare0Parm4(bool b, int[] i /*4*/)	
		{
			Log("Spare0Parm4",i);
			return 0;
		}

		/*function*/
		static public int Spare1Parm4(bool b, int[] i /*4*/)	
		{
			Log("Spare1Parm4",i);
			return 0;
		}

		/*function*/
		static public int Spare0Parm5(bool b, int[] i /*5*/)	
		{
			Log("Spare0Parm5",i);
			return 0;
		}

		/*function*/
		static public int Spare1Parm5(bool b, int[] i /*5*/)	
		{
			Log("Spare1Parm5",i);
			return 0;
		}

		/*function*/
		static public int Spare0Parm6(bool b, int[] i /*6*/)	
		{
			Log("Spare0Parm6",i);
			return 0;
		}

		/*function*/
		static public int Spare1Parm6(bool b, int[] i /*6*/)	
		{
			Log("Spare1Parm6",i);
			return 0;
		}

		/*function*/
		static public int Spare0Parm7(bool b, int[] i /*7*/)	
		{
			Log("Spare0Parm7",i);
			return 0;
		}

		/*function*/
		static public int Spare1Parm7(bool b, int[] i /*8*/)	
		{
			Log("Spare1Parm7",i);
			return 0;
		}
        static Func<bool, int[], int>[] SetupFuncs()
        {
            int sdcnt = 367;
            Func<bool, int[], int>[] soupdeffcn = new Func<bool, int[], int>[sdcnt];
            soupdeffcn[0]=PLACEHOLDER_ZERO;
			soupdeffcn[1]=sRotate;
			soupdeffcn[2]=sRotateByAxis;
			soupdeffcn[3]=sRotateToAxis;
			soupdeffcn[4]=sMove;
			soupdeffcn[5]=sMoveByAxis;
			soupdeffcn[6]=sMoveAlongAxis;
			soupdeffcn[7]=HeaderHook;
			soupdeffcn[8]=swordAI;
			soupdeffcn[9]=projectileAI;
			soupdeffcn[10]=chargeWeapon;
			soupdeffcn[11]=PlayerMain;
			soupdeffcn[12]=PrintParms;
			soupdeffcn[13]=LogParms;
			soupdeffcn[14]=PrintStringParm;
			soupdeffcn[15]=PrintSingleParm;
			soupdeffcn[16]=SpacePressed;
			soupdeffcn[17]=WaitOnTasks;
			soupdeffcn[18]=updatePlayerViewAttrib;
			soupdeffcn[19]=cameraController;
			soupdeffcn[20]=showObjRot;
			soupdeffcn[21]=showObj;
			soupdeffcn[22]=showObjLoc;
			soupdeffcn[23]=showObjPan;
			soupdeffcn[24]=showObjPanLoc;
			soupdeffcn[25]=lookObj;
			soupdeffcn[26]=showPlayer;
			soupdeffcn[27]=showPlayerPan;
			soupdeffcn[28]=lookCyrus;
			soupdeffcn[29]=showCyrus;
			soupdeffcn[30]=showCyrusLoc;
			soupdeffcn[31]=showCyrusPan;
			soupdeffcn[32]=showCyrusPanLoc;
			soupdeffcn[33]=PlayAnimation;
			soupdeffcn[34]=lockoutPlayer;
			soupdeffcn[35]=menuNew;
			soupdeffcn[36]=menuProc;
			soupdeffcn[37]=menuAddItem;
			soupdeffcn[38]=menuSelection;
			soupdeffcn[39]=RTX;
			soupdeffcn[40]=rtxAnim;
			soupdeffcn[41]=RTXpAnim;
			soupdeffcn[42]=RTXp;
			soupdeffcn[43]=Rotate;
			soupdeffcn[44]=RotateByAxis;
			soupdeffcn[45]=RotateToAxis;
			soupdeffcn[46]=WalkForward;
			soupdeffcn[47]=WalkBackward;
			soupdeffcn[48]=MoveForward;
			soupdeffcn[49]=MoveBackward;
			soupdeffcn[50]=MoveLeft;
			soupdeffcn[51]=MoveRight;
			soupdeffcn[52]=Move;
			soupdeffcn[53]=MoveByAxis;
			soupdeffcn[54]=MoveAlongAxis;
			soupdeffcn[55]=MoveObjectAxis;
			soupdeffcn[56]=MoveToLocation;
			soupdeffcn[57]=WanderToLocation;
			soupdeffcn[58]=MoveToMarker;
			soupdeffcn[59]=SetObjectLocation;
			soupdeffcn[60]=Wait;
			soupdeffcn[61]=DistanceFromStart;
			soupdeffcn[62]=Light;
			soupdeffcn[63]=LightRadius;
			soupdeffcn[64]=LightIntensity;
			soupdeffcn[65]=LightOff;
			soupdeffcn[66]=LightOffset;
			soupdeffcn[67]=LightFlicker;
			soupdeffcn[68]=FlickerLight;
			soupdeffcn[69]=LightFlickerOff;
			soupdeffcn[70]=LightSize;
			soupdeffcn[71]=LightSizeOff;
			soupdeffcn[72]=FxPhase;
			soupdeffcn[73]=FxFlickerOnOff;
			soupdeffcn[74]=FxFlickerDim;
			soupdeffcn[75]=FxLightSize;
			soupdeffcn[76]=Flat;
			soupdeffcn[77]=FlatSetTexture;
			soupdeffcn[78]=FlatOff;
			soupdeffcn[79]=FlatOffset;
			soupdeffcn[80]=FlatLikeStatic;
			soupdeffcn[81]=FlatAnimate;
			soupdeffcn[82]=FlatStop;
			soupdeffcn[83]=SetAttribute;
			soupdeffcn[84]=GetAttribute;
			soupdeffcn[85]=SetGlobalFlag;
			soupdeffcn[86]=TestGlobalFlag;
			soupdeffcn[87]=ResetGlobalFlag;
			soupdeffcn[88]=FacePlayer;
			soupdeffcn[89]=FacePlayerInertia;
			soupdeffcn[90]=FaceAngle;
			soupdeffcn[91]=FacePos;
			soupdeffcn[92]=FaceObject;
			soupdeffcn[93]=Sound;
			soupdeffcn[94]=FlatSound;
			soupdeffcn[95]=AmbientSound;
			soupdeffcn[96]=AmbientRtx;
			soupdeffcn[97]=Jump;
			soupdeffcn[98]=WaitNonMulti;
			soupdeffcn[99]=WaitOnDialog;
			soupdeffcn[100]=HideMe;
			soupdeffcn[101]=ShowMe;
			soupdeffcn[102]=Rnd;
			soupdeffcn[103]=QuickRnd;
			soupdeffcn[104]=MyId;
			soupdeffcn[105]=FaceLocation;
			soupdeffcn[106]=DistanceFromLocation;
			soupdeffcn[107]=LoadWorld;
			soupdeffcn[108]=SetAiType;
			soupdeffcn[109]=SetAiMode;
			soupdeffcn[110]=MyAiType;
			soupdeffcn[111]=MyAiMode;
			soupdeffcn[112]=GotoLocation;
			soupdeffcn[113]=Guard;
			soupdeffcn[114]=Animal;
			soupdeffcn[115]=TrueFunction;
			soupdeffcn[116]=TurnToPos;
			soupdeffcn[117]=AtPos;
			soupdeffcn[118]=TurnToAngle;
			soupdeffcn[119]=MovePos;
			soupdeffcn[120]=SetStartPosition;
			soupdeffcn[121]=GetMyAttr;
			soupdeffcn[122]=SetMyAttr;
			soupdeffcn[123]=GetFramePassed;
			soupdeffcn[124]=SpeedScale;
			soupdeffcn[125]=InKey;
			soupdeffcn[126]=PressKey;
			soupdeffcn[127]=ACTIVATE;
			soupdeffcn[128]=TorchActivate;
			soupdeffcn[129]=Deactivate;
			soupdeffcn[130]=ReleaseAnimation;
			soupdeffcn[131]=HoldAnimation;
			soupdeffcn[132]=PauseAnimation;
			soupdeffcn[133]=SetAction;
			soupdeffcn[134]=ResetAction;
			soupdeffcn[135]=ResetAllAction;
			soupdeffcn[136]=PushAnimation;
			soupdeffcn[137]=PushControlAnimation;
			soupdeffcn[138]=WaitAnimFrame;
			soupdeffcn[139]=WaitPlayerAnimFrame;
			soupdeffcn[140]=KillMyTasks;
			soupdeffcn[141]=ObjectLineUp;
			soupdeffcn[142]=ObjectParallelLineUp;
			soupdeffcn[143]=ObjectPickupLineUp;
			soupdeffcn[144]=LineUp;
			soupdeffcn[145]=WaitLineUp;
			soupdeffcn[146]=MoveAboveMe;
			soupdeffcn[147]=KillMe;
			soupdeffcn[148]=EndSound;
			soupdeffcn[149]=EndMySounds;
			soupdeffcn[150]=EndMySound;
			soupdeffcn[151]=StopAllSounds;
			soupdeffcn[152]=ViewGXA;
			soupdeffcn[153]=AnimateGXA;
			soupdeffcn[154]=PlayerOffset;
			soupdeffcn[155]=PlayerOffsettask;
			soupdeffcn[156]=Offset;
			soupdeffcn[157]=UnOffset;
			soupdeffcn[158]=BounceObject;
			soupdeffcn[159]=OffsetLocation;
			soupdeffcn[160]=DistanceToLocation;
			soupdeffcn[161]=ShowBitmap;
			soupdeffcn[162]=UnShowBitmap;
			soupdeffcn[163]=ReplenishHealth;
			soupdeffcn[164]=InRectangle;
			soupdeffcn[165]=InCircle;
			soupdeffcn[166]=ScriptParticles;
			soupdeffcn[167]=LightTorch;
			soupdeffcn[168]=UnlightTorch;
			soupdeffcn[169]=LoadStatic;
			soupdeffcn[170]=UnLoadStatic;
			soupdeffcn[171]=PointAt;
			soupdeffcn[172]=beginCombat;
			soupdeffcn[173]=endCombat;
			soupdeffcn[174]=isFighting;
			soupdeffcn[175]=isDead;
			soupdeffcn[176]=adjustHealth;
			soupdeffcn[177]=setHealth;
			soupdeffcn[178]=health;
			soupdeffcn[179]=adjustStrength;
			soupdeffcn[180]=setStrength;
			soupdeffcn[181]=strength;
			soupdeffcn[182]=adjustArmor;
			soupdeffcn[183]=setArmor;
			soupdeffcn[184]=armor;
			soupdeffcn[185]=shoot;
			soupdeffcn[186]=shootPlayer;
			soupdeffcn[187]=resurrect;
			soupdeffcn[188]=EnableObject;
			soupdeffcn[189]=DisableObject;
			soupdeffcn[190]=EnableTasks;
			soupdeffcn[191]=DisableTasks;
			soupdeffcn[192]=EnableScript;
			soupdeffcn[193]=DisableScript;
			soupdeffcn[194]=EnableHook;
			soupdeffcn[195]=DisableHook;
			soupdeffcn[196]=EnableCombat;
			soupdeffcn[197]=DisableCombat;
			soupdeffcn[198]=SetRotation;
			soupdeffcn[199]=SetPosition;
			soupdeffcn[200]=ResetRotation;
			soupdeffcn[201]=ResetPosition;
			soupdeffcn[202]=ReloadRotation;
			soupdeffcn[203]=ReloadPosition;
			soupdeffcn[204]=SetLocation;
			soupdeffcn[205]=AttachMe;
			soupdeffcn[206]=DetachMe;
			soupdeffcn[207]=DetachSlaves;
			soupdeffcn[208]=KillSlaves;
			soupdeffcn[209]=HideSlaves;
			soupdeffcn[210]=ShowSlaves;
			soupdeffcn[211]=EnableSlaves;
			soupdeffcn[212]=DisableSlaves;
			soupdeffcn[213]=IsSlave;
			soupdeffcn[214]=IsMaster;
			soupdeffcn[215]=FixSlaveAngle;
			soupdeffcn[216]=PlayerMoved;
			soupdeffcn[217]=PlayerTurned;
			soupdeffcn[218]=PlayerTurnedDirection;
			soupdeffcn[219]=PlayerMovementSpeed;
			soupdeffcn[220]=GetMovementSpeed;
			soupdeffcn[221]=PlayerHealth;
			soupdeffcn[222]=PlayerStatus;
			soupdeffcn[223]=PlayerCollide;
			soupdeffcn[224]=PlayerStand;
			soupdeffcn[225]=PlayerArmed;
			soupdeffcn[226]=PlayerHanging;
			soupdeffcn[227]=PlayerInSight;
			soupdeffcn[228]=PlayerDistance;
			soupdeffcn[229]=AcuratePlayerDistance;
			soupdeffcn[230]=PlayerFacing;
			soupdeffcn[231]=PlayerArcTan;
			soupdeffcn[232]=PlayerArc;
			soupdeffcn[233]=PlayerLooking;
			soupdeffcn[234]=PlayerTilt;
			soupdeffcn[235]=PlayerTiltRevx;
			soupdeffcn[236]=PlayerSnap;
			soupdeffcn[237]=PlayerLineUp;
			soupdeffcn[238]=PlayerParallelLineUp;
			soupdeffcn[239]=PlayerPickupLineUp;
			soupdeffcn[240]=MovePlayerAboveMe;
			soupdeffcn[241]=PushPlayer;
			soupdeffcn[242]=PopPlayer;
			soupdeffcn[243]=PlayerFaceObj;
			soupdeffcn[244]=PlayerOnScape;
			soupdeffcn[245]=PlayerCollideStatic;
			soupdeffcn[246]=WonGame;
			soupdeffcn[247]=MainloopExit;
			soupdeffcn[248]=PlayerFaceTalk;
			soupdeffcn[249]=PlayerForceTalk;
			soupdeffcn[250]=CollidePlayerWeapon;
			soupdeffcn[251]=ZapPlayer;
			soupdeffcn[252]=ZapPlayerNoKill;
			soupdeffcn[253]=ZapObject;
			soupdeffcn[254]=ZapObjectNoKill;
			soupdeffcn[255]=SendMessage;
			soupdeffcn[256]=ReceiveMessage;
			soupdeffcn[257]=IsMessageWaiting;
			soupdeffcn[258]=GetMessageSender;
			soupdeffcn[259]=GetMessageSenderId;
			soupdeffcn[260]=ReplySender;
			soupdeffcn[261]=DeleteMessage;
			soupdeffcn[262]=SendBroadcast;
			soupdeffcn[263]=ReceiveBroadcast;
			soupdeffcn[264]=DeleteBroadcast;
			soupdeffcn[265]=IsBroadcastWaiting;
			soupdeffcn[266]=GetBroadcastSender;
			soupdeffcn[267]=GetBroadcastSenderId;
			soupdeffcn[268]=ReplyBroadcastSender;
			soupdeffcn[269]=ResetGroupSync;
			soupdeffcn[270]=ResetUserSync;
			soupdeffcn[271]=SyncWithGroup;
			soupdeffcn[272]=SyncWithUser;
			soupdeffcn[273]=ChangeGroup;
			soupdeffcn[274]=ChangeUser;
			soupdeffcn[275]=PipeMyGroup;
			soupdeffcn[276]=PipeGroup;
			soupdeffcn[277]=PipeUser;
			soupdeffcn[278]=PipeMasters;
			soupdeffcn[279]=PipeSlaves;
			soupdeffcn[280]=PipeAroundMe;
			soupdeffcn[281]=PipeAroundPos;
			soupdeffcn[282]=PipeHidden;
			soupdeffcn[283]=MoveNodeMarker;
			soupdeffcn[284]=MoveNodeLocation;
			soupdeffcn[285]=MoveNodePosition;
			soupdeffcn[286]=AtNodeMarker;
			soupdeffcn[287]=AtNodeLocation;
			soupdeffcn[288]=AtNodePosition;
			soupdeffcn[289]=Wander;
			soupdeffcn[290]=DisableNodeMaps;
			soupdeffcn[291]=EnableNodeMaps;
			soupdeffcn[292]=MoveMarker;
			soupdeffcn[293]=MoveLocation;
			soupdeffcn[294]=MovePosition;
			soupdeffcn[295]=DrawSword;
			soupdeffcn[296]=SheathSword;
			soupdeffcn[297]=CheckPlayerWeapon;
			soupdeffcn[298]=DisplayHandModel;
			soupdeffcn[299]=DisplayHandItem;
			soupdeffcn[300]=DisplayHiltModel;
			soupdeffcn[301]=DisplayHiltItem;
			soupdeffcn[302]=HandItem;
			soupdeffcn[303]=HaveItem;
			soupdeffcn[304]=RemoveItem;
			soupdeffcn[305]=ItemUsed;
			soupdeffcn[306]=PushItem;
			soupdeffcn[307]=PopItem;
			soupdeffcn[308]=AddItem;
			soupdeffcn[309]=SubItem;
			soupdeffcn[310]=DropItem;
			soupdeffcn[311]=UseItem;
			soupdeffcn[312]=ShowItem;
			soupdeffcn[313]=ShowItemNoRtx;
			soupdeffcn[314]=ActiveItem;
			soupdeffcn[315]=PushAllItems;
			soupdeffcn[316]=PopAllItems;
			soupdeffcn[317]=DropAllItems;
			soupdeffcn[318]=SelectItem;
			soupdeffcn[319]=IsOnEdge;
			soupdeffcn[320]=IsHoldingWeapon;
			soupdeffcn[321]=IsSheathingSword;
			soupdeffcn[322]=IsDrawingSword;
			soupdeffcn[323]=IsDrawingOrSheathing;
			soupdeffcn[324]=IsCarryingWeapon;
			soupdeffcn[325]=IsCombatCapable;
			soupdeffcn[326]=IsShellScript;
			soupdeffcn[327]=IsWithinView;
			soupdeffcn[328]=IsWithinMap;
			soupdeffcn[329]=IsInAir;
			soupdeffcn[330]=IsLightingOrUnlighting;
			soupdeffcn[331]=IsUnlightingTorch;
			soupdeffcn[332]=IsLightingTorch;
			soupdeffcn[333]=IsInLava;
			soupdeffcn[334]=IsInDeepWater;
			soupdeffcn[335]=IsInWater;
			soupdeffcn[336]=IsBouncing;
			soupdeffcn[337]=OpenShell;
			soupdeffcn[338]=CountAiShells;
			soupdeffcn[339]=FindAiShell;
			soupdeffcn[340]=AddFlatEffect;
			soupdeffcn[341]=AddLightEffect;
			soupdeffcn[342]=GolemSteam;
			soupdeffcn[343]=DragonBreath;
			soupdeffcn[344]=DragonBreathTask;
			soupdeffcn[345]=QueueMovie;
			soupdeffcn[346]=TestTask;
			soupdeffcn[347]=XResetCamera;
			soupdeffcn[348]=SetPotion;
			soupdeffcn[349]=GetPotion;
			soupdeffcn[350]=AddLog;
			soupdeffcn[351]=Spare0Parm0;
			soupdeffcn[352]=Spare1Parm0;
			soupdeffcn[353]=Spare0Parm1;
			soupdeffcn[354]=Spare1Parm1;
			soupdeffcn[355]=Spare0Parm2;
			soupdeffcn[356]=Spare1Parm2;
			soupdeffcn[357]=Spare0Parm3;
			soupdeffcn[358]=Spare1Parm3;
			soupdeffcn[359]=Spare0Parm4;
			soupdeffcn[360]=Spare1Parm4;
			soupdeffcn[361]=Spare0Parm5;
			soupdeffcn[362]=Spare1Parm5;
			soupdeffcn[363]=Spare0Parm6;
			soupdeffcn[364]=Spare1Parm6;
			soupdeffcn[365]=Spare0Parm7;
			soupdeffcn[366]=Spare1Parm7;
            return soupdeffcn;
        }
	}
}
