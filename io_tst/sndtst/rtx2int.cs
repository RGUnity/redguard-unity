using System;
using System.Collections.Generic;
using System.IO;
using RGFileImport;

namespace xyz
{
    public class test
	{

		public static void Main(string[] args)
		{
            string sin = "cicm";
            // expected out:
            // cicm = 0x6D 63 69 63
            byte[] retval = System.Text.Encoding.ASCII.GetBytes(sin);
            uint o = BitConverter.ToUInt32(retval);

            /*
            int o = 0;
            for(int i=0;i<4;i++)
            {
                o += (int)retval[i] << (i*8);
            }
            */
            Console.WriteLine($"OUT: {o:X}");

		}
	}
}
