using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.RGFileImport.RGGFXImport
{
    public class GraphicsConverter
    {
        public static List<Texture2D> RGTextureToTexture2D(RGTextureImage rgTextureImage)
        {
            var width = rgTextureImage.Header.Width;
            var height = rgTextureImage.Header.Width;
            var palette = rgTextureImage.Palette;
            var textures = new List<Texture2D>();
            foreach (var image in rgTextureImage.ImageData)
            {
                var texture2d = new Texture2D(width, height);
                for (var i = 0; i < image.Length; i++)
                {
                    var rgColor = palette[image[i]];
                    var color = new Color(rgColor.r / 255f, rgColor.g / 255f, rgColor.b / 255f);
                    texture2d.SetPixel(i % width, i / height, color);
                }

                texture2d.Apply();
                textures.Add(texture2d);
            }

            return textures;
        }

    }
}
