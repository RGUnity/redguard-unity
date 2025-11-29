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
			RGFileImport.RG3DFile file3d = new RG3DFile();
			RGFileImport2.RG3DFile file3d2 = new RGFileImport2.RG3DFile();
			file3d.LoadFile("../../game_3dfx/fxart/CYRSA001.3DC");
			file3d2.LoadFile("../../game_3dfx/fxart/CYRSA001.3DC");

            Console.WriteLine($"OG");
// face normals 
/*
            string o = new String($@"###################################
NormalDataList
###################################");
            o += "\n";
            for(int i=0;i<file3d.FaceNormals.Count;i++)
            {
                o += $"{file3d.FaceNormals[i].x},";
                o += $"{file3d.FaceNormals[i].y},";
                o += $"{file3d.FaceNormals[i].z}\n";
            }
            o += "###################################";
            Console.WriteLine(o);
*/
// frame vertex offsets
/*
                string o = new String($@"###################################
FrameVertexDataList
###################################");
                o += "\n";
                for(int f=0;f<file3d.VertexFrameDeltas.Count;f++)
                {
                    o += $"FRAME {f}:\n";
                    for(int i=0;i<file3d.VertexFrameDeltas.Count;i++)
                    {
                        o += $"{file3d.VertexFrameDeltas[f][i].x},";
                        o += $"{file3d.VertexFrameDeltas[f][i].y},";
                        o += $"{file3d.VertexFrameDeltas[f][i].z}\n";
                    }
                }
                o += "###################################";
            Console.WriteLine(o);

*/
// vertex coords
/*
            string o = new String($@"###################################
VertexDataList
###################################");
            o += "\n";
            for(int i=0;i<file3d.VertexCoordinates.Count;i++)
            {
                o += $"{file3d.VertexCoordinates[i].x},";
                o += $"{file3d.VertexCoordinates[i].y},";
                o += $"{file3d.VertexCoordinates[i].z}\n";
            }
            o += "###################################";
            Console.WriteLine(o);
*/

// facedata
/*
            string o = new String($@"###################################
            FaceDataList
            ###################################");
            o += "\n";
            for(int i=0;i<file3d.FaceDataCollection.Count;i++)
            {
                o += $"{i:D3} ";
                o += $"{file3d.FaceDataCollection[i].VertexCount:X8} ";
                o += $"{file3d.FaceDataCollection[i].U1:X8} ";
                o += $"{file3d.FaceDataCollection[i].TextureData:X8} ";
                o += $"{file3d.FaceDataCollection[i].U4:X8} ";
                // vertexdata

                // calculated values
                o += $"{file3d.FaceDataCollection[i].TextureId:X8} ";
                o += $"{file3d.FaceDataCollection[i].ImageId:X8} ";
                o += $"{file3d.FaceDataCollection[i].solid_color:X8} ";
                o += $"{file3d.FaceDataCollection[i].ColorIndex:X8}\n";

                for(int j=0;j<file3d.FaceDataCollection[i].VertexData.Count;j++)
                {
                    o += $"{file3d.FaceDataCollection[i].VertexData[j].VertexIndex:X3} ";
                    o += $"{file3d.FaceDataCollection[i].VertexData[j].U:X3} ";
                    o += $"{file3d.FaceDataCollection[i].VertexData[j].V:X3}\n";
                }

            }
            o += "###################################";
            Console.WriteLine(o);
*/
// framedata
/*
            string o = new String($@"###################################
FrameDataList
###################################");
                o += "\n";
                for(int i=0;i<file3d.frameData.Count;i++)
                {
                    o += $"{file3d.frameData[i].FrameVertexOffset:X8} {file3d.frameData[i].FrameNormalOffset:X8} {file3d.frameData[i].u1:X8} {file3d.frameData[i].u2:X8}\n";
                }
                o += "###################################";
                Console.WriteLine(o);
 
*/
            /*
            Console.WriteLine($"Filesize: {file3d.fileSize}");
            Console.WriteLine($"{file3d.header}");
            */

            Console.WriteLine($"NW");

            /*
            Console.WriteLine($"framevertices:\n{file3d2.frameVertexData}");
            Console.WriteLine($"vertices:\n{file3d2.vertexData}");
            Console.WriteLine($"faces:\n{file3d2.faceDataList}");
            Console.WriteLine($"Filesize: {file3d2.fileSize}");
            */
            Console.WriteLine($"frames:\n{file3d2.frameDataList}");
            Console.WriteLine($"{file3d2.header}");
            Console.WriteLine($"size: {file3d2.fileSize}");

            int num = 1884;
            for(int i=1;i<num;i++)
            {
                if(num%i == 0)
                    Console.WriteLine($"NUM: {num} / {i} = {num/i}");
            }
/*
*/
		}
	}
}
