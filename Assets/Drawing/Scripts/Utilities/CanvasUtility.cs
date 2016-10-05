using UnityEngine;
using System.Collections;

namespace Drawing.Util
{
    public static class CanvasUtility
    {
        public static Sprite GetSprite (Texture2D texture)
        {
            return UnityEngine.Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        public static void ScreenPointToRectPosition (this UnityEngine.RectTransform rectTransform, Vector2 screenPosition, float screenRatio = 1f)
        {
            
            int width = Screen.width / 2;
            int height = Screen.height / 2;

            float xPos = (screenPosition.x - (float)width) / screenRatio;
            float yPos = (screenPosition.y - (float)height) / screenRatio;

            rectTransform.anchoredPosition = new Vector2(xPos, yPos);
        }
    }
}
