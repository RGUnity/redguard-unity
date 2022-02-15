using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PixelateEffect : MonoBehaviour
{

    [Range(1, 3)]
    public float pixelScale = 1;
    //public FilterMode fm;
    public bool bilinearFilterEnabled;

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Dynamic Super Resolution (Dividing 1/ pixel scale to match resolution of the RT)
        //ScalableBufferManager.ResizeBuffers(1 / pixelScale, 1 / pixelScale);

        //temp RT attributes
        RenderTextureFormat format = RenderTextureFormat.ARGB32;
        int lowresDepthWidth = (int)(source.width / pixelScale);
        int lowresDepthHeight = (int)(source.height / pixelScale);

        //make RT
        RenderTexture lowresRT = RenderTexture.GetTemporary(lowresDepthWidth, lowresDepthHeight, 0, format);

        //point sampling for crispness sake
        if (bilinearFilterEnabled)
        {
            lowresRT.filterMode = FilterMode.Bilinear;
        }
        else
        {
            lowresRT.filterMode = FilterMode.Point;
        }


        //Blit source to lowres RT
        Graphics.Blit(source, lowresRT);
        //Blit lowres to screen
        Graphics.Blit(lowresRT, destination);

        //release lowres RT from memory
        RenderTexture.ReleaseTemporary(lowresRT);


    }
}