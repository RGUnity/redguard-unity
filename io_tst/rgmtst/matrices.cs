using System;
using System.Collections.Generic;
using System.IO;
namespace xyz
{
    public class test
	{
        public struct matrix
        {
            public float a0,a1,a2;
            public float a3,a4,a5;
            public float a6,a7,a8;
        }
        unsafe static string ToHexString(float f) {
           var i = *((int*) &f);
            return "0x" + i.ToString("X8");
        }
        static void printmat_regf(matrix m)
        {
            Console.WriteLine($"{m.a0:F2} {m.a1:F2} {m.a2:F2}");
            Console.WriteLine($"{m.a3:F2} {m.a4:F2} {m.a5:F2}");
            Console.WriteLine($"{m.a6:F2} {m.a7:F2} {m.a8:F2}");
        }

        static void printmat_reg(matrix m)
        {
            Console.WriteLine($"{ToHexString(m.a0)} {ToHexString(m.a1)} {ToHexString(m.a2)}");
            Console.WriteLine($"{ToHexString(m.a3)} {ToHexString(m.a4)} {ToHexString(m.a5)}");
            Console.WriteLine($"{ToHexString(m.a6)} {ToHexString(m.a7)} {ToHexString(m.a8)}");
        }
        static void printmat_flip(matrix m)
        {
            Console.WriteLine($"{ToHexString(m.a0)} {ToHexString(m.a3)} {ToHexString(m.a6)}");
            Console.WriteLine($"{ToHexString(m.a1)} {ToHexString(m.a4)} {ToHexString(m.a7)}");
            Console.WriteLine($"{ToHexString(m.a2)} {ToHexString(m.a5)} {ToHexString(m.a8)}");
        }
        static matrix mat_rot(float a, float b, float c)
        {
            matrix m = new matrix();
            m.a0 = (float)(Math.Cos((float)b)*Math.Cos((float)c));
            m.a1 = (float)(-Math.Cos((float)b)*Math.Sin((float)c));
            m.a2 = (float)(Math.Sin((float)b));
            m.a3 = (float)(Math.Sin((float)a)*Math.Sin((float)b)*Math.Cos((float)c) + Math.Cos((float)a)*Math.Sin((float)c));
            m.a4 = (float)(-Math.Sin((float)a)*Math.Sin((float)b)*Math.Sin((float)c) + Math.Cos((float)a)*Math.Cos((float)c));
            m.a5 = (float)(-Math.Sin((float)a)*Math.Cos((float)b));
            m.a6 = (float)(-Math.Cos((float)a)*Math.Sin((float)b)*Math.Cos((float)c) + Math.Sin((float)a)*Math.Sin((float)c));
            m.a7 = (float)(Math.Cos((float)a)*Math.Sin((float)b)*Math.Sin((float)c) + Math.Sin((float)a)*Math.Cos((float)c));
            m.a8 = (float)(Math.Cos((float)a)*Math.Cos((float)b));
            return m;
        }
        static float cos(float i)
        {
            return (float)Math.Cos((float)i);
        }
        static float sin(float i)
        {
            return (float)Math.Sin((float)i);
        }
        static matrix mat_rot_dif(float a, float x, float y, float z)
        {
            matrix m = new matrix();
            m.a0 = x*x*(1-cos(a))+cos(a);
            m.a1 = y*x*(1-cos(a))-z*sin(a);
            m.a2 = z*x*(1-cos(a))+y*sin(a);
            m.a3 = x*y*(1-cos(a))+z*sin(a);
            m.a4 = y*y*(1-cos(a))+cos(a);
            m.a5 = z*y*(1-cos(a))-x*sin(a);
            m.a6 = x*z*(1-cos(a))-y*sin(a);
            m.a7 = y*z*(1-cos(a))+x*sin(a);
            m.a8 = z*z*(1-cos(a))+cos(a);
            return m;
        }


        public static void Main(string[] args)
		{
            float rad_90 = 90*((float)Math.PI/180.0f);
            matrix m901 = mat_rot(rad_90, 0, 0);
            matrix m902 = mat_rot(0, rad_90, 0);
            matrix m903 = mat_rot(0, 0, rad_90);

            Console.WriteLine("M901 F:");
            printmat_regf(m901);
            Console.WriteLine("########################");
            Console.WriteLine("M901 F:");
            printmat_regf(m902);
            Console.WriteLine("########################");
            Console.WriteLine("M901 F:");
            printmat_regf(m903);
            Console.WriteLine("########################");

		}
	}
}
