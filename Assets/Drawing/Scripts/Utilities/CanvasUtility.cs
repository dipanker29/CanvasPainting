using UnityEngine;
using System.Collections;

namespace Drawing.Util
{
    public static class CanvasUtility
    {
        public static void CreateSprite (this UnityEngine.Sprite sprite, Texture2D texture)
        {
            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        public static void ScreenPointToRectPosition (this UnityEngine.RectTransform rectTransform, Vector2 screenPosition)
        {
            rectTransform.anchoredPosition = new Vector2(screenPosition.x - ((float)(Screen.width / 2)), screenPosition.y - ((float)(Screen.height/2)));
        }
    }
}
