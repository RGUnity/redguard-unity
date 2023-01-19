using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.RGFileImport.RGGFXImport;
public enum RGTexture
{
    

}
public enum WorldOrientation
{
    YUp,YDown,ZUp,ZDown,XUp,XDown
}
[System.Serializable]
public enum RGFiles
{
    AVIKA001,
    BASIA001,
    BAT,
    BEAMA001,
    BLOBA001,
    BMANA001,
    BULLA001,
    BWAGA001,
    CANAA001,
    CANAA002,
    CLAVA001,
    COYLA001,
    COYLA002,
    CRENA001,
    CV_EXPL1,
    CV_FISH,
    CV_MUSH1,
    CV_MUSH2,
    CV_ROPE,
    CV_SKUL3,
    CVFTL001,
    CYRSA001,
    CYRSA002,
    CYRSA003,
    CYRSA004,
    CYRSA005,
    CYRSA006,
    CYRSA007,
    CYRSA008,
    CYRSA009,
    CYRSA010,
    CYRSA011,
    CYRSA012,
    CYRSA013,
    CYRSA014,
    CYRSA015,
    CYRSA016,
    CYRSA017,
    CYRSA018,
    CYRSA019,
    CYRSA020,
    CYRSA021,
    CYRSA022,
    CYRSA023,
    DEAD,
    DGOLA001,
    DOGA001,
    DRAMA001,
    DRAMA002,
    DRAMA003,
    DREKA001,
    DR_SPIKE,
    EFCTA001,
    ERASA001,
    FALIA001,
    FIDOA001,
    FLAGA01,
    FVPRA001,
    FVPRA002,
    GARDA001,
    GARDA002,
    GARDA003,
    GARDA004,
    GBLNA001,
    GBLNA002,
    GBLNA003,
    GERRA001,
    GOATA001,
    GOLMA001,
    GOLMA002,
    GREMA001,
    GRRKA001,
    HEADA001,
    ISZAA001,
    JAGAA001,
    JFFRA001,
    JOTOA001,
    JOTOA002,
    JOTOA003,
    KANA001,
    KOTAA001,
    LAKEA001,
    LASRA001,
    LHMBA001,
    MAKOA001,
    NFARA001,
    NFARA002,
    NFARA003,
    NGASA001,
    NGASA002,
    NIDAA001,
    OB_TEL05,
    OB_TEL06,
    OGREA001,
    OGREA002,
    OXA001,
    PIGA001,
    PRISA001,
    PRRTA001,
    RICHA001,
    RICHA002,
    ROBOA001,
    ROBOA002,
    ROBOA003,
    SABAA001,
    SABAA002,
    SABAA003,
    SEAGA001,
    SEAGULL,
    SEALOW,
    SERPA001,
    SKELA001,
    SKELA002,
    SKELA003,
    SKELA004,
    SNAKA01,
    SWAPA001,
    TENTA001,
    THUGA001,
    THUGA002,
    THUGA003,
    THUGA004,
    THUGA005,
    TIRBA001,
    TIRBA002,
    TOBIAS,
    TOBIAS_A,
    TOBYA001,
    TOMJONES,
    TROLA001,
    TROLA002,
    TROLA003,
    TS_RP,
    TUFFA001,
    VANDA001,
    VANDA002,
    VANDA003,
    VERMA001,VERMA002,
    VILEGARD,
    VR_DOOR,
    VULTA,
    WOMEA001,XBOWA001,YAELA001,ZOMBA001,ZOMBA002


}
// [RequireComponent(typeof(MeshFilter))]
// [RequireComponent(typeof(MeshRenderer))]
public class ModelUnityReader : MonoBehaviour
{
    public void SetScale(float newScale)
    {
        scaleSize = newScale;
        if(modelView!=null) modelView.transform.localScale = Vector3.one*newScale;
    }
    #region 3D Model Loading
    [SerializeField] WorldOrientation worldDirection;
    [SerializeField] string filePath;
    [SerializeField] RGFiles fileToLoad;
    [SerializeField] string extension;
    // Start is called before the first frame update
    public RGFiles CurrentFile => fileToLoad;
    
    #region Currently viewed model (Debug/Viewer)
    GameObject modelView;
    Mesh mesh;
    MeshRenderer mren; 
    #endregion
    #region Mesh Data
    [SerializeField] Vector3[] vertices;
    [SerializeField] int[] triangles;
    [SerializeField] Vector3[] normals;
    [SerializeField] Vector2[] uvs;
    RGFileImport.RG3DFile file3d;
    #endregion
    #region Debug
    [SerializeField] Material placeholderMat;
    public GameObject CurrentTestModel => modelView;
    #endregion
    [SerializeField] float scaleSize;
    public void SetModel(/*GameObject target,*/RGFiles nextFile)
    {
        if(modelView!=null) 
            Destroy(modelView.gameObject);
        modelView = new GameObject("Display Model : "+nextFile.ToString());
        modelView.transform.localScale = new Vector3(scaleSize,scaleSize,scaleSize);
        
        //Load in file from game
        file3d = new RGFileImport.RG3DFile(); 
        file3d.LoadFile(filePath+fileToLoad.ToString()+extension);
        fileToLoad.ToString();
        Debug.Log("Attempted to load "+filePath + fileToLoad.ToString()+extension);        
        if (file3d==null) 
        {
            Debug.Log("The file "+fileToLoad.ToString()+extension+" could not be loaded.");
            return;
            }

        //Create Unity mesh and map data to it
        mesh = new Mesh();
        mren = modelView.AddComponent<MeshRenderer>(); 
        modelView.AddComponent<MeshFilter>().mesh = mesh;
        PrintToScreen();
        UpdateMesh();
        CreateShape();        
        PrintToScreen(true);

    }
    void CreateShape()
    {
        //Convert int coordinates to Vector3 with floats
        Vector3[] newCoord = new Vector3[file3d.VertexCoordinates.Count];
        for(int i = 0; i < file3d.VertexCoordinates.Count; i++)
        {
            //todo: switch(worldOrientation)
            switch(worldDirection)
            {
                case WorldOrientation.YUp:
                    newCoord[i].x = file3d.VertexCoordinates[i].x;
                    newCoord[i].y = file3d.VertexCoordinates[i].y;
                    newCoord[i].z = file3d.VertexCoordinates[i].z;
                break;
            }
        }
        vertices = newCoord;
        
        
        //Calculate triangle loop from the vertices above
        Debug.Log("Starting triangle loop");
        triangles = new int[newCoord.Length*3];
        for (int i = 0; i < (triangles.Length / 6)-1; i++) {
                triangles[i * 6 + 0] = i * 2 + triPattern[0];
                triangles[i * 6 + 1] = i * 2 + triPattern[1];
                triangles[i * 6 + 2] = i * 2 + triPattern[2];
     
                triangles[i * 6 + 3] = i * 2 + triPattern[3];
                triangles[i * 6 + 4] = i * 2 + triPattern[4];
                triangles[i * 6 + 5] = i * 2 + triPattern[5];   
        }

        
        Vector3[] newNorm = new Vector3[file3d.FaceNormals.Count];
        for(int i = 0; i < file3d.FaceNormals.Count; i++)
        {
            switch(worldDirection)
            {
                case WorldOrientation.YUp:
                    newNorm[i].x = file3d.FaceNormals[i].x;
                    newNorm[i].y = file3d.FaceNormals[i].y;
                    newNorm[i].z = file3d.FaceNormals[i].z;
                break;
            }
        }
        normals = newNorm;
        Debug.Log(file3d.UvCoordinates.ToString());
        Vector3[] newUV = new Vector3[file3d.VertexCoordinates.Count];
        for(int i = 0; i < file3d.UvOffsets.Count; i++)
        {
            //todo: switch(worldOrientation)
            switch(worldDirection)
            {
                case WorldOrientation.YUp:
                    newUV[i].x = file3d.UvCoordinates[i].x;
                    newUV[i].y = file3d.UvCoordinates[i].y;
                    newUV[i].z = file3d.UvCoordinates[i].z;
                break;
            }
        }
        PlaceholderUV = newUV;

    }
    [SerializeField] int[] triPattern = new int[]{0,1,2,2,1,3}; 
    [SerializeField] Vector3[] PlaceholderUV;
    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        // mesh.SetUVs(0,uvs);
        if(autoNorm)
            mesh.RecalculateNormals();
        else 
            mesh.normals = normals; 
        mren.material = placeholderMat;
    }
    #endregion
    void Start()
    {
        SetModel(fileToLoad);
        MakeMat(texToLoad);
    }
    RGTextureBSIFile texFile;
    public void MakeMat(string texToLoad)
    {
        
        string loc = filePath + texToLoad;
        texFile = new RGTextureBSIFile();
        texFile.LoadFile(loc);  
        GraphicsConverter.RGTextureToTexture2D(texFile.Images[0]);
        //Convert image textures from texFile.Images;
    }
    [SerializeField] string texToLoad;
    #region Debug Info (PlayMode)
    [SerializeField] TMPro.TMP_Text DebugConsole;
    void PrintToScreen(bool verbose = false)
    {
        if(DebugConsole!=null)
        {   string DebugText = string.Empty;
            DebugText+="Model loaded: "+fileToLoad.ToString()+extension+"\n";
            DebugText+="3DC Version: "+file3d.ModelVersion.ToString()+"\n";
            if(verbose)
            {
                DebugText+="Verts: "+file3d.VertexCoordinates.Count+"\n";
                // DebugText+="UVs: "+file3d.UvCoordinates.Count;
            }
            
            DebugConsole.SetText(DebugText);
        }
    }
    #endregion
    [SerializeField] bool autoNorm = true;
    // Update is called once per frame
    void Update()
    {
        
    }
}
