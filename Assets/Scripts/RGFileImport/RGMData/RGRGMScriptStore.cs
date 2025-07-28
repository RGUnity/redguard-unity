using System;
using System.Collections.Generic;
using RGFileImport;

public class ScriptData
{
    string scriptName;
    int[] vars;
    // attributes never used?
    public byte[] attributes;
    public int scriptPC;
    Stack<int> callstack;
    MemoryReader memoryReader;

    uint objectId;


    public RGRGMScriptStore.RGMScript scriptData;
    public ScriptData(string scriptname, uint meId)
    {
        scriptName = scriptname;
        scriptData = RGRGMScriptStore.getScript(scriptname);
        vars = scriptData.scriptVariables.ToArray();
        attributes = scriptData.scriptAttributes.ToArray();

        scriptPC = scriptData.scriptPC;
        callstack = new Stack<int>();
        memoryReader = new MemoryReader(scriptData.scriptBytes);

        objectId = meId;
    }
    enum readerState
    {
        main,
        LHS,
        RHS,
        param,
        reference,
        formula
    }
    enum taskType
    {
        task      = 0,
        multitask = 1,
        function  = 2,
    }
    enum objectLoc
    {
        general,
        str
    }

// START DEBUG FUNCS
string DBG_op_2_str(byte op)
{
    string o = new string("");
    switch(op)
    {
        case 0:
            o+=";";
            break;
        case 1:
            o+="+";
            break;
        case 2:
            o+="-";
            break;
        case 3:
            o+="*";
            break;
        case 4:
            o+="/";
            break;
        case 5:
            o+="<<";
            break;
        case 6:
            o+=">>";
            break;
        case 7:
            o+="&";
            break;
        case 8:
            o+="|";
            break;
        case 9:
            o+="^";
            break;
        case 10:
            o+="++;";
            break;
        case 11:
            o+="--;";
            break;
        default:
            o+=$"NIMPL: {op}";
            break;
    }
    return o;
}

string DBG_cmp_2_str(byte cmp)
{
    string o = new string("");
    switch(cmp)
    {
        case 0:
            o+="=";
            break;
        case 1:
            o+="!=";
            break;
        case 2:
            o+="<";
            break;
        case 3:
            o+=">";
            break;
        case 4:
            o+="<=";
            break;
        case 5:
            o+=">=";
            break;
        default:
            o+=$"NIMPL: {cmp}";
            break;
    }
    return o;
}
string DBG_con_2_str(byte con)
{
    string o = new string("");
    switch(con)
    {
        case 0:
            o+=")";
            break;
        case 1:
            o+="&&";
            break;
        default:
            o+="||";
            break;
    }
    return o;
}

void logDBG(string i)
{
    Console.WriteLine($"0x{memoryReader.Position:X4}: DBG_{i}");
}
// END DEBUG FUNCS
// START DO* FUNCS
    bool doCmp(int lhs, int rhs, byte cmp)
    {
        switch(cmp)
        {
            case 0:
                return lhs == rhs;
            case 1:
                return lhs != rhs;
            case 2:
                return lhs < rhs;
            case 3:
                return lhs > rhs;
            case 4:
                return lhs <= rhs;
            case 5:
                return lhs >= rhs;
            default:
                return false;
        }
    }
    int doFormula(List<int> vals, List<byte> ops)
    {
        int o = vals[0];
        int i = 1;
        while(ops[i-1] != 0)
        {
            switch(ops[i-1])
            {
                case 1:
                    o += vals[i];
                    break;
                case 2:
                    o -= vals[i];
                    break;
                case 3:
                    o *= vals[i];
                    break;
                case 4:
                    o /= vals[i];
                    break;
                case 5:
                    o = o<<vals[i];
                    break;
                case 6:
                    o = o>>vals[i];
                    break;
                case 7:
                    o = o&vals[i];
                    break;
                case 8:
                    o = o|vals[i];
                    break;
                case 9:
                    o = o^vals[i];
                    break;
                case 10:
                    o++;
                    break;
                case 11:
                    o--;
                    break;
                default:
                    throw new Exception($"NIMPL: {ops[i-1]}");
            }
            i++;
        }
        return o;
    }
    bool doCon(bool lhs, bool rhs, byte con)
    {
        bool o = lhs;
        switch(con)
        {
            case 0:
                o = lhs;
                break;
            case 1:
                o = lhs&&rhs;
                break;
            default:
                o = lhs||rhs;
                break;
        }
        Console.WriteLine($"CON: {lhs} {DBG_con_2_str(con)} {rhs} = {o}");
        return o;
    }


    int doTask(string obj, taskType type, ushort taskId, int[] parameters)
    {
        Console.Write($"OBJ: {obj}_{type}_{taskId}:");
        bool isMultiTask = (type == taskType.multitask);
        return RGObjectStore.DoObjectTask(objectId, obj, taskId, isMultiTask, parameters);
    }
    int doFlagAssign(ushort flag_id, List<int> vals, List<byte> ops)
    {
        string o = new string($"0x{memoryReader.Position:X4}: FLAG_{flag_id} = ");
        for(int i=0;i<ops.Count;i++)
        {
            o+= $"{vals[i]} {DBG_op_2_str(ops[i])}";
        }
        Console.WriteLine(o);
        RGRGMScriptStore.flags[flag_id] = doFormula(vals, ops);
        return RGRGMScriptStore.flags[flag_id];
    }
    int doGetFlag(ushort flag_id)
    {
        string o = new string($"0x{memoryReader.Position:X4}: FLAG_GET_{flag_id}({RGRGMScriptStore.flags[flag_id]})");
        Console.WriteLine(o);
        return RGRGMScriptStore.flags[flag_id];
    }
    int doVarAssign(ushort var_id, List<int> vals, List<byte> ops)
    {
        string o = new string($"0x{memoryReader.Position:X4}: VAR_{var_id} = ");
        for(int i=0;i<ops.Count;i++)
        {
            o+= $"{vals[i]} {DBG_op_2_str(ops[i])}";
        }
        Console.WriteLine(o);
        vars[var_id] = doFormula(vals, ops);
        return vars[var_id];
    }
    int doGetVar(ushort var_id)
    {
        string o = new string($"0x{memoryReader.Position:X4}: VAR_GET_{var_id}({vars[var_id]})");
        Console.WriteLine(o);
        return vars[var_id];
    }

    int doEnd(int IP)
    {
        string o = new string($"0x{memoryReader.Position:X4}: END: {IP:X4}");
        Console.WriteLine(o);
        scriptPC = IP;;
        return 0xDEAD;
    }
    int doGoto(int IP)
    {
        string o = new string($"0x{memoryReader.Position:X4}: GOTO: {IP:X4}");
        Console.WriteLine(o);
        return IP;
    }
    bool doIf(List<int> LHS, List<int> RHS, List<byte> cmp, List<byte> con)
    {
        bool ret = true;
        string o = new string($"0x{memoryReader.Position:X4}: IF(");

        List<bool> evals = new List<bool>();
        for(int i=0;i<con.Count;i++)
        {
            o+= $"{LHS[i]} {DBG_cmp_2_str(cmp[i])} {RHS[i]} {DBG_con_2_str(con[i])}";
            evals.Add(doCmp(LHS[i], RHS[i], cmp[i]));
        }
        ret = evals[0];
        for(int i=1;i<evals.Count;i++)
        {
            bool ret_o = ret;
            ret = doCon(ret, evals[i], con[i-1]);
        }
        o += $" == {ret}";
        Console.WriteLine(o);

        return ret;
    }
    string doGetObject(byte ofs, objectLoc loc)
    {
        // TODO: do :)
        string obj = new string("");
        switch(loc)
        {
            case objectLoc.general:
                switch(ofs)
                {
                    case 0:
                        obj = "object_me";
                        break;
                    case 1:
                        obj = "object_player";
                        break;
                    case 2:
                        obj = "object_camera";
                        break;
                    default:
                        obj = "UNKNOWN";
                        break;
                }
                break;
            case objectLoc.str:
                obj = scriptData.scriptStrings[ofs];
                break;
        }
        return obj;
    }
    int doSetObjectReference(string obj, int reference, List<int> vals, List<byte> ops)
    {
        // TODO: do :)
        string o = new string($"0x{memoryReader.Position:X4}: OBJ_{obj}.REF_{reference} =");
        for(int i=0;i<ops.Count;i++)
        {
            o+= $"{vals[i]} {DBG_op_2_str(ops[i])}";
        }
        Console.WriteLine(o);
        return 0xBEEF;
    }
    int doGetObjectReference(string obj, int reference)
    {
        // TODO: do :)
        string o = new string($"0x{memoryReader.Position:X4}: OBJ_{obj}.GETREF_{reference}");
        Console.WriteLine(o);
        return reference;
    }

// END DO* FUNCS
// START READ* FUNCS
    int readTask(string obj, taskType type)
    {
        ushort task_id = memoryReader.ReadUInt16();
        byte param_cnt = memoryReader.ReadByte();
        int[] parameters = new int[param_cnt];
        for(int i=0;i<param_cnt;i++)
        {
            parameters[i] = readInstruction(readerState.param);
        }
        return doTask(obj, type, task_id, parameters);
    }
    int readFlag(readerState state)
    {
        ushort flag_id = memoryReader.ReadUInt16();
        List<int> vals = new List<int>();
        List<byte> ops = new List<byte>();
        switch(state)
        {
            case readerState.main:
                byte op = 0;
                do
                {
                    vals.Add(readInstruction(readerState.formula));
                    op = memoryReader.ReadByte();
                    ops.Add(op);
                } while(op>0 && op <10);
                return doFlagAssign(flag_id, vals, ops);
            case readerState.LHS:
            case readerState.RHS:
                memoryReader.ReadByte(); // TODO: does this operator do anything?
                break;
            case readerState.param:
                memoryReader.ReadUInt16();
                break;
            case readerState.formula:
                break;
            default:
                throw new Exception($"NIMPL: {state}");
        }
        return doGetFlag(flag_id);
    }
    int readVar(readerState state)
    {
        byte var_id = memoryReader.ReadByte();
        List<int> vals = new List<int>();
        List<byte> ops = new List<byte>();
        switch(state)
        {
            case readerState.main:
                byte op = 0;
                do
                {
                    vals.Add(readInstruction(readerState.formula));
                    op = memoryReader.ReadByte();
                    ops.Add(op);
                } while(op>0 && op <10);
                return doVarAssign(var_id, vals, ops);
            case readerState.LHS:
            case readerState.RHS:
                memoryReader.ReadByte(); // TODO: does this operator do anything?
                break;
            case readerState.param:
                memoryReader.ReadBytes(3);
                break;
            case readerState.formula:
                break;
            default:
                throw new Exception($"NIMPL: {state}");
        }
        return doGetVar(var_id);
    }
    int readEnd()
    {
        memoryReader.Position = memoryReader.ReadInt32();
        return doEnd(memoryReader.Position);
    }
    int readGoto()
    {
        memoryReader.Position = memoryReader.ReadInt32();
        return doGoto(memoryReader.Position);
    }
    int readImm()
    {
        int imm = memoryReader.ReadInt32();
        return imm;
    }
    int readIf()
    {
        List<int> LHS = new List<int>();
        List<int> RHS = new List<int>();
        List<byte> cmp = new List<byte>();
        List<byte> con = new List<byte>();
        do
        {
            LHS.Add(readInstruction(readerState.LHS));
            cmp.Add(memoryReader.ReadByte());
            RHS.Add(readInstruction(readerState.RHS));
            con.Add(memoryReader.ReadByte());
        } while(con[con.Count-1] != 0x00);
        int jmp = memoryReader.ReadInt32();
        if(doIf(LHS, RHS, cmp, con))
            return memoryReader.Position;
        else
            return jmp;
    }
    string readObject()
    {
        byte val = memoryReader.ReadByte();
        string obj = "";
        switch(val)
        {
            case 0x00:
            case 0x01:
            case 0x02:
                obj = doGetObject(val, objectLoc.general);
                memoryReader.ReadByte();
                break;
            case 0x04:
                obj = doGetObject(memoryReader.ReadByte(), objectLoc.str);
                break;
            default:
                throw new Exception($"NIMPL: 0x{val:X2}");
        }
        return obj;
    }
    int readObjectTask(readerState state)
    {
        string obj = readObject();
        if(state != readerState.main)
        {
            return readTask(obj, taskType.function);
        }
        else
        {
            byte nxt = memoryReader.ReadByte();
            switch(nxt)
            {
                case 0x00:
                    return readTask(obj, taskType.task);
                case 0x01:
                    return readTask(obj, taskType.multitask);
                case 0x02:
                    return readTask(obj, taskType.function);
                default:
                    throw new Exception($"NIMPL: 0x{nxt:X2}");
            }
        }
    }
    int readObjectRef(readerState state)
    {
        string obj = readObject();
        int reference = memoryReader.ReadUInt16()&0xFF;
        List<int> vals = new List<int>();
        List<byte> ops = new List<byte>();
        switch(state)
        {
            case readerState.main:
                byte op = 0;
                do
                {
                    vals.Add(readInstruction(readerState.formula));
                    op = memoryReader.ReadByte();
                    ops.Add(op);
                } while(op>0 && op <10);
                return doSetObjectReference(obj, reference, vals, ops);
            case readerState.LHS:
                op = memoryReader.ReadByte(); // TODO: operator
                return doGetObjectReference(obj, reference);
            default:
                return doGetObjectReference(obj, reference);
        }
    }
    int readString()
    {
        int str = memoryReader.ReadInt32();
        return str;
    }
    int readReturn()
    {
        memoryReader.Position = callstack.Pop();
        return 0;
    }
    int readEndint()
    {
        logDBG($"ENDINT"); // TODO: figure out what this does
        return 0;
    }
    int readGoSub()
    {
        int adr = memoryReader.ReadInt32();
        callstack.Push(memoryReader.Position);
        memoryReader.Position = adr;
        return 0;
    }

    int readTaskPause()
    {
        int taskval = memoryReader.ReadInt32();
        logDBG($"TASKPAUSE: {taskval}"); // TODO: do the pause somehow; also whats the taskval?
        return 0;
    }
    int readScriptRV()
    {
        byte retval = memoryReader.ReadByte();
        logDBG($"SCRIPTRV: {retval}"); // TODO: where does the return val come from?
        return 0;
    }
    int readInstruction(readerState state)
    {
        byte instruction = memoryReader.ReadByte();
        switch(instruction)
        {
            case 0x00: // task
                return readTask("object_me", taskType.task); // TODO: object name
            case 0x01: // multitask (?)
                return readTask("object_me", taskType.multitask); // TODO: object name
            case 0x02: // function
                return readTask("object_me", taskType.function); // TODO: object name
            case 0x03:
                memoryReader.Position = readIf();
                return 0xBEEF;
            case 0x04:
                return readGoto();
            case 0x05:
                return readEnd();
            case 0x06:
                return readFlag(state);
            case 0x07:
            case 0x16:
                return readImm();
            case 0x0A:
                return readVar(state);
            case 0x11:
                return readGoSub();
            case 0x12:
                return readReturn();
            case 0x13:
                return readEndint();
            case 0x14:
                return readObjectRef(state);
            case 0x15:
                return readString();
            case 0x19:
            case 0x1A:
                return readObjectTask(state);
            case 0x1B:
                return readTaskPause();
            case 0x1E:
                return readScriptRV();
            default:
                throw new Exception($"NIMPL: {instruction}");
        }
    }
// END READ* FUNCS
    public void runScript()
    {
        string o = new string($"{scriptName} ():\n");
        o+=$"       ";
        for(int i=0;i<16;i++)
        {
            o+=$"{i:X2} ";
        }
        for(int i=0;i<scriptData.scriptBytes.Length;i++)
        {
            if(i%16==0)
                o+= $"\n0x{i:X4}:";
            o+=$"{scriptData.scriptBytes[i]:X2},";
        }
        Console.WriteLine(o);

        memoryReader.Position = scriptPC;
        bool end = false;
int INF_LOOP_DET = 0;
        while(!end)
        {
            if(readInstruction(readerState.main) == 0xDEAD)
                end = true;
             if(INF_LOOP_DET++ > 1024)
             {
                 logDBG("INF LOOP; BAILING");
                 end = true;
             }
        }
    }
    public void tickScript()
    {
        readInstruction(readerState.main);
    }
}

public static class RGRGMScriptStore
{
    public class RGMScript
    {
        public int scriptIndex;
        public string scriptName;
        public string objectName;
        public byte[] scriptBytes;
        public List<string> scriptStrings;
        public List<int> scriptVariables;
        public List<byte> scriptAttributes;

        // need entry point and callstack
        public int scriptPC;
        public Stack<int> callstack;

        public RGMScript(int script_i, RGRGMFile.RGMRAHDItem RAHD, RGRGMFile.RGMRASTSection RAST, RGRGMFile.RGMRASBSection RASB, RGRGMFile.RGMRAVASection RAVA, RGRGMFile.RGMRASCSection RASC, RGRGMFile.RGMRAATSection RAAT, RGRGMFile.RGMRANMSection RANM)
        {

            char[] tmp_char_arr2 = new char[RAHD.RANMLength];

            Array.Copy(RANM.data, RAHD.RANMOffset, tmp_char_arr2, 0, RAHD.RANMLength);
            objectName = new string(tmp_char_arr2);

            byte[] tmp_arr;
            scriptIndex = script_i;
            scriptName = RAHD.scriptName;
            scriptStrings = new List<string>();
            scriptVariables = new List<int>();
            scriptAttributes = new List<byte>();

            scriptBytes = new byte[RAHD.RASCLength];
            Array.Copy(RASC.scripts, RAHD.RASCOffset, scriptBytes, 0, RAHD.RASCLength);

            rasbcnt = RAHD.RASBCount;
            rasbofs = RASB.offsets;
            RAHDdat = RAHD;
            RASTdat = RAST;
            RASBdat = RASB;
            for(int i=0;i<RAHD.RASBCount;i++)
            {
                int ofs = RASB.offsets[(RAHD.RASBOffset/4)+i];
                int end = Array.IndexOf(RAST.text, '\0', ofs);
                char[] tmp_char_arr = new char[end-ofs];

                Array.Copy(RAST.text, ofs, tmp_char_arr, 0, end-ofs);
                scriptStrings.Add(new string(tmp_char_arr));
            }

            for(int i=0;i<RAHD.RAVACount;i++)
            {
                if(RAHD.RAVACount> 0)
                {
                    int[] tmp_int_arr = new int[RAHD.RAVACount];
                    Array.Copy(RAVA.data, RAHD.RAVAOffset/4, tmp_int_arr, 0, RAHD.RAVACount);
                    scriptVariables = new List<int>(tmp_int_arr);
                }
            }

            tmp_arr = new byte[256];
            Array.Copy(RAAT.attributes, script_i*256, tmp_arr, 0, 256);
            scriptAttributes = new List<byte>(tmp_arr);

            scriptPC = RAHD.RASCStartAt;
            callstack = new Stack<int>();
        }
        int rasbcnt;
        int []rasbofs;
        RGRGMFile.RGMRAHDItem RAHDdat;
        RGRGMFile.RGMRASTSection RASTdat;
        RGRGMFile.RGMRASBSection RASBdat;
        public string DumpStrings()
        {
            string o = new string($"SCR:{scriptName}:");
            for(int i=0;i<RASBdat.offsets.Length;i++)
            {
                o+= $"RASB_{i}: {RASBdat.offsets[i]}\n";
            }
            for(int i=0;i<RASBdat.offsets.Length;i++)
            {
                int ofs = RASBdat.offsets[i];
                int end = Array.IndexOf(RASTdat.text, '\0', ofs);
                char[] tmp_char_arr = new char[end-ofs];

                Array.Copy(RASTdat.text, ofs, tmp_char_arr, 0, end-ofs);
                o += $"\n{i}: {ofs:X}:"+(new string(tmp_char_arr));
            }
            o += "\n########################################";
            o += $"\nRASTlen: {RASTdat.text.Length}";
            o += $"\nRAHD RASBcnt: {RAHDdat.RASBCount}";
            o += $"\nRAHD RASBlen: {RAHDdat.RASBLength}";
            o += $"\nRAHD RASBofs: {RAHDdat.RASBOffset}";
            return o;
        }
        public override string ToString()
        {
            string o = new string($"SCR:{scriptName}:");
            for(int i=0;i<scriptBytes.Length;i++)
            {
                o+=$"{scriptBytes[i]:X2},";
                if(i%32 == 0)
                    o+=$"\n{i}\n";
            }
            o += "\nSTRINGS:\n";
            for(int i=0;i<scriptStrings.Count;i++)
            {
                o+=$"{scriptStrings[i]}\n";
            }

            o += "\nVARS:\n";
            for(int i=0;i<scriptVariables.Count;i++)
            {
                o+=$"{scriptVariables[i]:X4},";
                if(i%4 == 0)
                    o+=$"\n{i}\n";
            }
            o += "\nATTRS:\n";
            for(int i=0;i<scriptAttributes.Count;i++)
            {
                o+=$"{scriptAttributes[i]:X2},";
                if(i%32 == 0)
                    o+=$"\n{i}\n";
            }
            return o;
        }
    }

    static public Dictionary<string, RGMScript> Scripts; //key: RAHD.scriptName
    static public int[] flags;
    static public void SetDefaultFlagValues()
    {

        flags = new int[369];
        flags[0] = 1; // TimeOfDay	NUMBER	"1 day
        flags[1] = 0; // After_Catacombs	BOOL	Cyrus has been through the catacombs
        flags[2] = 0; // After_League	BOOL	Cyrus has been to the league(Act2)
        flags[3] = 0; // Gremlin	BOOL	Cyrus is gremlin
        flags[4] = 0; // After_Jail	BOOL	Cyrus is out of the jail/chase section
        flags[5] = 0; // After_Iszara	BOOL	Cyrus has rescued Iszara (Act3)
        flags[6] = 0; // After_Dragon	BOOL	dragon is dead
        flags[7] = 0; // In_Palace	BOOL	Cyrus is in the palace at the end
        flags[8] = 0; // Won_Game	BOOL	killed richton game over!! YOU WIN!!! Yeh..uh..wait
        flags[9] = 0; // Have_Gear	BOOL	 
        flags[10] = 0; // Have_Blessing	BOOL	 
        flags[11] = 0; // Have_DRBook	BOOL	 
        flags[12] = 0; // AfterTrap	BOOL	TRUE after Brennan's Trap dialog
        flags[13] = 0; // AmuletDone	BOOL	"TRUE after Cyrus delivers amulet to Richton
        flags[14] = 0; // AmuletJob	BOOL	TRUE after Cyrus accepts job from Kotaro
        flags[15] = 0; // Asked_Once	BOOL	TRUE after Cyrus asks one question of either guard in vile's realm
        flags[16] = 0; // BasilTalk	BOOL	TRUE after beginning of first dialog with basil
        flags[17] = 0; // BrennanWarn	BOOL	TRUE after someone warns you of Brennan's Trap
        flags[18] = 0; // BrennanTrap	BOOL	 
        flags[19] = 0; // BrennanDead	BOOL	true
        flags[20] = 0; // CoyleFriend	BOOL	TRUE after Cyrus has befriended Coyle -- KEN: see CyrusFriend -- could be a screwup
        flags[21] = 0; // Crowns_G	BOOL	sets like local crowns flag in Dreekius
        flags[22] = 0; // CrendalFlask	BOOL	TRUE after Crendal reveals he has Trithik's map piece
        flags[23] = 0; // CrendalMap	BOOL	TRUE after Trithik tells Cyrus what's on his map piece
        flags[24] = 0; // CyCynic	BOOL	TRUE after Cyrus acts cynical about Hammerfell's fate in front of Dreekius
        flags[25] = 0; // CyNoGrem	BOOL	TRUE after Cyrus is changed back from gremlin to Cyrus
        flags[26] = 0; // CyrusFriend	BOOL	"TRUE after Cyrus is friends with Coyle -- KEN: have replaced CYRUSFRIEND wiht CoyleFriend in Saban
        flags[27] = 0; // DragonDead	BOOL	TRUE after Nafaalilargus is dead
        flags[28] = 0; // Dram_Appears	BOOL	TRUE to have Dram appear and shoot Joto in the jail. can be used for more...
        flags[29] = 0; // DreekCro	BOOL	TRUE when Dreekius tells you what he knows about the Crowns during Tobias' dialogue
        flags[30] = 0; // DreekImp	BOOL	TRUE when Dreekius tells you about the Imperials' list of dissidents during Tobias' dialogue
        flags[31] = 0; // DreekiusName	BOOL	TRUE when Cyrus knows Dreekius' name
        flags[32] = 0; // DreekiusTalk	BOOL	TRUE after you first talk to Dreekius
        flags[33] = 0; // DreekOcc	BOOL	TRUE when Dreekius tells you what he knows about the Imperial Occupation during Tobias' dialogue
        flags[34] = 0; // DreekSister	BOOL	TRUE when Dreekius tells you what he knows about Iszara
        flags[35] = 0; // EnterCaverns	BOOL	TRUE if Cyrus has been in the Caverns
        flags[36] = 0; // EyePieceFound	BOOL	 
        flags[37] = 0; // EyePieceLost	BOOL	 
        flags[38] = 0; // Fatboy	BOOL	TRUE after someone explains that Fatboy = Richton
        flags[39] = 0; // FavisHelp	BOOL	 
        flags[40] = 0; // Favis_Bell	BOOL	"favis tells cyrus about broken bell
        flags[41] = 0; // FavisPrnellPal	BOOL	 
        flags[42] = 0; // FixBell	BOOL	 
        flags[43] = 0; // FlagshipStory	BOOL	TRUE after someone tells you the League blew up Richton's flagship
        flags[44] = 0; // FlaskFalicia	BOOL	TRUE after Falicia tells Cyrus what she knows about the Flask of Lillandril
        flags[45] = 0; // FlaskStart	BOOL	"TRUE after you read the book about the Flask
        flags[46] = 0; // GremlinSolve	BOOL	true after cyrus is able to say change me back to anyone
        flags[47] = 0; // GypsyHelp	BOOL	TRUE after you agree to help solve Saban's problem
        flags[48] = 0; // GypsySolve	BOOL	 
        flags[49] = 0; // HaveAmulet	BOOL	TRUE when Cyrus has the Amulet
        flags[50] = 0; // HaveGear	BOOL	TRUE when Cyrus has Dwarven gear
        flags[51] = 0; // HaveGem	BOOL	TRUE when Cyrus has soulgem
        flags[52] = 0; // HaveInsignia	BOOL	 
        flags[53] = 0; // HaveLocket	BOOL	TRUE when Cyrus has Lakene's Locket
        flags[54] = 0; // HaveRing	BOOL	TRUE when Cyrus has Voa's Ring
        flags[55] = 0; // HayleStory	BOOL	 
        flags[56] = 0; // HiToFavis	BOOL	 
        flags[57] = 0; // HiToSiona	BOOL	TRUE after Tobias tells you to see Siona
        flags[58] = 0; // Imperials_G	BOOL	sets like local imperials flag in Dreekius
        flags[59] = 0; // IszaraMetHayle	BOOL	 
        flags[60] = 0; // Journal	BOOL	 
        flags[61] = 0; // JotoInJail	BOOL	 
        flags[62] = 0; // KillBrennan	BOOL	set when Brennan is dead
        flags[63] = 0; // KithJournal	BOOL	TRUE after Cyrus has read Kithral's Journal
        flags[64] = 0; // KnowBody	BOOL	TRUE when Cyrus knows Prince A'tor's body is at the Temple and covered in the stasis field
        flags[65] = 0; // KnowsJaganvir	BOOL	TRUE when Cyrus knows from Avik or Falicia that Jaganvir is the new archmage
        flags[66] = 0; // KotaroTalk	BOOL	 
        flags[67] = 0; // KrisGuardsKill	NUMBER	"number of Cyrus kills the guards in Krisandra's house
        flags[68] = 0; // KrisGuardsSee	BOOL	TRUE after Cyrus enters Krisandra's house while guards are searching it (only after FlaskStart = TRUE)
        flags[69] = 0; // LakeneDen	BOOL	"TRUE after someone tells Cyrus that Lakene knows about SD. (Krisandra
        flags[70] = 0; // LakeneMove	BOOL	TRUE when Cyrus confronts Lakene and he moves
        flags[71] = 0; // LakeneTalk	BOOL	TRUE after Cyrus talks to Lakene
        flags[72] = 0; // MakeMap	BOOL	"true after cyrus gives maiko the elven book
        flags[73] = 0; // MapFin	BOOL	"true after counter finishes
        flags[74] = 0; // MariahTellIszara	BOOL	TRUE if Mariah tells you she saw Iszara at Maiko's
        flags[75] = 0; // NecroDone	BOOL	TRUE after Cyrus completes the amulet delivery under the auspice of N'gasta rather than Kotaro
        flags[76] = 0; // NecroJob	BOOL	TRUE after Cyrus accepts amulet job from N'gasta instead of Kotaro
        flags[77] = 0; // NeedFixObserv	BOOL	TRUE after Erasmo tells Cyrus that observatory must be fixed.
        flags[78] = 0; // NeedPassword	BOOL	TRUE after someone tells you that you need a password to get into Smuggler's Den (SmugglersDen is true and knock on door)
        flags[79] = 0; // NeedProof	BOOL	 
        flags[80] = 0; // NewCombs	BOOL	TRUE when Cyrus enters the second part of the Catacombs
        flags[81] = 0; // NidalKnow	BOOL	 
        flags[82] = 0; // NidalTalk	BOOL	TRUE if player has spoken with Nidal [set in Brother Nidal; referenced in Tobias]
        flags[83] = 0; // Occupation_G	BOOL	sets like local occupation flag in Dreekius
        flags[84] = 0; // ObservFixed	BOOL	TRUE after Cyrus fixes the observatory
        flags[85] = 0; // OpenDoor	BOOL	TRUE after the Door to the Dwarven Ruins is opened
        flags[86] = 0; // Open_Journal	BOOL	true after cyrus opens iszara*s journal
        flags[87] = 0; // Password	BOOL	TRUE after someone tells you the password to the Smuggler's Den
        flags[88] = 0; // PrnellFriend	BOOL	 
        flags[89] = 0; // Flag_Picked_Up	BOOL	"when false; show Prnell's harbor tower redflag [xflag.scr]; when true
        flags[90] = 0; // Put_Flag_Back	BOOL	prnell allows cy to put flag back
        flags[91] = 0; // PullBellRope	BOOL	"Cyrus tries to pull bell rope
        flags[92] = 0; // RolloAnnoy	BOOL	 
        flags[93] = 0; // RolloDen	BOOL	"TRUE after someone tells Cyrus that Rollo knows about Smuggler's Den. (Krisandra
        flags[94] = 0; // RolloHate	BOOL	 
        flags[95] = 0; // RolloLocket	BOOL	TRUE after Lakene tells you Rollo has his wife's locket
        flags[96] = 0; // RolloTalk	BOOL	 
        flags[97] = 0; // SabanCanHelp	BOOL	TRUE when Cyrus knows Saban can help him out (thus GypsyHelp=TRUE)
        flags[98] = 0; // SabanSeeTruth	BOOL	 
        flags[99] = 0; // SeeBrennan	BOOL	TRUE after someone tells you Brennan is looking for you
        flags[100] = 0; // SeeCrendal	BOOL	"TRUE if Dreekius tells you that Crendal has a piece of the map- opens up ""map"" inquiry in Crendal's dialogue"
        flags[101] = 0; // SeenDoor	BOOL	TRUE when Cyrus sees the Door to the Dwarven Ruins
        flags[102] = 0; // SionaFriend	BOOL	TRUE after Siona becomes your friend (by you telling her Tobias is your friend)
        flags[103] = 0; // SmugglersDen	BOOL	"TRUE after someone tells you about the Smuggler's Den (Siona
        flags[104] = 0; // SoulSnare	BOOL	 
        flags[105] = 0; // Spoke_With_Kotaro	BOOL	TRUE after Cyrus has spoken with Kitaro at least once
        flags[106] = 0; // Stop_Talk	BOOL	disables dialog activation for Basil while Vandar fights
        flags[107] = 0; // Take_Flag	BOOL	lets cyrus take prnell's flag
        flags[108] = 0; // Talking	BOOL	we set this when cyrus is in dialog
        flags[109] = 0; // TavernBrawl	BOOL	TRUE if Cyrus has gotten into a fight with the Forebears at the Draggin Tale.
        flags[110] = 0; // TempleVisit	BOOL	TRUE if Cyrus has been to the Temple
        flags[111] = 0; // TirbInJail	BOOL	 
        flags[112] = 0; // TobiasLeave	BOOL	TRUE after Tobias leaves and Cyrus had known previously that he planned to leave (not the same as ActTwo=TRUE)
        flags[113] = 0; // TobiasTalk	BOOL	TRUE after you first talk to Tobias
        flags[114] = 0; // TrithikBoat	BOOL	TRUE after Lakene mentions Trithick's boat needs fixing.
        flags[115] = 0; // TrithikMap	BOOL	TRUE after Trithik tells Cyrus that someone stole his map piece
        flags[116] = 0; // UseTelescope	BOOL	 
        flags[117] = 0; // VandarFight	BOOL	true after cyrus and vandar fight
        flags[118] = 0; // Weight1_Down	BOOL	weight1 for catacomb weight puzzle
        flags[119] = 0; // Weight2_Down	BOOL	weight2 for catacomb weight puzzle
        flags[120] = 0; // Weight3_Down	BOOL	weight3 for catacomb weight puzzle
        flags[121] = 0; // WorkMap	BOOL	true if makemap is true when cyrus leaves cartographers
        flags[122] = 0; // Wounded	BOOL	TRUE if Cyrus is wounded
        flags[123] = 0; // Equipped_Torch	BOOL	 
        flags[124] = 0; // Equipped_Key_Bone	BOOL	 
        flags[125] = 0; // Equipped_Key_Palace	BOOL	 
        flags[126] = 0; // Digging	BOOL	set when cyrus is digging
        flags[127] = 0; // Dig_Item_Found	BOOL	set if something is found when digging
        flags[128] = 0; // HoldingFlag	BOOL	shen cyrus is holding flag
        flags[129] = 0; // Torch	BOOL	true when cyrus has torch out
        flags[130] = 0; // CTBlock1	FLIPFLOP	 
        flags[131] = 0; // CTBlock2	FLIPFLOP	 
        flags[132] = 0; // CTDoor01	FLIPFLOP	 
        flags[133] = 0; // CTDoor02	FLIPFLOP	 
        flags[134] = 0; // CTDoor58	FLIPFLOP	 
        flags[135] = 0; // CTDoor73	FLIPFLOP	 
        flags[136] = 0; // CTDoor86	FLIPFLOP	 
        flags[137] = 0; // CTRot86	FLIPFLOP	 
        flags[138] = 0; // CTPad01	FLIPFLOP	 
        flags[139] = 0; // CTPad02	FLIPFLOP	 
        flags[140] = 0; // CTPad03	FLIPFLOP	 
        flags[141] = 0; // CTPad04	FLIPFLOP	 
        flags[142] = 0; // CTPadA1	FLIPFLOP	 
        flags[143] = 0; // CTRuneD	FLIPFLOP	 
        flags[144] = 0; // CTCount	FLIPFLOP	 
        flags[145] = 0; // CTCounter	NUMBER	 
        flags[146] = 0; // CTCounter2	NUMBER	 
        flags[147] = 0; // CTRack	NUMBER	 
        flags[148] = 0; // CTRakDor	NUMBER	 
        flags[149] = 0; // CTLock01	FLIPFLOP	 
        flags[150] = 0; // CTLock02	FLIPFLOP	 
        flags[151] = 0; // CTWeight	NUMBER	 
        flags[152] = 0; // CTWPad01	FLIPFLOP	 
        flags[153] = 0; // CTWPad02	FLIPFLOP	 
        flags[154] = 0; // CTWPad03	FLIPFLOP	 
        flags[155] = 0; // CTWPad04	FLIPFLOP	 
        flags[156] = 0; // CTWPad05	FLIPFLOP	 
        flags[157] = 0; // CTWPad06	FLIPFLOP	 
        flags[158] = 0; // CTWPad07	FLIPFLOP	 
        flags[159] = 0; // CTWPad08	FLIPFLOP	 
        flags[160] = 0; // CTWBlade	FLIPFLOP	 
        flags[161] = 0; // CTWDoor1	FLIPFLOP	 
        flags[162] = 0; // CTSKey	FLIPFLOP	 
        flags[163] = 0; // CTStop	FLIPFLOP	 
        flags[164] = 0; // CT_Fire	BOOL	 
        flags[165] = 0; // CatacombsFoundStuff	BOOL	 
        flags[166] = 0; // Scene2	BOOL	 
        flags[167] = 0; // Scene9	BOOL	 
        flags[168] = 0; // Cyrus_Face_Gem	BOOL	 
        flags[169] = 0; // Dragon_Face_Gem	BOOL	 
        flags[170] = 0; // Cyrus_Face_Dragon	BOOL	 
        flags[171] = 0; // Braze_Fire	BOOL	 
        flags[172] = 0; // CV_Lock1	BOOL	 
        flags[173] = 0; // CV_Lock2	BOOL	 
        flags[174] = 0; // CV_Dor01	BOOL	 
        flags[175] = 0; // CV_Gear1	BOOL	 
        flags[176] = 0; // CV_Gate1	BOOL	 
        flags[177] = 0; // CV_PillarA1	BOOL	 
        flags[178] = 0; // CV_PillarA2	BOOL	 
        flags[179] = 0; // CV_PillarB1	BOOL	 
        flags[180] = 0; // CV_PillarB2	BOOL	 
        flags[181] = 0; // CV_PillarC1	BOOL	 
        flags[182] = 0; // CV_PillarC2	BOOL	 
        flags[183] = 0; // CV_River	FLIPFLOP	 
        flags[184] = 0; // CV_PillarA	BOOL	 
        flags[185] = 0; // CV_PillarB	BOOL	 
        flags[186] = 0; // CV_PillarC	BOOL	 
        flags[187] = 0; // CV_Skull	BOOL	 
        flags[188] = 0; // CV_Ship	BOOL	 
        flags[189] = 0; // CV_Sword	BOOL	 
        flags[190] = 0; // CV_Arm	BOOL	 
        flags[191] = 0; // CV_Main_Door	BOOL	 
        flags[192] = 0; // CV_Bait	FLIPFLOP	 
        flags[193] = 0; // CV_Crack	BOOL	opens crack to secret brazier
        flags[194] = 1; // OB_TelV	NUMBER	 
        flags[195] = 12; // OB_TelH	NUMBER	 
        flags[196] = 0; // OB_Platform	BOOL	 
        flags[197] = 0; // OB_Arms	BOOL	 
        flags[198] = 0; // OB_Plt_Up	BOOL	 
        flags[199] = 0; // OB_Plt_Move	BOOL	 
        flags[200] = 0; // OB_Look	BOOL	 
        flags[201] = 0; // OB_StepUp	BOOL	 
        flags[202] = 0; // OB_StepDown	BOOL	 
        flags[203] = 0; // OB_Fixed	BOOL	 
        flags[204] = 0; // OB_Elevator	BOOL	 
        flags[205] = 0; // OB_Lift	BOOL	 
        flags[206] = 0; // OB_Lever	NUMBER	 
        flags[207] = 0; // KurtTest1	BOOL	 
        flags[208] = 0; // KurtTest2	BOOL	 
        flags[209] = 0; // OB_Engine	BOOL	 
        flags[210] = 0; // PI_Pot	FLIPFLOP	 
        flags[211] = 0; // PI_Pig	BOOL	 
        flags[212] = 0; // PI_Crane	BOOL	 
        flags[213] = 0; // PI_Door1	BOOL	 
        flags[214] = 0; // PI_Door2	BOOL	 
        flags[215] = 0; // PI_Door3	BOOL	 
        flags[216] = 0; // PI_Door4	BOOL	 
        flags[217] = 0; // PI_Door5	BOOL	 
        flags[218] = 0; // PI_Platform	FLIPFLOP	 
        flags[219] = 0; // PI_ThroneA	FLIPFLOP	 
        flags[220] = 0; // PI_ThroneB	FLIPFLOP	 
        flags[221] = 0; // PI_ThroneC	FLIPFLOP	 
        flags[222] = 0; // PI_ThroneD	FLIPFLOP	 
        flags[223] = 0; // PalaceGuardsDead	NUMBER	"for scene1
        flags[224] = 0; // DramShootCyrus	BOOL	"for scene1
        flags[225] = 0; // PIOnBlimp	BOOL	 
        flags[226] = 0; // NCDoor1	FLIPFLOP	 
        flags[227] = 0; // Open_NCGate	BOOL	 
        flags[228] = 0; // NCHead	FLIPFLOP	 
        flags[229] = 0; // Close_NCDoorT1	BOOL	 
        flags[230] = 0; // NC_View_Skeleton	BOOL	 
        flags[231] = 1; // At_Shoals	BOOL	these are for the boatman's state
        flags[232] = 0; // At_Isle	BOOL	 
        flags[233] = 0; // To_Isle	BOOL	 
        flags[234] = 0; // To_Shoals	BOOL	 
        flags[235] = 0; // NC_Pour_Elixir	BOOL	 
        flags[236] = 0; // NC_Explosion	BOOL	 
        flags[237] = 0; // Cyrus_At_Gate	BOOL	is true when cyrus arives at tower gate
        flags[238] = 0; // SerpentDead	BOOL	when serpent is dead
        flags[239] = 0; // Scene6	BOOL	"start ""The Fate of Iszara"" ingame cutscene"
        flags[240] = 0; // NgastaFight	BOOL	 
        flags[241] = 0; // Scene6_Door	BOOL	true when the door is closed; playerlineup with iszara; start iszara dialog; from iszara.scr to scene6
        flags[242] = 0; // DRArrowLoaded	BOOL	 
        flags[243] = 0; // DR_Steam	NUMBER	 
        flags[244] = 0; // DR_Boiler	BOOL	 
        flags[245] = 0; // DR_Boiler_Move	BOOL	 
        flags[246] = 0; // DR_Pipe1	BOOL	 
        flags[247] = 0; // DR_Pipe2	BOOL	 
        flags[248] = 0; // DR_Pipe3	BOOL	 
        flags[249] = 0; // DR_Pipe4	BOOL	 
        flags[250] = 0; // DR_Pipe5	BOOL	 
        flags[251] = 0; // DR_Pipe6	BOOL	 
        flags[252] = 0; // DR_Flower1	BOOL	 
        flags[253] = 0; // DR_Flower2	BOOL	 
        flags[254] = 0; // DR_Flower3	BOOL	 
        flags[255] = 0; // DR_Flower4	BOOL	 
        flags[256] = 0; // DR_Flower5	BOOL	 
        flags[257] = 0; // DR_Flower6	BOOL	 
        flags[258] = 0; // DR_Slider2	BOOL	 
        flags[259] = 0; // DR_Slider1	BOOL	 
        flags[260] = 0; // DR_Output1	NUMBER	 
        flags[261] = 0; // DR_Output2	NUMBER	 
        flags[262] = 0; // DR_Output3	NUMBER	 
        flags[263] = 0; // DR_Output4	NUMBER	 
        flags[264] = 0; // DR_Output5	NUMBER	 
        flags[265] = 0; // DR_Output6	NUMBER	 
        flags[266] = 0; // DR_Output7	NUMBER	 
        flags[267] = 0; // DR_Output8	NUMBER	 
        flags[268] = 0; // DR_Steam_Done	NUMBER	 
        flags[269] = 0; // DR_Trigger1	BOOL	 
        flags[270] = 0; // DR_Trigger2	BOOL	 
        flags[271] = 0; // DR_Door1	BOOL	 
        flags[272] = 0; // DR_Door1A	BOOL	 
        flags[273] = 0; // DR_Door2	BOOL	 
        flags[274] = 0; // DR_Door2A	BOOL	 
        flags[275] = 0; // DR_Mask	BOOL	 
        flags[276] = 0; // DR_DoorA	NUMBER	 
        flags[277] = 0; // DR_DoorB	BOOL	 
        flags[278] = 0; // DR_DoorC	BOOL	 
        flags[279] = 0; // DR_Loader	BOOL	 
        flags[280] = 0; // DR_LaserFire	FLIPFLOP	 
        flags[281] = 0; // DR_Robot	BOOL	 
        flags[282] = 0; // DR_Golem_Dead	BOOL	 
        flags[283] = 0; // DR_DOrb	BOOL	 
        flags[284] = 0; // DR_Bridge	BOOL	 
        flags[285] = 0; // Open_DRDoor	BOOL	 
        flags[286] = 0; // DR_ExDra	BOOL	 
        flags[287] = 0; // DR_Scarab_Turn	FLIPFLOP	 
        flags[288] = 0; // DR_Scarab_Done	BOOL	 
        flags[289] = 0; // SCB_Position	NUMBER	 
        flags[290] = 0; // SCB_ArmL	NUMBER	 
        flags[291] = 0; // SCB_ArmR	NUMBER	 
        flags[292] = 0; // SCB_Head	BOOL	 
        flags[293] = 0; // SCB_Legs	FLIPFLOP	 
        flags[294] = 0; // SCB_Spikes	BOOL	 
        flags[295] = 0; // SCB_Door	NUMBER	 
        flags[296] = 0; // SCB_ChnL	FLIPFLOP	 
        flags[297] = 0; // SCB_ChnR	FLIPFLOP	 
        flags[298] = 0; // SCB_ChnM	NUMBER	 
        flags[299] = 0; // SCB_HndL	BOOL	 
        flags[300] = 0; // SCB_HndR	BOOL	 
        flags[301] = 0; // CampDirgFlyAway	BOOL	 
        flags[302] = 0; // VR_Door1_Activate	BOOL	 
        flags[303] = 0; // VR_Door2_Activate	BOOL	 
        flags[304] = 0; // VR_Gard1_Activate	BOOL	 
        flags[305] = 0; // VR_Gard2_Activate	BOOL	 
        flags[306] = 0; // VR_Doors_Activate	BOOL	 
        flags[307] = 0; // Talk_Now	BOOL	;toggle for vilechat crates -- vile comments while exploring
        flags[308] = 0; // Dog_Dialog	BOOL	;turns off dog morphing for Vile's dialog dog reference
        flags[309] = 0; // End_Vile	BOOL	;shifts control from guards to Vile end puzzle dialog
        flags[310] = 0; // Wrong_Door	BOOL	;Vile dialog after puzzle lose
        flags[311] = 0; // Right_Door	BOOL	;Vile dialog after puzzle win
        flags[312] = 0; // Vile_Choose_Right	BOOL	;true if you ask right question and choose right door
        flags[313] = 0; // HI_Ride	BOOL	 
        flags[314] = 0; // Sailed_In	BOOL	 
        flags[315] = 0; // Dead_Marine	NUMBER	to determine when all guards are dead
        flags[316] = 0; // Show_Marine	BOOL	to show marines on boat
        flags[317] = 0; // DoorOpen	BOOL	"when hi_door.scr gets right key
        flags[318] = 0; // Strike_Counter	NUMBER	"if Cyrus activates locked doors or picks fights
        flags[319] = 0; // MenuRet	NUMBER	 
        flags[320] = 0; // Menu1State	BOOL	 
        flags[321] = 0; // Menu2State	BOOL	 
        flags[322] = 0; // Menu3State	BOOL	 
        flags[323] = 0; // Menu4State	BOOL	 
        flags[324] = 0; // Menu5State	BOOL	 
        flags[325] = 0; // Menu6State	BOOL	 
        flags[326] = 0; // Snake_Dance	BOOL	tells aviks snake to dance
        flags[327] = 0; // TS_Wheel	FLIPFLOP	"town square wheel
        flags[328] = 0; // Bucket_Full	FLIPFLOP	tells if there is water in the bucket in town square
        flags[329] = 0; // Aloe_In_Water	FLIPFLOP	 
        flags[330] = 0; // BoatCam1	FLIPFLOP	turns on camera for boat pulling into harbor
        flags[331] = 0; // Door_A	FLIPFLOP	 
        flags[332] = 0; // Camera_A	BOOL	 
        flags[333] = 0; // DrawbridgeUp	BOOL	 
        flags[334] = 0; // Bell_Ringing	BOOL	 
        flags[335] = 0; // Rollo_Chest	BOOL	rollo's chest open or closed
        flags[336] = 0; // Silver_Chest	BOOL	krisandra's chest open or closed
        flags[337] = 0; // Brennans_Boat	BOOL	set by boatstar when player gets away from it after it has arived
        flags[338] = 0; // Boat_Arrived	BOOL	set by boatstar when it lands so brennan knows to move around
        flags[339] = 0; // BrennansHatch	BOOL	"set true when brennan goes
        flags[340] = 0; // StartTavernBrawl	BOOL	set when tavernfight starts
        flags[341] = 0; // BrennanIsleKilled	BOOL	true if he has been killed on isle
        flags[342] = 0; // MG_Door2	BOOL	for back door
        flags[343] = 0; // Gremlin_Scene	BOOL	"starts gremlin scene
        flags[344] = 0; // Show_Jaganvir	BOOL	"if true
        flags[345] = 0; // LH_Light	FLIPFLOP	 
        flags[346] = 0; // LH_Door	FLIPFLOP	 
        flags[347] = 0; // LH_Shut	FLIPFLOP	 
        flags[348] = 0; // YaeliBoat	BOOL	tells her boat to sail in
        flags[349] = 0; // YaeliBoatArrived	BOOL	 
        flags[350] = 0; // YaeliBoatLeave	BOOL	 
        flags[351] = 0; // YaeliBoatHasLeft	BOOL	 
        flags[352] = 1; // Rock_1_Down	BOOL	 
        flags[353] = 1; // Rock_2_Down	BOOL	 
        flags[354] = 0; // Rock_1_At_Location_1	BOOL	 
        flags[355] = 0; // Rock_1_At_Location_2	BOOL	 
        flags[356] = 0; // Rock_2_At_Location_1	BOOL	 
        flags[357] = 0; // Rock_2_At_Location_2	BOOL	 
        flags[358] = 0; // SabanTalk	BOOL	 
        flags[359] = 0; // Seen_Dwarven_Lore	BOOL	true when Cyrus has reviewed book of dwarven lore
        flags[360] = 0; // Bought_Elven_Artifacts	BOOL	true when Cyrus buys copy of elven artifacts
        flags[361] = 0; // LillandrilInUse	BOOL	 
        flags[362] = 0; // DeadPirates	NUMBER	number of pirates cyrus has killed
        flags[363] = 0; // Scene5	BOOL	gary's test shortcut avoiding dialog to the cutscene
        flags[364] = 0; // GlobalTemp	NUMBER	used for global temp variables
        flags[365] = 0; // DramStartJailFight	BOOL	tells dram to draw sword and begin fighting
        flags[366] = 0; // StrengthTimer	NUMBER	timers for effects
        flags[367] = 0; // ArmorTimer	NUMBER	 
        flags[368] = 0; // MapTimer	NUMBER	for Maiko making map

    }
    static public void ReadScript(RGFileImport.RGRGMFile filergm)
    {
        SetDefaultFlagValues();
        Scripts = new Dictionary<string, RGMScript>();
        int i=0;
        foreach(KeyValuePair<string,RGRGMFile.RGMRAHDItem> entry in filergm.RAHD.dict)
        {
            RGMScript script = new RGMScript(i, entry.Value, filergm.RAST, filergm.RASB, filergm.RAVA, filergm.RASC, filergm.RAAT, filergm.RANM);
            Scripts.Add(entry.Value.scriptName, script);
            i++;
        }
    }
    static public RGMScript getScript(string scriptname)
    {
        return Scripts[scriptname];
    }
}
