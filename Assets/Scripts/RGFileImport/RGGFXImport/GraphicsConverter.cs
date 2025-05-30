﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.RGFileImport.RGGFXImport
{
    public class GraphicsConverter
    {
        public static List<Texture2D> RGTextureToTexture2D(RGTextureImage rgTextureImage, RGPaletteFile palette)
        {
            var width = rgTextureImage.Header.Width;
            var height = rgTextureImage.Header.Height;
            var textures = new List<Texture2D>();
            foreach (var image in rgTextureImage.ImageData)
            {
                var texture2d = new Texture2D(width, height, TextureFormat.RGBA32,true);
                byte[] pixels = new byte[width*height*4];
                for (var i = 0; i < image.Length; i++)
                {
                    var rgColor = palette.colors[image[i]];
                    pixels[i*4+0] = rgColor.r;
                    pixels[i*4+1] = rgColor.g;
                    pixels[i*4+2] = rgColor.b;
                    pixels[i*4+3] = 0;
                }
                texture2d.SetPixelData(pixels,0,0);

                texture2d.Apply();
                textures.Add(texture2d);
            }

            return textures;
        }
        public static Texture2D RGPaletteColorToTexture2D(RGPaletteFile palette, int colorid)
        {
            int width = 8;
            int height = 8;
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, true);
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
            texture.Apply();
            return texture;
        }
    }
}
