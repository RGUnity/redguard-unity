using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets.Scripts.RGFileImport.RGGFXImport;

public static class RGTexStore
{
    // keys like this:
    // PAL/TEXBSI#/IMG#
    // PAL/-1/COL#
    static Dictionary<string, Material> MaterialDict;
    static Dictionary<string, RGPaletteFile> PaletteDict;
    static Dictionary<string, RGTextureBSIFile> BSIFDict;

    public static string path_to_game;
    static string fxart_path;


    static RGTexStore()
    {
        MaterialDict = new Dictionary<string, Material>();
        PaletteDict = new Dictionary<string, RGPaletteFile>();
        BSIFDict = new Dictionary<string, RGTextureBSIFile>();
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
                RGTextureBSIFile bsif = LoadTEXBSI(texbsi);
                RGPaletteFile palette = LoadPalette(palname);

                List<Texture2D>[] tex_lst_sorted = new List<Texture2D>[bsif.Images.Count];
                for(int i =0;i<bsif.Images.Count;i++)
                {
                    string new_mat_key = $"{palname}/{texbsi:D3}/{Int32.Parse(bsif.Images[i].Name.Substring(3)):D3}";

                    List<Texture2D> cur_tex = GraphicsConverter.RGTextureToTexture2D(bsif.Images[i], palette);

                    MaterialDict.Add(new_mat_key, new Material(Shader.Find("Redguard/Diffuse")));
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
                RGPaletteFile palette = LoadPalette(palname);
                Texture2D cur_tex = GraphicsConverter.RGPaletteColorToTexture2D(palette, img);
                MaterialDict.Add(mat_key, new Material(Shader.Find("Redguard/Diffuse")));
                MaterialDict[mat_key].mainTexture = cur_tex;

            }
        }

        return MaterialDict[mat_key];
    }
    private static RGTextureBSIFile LoadTEXBSI(int texbsi)
    {
        fxart_path = path_to_game + "/fxart/";
        
        string texbsiname = $"{texbsi:D3}";
        RGTextureBSIFile o;
        if(BSIFDict.TryGetValue(texbsiname, out o))
        {
            return o;
        }
        else
        {
            BSIFDict.Add(texbsiname, new RGTextureBSIFile());
            BSIFDict[texbsiname].LoadFile(fxart_path + "TEXBSI." + texbsiname);
            return BSIFDict[texbsiname];
        }
    }
    private static RGPaletteFile LoadPalette(string palname)
    {
        fxart_path = path_to_game + "/fxart/";
        
        RGPaletteFile o;
        if(PaletteDict.TryGetValue(palname, out o))
        {
            return o;
        }
        else
        {
            PaletteDict.Add(palname, new RGPaletteFile());
            PaletteDict[palname].LoadFile(fxart_path + palname + ".COL");
            return PaletteDict[palname];
        }
    }
}
