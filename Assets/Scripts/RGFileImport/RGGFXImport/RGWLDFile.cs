using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace RGFileImport
{
	public class RGWLDFile
	{
        // Engine uses 256 units per grid cell (grid→world scale stored as
        // double at 0x00198dc4 in RGFX.EXE). Divided by the model vertex
        // export scale to get the Unity-space cell size.
        // Previous empirical value was 12.9; the exact ratio is 256/20 = 12.8.
        const float WLD_SIZE_SCALE = 256.0f / 20.0f;

        // Engine terrain vertex builder (FUN_000e24e8) computes
        // world_pos = grid_index * 256 with NO origin offset.
        // The half-cell offsets (-0.5, +0.5) found at 0x00198dfc/0x00198e04
        // are only used for world→grid reverse lookup (camera cell detection),
        // not for geometry. Previous values (-12, 0, 11) were empirical.
        const float WLD_OFFSET_X = 0.0f;
        const float WLD_OFFSET_Y = 0.0f;
        const float WLD_OFFSET_Z = 0.0f;

        // Engine height lookup table (128 entries, indexed by 7-bit heightmap
        // byte). Values are in engine units; divided by 20 at lookup time to
        // match the model vertex export scale. The engine negates these at
        // load time (-ABS(value)) so terrain sits below a water-level
        // reference plane. For Unity (Y-up) we keep them positive.
        //
        // Replaces the previous quartic polynomial approximation which had a
        // +1.93 base offset at index 0 (should be 0) and diverged at high
        // indices (239 vs 388 at index 127).
        //
        // Full table documentation:
        // https://michidk.github.io/redguard-preservation/formats/WLD.html#height-lookup-table
        private static readonly float[] WLD_HEIGHT_TABLE = new float[128]
        {
               0,   40,   40,   40,   80,   80,   80,  120,  120,  120,
             160,  160,  160,  200,  200,  200,  240,  240,  240,  280,
             280,  320,  320,  320,  360,  360,  400,  400,  400,  440,
             440,  480,  480,  480,  520,  520,  560,  560,  600,  600,
             600,  640,  640,  680,  680,  720,  720,  760,  760,  800,
             800,  840,  840,  880,  880,  920,  920,  960, 1000, 1000,
            1040, 1040, 1080, 1120, 1120, 1160, 1160, 1200, 1240, 1240,
            1280, 1320, 1320, 1360, 1400, 1440, 1440, 1480, 1520, 1560,
            1600, 1600, 1640, 1680, 1720, 1760, 1800, 1840, 1880, 1920,
            1960, 2000, 2040, 2080, 2120, 2200, 2240, 2280, 2320, 2400,
            2440, 2520, 2560, 2640, 2680, 2760, 2840, 2920, 3000, 3080,
            3160, 3240, 3360, 3440, 3560, 3680, 3800, 3960, 4080, 4280,
            4440, 4680, 4920, 5200, 5560, 6040, 6680, 7760,
        };

        private float WLD_HEIGHT_FUN(int y)
        {
            return WLD_HEIGHT_TABLE[y & 0x7F] / 20.0f;
        }

		public struct WLDHeader
		{
			const int unknown1_size = 6;
			const int unknown2_size = 28;
			const int unknown3_size = 256;
			const int num_sections = 4;

			public int[] unknown1; // 6*4bytes
			public int sec_hdr_size;
			public int file_size;
			public int[] unknown2; // 28*4 bytes
			public int[] sec_ofs;   // 4*4 bytes
			public int[] unknown3; // 256*4 bytes

			public WLDHeader(MemoryReader memoryReader)
            {
                try
                {
                    unknown1 = memoryReader.ReadInt32s(unknown1_size);
                    sec_hdr_size = memoryReader.ReadInt32();
                    file_size = memoryReader.ReadInt32();
                    unknown2 = memoryReader.ReadInt32s(unknown2_size);
                    sec_ofs = memoryReader.ReadInt32s(num_sections);
                    unknown3 = memoryReader.ReadInt32s(unknown3_size);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load WLD header with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
				return $@"###################################
IO_WLD_hdr_t
###################################
unknown1: [{string.Join(", ", unknown1)}]
sec_hdr_size: {sec_hdr_size}
file_size: {file_size}
unknown2: [{string.Join(", ", unknown2)}]
sec_1_ofs: {sec_ofs[0]}
sec_2_ofs: {sec_ofs[1]}
sec_3_ofs: {sec_ofs[2]}
sec_4_ofs: {sec_ofs[3]}
unknown3: [{string.Join(", ", unknown3)}]
###################################";
			}
		}
		public struct WLDSection
		{
			const int unknown1_size = 3;
			const int unknown2_size = 6;
		// header
			public short[] unknown1; // 3*2 bytes
			public short texbsi_file;
			public int size;         // always 256, assuming size
			public short[] unknown2; // 6*2 bytes
		// map data
			public byte[] map1;
			public byte[] map2;
			public byte[] map3;
			public byte[] map4;

			public WLDSection(MemoryReader memoryReader)
            {
                try
                {
                    unknown1 = memoryReader.ReadInt16s(unknown1_size);
                    texbsi_file = memoryReader.ReadInt16();
                    size = (int)memoryReader.ReadInt16();
                    unknown2 = memoryReader.ReadInt16s(unknown2_size);

                    int size_half = size/2;
                    map1 = memoryReader.ReadBytes(size_half*size_half);
                    map2 = memoryReader.ReadBytes(size_half*size_half);
                    map3 = memoryReader.ReadBytes(size_half*size_half);
                    map4 = memoryReader.ReadBytes(size_half*size_half);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load WLD section with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
				return $@"###################################
IO_WLD_section_t
###################################
unknown1: [{string.Join(", ", unknown1)}]
texbsi_file: {texbsi_file}
size: {size}
unknown2: [{string.Join(", ", unknown2)}]
###################################";
			}
		}

		public struct WLDMaps
		{
		// bitmaps
			public int map_size;
			public byte[] heightmap;
			public byte[] heightmap_flag;
			public byte[] texturemap;
			public byte[] texturemap_flag;

			public WLDMaps(WLDHeader hdr, WLDSection[] sections)
			{
				map_size = sections[0].size;
				int size = map_size;
				int size_half = map_size/2;
				heightmap = new byte[size*size];
				heightmap_flag = new byte[size*size];
				texturemap = new byte[size*size];
				texturemap_flag = new byte[size*size];

				for(int s=0;s<4;s++)
				{
					for(int y=0;y<size_half;y++)
					{
						int mx = size_half*(s%2);
						int my = y+(size_half*(s>=2?1:0));
						int mw = size_half*2;
						Array.Copy(sections[s].map1, y*size_half, heightmap, mx+(my*mw), size_half);
						Array.Copy(sections[s].map1, y*size_half, heightmap_flag, mx+(my*mw), size_half);
						Array.Copy(sections[s].map3, y*size_half, texturemap, mx+(my*mw), size_half);
						Array.Copy(sections[s].map3, y*size_half, texturemap_flag, mx+(my*mw), size_half);
					}
				}
				for(int i=0;i<size*size;i++)
				{
					heightmap[i] = (byte)(heightmap[i]&127);			      // first 7 bits are heightmap
					heightmap_flag[i] = (byte)(heightmap_flag[i]&128);        // last bit is an unknown flag
					texturemap[i] = (byte)(texturemap[i]&63);                 // first 6 bits are the texture idnex
					texturemap_flag[i] = (byte)((texturemap_flag[i]&192)>>6); // last 2 bits are texture rotation
				}
			}
			public override string ToString()
			{
				return $@"###################################
IO_WLD_data_t
###################################
###################################";
			}
		}
		public struct WLDMesh
		{
			public List<Vector3> vertices;
			public List<Vector2> uv;
			public List<int> triangles;
			public int TextureId;
			
			public WLDMesh(int texid)
			{
				TextureId = texid;
				vertices = new List<Vector3>();
				uv = new List<Vector2>();
				triangles = new List<int>();
			}

			public void AppendTri(Vector3 a, Vector3 b, Vector3 c, Vector2 uva, Vector2 uvb, Vector2 uvc)
			{
				vertices.Add(a);
				vertices.Add(b);
				vertices.Add(c);
				uv.Add(uva);
				uv.Add(uvb);
				uv.Add(uvc);
				triangles.Add(vertices.Count()-3);
				triangles.Add(vertices.Count()-2);
				triangles.Add(vertices.Count()-1);
			}
			public int GetTextureId()
			{
				return TextureId;
			}
			public Vector3[] GetVertices()
			{
				return vertices.ToArray();
			}
			public Vector2[] GetUv()
			{
				return uv.ToArray();
			}

		}

	// data
		WLDHeader hdr;
		public WLDSection[] sec;
		public WLDMaps maps_data;
		public WLDMesh[] meshes;
        public long fileSize;

		public void LoadFile(string filename)
        {
            try
            {
                byte[] buffer;
                BinaryReader binaryReader = new BinaryReader(File.OpenRead(filename));
                fileSize = binaryReader.BaseStream.Length;
                buffer = binaryReader.ReadBytes((int)fileSize);
                binaryReader.Close();
                LoadMemory(buffer);
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load WLD file {filename} with error:\n{ex.Message}");
            }
        }

		public void LoadMemory(byte[] buffer)
        {
            try
            {
                MemoryReader memoryReader = new MemoryReader(buffer);
                hdr = new WLDHeader(memoryReader);
                sec = new WLDSection[4];
                for(int i=0;i<4;i++)
                {
                    sec[i] = new WLDSection(memoryReader);
                }
                maps_data = new WLDMaps(hdr, sec);

            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load WLD file from memory with error:\n{ex.Message}");
            }
        }
		public void BuildMeshes()
		{
			// assuming always 64 textures; safe bet?
			const int texid_cnt = 64;

            // hardcoded to avoid rounding errors in UVs
            Vector2[,] uv_rotations = 
            {	
                { // 00
                new Vector2(1.0f, 1.0f),
				new Vector2(1.0f, 0.0f),
				new Vector2(0.0f, 0.0f),
				new Vector2(0.0f, 0.0f),
				new Vector2(0.0f, 1.0f),
				new Vector2(1.0f, 1.0f)
                },
                { // 01
                new Vector2(0.0f, 1.0f),
				new Vector2(1.0f, 1.0f),
				new Vector2(1.0f, 0.0f),
				new Vector2(1.0f, 0.0f),
				new Vector2(0.0f, 0.0f),
				new Vector2(0.0f, 1.0f)
                },
                { // 10
                new Vector2(0.0f, 0.0f),
				new Vector2(0.0f, 1.0f),
				new Vector2(1.0f, 1.0f),
				new Vector2(1.0f, 1.0f),
				new Vector2(1.0f, 0.0f),
				new Vector2(0.0f, 0.0f)
                },
                { // 11
                new Vector2(1.0f, 0.0f),
				new Vector2(0.0f, 0.0f),
				new Vector2(0.0f, 1.0f),
				new Vector2(0.0f, 1.0f),
				new Vector2(1.0f, 1.0f),
				new Vector2(1.0f, 0.0f)
                },


            };

			int map_size =  maps_data.map_size;

			meshes = new WLDMesh[texid_cnt];
			for(int i=0;i<texid_cnt;i++)
			{
				meshes[i] = new WLDMesh(i);
			}
			for(int y=0;y<map_size-1;y++)
			{
				for(int x=0;x<map_size-1;x++)
				{
					Vector3 a1;
					Vector3 b1;
					Vector3 c1;
					Vector2 uva1;
					Vector2 uvb1;
					Vector2 uvc1;

					Vector3 a2;
					Vector3 b2;
					Vector3 c2;
					Vector2 uva2;
					Vector2 uvb2;
					Vector2 uvc2;
					int tex_id;
					int tex_rot;

				// vertices
					// tri 1;
					a1 = new Vector3((float)(x+1)*WLD_SIZE_SCALE,
                                     WLD_HEIGHT_FUN(maps_data.heightmap[(x+1)+(y+1)*map_size]),
							    	 -(float)(y+1)*WLD_SIZE_SCALE);
					a1 += new Vector3(WLD_OFFSET_X,WLD_OFFSET_Y,WLD_OFFSET_Z);

					b1 = new Vector3((float)(x+1)*WLD_SIZE_SCALE,
                                     WLD_HEIGHT_FUN(maps_data.heightmap[(x+1)+(y+0)*map_size]),
							    	 -(float)(y+0)*WLD_SIZE_SCALE);
					b1 += new Vector3(WLD_OFFSET_X,WLD_OFFSET_Y,WLD_OFFSET_Z);

					c1 = new Vector3((float)(x+0)*WLD_SIZE_SCALE,
                                     WLD_HEIGHT_FUN(maps_data.heightmap[(x+0)+(y+0)*map_size]),
							    	 -(float)(y+0)*WLD_SIZE_SCALE);
					c1 += new Vector3(WLD_OFFSET_X,WLD_OFFSET_Y,WLD_OFFSET_Z);

					// tri 2
					a2 = c1;
					b2 = new Vector3((float)(x+0)*WLD_SIZE_SCALE,
                                     WLD_HEIGHT_FUN(maps_data.heightmap[(x+0)+(y+1)*map_size]),
							    	 -(float)(y+1)*WLD_SIZE_SCALE);
					b2 += new Vector3(WLD_OFFSET_X,WLD_OFFSET_Y,WLD_OFFSET_Z);

					c2 = a1;

				// uvs
					tex_rot = maps_data.texturemap_flag[x+y*map_size];

					// tri 1;
					uva1 = uv_rotations[tex_rot,0];
					uvb1 = uv_rotations[tex_rot,1];
					uvc1 = uv_rotations[tex_rot,2];

					// tri 2;
					uva2 = uv_rotations[tex_rot,3];
					uvb2 = uv_rotations[tex_rot,4];
					uvc2 = uv_rotations[tex_rot,5];

					tex_id = (int)maps_data.texturemap[x+y*map_size];
					meshes[tex_id].AppendTri(c1, b1, a1, uvc1, uvb1, uva1);
					meshes[tex_id].AppendTri(c2, b2, a2, uvc2, uvb2, uva2);
				}
			}

		}

		public void PrintWLD()
		{
			Console.WriteLine(hdr);
			for(int i=0;i<4;i++)
			{
				Console.WriteLine(sec[i]);
			}
		}
	}
}
