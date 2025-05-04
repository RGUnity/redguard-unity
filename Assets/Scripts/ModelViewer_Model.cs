using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets.Scripts.RGFileImport;

using Assets.Scripts.RGFileImport.RGGFXImport;
public class ModelViewer_Model : MonoBehaviour
{
    Mesh mesh;
    MeshRenderer mren;
    public void SetModel_wld(/*GameObject target,*/)
    {
        // FILL IN
        string filename_wld = new string("./game_3dfx/maps/ISLAND.WLD");

        // ISLAND.WLD
        mesh = RG2Mesh.WLD2Mesh(filename_wld);
        mren = new MeshRenderer();
        GetComponent<MeshFilter>().mesh = mesh;

        // TEXBSI.302
        string filename_texbsi = new string("./game_3dfx/fxart/TEXBSI.302");

        List<Material> materials = new List<Material>();
        List<Texture2D> textures = LoadTexture(filename_texbsi);
        for(int i =0;i<textures.Count;i++)
        {
            materials.Add(new Material(Shader.Find("Legacy Shaders/Diffuse Fast")));
            materials[materials.Count-1].mainTexture = textures[i];
        }
        GetComponent<MeshRenderer>().SetMaterials(materials);

        //UpdateMesh();
    }
    private List<Texture2D> LoadTexture(string filename)
    {
        List<Texture2D> tex_lst = new List<Texture2D>();
        // todo: palette
        RGPaletteFile.RGColor[] palette = RGPaletteFile.Load("./game_3dfx/fxart/ISLAND.COL");
        RGTextureBSIFile bsif = new RGTextureBSIFile(palette);

        bsif.LoadFile(filename);

        for(int i =0;i<bsif.Images.Count;i++)
        {
            tex_lst.Add(GraphicsConverter.RGTextureToTexture2D(bsif.Images[i])[0]);
        }

        return tex_lst;
    }
   // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetModel_wld(/*GameObject target,*/);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
