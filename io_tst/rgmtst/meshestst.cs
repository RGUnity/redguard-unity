using System;
using System.Collections.Generic;
using System.IO;
namespace xyz
{
    public class test
	{
        static int currentMesh = 0;
        static string[] meshes = new string[] {"mesh1", "mesh2", "mesh3"};
        static int[] meshFrameCount = new int[] {100, 100, 50};
        static int get_frame(int frame)
        {
            int cnt_tot = 0;
            for(int i=0;i<meshes.Length;i++)
            {
                Console.WriteLine($"{frame}<{cnt_tot}");
                if(frame < cnt_tot+meshFrameCount[i])
                {
                    currentMesh = i;
                    return frame-cnt_tot;
                }
                cnt_tot += meshFrameCount[i];
            }
            return frame;
        }
		public static void Main(string[] args)
		{
            int cf = 0;
            cf = get_frame(200);
            Console.WriteLine($"{cf}:{meshes[currentMesh]}");
            cf = get_frame(50);
            Console.WriteLine($"{cf}:{meshes[currentMesh]}");
            cf = get_frame(150);
            Console.WriteLine($"{cf}:{meshes[currentMesh]}");
		}
	}
}
