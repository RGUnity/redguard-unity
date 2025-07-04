using System;
using System.Collections.Generic;
using RGFileImport;

public static class RGRGMScriptStore
{
    static public List<RGMScript> scripts;
    public class RGMScript
    {
        enum readerState
        {
            main,
            LHS,
            RHS,
            param,
            reference,
            formula
        }
        enum objectLoc
        {
            general,
            str
        }

        MemoryReader memoryReader;
public bool TST_IFRET = false;
public bool TST_IFFLIP = false;
        public int scriptIndex;
        public string scriptName;
        public byte[] scriptBytes;
        public List<string> scriptStrings;
        public List<int> scriptVariables;
        public List<byte> scriptAttributes;

        // need entry point and callstack
        public int scriptPC;
        public Stack<int> callstack;

        public RGMScript(int script_i, RGRGMFile.RGMRAHDItem RAHD, RGRGMFile.RGMRASTSection RAST, RGRGMFile.RGMRASBSection RASB, RGRGMFile.RGMRAVASection RAVA, RGRGMFile.RGMRASCSection RASC, RGRGMFile.RGMRAATSection RAAT)
        {
            byte[] tmp_arr;
            scriptIndex = script_i;
            scriptName = RAHD.scriptName;
            scriptStrings = new List<string>();
            scriptVariables = new List<int>();
            scriptAttributes = new List<byte>();

            scriptBytes = new byte[RAHD.RASCLength];
            Array.Copy(RASC.scripts, RAHD.RASCOffset, scriptBytes, 0, RAHD.RASCLength);

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

            memoryReader = new MemoryReader(scriptBytes);

            scriptPC = RAHD.RASCStartAt;
            callstack = new Stack<int>();
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
            o+="/";
            break;
        case 4:
            o+="*";
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
                        o /= vals[i];
                        break;
                    case 4:
                        o *= vals[i];
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
        int doTask(int obj, ushort task_id, int[] parameters)
        {
            string o = new string($"0x{memoryReader.Position:X4}: ");
            o+= obj>=0?$"OBJ_{obj}.":"";
            o+= $"TASK_{task_id}(";
            for(int i=0;i<parameters.Length;i++)
            {
                o+= $"{parameters[i]}";
                o += (i==parameters.Length-1)?"":", ";
            }
            o += ")";
            Console.WriteLine(o);
            return 0xBEEF;
        }
        int doFlagAssign(ushort flag_id, List<int> vals, List<byte> ops)
        {
            string o = new string($"0x{memoryReader.Position:X4}: FLAG_{flag_id} = ");
            for(int i=0;i<ops.Count;i++)
            {
                o+= $"{vals[i]} {DBG_op_2_str(ops[i])}";
            }
            Console.WriteLine(o);
            return 0xBEEF;
        }
        int doVarAssign(ushort var_id, List<int> vals, List<byte> ops)
        {
            string o = new string($"0x{memoryReader.Position:X4}: VAR_{var_id} = ");
            for(int i=0;i<ops.Count;i++)
            {
                o+= $"{vals[i]} {DBG_op_2_str(ops[i])}";
            }
            Console.WriteLine(o);
            scriptVariables[var_id] = doFormula(vals, ops);
            return scriptVariables[var_id];
        }
        int doGetVar(ushort var_id)
        {
            string o = new string($"0x{memoryReader.Position:X4}: VAR_GET_{var_id}({scriptVariables[var_id]})");
            Console.WriteLine(o);
            return scriptVariables[var_id];
        }

        int doEnd(int IP)
        {
            string o = new string($"0x{memoryReader.Position:X4}: END: {IP:X4}");
            Console.WriteLine(o);
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
            bool ret = TST_IFRET;
            string o = new string($"0x{memoryReader.Position:X4}: IF(");
            for(int i=0;i<con.Count;i++)
            {
                o+= $"{LHS[i]} {DBG_cmp_2_str(cmp[i])} {RHS[i]} {DBG_con_2_str(con[i])}";
            }
            o += $" == {ret}";
            Console.WriteLine(o);

            if(TST_IFFLIP)
                TST_IFRET = !TST_IFRET;
            return ret;
        }
        int doGetObject(byte ofs, objectLoc loc)
        {
            string o = new string($"0x{memoryReader.Position:X4}: OBJ_");
            switch(loc)
            {
                case objectLoc.general:
                    o += $"GEN_{ofs:X4}";
                    break;
                case objectLoc.str:
                    o += $"STR_{ofs:X4}";
                    break;
            }
            Console.WriteLine(o);
            return ofs;
        }
        int doSetObjectReference(int obj, int reference, List<int> vals, List<byte> ops)
        {
            string o = new string($"0x{memoryReader.Position:X4}: OBJ_{obj}.REF_{reference} =");
            for(int i=0;i<ops.Count;i++)
            {
                o+= $"{vals[i]} {DBG_op_2_str(ops[i])}";
            }
            Console.WriteLine(o);
            return 0xBEEF;
        }
        int doGetObjectReference(int obj, int reference)
        {
            string o = new string($"0x{memoryReader.Position:X4}: OBJ_{obj}.GETREF_{reference}");
            Console.WriteLine(o);
            return reference;
        }

// END DO* FUNCS
// START READ* FUNCS
        int readTask(int obj)
        {
            ushort task_id = memoryReader.ReadUInt16();
            byte param_cnt = memoryReader.ReadByte();
            int[] parameters = new int[param_cnt];
            for(int i=0;i<param_cnt;i++)
            {
                parameters[i] = readInstruction(readerState.param);
            }
            return doTask(obj, task_id, parameters);
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
            return flag_id;
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
        int readObject()
        {
            byte val = memoryReader.ReadByte();
            int obj = -1;
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
            int obj = readObject();
            if(state != readerState.main)
            {
                return readTask(obj);
            }
            else
            {
                byte nxt = memoryReader.ReadByte();
                switch(nxt)
                {
                    case 0x00:
                    case 0x01:
                    case 0x02:
                        return readTask(obj);
                    default:
                        throw new Exception($"NIMPL: 0x{nxt:X2}");
                }
            }
        }
        int readObjectRef(readerState state)
        {
            int obj = readObject();
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
                case 0x01: // multitask (?)
                case 0x02: // function
                    return readTask(-1);
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
            string o = new string($"{scriptName} ({scriptIndex}):\n");
            o+=$"       ";
            for(int i=0;i<16;i++)
            {
                o+=$"{i:X2} ";
            }
            for(int i=0;i<scriptBytes.Length;i++)
            {
                if(i%16==0)
                    o+= $"\n0x{i:X4}:";
                o+=$"{scriptBytes[i]:X2},";
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

        public override string ToString()
        {
            string o = new string($"SCR:{scriptName}:");
            o += "\nBYTES:\n";
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

    static public void ReadScript(RGFileImport.RGRGMFile filergm)
    {
		/*
        scripts = new List<RGMScript>();
        for(int i=0;i<filergm.RAHD.dict.Count;i++)
        {
            RGMScript script = new RGMScript(i, filergm.RAHD.dict[i], filergm.RAST, filergm.RASB, filergm.RAVA, filergm.RASC, filergm.RAAT);
            scripts.Add(script);
        }
		*/
    }
}
