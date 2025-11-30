using UnityEngine;
using System.Collections.Generic;
using RGFileImport;

namespace Assets.Scripts.RGFileImport.RGGFXImport
{
    public class GraphicsConverter
    {
        public static Texture2D RGPaletteColorToTexture2D(RGCOLFile palette, int colorid)
        {
            int width = 8;
            int height = 8;
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            byte[] pixels = new byte[width*height*4];

            for (int i=0; i<width*height;i++)
            {
                var rgColor = palette.colors[colorid];
                pixels[i*4+0] = rgColor.r;
                pixels[i*4+1] = rgColor.g;
                pixels[i*4+2] = rgColor.b;
                pixels[i*4+3] = 0;
            }

            texture.SetPixelData(pixels,0,0);
            // Disable pixel interpolation
            //texture.filterMode = FilterMode.Point;
            //texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();
            return texture;
        }
        public static List<List<Texture2D>> RGTEXBSIToTexture2D(RGTEXBSIFile texbsi, RGCOLFile palette)
        {
            List<List<Texture2D>> textures = new List<List<Texture2D>>();
            for (int i=0; i<texbsi.images.Count;i++)
            {
                textures.Add(RGBSIToTexture2D(texbsi.images[i].imageData, palette));
            }

            return textures;
        }
        public static List<Texture2D> RGBSIToTexture2D(RGBSIFile bsi, RGCOLFile palette)
        {
            int width = (int)bsi.BHDR.width;
            int height = (int)bsi.BHDR.height;
            List<Texture2D> textures = new List<Texture2D>();
            byte[] pixels = new byte[width*height*4];

            for (int f=0; f<bsi.BHDR.frameCount;f++)
            {
                Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
                for (int i=0; i<width*height;i++)
                {
                    if(bsi.DATA.data[f][i] == 0) // assuming 0 is transparent
                    {
                        pixels[i*4+0] = 0;
                        pixels[i*4+1] = 0;
                        pixels[i*4+2] = 0;
                        pixels[i*4+3] = 0;
                    }
                    else
                    {
                        var rgColor = palette.colors[bsi.DATA.data[f][i]];
                        pixels[i*4+0] = rgColor.r;
                        pixels[i*4+1] = rgColor.g;
                        pixels[i*4+2] = rgColor.b;
                        pixels[i*4+3] = 255;
                    }
                }
                texture.SetPixelData(pixels,0,0);
                // Disable pixel interpolation
                //texture.filterMode = FilterMode.Point;
                //texture.wrapMode = TextureWrapMode.Clamp;
                texture.Apply();
                textures.Add(texture);
            }

            return textures;
        }
        public static List<Texture2D> RGGXAToTexture2D(RGGXAFile gxa)
        {
            List<Texture2D> textures = new List<Texture2D>();

            for (int f=0; f<gxa.BMHD.numImages;f++)
            {
                int width = gxa.BBMP.BBMPItems[f].width;
                int height = gxa.BBMP.BBMPItems[f].height;

                byte[] pixels = new byte[width*height*4];
                // flip width and height here to rotate image upright
                Texture2D texture = new Texture2D(height, width, TextureFormat.RGBA32, false);
                for (int y=0; y<height;y++)
                {
                    for (int x=0; x<width;x++)
                    {
                        int i = x+(y*width);
                        if(gxa.BBMP.BBMPItems[f].data[i] == 0) // assuming 0 is transparent
                        {
                            // flip width and height here
                            int ti = y+(x*height);
                            pixels[ti*4+0] = 0;
                            pixels[ti*4+1] = 0;
                            pixels[ti*4+2] = 0;
                            pixels[ti*4+3] = 0;
                        }
                        else
                        {
                            var bpalColor = gxa.BPAL.colors[gxa.BBMP.BBMPItems[f].data[i]];
                            // flip width and height here
                            int ti = y+(x*height);
                            pixels[ti*4+0] = bpalColor.r;
                            pixels[ti*4+1] = bpalColor.g;
                            pixels[ti*4+2] = bpalColor.b;
                            pixels[ti*4+3] = 255;
                        }
                    }
                }
                texture.SetPixelData(pixels,0,0);
                // Disable pixel interpolation
                //texture.filterMode = FilterMode.Point;
                //texture.wrapMode = TextureWrapMode.Clamp;
                texture.Apply();
                textures.Add(texture);
            }

            return textures;
        }

    }
}
