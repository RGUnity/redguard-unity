using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace RGFileImport
{
	public class RGWLDFile
	{
        // map scale; needs a look in-game to see what fits
        const float WLD_SIZE_SCALE = 13.0f;
        const float WLD_SIZE_SCALE_HEIGHT = 0.5f*1.6f;

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

			public WLDHeader(byte[] buffer)
			{
				// is there a better way to do this? probably
				// can i be bothered to find a better way to do this? probably not
				// should C# man up and let me dump binary data into memory with no sanitychecks? resounding yes
                // update: I made a MemoryReader class that handles reading from memory, *someone* should update this *sometime*
				int ptr = 0;
				unknown1 = new int[unknown1_size];
				for(int i=0;i<unknown1_size;i++)
				{
					
					unknown1[i] = BitConverter.ToInt32(buffer, ptr);
					ptr += 4;
				}
				sec_hdr_size = BitConverter.ToInt32(buffer, ptr);
				ptr += 4;
				file_size = BitConverter.ToInt32(buffer, ptr);
				ptr += 4;
				unknown2 = new int[unknown2_size];
				for(int i=0;i<unknown2_size;i++)
				{
					unknown2[i] = BitConverter.ToInt32(buffer, ptr);
					ptr += 4;
				}
				sec_ofs= new int[num_sections];
				for(int i=0;i<num_sections;i++)
				{
					sec_ofs[i] = BitConverter.ToInt32(buffer, ptr);
					ptr += 4;
				}
				unknown3 = new int[unknown3_size];
				for(int i=0;i<unknown3_size;i++)
				{
					unknown3[i] = BitConverter.ToInt32(buffer, ptr);
					ptr += 4;
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

			public WLDSection(byte[] buffer)
			{
				int ptr = 0;
				unknown1 = new short[unknown1_size];
				for(int i=0;i<unknown1_size;i++)
				{
					unknown1[i] = BitConverter.ToInt16(buffer, ptr);
					ptr += 2;
				}
				texbsi_file = BitConverter.ToInt16(buffer, ptr);
				ptr += 2;
				size = (int)(BitConverter.ToInt16(buffer, ptr));
				ptr += 2;

				unknown2 = new short[unknown2_size];
				for(int i=0;i<unknown2_size;i++)
				{
					unknown2[i] = BitConverter.ToInt16(buffer, ptr);
					ptr += 2;
				}
				int size_half = size/2;
				map1 = new byte[size_half*size_half];
				Array.Copy(buffer, ptr, map1, 0, size_half*size_half);
				ptr += size_half*size_half;
				map2 = new byte[size_half*size_half];
				Array.Copy(buffer, ptr, map2, 0, size_half*size_half);
				ptr += size_half*size_half;
				map3 = new byte[size_half*size_half];
				Array.Copy(buffer, ptr, map3, 0, size_half*size_half);
				ptr += size_half*size_half;
				map4 = new byte[size_half*size_half];
				Array.Copy(buffer, ptr, map4, 0, size_half*size_half);
				ptr += size_half*size_half;
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

		public void LoadFile(string filename)
		{
			const int WLD_hdr_size = 1184;
			const int WLD_section_size = 65558;
			byte[] buffer;
			byte[] buffer_work;


			BinaryReader br = new BinaryReader(new FileStream(filename, FileMode.Open));
			buffer = br.ReadBytes((int)br.BaseStream.Length);
			br.Close();

			buffer_work = new byte[WLD_hdr_size];
			Array.Copy(buffer, 0, buffer_work, 0, WLD_hdr_size);

			hdr = new WLDHeader(buffer);
			sec = new WLDSection[4];
			for(int i=0;i<4;i++)
			{
				buffer_work = new byte[WLD_section_size];
				Array.Copy(buffer, hdr.sec_ofs[i], buffer_work, 0, WLD_section_size);

				sec[i] = new WLDSection(buffer_work);
			}
			maps_data = new WLDMaps(hdr, sec);
		}

		public void BuildMeshes()
		{
			// assuming always 64 textures; safe bet?
			const int texid_cnt = 64;
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
					float tex_rot;
					Vector2 uv_tmp;

				// vertices
					// tri 1;
					a1 = new Vector3((float)(x+1)*WLD_SIZE_SCALE,
							    	 (float)maps_data.heightmap[(x+1)+(y+1)*map_size]*WLD_SIZE_SCALE_HEIGHT,
							    	 -(float)(y+1)*WLD_SIZE_SCALE);

					b1 = new Vector3((float)(x+1)*WLD_SIZE_SCALE,
							    	 (float)maps_data.heightmap[(x+1)+(y+0)*map_size]*WLD_SIZE_SCALE_HEIGHT,
							    	 -(float)(y+0)*WLD_SIZE_SCALE);

					c1 = new Vector3((float)(x+0)*WLD_SIZE_SCALE,
							    	 (float)maps_data.heightmap[(x+0)+(y+0)*map_size]*WLD_SIZE_SCALE_HEIGHT,
							    	 -(float)(y+0)*WLD_SIZE_SCALE);
					// tri 2
					a2 = c1;
					b2 = new Vector3((float)(x+0)*WLD_SIZE_SCALE,
							    	 (float)maps_data.heightmap[(x+0)+(y+1)*map_size]*WLD_SIZE_SCALE_HEIGHT,
							    	 -(float)(y+1)*WLD_SIZE_SCALE);
					c2 = a1;

				// uvs
					// tri 1;
					uva1 = new Vector2(1.0f, 1.0f);
					uvb1 = new Vector2(1.0f, 0.0f);
					uvc1 = new Vector2(0.0f, 0.0f);

					// tri 2;
					uva2 = new Vector2(0.0f, 0.0f);
					uvb2 = new Vector2(0.0f, 1.0f);
					uvc2 = new Vector2(1.0f, 1.0f);

					// rotate them here so no fancy shaders needed
					tex_rot = (float)maps_data.texturemap_flag[x+y*map_size]*(Mathf.PI/2);

					uv_tmp = uva1;
					uva1.x = uv_tmp.x*Mathf.Cos(tex_rot)-uv_tmp.y*Mathf.Sin(tex_rot);
					uva1.y = uv_tmp.x*Mathf.Sin(tex_rot)+uv_tmp.y*Mathf.Cos(tex_rot);

					uv_tmp = uvb1;
					uvb1.x = uv_tmp.x*Mathf.Cos(tex_rot)-uv_tmp.y*Mathf.Sin(tex_rot);
					uvb1.y = uv_tmp.x*Mathf.Sin(tex_rot)+uv_tmp.y*Mathf.Cos(tex_rot);

					uv_tmp = uvc1;
					uvc1.x = uv_tmp.x*Mathf.Cos(tex_rot)-uv_tmp.y*Mathf.Sin(tex_rot);
					uvc1.y = uv_tmp.x*Mathf.Sin(tex_rot)+uv_tmp.y*Mathf.Cos(tex_rot);

					uv_tmp = uva2;
					uva2.x = uv_tmp.x*Mathf.Cos(tex_rot)-uv_tmp.y*Mathf.Sin(tex_rot);
					uva2.y = uv_tmp.x*Mathf.Sin(tex_rot)+uv_tmp.y*Mathf.Cos(tex_rot);
					uv_tmp = uvb2;
					uvb2.x = uv_tmp.x*Mathf.Cos(tex_rot)-uv_tmp.y*Mathf.Sin(tex_rot);
					uvb2.y = uv_tmp.x*Mathf.Sin(tex_rot)+uv_tmp.y*Mathf.Cos(tex_rot);

					uv_tmp = uvc2;
					uvc2.x = uv_tmp.x*Mathf.Cos(tex_rot)-uv_tmp.y*Mathf.Sin(tex_rot);
					uvc2.y = uv_tmp.x*Mathf.Sin(tex_rot)+uv_tmp.y*Mathf.Cos(tex_rot);


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
