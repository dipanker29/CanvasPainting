using UnityEngine;
using System.Collections;

namespace Drawing
{
    public class PaintTexture
    {
        /// <summary>
        /// Gets the cordinate of texture point.
        /// </summary>
        /// <returns>The texture cordinate.</returns>
        /// <param name="textureObjectPosition">Vector2 Screen position of image.</param>
        /// <param name="pointerPosition">Vector2 Touch scren postition.</param>
        /// <param name="screenRatio">float Screen Aspect ratio.</param>
        /// <param name="texture">Texture.</param>
        /// <param name="rotation">float Z Rotation of image.</param>
        public static Vector2 GetTextureCordinate (Vector2 textureObjectPosition, Vector2 pointerPosition, float screenRatio, Texture2D texture, float rotation)
        {
            Vector2 textureCordinates = Vector2.zero;
            
            float textureCordX = textureObjectPosition.x - (GetTextureSize(texture, screenRatio).x / 2f);
            float textureCordy = textureObjectPosition.y - (GetTextureSize(texture, screenRatio).y / 2f);
            
            float pointerTextureCordX = pointerPosition.x - textureCordX;
            float pointerTextureCordY = pointerPosition.y - textureCordy;
            
            textureCordinates.x = pointerTextureCordX / screenRatio;
            textureCordinates.y = pointerTextureCordY / screenRatio;
            
            //Rotation
            rotation = (rotation / -57f);
            Vector2 textureMiddlePoint = new Vector2(((float)texture.width / 2f), ((float)texture.height / 2f));
            float newX = ((textureCordinates.x - textureMiddlePoint.x) * Mathf.Cos(rotation) -
                (textureCordinates.y - textureMiddlePoint.y) * Mathf.Sin(rotation) + textureMiddlePoint.x);
            float newY = ((textureCordinates.x - textureMiddlePoint.x) * Mathf.Sin(rotation) + 
                (textureCordinates.y - textureMiddlePoint.y) * Mathf.Cos(rotation) + textureMiddlePoint.y);
            
            textureCordinates = new Vector2(newX, newY);
            
            return textureCordinates;
            
        }
        
        //Return size of texture according to aspect ration and scale value...
        private static Vector2 GetTextureSize (Texture2D texture, float screenRatio)
        {
            Vector2 textureSize = Vector2.zero;
            
            float textureWidth = (float)texture.width * screenRatio;
            float textureHeight = (float)texture.height * screenRatio;
            
            textureSize.x = textureWidth;
            textureSize.y = textureHeight;
            
            return textureSize;
        }
        
        /// <summary>
        /// Paint the specified Texture with given color.
        /// Can ignore alpha value, if alpha value is 0 then color wan't affect.
        /// </summary>
        /// <param name="from">Vector2 Start position.</param>
        /// <param name="to">Vector2 End position.</param>
        /// <param name="rad">float Radius area, brush size.</param>
        /// <param name="col">Color to replace the texture with.</param>
        /// <param name="hardness">float opacity of texture color.</param>
        /// <param name="tex">Texture2D texture where color to be paint.</param>
        /// <param name="ignoreAlpha">If set to <c>true</c> ignore the alpha 0 part.</param>
        public static Texture2D Paint(Vector2 from, Vector2 to, float rad, Color col, float hardness, Texture2D tex, bool ignoreAlpha = true){
            var extent = rad;
            var stY = Mathf.Clamp(Mathf.Min(from.y, to.y) - extent, 0, tex.height);
            var stX = Mathf.Clamp(Mathf.Min(from.x, to.x) - extent, 0, tex.width);
            var endY = Mathf.Clamp(Mathf.Max(from.y, to.y) + extent, 0, tex.height);
            var endX = Mathf.Clamp(Mathf.Max(from.x, to.x) + extent, 0, tex.width);
            
            var lengthX = endX - stX;
            var lengthY = endY - stY;
            
            var sqrRad2 = (rad + 1) * (rad + 1);
            Color[] pixels = tex.GetPixels((int)stX, (int)stY, (int)lengthX, (int)lengthY, 0);
            var start = new Vector2(stX, stY);
            
            for (int y = 0; y < (int)lengthY; y++){
                
                for (int x = 0; x < (int)lengthX; x++){
                    
                    var p = new Vector2(x, y) + start;
                    var center = p + new Vector2(0.5f, 0.5f);
                    float dist = (center - PaintMathfx.NearestPointStrict(from, to, center)).sqrMagnitude;
                    if (dist > sqrRad2){
                        continue;
                    }
                    dist = PaintMathfx.GaussFalloff(Mathf.Sqrt(dist), rad) * hardness;
                    
                    Color c;
                    if (dist > 0){
                        c = Color.Lerp(pixels[y * (int)lengthX + x], col, dist);
                    }
                    else{
                        c = pixels[y * (int)lengthX + x];
                    }
                    
                    if (ignoreAlpha == false)
                    {
                        c.a = pixels[y * (int)lengthX + x].a;
                    }
                    
                    pixels[y * (int)lengthX + x] = c;
                }
            }
            
            tex.SetPixels((int)start.x, (int)start.y, (int)lengthX, (int)lengthY, pixels, 0);
            return tex;
        }
        
        /// <summary>
        /// Erase the specified Texture making alpha 0.
        /// </summary>
        /// <param name="from">Vector2 Start position.</param>
        /// <param name="to">Vector2 End position.</param>
        /// <param name="rad">float Radius area, eraser size.</param>
        /// <param name="hardness">float opacity of texture color.</param>
        /// <param name="tex">Texture2D texture where color to be paint.</param>
        public static Texture2D Erase(Vector2 from, Vector2 to, float rad, float hardness, Texture2D tex){
            var extent = rad;
            var stY = Mathf.Clamp(Mathf.Min(from.y, to.y) - extent, 0, tex.height);
            var stX = Mathf.Clamp(Mathf.Min(from.x, to.x) - extent, 0, tex.width);
            var endY = Mathf.Clamp(Mathf.Max(from.y, to.y) + extent, 0, tex.height);
            var endX = Mathf.Clamp(Mathf.Max(from.x, to.x) + extent, 0, tex.width);
            
            var lengthX = endX - stX;
            var lengthY = endY - stY;
            
            var sqrRad2 = (rad + 1) * (rad + 1);
            Color[] pixels = tex.GetPixels((int)stX, (int)stY, (int)lengthX, (int)lengthY, 0);
            var start = new Vector2(stX, stY);
            
            for (int y = 0; y < (int)lengthY; y++){
                
                for (int x = 0; x < (int)lengthX; x++){
                    
                    var p = new Vector2(x, y) + start;
                    var center = p + new Vector2(0.5f, 0.5f);
                    float dist = (center - PaintMathfx.NearestPointStrict(from, to, center)).sqrMagnitude;
                    if (dist > sqrRad2){
                        continue;
                    }
                    dist = PaintMathfx.GaussFalloff(Mathf.Sqrt(dist), rad) * hardness;
                    
                    Color c;
                    if (dist > 0){
                        c = Color.Lerp(pixels[y * (int)lengthX + x], pixels[y * (int)lengthX + x], dist);
                    }
                    else{
                        c = pixels[y * (int)lengthX + x];
                    }
                    
                    c.a = 0;
                    pixels[y * (int)lengthX + x] = c;
                }
            }
            
            tex.SetPixels((int)start.x, (int)start.y, (int)lengthX, (int)lengthY, pixels, 0);
            return tex;
        }
        
        /// <summary>
        /// Patterns the paint of texture.
        /// </summary>
        /// <returns>The paint.</returns>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="rad">RAD.</param>
        /// <param name="hardness">Hardness.</param>
        /// <param name="tex">Tex.</param>
        /// <param name="pattern">Pattern.</param>
        public static Texture2D PatternPaint(Vector2 from, Vector2 to, float rad, float hardness, Texture2D tex, Texture2D pattern)
        {
            
            float extentX = (float)(pattern.width/2);
            float extentY = (float)(pattern.height/2);
            
            float stY = Mathf.Clamp(Mathf.Min(from.y, to.y) - extentY, 0, tex.height);
            float stX = Mathf.Clamp(Mathf.Min(from.x, to.x) - extentX, 0, tex.width);
            float endY = Mathf.Clamp(Mathf.Max(from.y, to.y) + extentY, 0, tex.height);
            float endX = Mathf.Clamp(Mathf.Max(from.x, to.x) + extentX, 0, tex.width);
            
            int lengthX = Mathf.RoundToInt(endX - stX);
            int lengthY = Mathf.RoundToInt(endY - stY);
            
            if (lengthX > pattern.width)
                lengthX = pattern.width;
            if (lengthY > pattern.height)
                lengthY = pattern.height;
            
            float sqrRad2 = (extentX + 1) * (extentY + 1);
            
            Color[] pixels = tex.GetPixels((int)stX, (int)stY, (int)lengthX, (int)lengthY, 0);
            Color[] patPixels = pattern.GetPixels(0, 0, lengthX, lengthY);
            
            var start = new Vector2(stX, stY);
            
            for (int y = 0; y < (int)lengthY; y++){
                
                for (int x = 0; x < (int)lengthX; x++){
                    
                    var p = new Vector2(x, y) + start;
                    var center = p + new Vector2(0.5f, 0.5f);
                    float dist = (center - PaintMathfx.NearestPointStrict(from, to, center)).sqrMagnitude;
                    if (dist > sqrRad2){
                        continue;
                    }
                    dist = PaintMathfx.GaussFalloff(Mathf.Sqrt(dist), rad) * hardness;
                    
                    if(patPixels[y * (int)lengthX + x].a > 0.25f)
                    {
                        pixels[y * (int)lengthX + x] = patPixels[y * (int)lengthX + x];
                    }
                }
            }
            
            tex.SetPixels((int)start.x, (int)start.y, (int)lengthX, (int)lengthY, pixels, 0);
            return tex;
        }
        
        
        
        public class PaintMathfx
        {
            public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point){
                
                var fullDirection = lineEnd - lineStart;
                var lineDirection = Vector3.Normalize(fullDirection);
                var closestPoint = Vector3.Dot((point - lineStart), lineDirection) / Vector3.Dot(lineDirection, lineDirection);
                return lineStart + (Mathf.Clamp(closestPoint, 0.0f, Vector3.Magnitude(fullDirection)) * lineDirection);
            }
            
            public static Vector2 NearestPointStrict(Vector2 lineStart, Vector2 lineEnd, Vector2 point){
                
                var fullDirection = lineEnd - lineStart;
                var lineDirection = Normalize(fullDirection);
                var closestPoint = Vector2.Dot((point - lineStart), lineDirection) / Vector2.Dot(lineDirection, lineDirection);
                return lineStart + (Mathf.Clamp(closestPoint, 0.0f, fullDirection.magnitude) * lineDirection);
            }
            
            public static float GaussFalloff(float distance, float inRadius){
                
                return Mathf.Clamp01(Mathf.Pow(360.0f, -Mathf.Pow(distance / inRadius, 2.5f) - 0.01f));
            }
            
            public static Vector2 Normalize(Vector2 p){
                float mag = p.magnitude;
                return p / mag;
            }
        }
    }
}
