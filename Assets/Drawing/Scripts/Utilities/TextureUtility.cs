using UnityEngine;
using System.Collections;

namespace Drawing.Util
{
    public static class TextureUtility
    {
        public static void CopyTexture (this UnityEngine.Texture2D texture, Texture2D from)
        {
            texture = new Texture2D(from.width, from.height);
            texture.SetPixels(from.GetPixels());
            texture.Apply();
        }
    }
}
