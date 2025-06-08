using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets.Scripts.RGFileImport.RGGFXImport;
using RGFileImport;

public static class RGTexStore
{
    // keys like this:
    // PAL/TEXBSI#/IMG#
    // PAL/-1/COL#
    static Dictionary<string, Material> MaterialDict;
    static Dictionary<string, RGCOLFile> PaletteDict;
    static Dictionary<string, RGTEXBSIFile> BSIFDict;

    public static string path_to_game = "./game_3dfx";
    static string fxart_path;


    static RGTexStore()
    {
        MaterialDict = new Dictionary<string, Material>();
        PaletteDict = new Dictionary<string, RGCOLFile>();
        BSIFDict = new Dictionary<string, RGTEXBSIFile>();
    }

    public static Material GetMaterial(string palname, int texbsi, int img)
    {
        fxart_path = path_to_game + "/fxart/";
        
        string mat_key = $"{palname}/{texbsi:D3}/{img:D3}";
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
                    string new_mat_key = $"{palname}/{texbsi:D3}/{Int32.Parse(bsif.images[i].imageName.Substring(3)):D3}";

                    List<Texture2D> cur_tex = GraphicsConverter.RGBSIToTexture2D(bsif.images[i].imageData, palette);

                    MaterialDict.Add(new_mat_key, new Material(Shader.Find("Legacy Shaders/Diffuse Fast")));
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
                MaterialDict.Add(mat_key, new Material(Shader.Find("Legacy Shaders/Diffuse Fast")));
                MaterialDict[mat_key].mainTexture = cur_tex;

            }
        }

        return MaterialDict[mat_key];
    }

    public static Material GetMaterial_BSI(string palname, string bsi)
    {
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

            string new_mat_key = $"{palname}/{bsi}/{000}";
            List<Texture2D> cur_tex = GraphicsConverter.RGBSIToTexture2D(bsif, palette);

            MaterialDict.Add(new_mat_key, new Material(Shader.Find("Legacy Shaders/Diffuse Fast")));
            MaterialDict[new_mat_key].mainTexture = cur_tex[0];

            // note that FRAME_0 does not exist, its called _MainTex or smt
            for(int j=0;j<cur_tex.Count;j++)
            {
                MaterialDict[new_mat_key].SetTexture($"FRAME_{j}", cur_tex[j]);
            }
        }

        return MaterialDict[mat_key];
    }

    private static RGTEXBSIFile LoadTEXBSI(int texbsi)
    {
        string texname = $"{texbsi:D3}";
        string path = new string(fxart_path + "TEXBSI." + texname);
        return LoadTextureFile(texname, path);
    }
    private static RGBSIFile LoadBSI(string bsiname)
    {
        string texname = $"bsiname";
        string path = new string(fxart_path + bsiname + ".BSI");
        RGBSIFile o = new RGBSIFile();
        o.LoadFile(path);

        return o;
    }
    public static Material LoadMaterialBSI(string bsiname)
    {
        string path = new string(fxart_path + bsiname + ".BSI");
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


    private static RGTEXBSIFile LoadTextureFile(string texname, string path)
    {
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
    private static RGCOLFile LoadPalette(string palname)
    {
        fxart_path = path_to_game + "/fxart/";
        
        RGCOLFile o;
        if(PaletteDict.TryGetValue(palname, out o))
        {
            return o;
        }
        else
        {
            PaletteDict.Add(palname, new RGCOLFile());
            PaletteDict[palname].LoadFile(fxart_path + palname + ".COL");
            return PaletteDict[palname];
        }
    }
}
