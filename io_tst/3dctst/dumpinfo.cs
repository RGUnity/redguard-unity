using System;
using System.Collections.Generic;
using System.IO;
using RGFileImport;
using RGFileImport2;

namespace xyz
{
    public class test
	{

		public static void Main(string[] args)
		{
            List<string> files = new List<string>(){
"CYRUSA01.3DC",
"ROBOA001.3DC",
           };
            
			RGFileImport2.RG3DFile file3d2 = new RGFileImport2.RG3DFile();

            for(int i=0;i<files.Count;i++)
            {
                file3d2.LoadFile($"../../game_3dfx/fxart/{files[i]}");
                    /*
                if(file3d2.frameDataList.frameData[0].u2==4)
                    Console.WriteLine($"{file3d2.header.numVertices:D3}, {file3d2.frameDataList.frameData[0].u1}, {file3d2.frameDataList.frameData[0].u2} \t {files[i]} MARK");
                else
                    Console.WriteLine($"NOPE");
                    */

            /*
            Console.WriteLine($"framevertices:\n{file3d2.frameVertexData}");
            Console.WriteLine($"vertices:\n{file3d2.vertexData}");
            Console.WriteLine($"faces:\n{file3d2.faceDataList}");
            Console.WriteLine($"Filesize: {file3d2.fileSize}");
            Console.WriteLine($"frames:\n{file3d2.frameDataList}");
            Console.WriteLine($"size: {file3d2.fileSize}");
            */
            Console.WriteLine($"{file3d2.header}");

            }
		}
	}
}
