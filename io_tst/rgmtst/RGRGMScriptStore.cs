using System;
using System.Collections.Generic;
using RGFileImport;

public static class RGRGMScriptStore
{
    public struct RGMScript
    {
        public List<byte> scriptBytes;
        public List<string> scriptStrings;
        public List<int> scriptVariables;
        public List<byte> scriptAttributes;
        public RGMScript(int script_i, RGRGMFile.RGMRAHDItem RAHD, RGRGMFile.RGMRASTSection RAST, RGRGMFile.RGMRASBSection RASB, RGRGMFile.RGMRAVASection RAVA, RGRGMFile.RGMRASCSection RASC, RGRGMFile.RGMRAATSection RAAT)
        {
            byte[] tmp_arr;
            scriptBytes = new List<byte>();
            scriptStrings = new List<string>();
            scriptVariables = new List<int>();
            scriptAttributes = new List<byte>();

            tmp_arr = new byte[RAHD.scriptLength];
            Array.Copy(RASC.scripts, RAHD.scriptDataOffset, tmp_arr, 0, RAHD.scriptLength);
            scriptBytes = new List<byte>(tmp_arr);

Console.WriteLine($"RAHD: {RAHD}");
Console.WriteLine($"RAHD: {script_i} : {RAHD.stringCount:X} : {RAHD.stringOffsetIndex}");
Console.WriteLine($"RASB: {RASB.offsets.Length}");
            for(int i=0;i<RAHD.stringCount;i++)
            {
Console.WriteLine($"SC: {RAHD.stringOffsetIndex+i} = {RAHD.stringOffsetIndex}+{i}");
                int ofs = RASB.offsets[RAHD.stringOffsetIndex+i];
                int end = Array.IndexOf(RAST.text, '\0', ofs);
                char[] tmp_char_arr = new char[end-ofs];

                Array.Copy(RAST.text, ofs, tmp_char_arr, 0, end-ofs);
                scriptStrings.Add(new string(tmp_char_arr));
            }

            for(int i=0;i<RAHD.variableCount;i++)
            {
                if(RAHD.variableCount > 0)
                {
                    int[] tmp_int_arr = new int[RAHD.variableCount];
                    Array.Copy(RAVA.data, RAHD.variableOffset, tmp_int_arr, 0, RAHD.variableCount);
                    scriptVariables = new List<int>(tmp_int_arr);
                }
            }

            tmp_arr = new byte[256];
            Array.Copy(RAAT.attributes, script_i*256, tmp_arr, 0, 256);

        }
    }
    static public void ReadScript(RGFileImport.RGRGMFile filergm)
    {
        List<RGMScript> scripts = new List<RGMScript>();
        for(int i=0;i<filergm.RAHD.items.Count;i++)
        {
            RGMScript script = new RGMScript(i, filergm.RAHD.items[i], filergm.RAST, filergm.RASB, filergm.RAVA, filergm.RASC, filergm.RAAT);
        }
    }
}
