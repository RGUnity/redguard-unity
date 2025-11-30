using System.Collections.Generic;
using UnityEngine;

public class GXATest : MonoBehaviour
{
    List<string> GXAfiles;
    int curmat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GXAfiles = new List<string>() {
"CLOUD512",
"GREY256",
"GUI",
"NECROSKY",
"NIGHTSKY",
"PAPER",
"PICKBLOB",
"PICKBOX",
"RGMAP",
"SKY61",
"SNUFF",
"SQUARE",
"CATACOMB",
"CAVERNS",
"COMPASS2",
"CROSS",
"DRINT",
"DUSK",
"GXICONS",
"INVARROW",
"INVBACK",
"INVBORDR",
"INVMASK",
"INVSEL",
"ISLAND1",
"ISLAND",
"LOGPAPER",
"MAPMAP",
"MINIMENU",
"MIRROR1",
"MIRROR2",
"MIRROR3",
"MM_CHECK",
"MM_MOVIE",
"MM_SCRSZ",
"MM_SLIDE",
"NECRISLE",
"NECRTOWR",
"OBSERVE",
"PICKUPS",
"PICKUPSS",
"POWERUP",
"SCROLL",
"STARTUP2",
"STMAP01",
"STMAP02",
"STMAP03",
"STMAP04",
"STMAP05",
"STMAP06",
"STMAP07",
"STMAP08",
"STMAP09",
"STMAP10",
"STMAP11",
"STMAP12",
"STMAP13",
"STMAP14",
"STMAP15",
"STMAP16",
"STMAP17",
"SUNSET",
"TAVERN",
"TEMPLE",
"VIEWBACK",
        };
        curmat = 0;
        Material mat = RGTexStore.GetMaterial_GXA("MAPMAP",0);
        List<Material> mats = new List<Material>() {mat};
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.SetMaterials(mats);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp("space"))
        {
            curmat++;
            if(curmat >= GXAfiles.Count)
                curmat = 0;
            Material mat = RGTexStore.GetMaterial_GXA(GXAfiles[curmat],0);
            List<Material> mats = new List<Material>() {mat};
            MeshRenderer mr = GetComponent<MeshRenderer>();
            mr.SetMaterials(mats);
            Debug.Log($"Loaded GXA {GXAfiles[curmat]}");
        }
    }
}
