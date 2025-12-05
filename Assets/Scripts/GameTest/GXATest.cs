using System.Collections.Generic;
using System.IO;
using UnityEngine;
using RGFileImport;

public class GXATest : MonoBehaviour
{
    List<Material> mats;
    List<Texture2D> textures;
    int curchar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // SFX
        RGSoundStore.LoadSFX("MAIN");
//dmpall();
        curchar = 0;

        RGFNTFile fntFile = new RGFNTFile();
        fntFile.LoadFile("~/redguard-unity/game_3dfx/fonts/REDGUARD.FNT");
        textures = Assets.Scripts.RGFileImport.RGGFXImport.GraphicsConverter.RGFNTToTexture2D(fntFile);

        Material mat = new Material(Shader.Find("Universal Render Pipeline/Simple Lit"));
        mat.mainTexture = textures[0];

        mats = new List<Material>() {mat};
/*
        Material mat = RGTexStore.GetMaterial_GXA("MAPMAP",0);
*/
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.SetMaterials(mats);

        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp("space"))
        {
            AudioSource ar = GetComponent<AudioSource>();
            ar.clip = RGSoundStore.GetSFX(117);
            ar.Play();
        }
        if(false)
        {
            curchar++;
            if(curchar >= textures.Count)
                curchar = 0;
            mats[0].mainTexture = textures[curchar];
            MeshRenderer mr = GetComponent<MeshRenderer>();
            mr.SetMaterials(mats);

            Debug.Log($"Loaded char {curchar}: {(char)curchar}");
        }
    }


    List<string> files = new List<string>(){
"ARIALBG.FNT",
"ARIALMD.FNT",
"ARIALSB.FNT",
"ARIALSM.FNT",
"ARIALVB.FNT",
"ARIALVS.FNT",
"FONTNORM.FNT",
"FONTNORS.FNT",
"FONTPALE.FNT",
"FONTRED.FNT",
"FONTSEL.FNT",
"FONTSELS.FNT",
"HIDONE.FNT",
"HINORMAL.FNT",
"HISELECT.FNT",
"LODONE.FNT",
"LONORMAL.FNT",
"LOSELECT.FNT",
"LOWFONT.FNT",
"REDDARK.FNT",
"REDGUARD.FNT",
"REDLOW.FNT",
"REDNORM.FNT",
"REDONE.FNT",
"REDRED.FNT",
"REDSEL.FNT",
"SREDDARK.FNT",
"SREDNORM.FNT",
"SREDSEL.FNT",
    };
    void dmpall()
    {

        for(int i=0;i<files.Count;i++)
        {
            RGFileImport.RGFNTFile fntFile = new RGFileImport.RGFNTFile();
            fntFile.LoadFile($"./game_3dfx/fonts/{files[i]}");
            textures = Assets.Scripts.RGFileImport.RGGFXImport.GraphicsConverter.RGFNTToTexture2D(fntFile);
            for(int j=0;j<textures.Count;j++)
            {
                dump_to_png(textures[j], files[i], j);
            }
        }

    }

    void dump_to_png(Texture2D tex, string f, int c)
    {
        byte[] buff = ImageConversion.EncodeToPNG(tex);
        string f2 = f.Substring(0,f.IndexOf("."));
        File.WriteAllBytes($"./io_tst/textst/fnt/{f2}/{f2}_{c}.png", buff);
    }
}
