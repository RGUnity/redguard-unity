using System;
using System.Collections.Generic;
using RGFileImport;


public class AnimData
{
    public static float FRAMETIME_VAL = 0.1f;

    public bool running;
    int currentKeyFrame;
    int nextKeyFrame;
    public int offsetKeyFrame;
    int currentAnimFrame;
    public float frameTime;
    public RGRGMAnimStore.RGMAnim animationData;
    public Stack<RGRGMAnimStore.AnimGroup> animationStack;
    public List<RGRGMAnimStore.AnimGroup> validAnims;
    public Func<bool> shouldExitFcn;
    public Action doExitFcn;

    public bool alwaysPanic;
    public AnimData(string scriptname)
    {
        running = false;
        currentKeyFrame = 0;
        nextKeyFrame = 0;
        offsetKeyFrame = 0;
        currentAnimFrame = 0;
        frameTime = FRAMETIME_VAL;
        animationData = RGRGMAnimStore.getAnim(scriptname);
        animationStack = new Stack<RGRGMAnimStore.AnimGroup>();
        animationStack.Push(RGRGMAnimStore.AnimGroup.anim_panic);
        validAnims = getValidAnimations();

        shouldExitFcn = this.shouldExitFcn_default;
        doExitFcn = this.doExitFcn_default;
        alwaysPanic = false;

    }

    bool shouldExitFcn_default()
    {
        return false;
    }
    void doExitFcn_default()
    {}

    public int getCurrentFrame()
    {
        return currentKeyFrame - offsetKeyFrame;
    }
    public int getNextFrame()
    {
        return nextKeyFrame - offsetKeyFrame;
    }
    public List<RGRGMAnimStore.AnimGroup> getValidAnimations()
    {
        List<RGRGMAnimStore.AnimGroup> o = new List<RGRGMAnimStore.AnimGroup>();
        for(int i=0;i<animationData.RAGRItems.Count;i++)
        {
            o.Add(animationData.RAGRItems[i].animGroup);
        }

        return o;
    }

    // SOUPDEF:
    // public int PushAnimation(int obj, int animationid, int firstframe)
    // returns 1 on failure, 0 on success
    public int PushAnimation(RGRGMAnimStore.AnimGroup animId, int firstframe = 0)
    {
        int anim_i = -1;
        List<RGRGMAnimStore.AnimGroup> va = validAnims;
        for(int i=0;i<va.Count;i++)
        {
            if(va[i] == animId)
            {
                anim_i = i;
                break;
            }
        }

        if(anim_i == -1)
            return 1;

        running = true;
        animationStack.Push((RGRGMAnimStore.AnimGroup)anim_i);
        currentAnimFrame = firstframe;
        currentKeyFrame = NextFrame();
        nextKeyFrame = peekNextFrame();
        return 0;
    }
    public void runAnimation(float deltatime, bool force_tick = false)
    {
        if(!running)
            return;
        else
        {
            if(animationStack.Count == 1)
                return;

            frameTime -= deltatime;
            if(frameTime <= 0.0f || force_tick == true)
            {
                currentKeyFrame = nextKeyFrame;
                nextKeyFrame = NextFrame();
                frameTime = FRAMETIME_VAL;
            }
        }
    }

    public int peekNextFrame()
    {
        if(!running)
            return 0;
        int animframe_pre = currentAnimFrame;
        int nextframe3DC = NextFrame();
        currentAnimFrame = animframe_pre;
        return nextframe3DC - offsetKeyFrame;
    }
    int EndAnimation()
    {
        // reached the end, pop anim, stop playing and call the exit function
        animationStack.Pop();
        running = false;
        doExitFcn();
        return -1;

    }

    public int NextFrame()
    {
        if(animationData.RAGRItems[(int)animationStack.Peek()].animType == RGRGMAnimStore.AnimType.animtype_stop || alwaysPanic == true)
        {
            if(shouldExitFcn() == true)
            {
                return EndAnimation();
            }
        }

        if(currentAnimFrame >= (int)animationData.RAGRItems[(int)animationStack.Peek()].animFrames.Length)
            return EndAnimation();

        currentAnimFrame++;
        int frame3DC = -1;
        
        switch(animationData.RAGRItems[(int)animationStack.Peek()].animFrames[currentAnimFrame].frameType)
        {
            case RGRGMAnimStore.FrameType.frametype_normal:
                frame3DC = (int)animationData.RAGRItems[(int)animationStack.Peek()].animFrames[currentAnimFrame].frameValue;
                break;
            case RGRGMAnimStore.FrameType.frametype_end:
                return EndAnimation();
                break;
            case RGRGMAnimStore.FrameType.frametype_goback:
                // if we want to exit, keep going forward
                // if not, loop back
                if(shouldExitFcn() == true)
                {
                    frame3DC = NextFrame();
                }
                else
                {
                    currentAnimFrame = (int)animationData.RAGRItems[(int)animationStack.Peek()].animFrames[currentAnimFrame].frameValue;
                    frame3DC = NextFrame();
                }
                break;
            case RGRGMAnimStore.FrameType.frametype_gofwd:
                // if we want to exit, skip forward to frame
                // if not, continue going
                if(shouldExitFcn() == true)
                {
                    currentAnimFrame = (int)animationData.RAGRItems[(int)animationStack.Peek()].animFrames[currentAnimFrame].frameValue;
                }
                else
                {
                    frame3DC = NextFrame();
                }
                break;
            case RGRGMAnimStore.FrameType.frametype_sound:
                // play sound: RAGRItems[currentAnimationId].frameValue;
                frame3DC = NextFrame();
                break;
            case RGRGMAnimStore.FrameType.frametype_break:
                // assumption was that we could break out here, but it looks like the goback frame type does that
                // not sure what this one does then; looks like all (most?) framevals are 0?
                frame3DC = NextFrame();
                break;
            case RGRGMAnimStore.FrameType.frametype_rumble:
                // ???
                frame3DC = NextFrame();
                break;
            case RGRGMAnimStore.FrameType.frametype_unknown:
                // ???
                frame3DC = NextFrame();
                break;

        }
        return frame3DC;
    }
}
public static class RGRGMAnimStore
{
    public enum AnimGroup
    {
        anim_panic = 		        0, //idles_&_panic
        anim_idle1 = 		        1, //idles_&_panic
        anim_idle2 =         		2, //idles_&_panic
        anim_idle3 = 		        3, //idles_&_panic
        anim_idle_hurt = 		    4, //idles_&_panic
        anim_idle_torch = 		    5, //idles_&_panic
        anim_idle_rope = 		    6, //idles_&_panic
        anim_idle_ledge = 		    7, //idles_&_panic
        anim_fight_panic = 		    8, //idles_&_panic
        anim_misc0 = 		        9, //misc_(low_priority)
        anim_misc1 = 		        10, //misc_(low_priority)
        anim_misc2 = 		        11, //misc_(low_priority)
        anim_misc3 = 		        12, //misc_(low_priority)
        anim_misc4 = 		        13, //misc_(low_priority)
        anim_turn_left = 		    14, //turns
        anim_turn_right = 		    15, //turns
        anim_turn_left_torch = 		16, //turns
        anim_turn_right_torch = 	17, //turns
        anim_run_forward = 		    18, //simple_movements
        anim_run_backward = 		19, //simple_movements
        anim_walk_forward = 		20, //simple_movements
        anim_walk_forward_torch = 	21, //simple_movements
        anim_walk_backward = 		22, //simple_movements
        anim_walk_backward_torch = 	23, //simple_movements
        anim_step_left = 		    24, //simple_movements
        anim_step_left_torch = 		25, //simple_movements
        anim_step_right = 		    26, //simple_movements
        anim_step_right_torch = 	27, //simple_movements
        anim_shimmy_left_ledge = 	28, //simple_movements
        anim_shimmy_right_ledge = 	29, //simple_movements
        anim_run_torch = 		    30, //simple_movements
        anim_fight_run = 		    31, //simple_movements
        anim_jump_start = 		    32, //falling_&_jumping
        anim_jump_start_torch = 	33, //falling_&_jumping
        anim_jump_right_start = 	34, //falling_&_jumping
        anim_jump_right_start_torch=35, //falling_&_jumping
        anim_jump_left_start = 		36, //falling_&_jumping
        anim_jump_left_start_torch =37, //falling_&_jumping
        anim_jump_torch = 	    	38, //falling_&_jumping
        anim_jump_up = 		        39, //falling_&_jumping
        anim_jump_up_arms = 		40, //falling_&_jumping
        anim_jump_forward_torch = 	41, //falling_&_jumping
        anim_jump_forward = 		42, //falling_&_jumping
        anim_jump_forward_arms = 	43, //falling_&_jumping
        anim_jump_right = 		    44, //falling_&_jumping
        anim_jump_right_torch = 	45, //falling_&_jumping
        anim_jump_left = 		    46, //falling_&_jumping
        anim_jump_left_torch = 		47, //falling_&_jumping
        anim_jump_run_arms = 		48, //falling_&_jumping
        anim_jump_backward = 		49, //falling_&_jumping
        anim_jump_backward_torch = 	50, //falling_&_jumping
        anim_jump_off_rope = 		51, //falling_&_jumping
        anim_jump_off_rope_sword = 	52, //falling_&_jumping
        anim_jump_draw_sword = 		53, //falling_&_jumping
        anim_attack_jump_up = 		54, //falling_&_jumping
        anim_attack_jump_forward = 	55, //falling_&_jumping
        anim_fall = 		        56, //falling_&_jumping
        anim_fall_torch = 		    57, //falling_&_jumping
        anim_fall_up_arms = 		58, //falling_&_jumping
        anim_fall_right = 		    59, //falling_&_jumping
        anim_fall_right_torch = 	60, //falling_&_jumping
        anim_fall_left = 		    61, //falling_&_jumping
        anim_fall_left_torch = 		62, //falling_&_jumping
        anim_fall_backward = 		63, //falling_&_jumping
        anim_fall_backward_torch = 	64, //falling_&_jumping
        anim_land = 		        65, //land_groups
        anim_land_torch = 		    66, //land_groups
        anim_land_right = 		    67, //land_groups
        anim_land_right_torch = 	68, //land_groups
        anim_land_left = 		    69, //land_groups
        anim_land_left_torch = 		70, //land_groups
        anim_land_backward = 		71, //land_groups
        anim_land_backward_torch = 	72, //land_groups
        anim_land_hurt = 		    73, //land_groups
        anim_land_death = 		    74, //land_groups
        anim_slide_forward = 		75, //slides
        anim_slide_backward = 		76, //slides
        anim_slide_forward_torch = 	77, //slides
        anim_slide_backward_torch = 78, //slides
        anim_rope_panic = 	    	79, //ropes
        anim_rope_swing_forward = 	80, //ropes
        anim_rope_swing_back = 		81, //ropes
        anim_rope_up = 		        82, //ropes
        anim_rope_down = 		    83, //ropes
        anim_rope_grab = 		    84, //ropes
        anim_rope_jump = 		    85, //ropes
        anim_pull_up_ledge = 		86, //ledges_&_hand-plants
        anim_drop_off_ledge = 		87, //ledges_&_hand-plants
        anim_swing_ledge = 		    88, //ledges_&_hand-plants
        anim_grab_ledge = 		    89, //ledges_&_hand-plants
        anim_hand_plant_ledge = 	90, //ledges_&_hand-plants
        anim_draw_sword = 		    91, //combat
        anim_fight_forward = 		92, //combat
        anim_fight_backward = 		93, //combat
        anim_fight_circle_left = 	94, //combat
        anim_fight_circle_right = 	95, //combat
        anim_fight_turn_left = 		96, //combat
        anim_fight_turn_right = 	97, //combat
        anim_defend_low = 		    98, //combat
        anim_defend_right = 		99, //combat
        anim_defend_left = 		    100, //combat
        anim_defend_high = 		    101, //combat
        anim_attack_1 = 		    102, //combat
        anim_attack_2 = 		    103, //combat
        anim_attack_3 = 		    104, //combat
        anim_attack_thrust = 		105, //combat
        anim_attack_lunge = 		106, //combat
        anim_attack_1_end = 		107, //combat
        anim_attack_2_end = 		108, //combat
        anim_fight_disarm = 		109, //combat
        anim_attack_low = 		    110, //combat
        anim_fight_jump_start = 	111, //combat
        anim_fight_jump = 		    112, //combat
        anim_fight_fall = 		    113, //combat
        anim_fight_land = 		    114, //combat
        anim_fight_fall_attack = 	115, //combat
        anim_fight_land_attack = 	116, //combat
        anim_fight_hurt_1 = 		117, //combat
        anim_fight_hurt_2 = 		118, //combat
        anim_fight_hurt_front = 	119, //combat
        anim_fight_hurt_back = 		120, //combat
        anim_death_fight_stab = 	121, //combat
        anim_death_fight_hard = 	122, //combat
        anim_death_fight_stab_end = 123, //combat
        anim_death_fight_hard_end = 124, //combat
        anim_death_fight_front = 	125, //combat
        anim_death_fight_back = 	126, //combat
        anim_death_fight_front_end =127, //combat
        anim_death_fight_back_end = 128, //combat
        anim_sheath_sword = 		129, //combat
        anim_fight_land_right = 	130, //combat
        anim_fight_land_left = 		131, //combat
        anim_explore_hurt_1 = 		132, //combat
        anim_explore_hurt_2 = 		133, //combat
        anim_death_explore = 		134, //combat
        anim_flask_panic = 		    135, //flask
        anim_draw_flask = 		    136, //flask
        anim_sheath_flask = 		137, //flask
        anim_catch_spell = 		    138, //flask
        anim_charge_flask = 		139, //flask
        anim_shoot_flask = 		    140, //flask
        anim_swim_idle = 		    141, //swimming
        anim_swim_forward = 		142, //swimming
        anim_swim_backward = 		143, //swimming
        anim_pickup = 		        144, //special_movements
        anim_pull_lever_down = 	    145, //special_movements
        anim_pull_lever_up = 	    146, //special_movements
        anim_turn_lever_left = 	    147, //special_movements
        anim_turn_lever_right = 	148, //special_movements
        anim_light_torch = 		    149, //special_movements
        anim_unlight_torch = 	    150, //special_movements
        anim_light_with_torch = 	151, //special_movements
        anim_still = 		        152, //speech
        anim_talk1 = 		        153, //speech
        anim_talk2 = 		        154, //speech
        anim_talk3 = 		        155, //speech
        anim_misc5 = 		        156, //misc_(high_priority)
        anim_misc6 = 		        157, //misc_(high_priority)
        anim_misc7 = 		        158, //misc_(high_priority)
        anim_misc8 = 		        159, //misc_(high_priority)
        anim_misc9 = 		        160, //misc_(high_priority)
        anim_misc10 = 		        161, //misc_(high_priority)
        anim_misc11 = 		        162, //misc_(high_priority)
        anim_misc12 = 		        163, //misc_(high_priority)
        anim_misc13 = 		        164, //misc_(high_priority)
        anim_misc14 = 		        165, //misc_(high_priority)
        anim_misc15 = 		        166, //misc_(high_priority)
        anim_misc16 = 		        167, //misc_(high_priority)
        anim_misc17 = 		        168 //misc_(high_priority)
    }
    public enum AnimType
    {
        animtype_stop       = 0,
        animtype_no_stop    = 1,
        animtype_no_panic   = 2,
    }
    public enum FrameType
    {
        frametype_normal    = 0,
        frametype_end       = 1,
        frametype_goback    = 2,
        frametype_gofwd     = 3,
        frametype_sound     = 4,
        frametype_break     = 5,
        frametype_rumble    = 11,
        frametype_unknown   = 15,
    }

    public class RGMAnim
    {
		public struct RGMRAANItem
		{
            public int faceCount;
            public byte frameCount;
            public byte unknown0;
            public string modelFile;

			public RGMRAANItem(MemoryReader memoryReader)
            {
                try
                {
                    faceCount = memoryReader.ReadInt32();
                    frameCount = memoryReader.ReadByte();
                    unknown0 = memoryReader.ReadByte();
                    modelFile = new string("");
                    string modelFile_tmp = new string("");
                    byte curc = 0x00;
                    while((curc = memoryReader.ReadByte())!= 0x00)
                    {
                        modelFile_tmp += (char)curc;
                    }
                    char[] delims = new char[2];
                    delims[0] = '\\';
                    delims[1] = '.';
                    string[] modelFile_tmp_split = modelFile_tmp.Split(delims);
                    modelFile = modelFile_tmp_split[1].ToUpper();
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RAAN item with error:\n{ex.Message}");
                }
            }
		}
        public struct RGMRAGRAnimFrame
        {
            public FrameType frameType;
            public short frameValue;
            public bool timeScale;
            public byte modifierValue;
            public RGMRAGRAnimFrame(MemoryReader memoryReader)
            {
                try
                {
                    short frameRecord = memoryReader.ReadInt16();
                    frameType = (FrameType)(frameRecord & 0x0F);
                    frameValue = (short)((frameRecord>>4)& 0x07FF);
                    timeScale = (((frameRecord>>15)&0x01)==0x01)?true:false;
                    modifierValue = memoryReader.ReadByte();
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RAGR animFrame with error:\n{ex.Message}");
                }
            }
        }
		public struct RGMRAGRItem
		{
            public short dataLength;
            public AnimGroup animGroup;
            public short frameSpeed;
            public AnimType animType;
            public short frameCount;
            public RGMRAGRAnimFrame[] animFrames;

			public RGMRAGRItem(MemoryReader memoryReader)
            {
                try
                {
                    dataLength = memoryReader.ReadInt16();
                    animGroup = (AnimGroup)memoryReader.ReadInt16();
                    frameSpeed = memoryReader.ReadInt16();
                    animType = (AnimType)memoryReader.ReadInt16();
                    frameCount = memoryReader.ReadInt16();
                    animFrames = new RGMRAGRAnimFrame[frameCount];
                    for(int i=0;i<frameCount;i++)
                    {
                        animFrames[i] = new RGMRAGRAnimFrame(memoryReader);
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RAGR item with error:\n{ex.Message}");
                }
            }
		}

        public string animationName;
        public List<RGMRAANItem> RAANItems;
        public List<RGMRAGRItem> RAGRItems;

        public RGMAnim(RGRGMFile.RGMRAHDItem RAHD, RGRGMFile.RGMRAANSection RAAN, RGRGMFile.RGMRAGRSection RAGR)
        {
            MemoryReader memoryReader;

            memoryReader = new MemoryReader(RAAN.data);
            memoryReader.Seek((uint)RAHD.RAANOffset, 0);
            RAANItems = new List<RGMRAANItem>();
            for(int i=0;i<RAHD.RAANCount;i++)
            {
                RAANItems.Add(new RGMRAANItem(memoryReader));
            }

            memoryReader = new MemoryReader(RAGR.data);
            memoryReader.Seek((uint)RAHD.RAGROffset, 0);
            RAGRItems = new List<RGMRAGRItem>();
            while(memoryReader.PeekInt16() != 0)
            {
                RAGRItems.Add(new RGMRAGRItem(memoryReader));
            }


        }
        public override string ToString()
        {
            string o = new string("");
            o+= "############### RAANS: ############### \n";
            for(int i=0;i<RAANItems.Count;i++)
            {
                o += $"RAAN {i} ############### \n";
                o += $"faceCount {RAANItems[i].faceCount}\n";
                o += $"frameCount {RAANItems[i].frameCount}\n";
                o += $"unknown0 {RAANItems[i].unknown0}\n";
                o += $"modelFile {RAANItems[i].modelFile}\n";
            }
            o+= "############### RAGRS: ############### \n";
            for(int i=0;i<RAGRItems.Count;i++)
            {
                o += $"RAGR {i} ############### \n";
                o += $"dataLength {RAGRItems[i].dataLength}\n";
                o += $"animGroup {RAGRItems[i].animGroup}\n";
                o += $"frameSpeed {RAGRItems[i].frameSpeed}\n";
                o += $"animType {RAGRItems[i].animType}\n";
                o += $"frameCount {RAGRItems[i].frameCount}\n";
                for(int j=0;j<RAGRItems[i].frameCount;j++)
                {
                    o += $"### FRAME {j} ##\n";
                    o += $"frameType: {RAGRItems[i].animFrames[j].frameType}\n";
                    o += $"frameValue: {RAGRItems[i].animFrames[j].frameValue}\n";
                    o += $"timeScale: {RAGRItems[i].animFrames[j].timeScale}\n";
                    o += $"modifierValue: {RAGRItems[i].animFrames[j].modifierValue}\n";
                }
            }
 
            return o;
        }
    }
    static public Dictionary<string, RGMAnim> Anims; // key: RAHD.scriptName
    static public void ReadAnim(RGFileImport.RGRGMFile filergm)
    {
        Anims = new Dictionary<string, RGMAnim>();
        foreach(KeyValuePair<string,RGRGMFile.RGMRAHDItem> entry in filergm.RAHD.dict)
        {
            RGMAnim Anim = new RGMAnim(entry.Value, filergm.RAAN, filergm.RAGR);
            Anims.Add(entry.Value.scriptName, Anim);
        }
    }
    static public RGMAnim getAnim(string scriptname)
    {
        return Anims[scriptname];
    }
}
