using System;
using System.Collections.Generic;
using System.IO;

namespace RGFileImport
{
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
    }
    public struct Vector4
    {
        public float x;
        public float y;
        public float z;
        public float w;
    }

    public class RGINIFile
	{
        public INIWorldData worldData;
        public struct INIWorldData
        {
            public int start_world;
            public int start_marker;
            public int test_map_delay;
            public List<int> test_map_order;
            public Dictionary<int,INIWorld> worlds;
        }
        public struct INIWorld
        {
            public string map;
            public string world;
            public int redbook;
            public string palette;
            public int shade;
            public int haze;
            public string sky;
            public Vector4 sun;
            public int ambient;
            public int background;
            public int sky_xrotate;
            public int sky_yrotate;
            public int rain_delay;
            public int rain_drops;
            public int rain_start;
            public int rain_end;
            public Vector4 rain_sphere1;
            public Vector4 rain_sphere2;
            public Vector4 rain_sphere3;
            public Vector4 rain_sphere4;
            public int compass;
            public string flash_filename;
            public int ambientfx;
            public Vector3 ambientrgb;
            public int sunangle;
            public int sunskew;
            public string sunimg;
            public Vector3 sunimgrgb;
            public int sunscale;
            public Vector4 sunrgb;
            public Vector3 fogrgb;
            public string skyfx;
            public int skyscale;
            public int skylevel;
            public int skyspeed;
            public Vector3 wave;

        }
        public enum INISection
        {
            inisection_empty = 0,
            inisection_world,
        }
        public void ParseWorldLine(string line)
        {
            if(line.Length == 0)
                return;
            string[] values;
            string[] parts = line.Split('=');
            string member = parts[0].Trim();
            string val = parts[1].Trim();
            
            if(member.Contains("[") && member.Contains("]"))
            {
                // world-specific data
                int worldIndex = int.Parse(member.Substring(member.IndexOf('[')+1,
                                                            member.IndexOf(']')-member.IndexOf('[')-1)); 
                string memberName = member.Substring(0, member.IndexOf('['));
                INIWorld tmp = new INIWorld();
                if(!worldData.worlds.TryGetValue(worldIndex, out tmp))
                {
                    worldData.worlds.Add(worldIndex, tmp);
                }
                switch(memberName)
                {
                    case "world_map":
                        tmp.map = val;
                        break;
                    case "world_world":
                        tmp.world = val;
                        break;
                    case "world_redbook":
                        tmp.redbook = int.Parse(val);
                        break;
                    case "world_palette":
                        tmp.palette = val;
                        break;
                    case "world_shade":
                        tmp.shade = int.Parse(val);
                        break;
                    case "world_haze":
                        tmp.haze = int.Parse(val);
                        break;
                    case "world_sky":
                        tmp.sky = val;
                        break;
                    case "world_sun":
                        values = val.Split(",");
                        tmp.sun.x = int.Parse(values[0]);
                        tmp.sun.y = int.Parse(values[1]);
                        tmp.sun.z = int.Parse(values[2]);
                        tmp.sun.w = int.Parse(values[3]);
                        break;
                    case "world_ambient":
                        tmp.ambient = int.Parse(val);
                        break;
                    case "world_background":
                        tmp.background = int.Parse(val);
                        break;
                    case "world_sky_xrotate":
                        tmp.sky_xrotate = int.Parse(val);
                        break;
                    case "world_sky_yrotate":
                        tmp.sky_yrotate = int.Parse(val);
                        break;
                    case "world_rain_delay":
                        tmp.rain_delay = int.Parse(val);
                        break;
                    case "world_rain_drops":
                        tmp.rain_drops = int.Parse(val);
                        break;
                    case "world_rain_start":
                        tmp.rain_start = int.Parse(val);
                        break;
                    case "world_rain_end":
                        tmp.rain_end = int.Parse(val);
                        break;
                    case "world_rain_sphere1":
                        values = val.Split(",");
                        tmp.rain_sphere1.x = int.Parse(values[0]);
                        tmp.rain_sphere1.y = int.Parse(values[1]);
                        tmp.rain_sphere1.z = int.Parse(values[2]);
                        tmp.rain_sphere1.w = int.Parse(values[3]);
                        break;
                    case "world_rain_sphere2":
                        values = val.Split(",");
                        tmp.rain_sphere2.x = int.Parse(values[0]);
                        tmp.rain_sphere2.y = int.Parse(values[1]);
                        tmp.rain_sphere2.z = int.Parse(values[2]);
                        tmp.rain_sphere2.w = int.Parse(values[3]);
                        break;
                    case "world_rain_sphere3":
                        values = val.Split(",");
                        tmp.rain_sphere3.x = int.Parse(values[0]);
                        tmp.rain_sphere3.y = int.Parse(values[1]);
                        tmp.rain_sphere3.z = int.Parse(values[2]);
                        tmp.rain_sphere3.w = int.Parse(values[3]);
                        break;
                    case "world_rain_sphere4":
                        values = val.Split(",");
                        tmp.rain_sphere4.x = int.Parse(values[0]);
                        tmp.rain_sphere4.y = int.Parse(values[1]);
                        tmp.rain_sphere4.z = int.Parse(values[2]);
                        tmp.rain_sphere4.w = int.Parse(values[3]);
                        break;
                    case "world_compass":
                        tmp.compass = int.Parse(val);
                        break;
                    case "world_flash_filename":
                        tmp.flash_filename = val;
                        break;
                    case "world_ambientfx":
                        tmp.ambientfx = int.Parse(val);
                        break;
                    case "world_ambientrgb":
                        values = val.Split(",");
                        tmp.ambientrgb.x = int.Parse(values[0]);
                        tmp.ambientrgb.y = int.Parse(values[1]);
                        tmp.ambientrgb.z = int.Parse(values[2]);
                        break;
                    case "world_sunangle":
                        tmp.sunangle = int.Parse(val);
                        break;
                    case "world_sunskew":
                        tmp.sunskew = int.Parse(val);
                        break;
                    case "world_sunimg":
                        tmp.sunimg = val;
                        break;
                    case "world_sunimgrgb":
                        values = val.Split(",");
                        tmp.sunimgrgb.x = int.Parse(values[0]);
                        tmp.sunimgrgb.y = int.Parse(values[1]);
                        tmp.sunimgrgb.z = int.Parse(values[2]);
                        break;
                    case "world_sunscale":
                        tmp.sunscale = int.Parse(val);
                        break;
                    case "world_sunrgb":
                        values = val.Split(",");
                        tmp.sunrgb.x = int.Parse(values[0]);
                        tmp.sunrgb.y = int.Parse(values[1]);
                        tmp.sunrgb.z = int.Parse(values[2]);
                        tmp.sunrgb.w = int.Parse(values[3]);
                        break;
                    case "world_fogrgb":
                        values = val.Split(",");
                        tmp.fogrgb.x = int.Parse(values[0]);
                        tmp.fogrgb.y = int.Parse(values[1]);
                        tmp.fogrgb.z = int.Parse(values[2]);
                        break;
                    case "world_skyfx":
                        tmp.skyfx = val;
                        break;
                    case "world_skyscale":
                        tmp.skyscale = int.Parse(val);
                        break;
                    case "world_skylevel":
                        tmp.skylevel = int.Parse(val);
                        break;
                    case "world_skyspeed":
                        tmp.skyspeed = int.Parse(val);
                        break;
                    case "world_wave":
                        values = val.Split(",");
                        tmp.wave.x = int.Parse(values[0]);
                        tmp.wave.y = int.Parse(values[1]);
                        tmp.wave.z = int.Parse(values[2]);
                        break;
                    default:
                        Console.WriteLine($"UNKNOWN: {memberName}");
                        break;
                }
                worldData.worlds[worldIndex] = tmp;
            }
            else
            {
                // worlds metadata
                switch(member)
                {
                    case "start_world":
                        worldData.start_world = int.Parse(val);
                        break;
                    case "start_marker":
                        worldData.start_marker = int.Parse(val);
                        break;
                    case "test_map_delay":
                        worldData.test_map_delay = int.Parse(val);
                        break;
                    case "test_map_order":
                        values = val.Split(",");
                        foreach( string v in values)
                        {
                            Console.WriteLine($"V: {v}");
                            worldData.test_map_order.Add(int.Parse(v));
                        }
                        break;
                    default:
                        break;
                }
            }
            Console.WriteLine(line);
        }
        public void ParseLine(INISection section, string line)
        {
            switch(section)
            {
                case INISection.inisection_world:
                    ParseWorldLine(line);
                    break;
                default:
                    Console.WriteLine("KAPUT");
                    break;
            }
        }

        public void LoadFile(string filename)
        {
            worldData.test_map_order = new List<int>();
            worldData.worlds = new Dictionary<int,INIWorld>();
            try
            {
                StreamReader streamReader = new StreamReader(filename);
                string line = streamReader.ReadLine();
                INISection currentSection = INISection.inisection_empty;
                while(line != null)
                {
                    Console.WriteLine($"L: {line}");
                    switch(line)
                    {
                        case "[world]":
                            currentSection = INISection.inisection_world;
                            break;
                        default:
                            ParseLine(currentSection, line);
                            break;
                    }
                    line = streamReader.ReadLine();
                }
                streamReader.Close();
            }
            catch(Exception ex)
            {
                Exception ex2 = ex;
                while(ex2.InnerException != null)
                {
                    Console.WriteLine($"ex: {ex2.Message}");
                    ex2 = ex2.InnerException;
                }
                throw new Exception($"Failed to load INI file {filename} with error:\n{ex.Message}\nStackTrace:\n${ex.StackTrace}");
            } 
        }

	}
    public class tst
    {
		public static void Main(string[] args)
		{
            RGINIFile inifile = new RGINIFile();
			inifile.LoadFile("../../game_3dfx/WORLD.INI");
            Console.WriteLine($"sw: {inifile.worldData.start_world}");
            Console.WriteLine($"sm: {inifile.worldData.start_marker}");
            Console.WriteLine($"tmd: {inifile.worldData.test_map_delay}");
            Console.WriteLine($"tmo: {string.Join(",",inifile.worldData.test_map_order)}");
            //for(int i=0;i<inifile.worldData.test_map_order.Count;i++)
            //test_map_order;
		}
	}
}
