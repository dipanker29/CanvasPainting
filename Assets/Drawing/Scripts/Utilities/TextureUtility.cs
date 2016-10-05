using UnityEngine;
using System.Collections;

namespace Drawing.Util
{
    public static class TextureUtility
    {
        /// <summary>
        /// Copies the given texture to new textures.
        /// </summary>
        /// <returns>The Texture2D.</returns>
        /// <param name="from">Texture2D Source texture.</param>
        public static Texture2D CopyTexture (Texture2D from)
        {
            Texture2D texture = new Texture2D(from.width, from.height);
            texture.SetPixels(from.GetPixels());
            texture.Apply();

            return texture;
        }
    }
}
