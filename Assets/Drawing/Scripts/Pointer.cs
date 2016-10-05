using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Drawing.Util;

namespace Drawing
{
    public class Pointer : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
    {
        public Vector2 pointerOffsets = new Vector2(5f, 5f);
        public float radius = 5f;
        public float opacity = 5f;
        public GameObject CurrentGameObject
        {
            get { return currentObject; } 
            set { currentObject = value; }
        }

        protected bool isPointerDown;
        protected float screenRatio;
        protected float objectScale;
        protected Vector2 imagePosition;
        protected Vector2 pointerPosition;
        protected Texture2D imageTexture;
        private GameObject currentObject;

        PointerEventData currentPointerEventData;
        PointerEventData pointerEventData;


        protected void Update ()
        {
            PointerEventRaycast();
        }

        #region Event System Handler
        /// <summary>
        /// Raises the pointer enter event.
        /// This event is trigger when the cursor enters the rect area of the selectable UI object.
        /// </summary>
        /// <param name="eventData">Pointer Event data.</param>
        public void OnPointerEnter (PointerEventData eventData)
        {
            Debug.Log("Enter");
            currentPointerEventData = eventData;
        }

        /// <summary>
        /// Raises the pointer exit event.
        /// This event is trigger when the cursor exit the rect area of the selectable UI object.
        /// </summary>
        /// <param name="eventData">Pointer Event data.</param>
        public void OnPointerExit (PointerEventData eventData)
        {
            Debug.Log("Exit");
            currentPointerEventData = null;
        }

        /// <summary>
        /// Raises the pointer down event.
        /// Interface to implement if you wish to receive OnPointerDown callbacks.
        /// </summary>
        /// <param name="eventData">Pointer Event data.</param>
        public void OnPointerDown (PointerEventData eventData)
        {
            Debug.Log("Down");
            currentPointerEventData = eventData;
            isPointerDown = true;
        }

        /// <summary>
        /// Raises the pointer up event.
        /// Interface to implement if you wish to receive OnPointerUp callbacks.
        /// </summary>
        /// <param name="eventData">Pointer Event data.</param>
        public void OnPointerUp (PointerEventData eventData)
        {
//            Debug.Log("Up");
            currentPointerEventData = eventData;
            isPointerDown = false;
        }
        #endregion Event System Handler

        /// <summary>
        /// This method replace the default pointer event with new pointer event.
        /// New pointer event can offset its position
        /// And raycast all the under this new pointer event.
        /// </summary>
        void PointerEventRaycast ()
        {
            if (pointerEventData == null)
            {
                InitPointerEvent();
                return;
            }
            if (currentPointerEventData == null)
            {
                pointerEventData = null;
                return;
            }

            Vector2 temp = currentPointerEventData.position;
            temp.x += pointerOffsets.x;
            temp.y += pointerOffsets.y;
            pointerEventData.position = temp;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            if (raycastResults.Count > 0)
            {
                pointerEventData.pointerCurrentRaycast = raycastResults[0];
                pointerPosition = pointerEventData.pointerCurrentRaycast.screenPosition;
            }
        }

        /// <summary>
        /// Initialize the custom new pointer event.
        /// </summary>
        void InitPointerEvent ()
        {
            pointerEventData = new PointerEventData(EventSystem.current);
        }

        /// <summary>
        /// ApplyPaint method is called to paint color on texture.
        /// Need to pass start and end point of pointer position.
        /// And color to apint in texture
        /// </summary>
        /// <param name="point1">Vector2 Start position of pointer.</param>
        /// <param name="point2">Vector2 End position of pointer.</param>
        /// <param name="color">Color to paint in texture.</param>
        public void ApplyPaint (Vector2 point1, Vector2 point2, Color color)
        {
            if (point2 == Vector2.zero)
            {
                point2 = point1;
            }

            PaintTexture.Paint(point1, point2, (radius / objectScale), color, (opacity / objectScale), imageTexture, false);
            imageTexture.Apply();
        }

        /// <summary>
        /// ErasePaint is use for erasing the texture in given position.
        /// Need to pass start and end point of pointer position.
        /// </summary>
        /// <param name="point1">Vector2 Start position of pointer.</param>
        /// <param name="point2">Vector2 End position of pointer.</param>
        public void ErasePaint (Vector2 point1, Vector2 point2)
        {
            if (point2 == Vector2.zero)
            {
                point2 = point1;
            }

            PaintTexture.Erase(point1, point2, (radius / objectScale), (opacity / objectScale), imageTexture);
            imageTexture.Apply();
        }

//        private Vector2 previousPosition;
//        public float patternDistance;
        public void PatternPaint (Vector2 point1, Vector2 point2, Texture2D pattern)
        {
            if (pointerPosition == Vector2.zero)
            {
                return;
            }

//            PaintTexture.PatternPaint(point1, point2, (radius / objectScale), (opacity / objectScale), imageTexture, pattern);
//            imageTexture.Apply();
            /*if (previousPosition == pointerPosition)
                return;
            if (previousPosition != Vector2.zero)
                Debug.Log(Vector2.Distance(previousPosition, pointerPosition));

            if (Vector2.Distance(previousPosition, pointerPosition) < ((float)pattern.width)/2f)
                return;

            GameObject go = new GameObject("Pattern");
            Image image = go.AddComponent<Image>();
            go.transform.SetParent(CurrentGameObject.GetComponent<Image>().canvas.transform, false);

            go.GetComponent<RectTransform>().ScreenPointToRectPosition(pointerPosition);
            
            Texture2D _texture = new Texture2D(0,0);
            _texture.CopyTexture(pattern);
            image.CreateSprite(_texture);
            previousPosition = pointerPosition;*/
        }

        /// <summary>
        /// Check for current raycast object.
        /// </summary>
        /// <returns>The raycast GameObject.</returns>
        public virtual GameObject CurrentRaycastObject ()
        {
            if (pointerEventData == null || currentPointerEventData == null)
                return null;

            return pointerEventData.pointerCurrentRaycast.gameObject;
        }

        /// <summary>
        /// This method need to be called when pointer event is pressed/down.
        /// </summary>
        public virtual void OnPointerDown ()
        {
//            Debug.Log("OnPointerDown");
            imagePosition = CurrentRaycastObject().transform.position;
        }

        /// <summary>
        /// Check the cureent pointer position and texture position.
        /// </summary>
        /// <returns>Return Vector2 the coordinate of texture point.</returns>
        public virtual Vector2 CurrentPointerPosition ()
        {
            if (currentObject != null)
            {
                return PaintTexture.GetTextureCordinate(imagePosition, pointerPosition, screenRatio, imageTexture, currentObject.transform.eulerAngles.z);
            }

            return Vector2.zero;
        }

        /// <summary>
        /// Check for raycast object is UI image with the texture assigned to sprite and texture must be read/write enabled.
        /// If all good then current selected object is assigned to currentObject variable.
        /// Also get the aspect ratio through imgage and scale of image.
        /// </summary>
        /// <returns><c>true</c>, if raycast texture match condition, <c>false</c> otherwise.</returns>
        /// <param name="raycastObject">Raycast object.</param>
        public virtual bool CheckRaycastTexture (GameObject raycastObject)
        {
            if (raycastObject.GetComponent<Image> () != null)
            {
                Image image = raycastObject.GetComponent<Image>();
                objectScale = image.rectTransform.localScale.x;
                screenRatio = (image.canvas.scaleFactor * objectScale);

                try
                {
                    image.sprite.texture.GetPixel(0,0);

                    if (image.sprite != null && image.sprite.texture != null)
                    {
                        imageTexture = image.sprite.texture;
                        currentObject = raycastObject;
                        return true;
                    }
                }
                catch (UnityException e)
                {
                    currentObject = null;
                    if(e.Message.StartsWith("Texture '" + image.sprite.texture.name + "' is not readable"))
                    {
                        Debug.LogError("Please enable read/write on texture [" + image.sprite.texture.name + "]");
                    }
                }
            }

            return false;
        }

        // For debugging purpose.....
        #if UNITY_EDITOR
        public Transform transTest;
        void OnGUI ()
        {
            if (isPointerDown)
            {
                float x = pointerPosition.x - 5f;
                float y = (((float)Screen.height) - (pointerPosition.y)) - 5f;
                GUI.Box(new Rect(x, y, 10f, 10f), "");
            }

            if (transTest != null & currentPointerEventData != null)
            {
                transTest.position = currentPointerEventData.position;
            }
        }
        #endif
    }
}
