using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PixelateEffect : MonoBehaviour
{

    [Range(1, 3)]
    public int pixelScale = 1;
    public bool bilinearFilterEnabled;
    
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int lowresW = (int)(source.width  / pixelScale);
        int lowresH = (int)(source.height / pixelScale);
        
        RenderTexture lowresRT = RenderTexture.GetTemporary(
            lowresW, 
            lowresH, 
            0, 
            RenderTextureFormat.ARGB32
        );
        lowresRT.filterMode = bilinearFilterEnabled 
            ? FilterMode.Bilinear 
            : FilterMode.Point;
        lowresRT.wrapMode   = TextureWrapMode.Clamp;
        
        FilterMode originalSourceMode = source.filterMode;
        source.filterMode = FilterMode.Point;
        
        Graphics.Blit(source, lowresRT);
        
        source.filterMode = originalSourceMode;
        
        Graphics.Blit(lowresRT, destination);
        
        RenderTexture.ReleaseTemporary(lowresRT);
    }
}