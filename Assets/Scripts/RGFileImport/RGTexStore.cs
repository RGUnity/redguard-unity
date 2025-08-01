using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets.Scripts.RGFileImport.RGGFXImport;
using RGFileImport;
using Unity.Profiling;

public static class RGTexStore
{

static readonly ProfilerMarker s_get_mat = new ProfilerMarker("GetMaterial");
static readonly ProfilerMarker s_get_mat_bsi = new ProfilerMarker("GetMaterial_BSI");
static readonly ProfilerMarker s_load_texbsi = new ProfilerMarker("LoadTEXBSI");
static readonly ProfilerMarker s_load_bsi = new ProfilerMarker("LoadBSI");
static readonly ProfilerMarker s_load_mat_bsi = new ProfilerMarker("LoadMaterialBSI");
static readonly ProfilerMarker s_load_texf = new ProfilerMarker("LoadTextureFile");
static readonly ProfilerMarker s_load_pal = new ProfilerMarker("LoadPalette");

    // keys like this:
    // PAL/TEXBSI#/IMG#/SHADER
    // PAL/-1/COL#/SHADER
    public static Dictionary<string, Material> MaterialDict;
    static Dictionary<string, RGCOLFile> PaletteDict;
    static Dictionary<string, RGTEXBSIFile> BSIFDict;
    static Dictionary<string, Shader> ShaderDict;

    static RGTexStore()
    {
        MaterialDict = new Dictionary<string, Material>();
        PaletteDict = new Dictionary<string, RGCOLFile>();
        BSIFDict = new Dictionary<string, RGTEXBSIFile>();
        ShaderDict = new Dictionary<string, Shader>();

        ShaderDict.Add("DEFAULT", Shader.Find("Universal Render Pipeline/Simple Lit"));
        ShaderDict.Add("FLATS", Shader.Find("Universal Render Pipeline/Unlit"));

    }

    public static void DumpDict()
    {
        Debug.Log($"MATS:");
        List<string> keys = new List<string>(MaterialDict.Keys);
        for(int i=0;i<keys.Count;i++)
            Debug.Log($"{i:D3}: {keys[i]}");
        Debug.Log($"COLS:");
        keys = new List<string>(PaletteDict.Keys);
        for(int i=0;i<keys.Count;i++)
            Debug.Log($"{i:D3}: {keys[i]}");
        Debug.Log($"TEXBSIS:");
        keys = new List<string>(BSIFDict.Keys);
        for(int i=0;i<keys.Count;i++)
            Debug.Log($"{i:D3}: {keys[i]}");
        Debug.Log($"SHADERS:");
        keys = new List<string>(ShaderDict.Keys);
        for(int i=0;i<keys.Count;i++)
            Debug.Log($"{i:D3}: {keys[i]}");
    }
    public static Material GetMaterial(string palname, int texbsi, int img, string shadername)
    {
        
using(s_get_mat.Auto()){
        string mat_key = $"{palname}/{texbsi:D3}/{img:D3}/{shadername}";
        Material o;
        if(MaterialDict.TryGetValue(mat_key, out o))
        {
            return o;
        }
        else
        {
            if(texbsi >= 0)
            {
                RGTEXBSIFile bsif = LoadTEXBSI(texbsi);
                RGCOLFile palette = LoadPalette(palname);

                List<Texture2D>[] tex_lst_sorted = new List<Texture2D>[bsif.images.Count];
                for(int i =0;i<bsif.images.Count;i++)
                {
                    string new_mat_key = $"{palname}/{texbsi:D3}/{Int32.Parse(bsif.images[i].imageName.Substring(3)):D3}/{shadername}";

                    List<Texture2D> cur_tex = GraphicsConverter.RGBSIToTexture2D(bsif.images[i].imageData, palette);

                    MaterialDict.Add(new_mat_key, new Material(ShaderDict[shadername]));
                    MaterialDict[new_mat_key].mainTexture = cur_tex[0];

                    // note that FRAME_0 does not exist, its called _MainTex or smt
                    for(int j=0;j<cur_tex.Count;j++)
                    {
                        MaterialDict[new_mat_key].SetTexture($"FRAME_{j}", cur_tex[j]);
                    }
                }

            }
            else
            {
                // make 8x8 material from palette color
                RGCOLFile palette = LoadPalette(palname);
                Texture2D cur_tex = GraphicsConverter.RGPaletteColorToTexture2D(palette, img);
                MaterialDict.Add(mat_key, new Material(ShaderDict[shadername]));
                MaterialDict[mat_key].mainTexture = cur_tex;

            }
        }

        return MaterialDict[mat_key];
}
    }

    public static Material GetMaterial_BSI(string palname, string bsi)
    {
using(s_get_mat_bsi.Auto()){
        string mat_key = $"{palname}/{bsi}";
        Material o;
        if(MaterialDict.TryGetValue(mat_key, out o))
        {
            return o;
        }
        else
        {
            RGBSIFile bsif = LoadBSI(bsi);
            RGCOLFile palette = LoadPalette(palname);

            List<Texture2D> cur_tex = GraphicsConverter.RGBSIToTexture2D(bsif, palette);

            MaterialDict.Add(mat_key, new Material(Shader.Find("Legacy Shaders/Diffuse Fast")));
            MaterialDict[mat_key].mainTexture = cur_tex[0];

            // note that FRAME_0 does not exist, its called _MainTex or smt
            for(int j=0;j<cur_tex.Count;j++)
            {
                MaterialDict[mat_key].SetTexture($"FRAME_{j}", cur_tex[j]);
            }
        }

        return MaterialDict[mat_key];
}
    }

    private static RGTEXBSIFile LoadTEXBSI(int texbsi)
    {
using(s_load_texbsi.Auto()){
        string texname = $"{texbsi:D3}";
        string path = new string(Game.pathManager.GetArtFolder() + "TEXBSI." + texname);
        return LoadTextureFile(texname, path);
}
    }
    private static RGBSIFile LoadBSI(string bsiname)
    {
using(s_load_bsi.Auto()){
        string texname = $"bsiname";
        string path = new string(Game.pathManager.GetArtFolder() + bsiname + ".BSI");
        RGBSIFile o = new RGBSIFile();
        o.LoadFile(path);

        return o;
}
    }
    public static Material LoadMaterialBSI(string bsiname)
    {
using(s_load_mat_bsi.Auto()){

        string path = new string(Game.pathManager.GetArtFolder() + bsiname + ".BSI");
        RGFileImport.RGBSIFile bsi = new RGFileImport.RGBSIFile();
        bsi.LoadFile(path);
        RGCOLFile pal = LoadPalette("SKY");
        List<Texture2D> cur_tex = GraphicsConverter.RGBSIToTexture2D(bsi, pal);

        Material mat = new Material(Shader.Find("Legacy Shaders/Diffuse Fast"));
        mat.mainTexture = cur_tex[0];

        for(int j=0;j<cur_tex.Count;j++)
        {
            mat.SetTexture($"FRAME_{j}", cur_tex[j]);
        }
        return mat;
}
    }


    private static RGTEXBSIFile LoadTextureFile(string texname, string path)
    {
using(s_load_texf.Auto()){
        RGTEXBSIFile o;

        if(BSIFDict.TryGetValue(texname, out o))
        {
            return o;
        }
        else
        {
            BSIFDict.Add(texname, new RGTEXBSIFile());
            BSIFDict[texname].LoadFile(path);
            return BSIFDict[texname];
        }
}
    }
    private static RGCOLFile LoadPalette(string palname)
    {
        
using(s_load_pal.Auto()){
        RGCOLFile o;
        if(PaletteDict.TryGetValue(palname, out o))
        {
            return o;
        }
        else
        {
            PaletteDict.Add(palname, new RGCOLFile());
            string palpath = Game.pathManager.GetArtFolder() + palname + ".COL";
            // sunset is the only palette in lowercase, thanks todd
            if(palname == "SUNSET")
                palpath = Game.pathManager.GetArtFolder() + "sunset.col";

            PaletteDict[palname].LoadFile(palpath);
            return PaletteDict[palname];
        }
}
    }
}
