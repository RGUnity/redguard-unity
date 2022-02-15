using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PixelateEffect : MonoBehaviour
{

    public float pixelScale = 1;

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //temp RT attributes
        RenderTextureFormat format = RenderTextureFormat.ARGB32;
        int lowresDepthWidth = (int)(source.width / pixelScale);
        int lowresDepthHeight = (int)(source.height / pixelScale);

        //make RT
        RenderTexture lowresRT = RenderTexture.GetTemporary(lowresDepthWidth, lowresDepthHeight, 0, format);

        //point sampling for crispness sake
        lowresRT.filterMode = FilterMode.Point;

        //Blit source to lowres RT
        Graphics.Blit(source, lowresRT);
        //Blit lowres to screen
        Graphics.Blit(lowresRT, destination);

        //release lowres RT from memory
        RenderTexture.ReleaseTemporary(lowresRT);
    }
}