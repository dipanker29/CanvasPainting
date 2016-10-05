using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Drawing.Util;

namespace Drawing
{
    /// <summary>
    /// PatternBrush class is use for painting the texture pattern in top of texture.
    /// </summary>
    public class PatternBrush : Pointer
    {
        public Image brush;
        public Color color = Color.white;
        public Texture2D patternImage;
        public float distance = 50f;

        private Vector2 brushStart;
        private Vector2 brushEnd;
        private Vector2 brushPolish;

        public RectTransform SetParent { set { parent = value; } }
        private Vector2 previousPosition;
        private RectTransform parent;

        // Use this for initialization
        void Start () {
        }
        
        // Update is called once per frame
        void Update () {
            base.Update();

            if (brush != null)
            {
                brush.transform.position = Input.mousePosition;
            }

            if (patternImage == null)
            {
                return;
            }

            if (isPointerDown)
            {
                if (pointerPosition == Vector2.zero  || previousPosition == pointerPosition || (Vector2.Distance(previousPosition, pointerPosition) < PatternDistance()))
                {
                    return;
                }

                CreatePattern(patternImage);
            }
            else
            {
                if (brush != null && brush.gameObject.activeInHierarchy)
                {
                    brush.gameObject.SetActive(false);
                }

                ResetPatternBrush();
            }
        }

        /// <summary>
        /// Resets pattern brush parameter.
        /// </summary>
        void ResetPatternBrush ()
        {
            if (previousPosition != Vector2.zero)
            {
                previousPosition = Vector2.zero;
            }
        }

        float PatternDistance ()
        {
            if (distance == 0)
                distance = 0.01f;

            return distance;
        }

        void CreatePattern (Texture2D pattern)
        {
            if (parent == null)
            {
                SetParent = CurrentRaycastObject ().GetComponent<Image>().canvas.GetComponent<RectTransform>();
            }

            GameObject go = new GameObject("Pattern");
            Image image = go.AddComponent<Image>();
            go.transform.SetParent(parent, false);

            go.GetComponent<RectTransform>().ScreenPointToRectPosition(pointerPosition, image.canvas.scaleFactor);
            image.sprite = CanvasUtility.GetSprite(TextureUtility.CopyTexture(pattern));
            previousPosition = pointerPosition;
        }

        public override void OnPointerDown()
        {
            base.OnPointerDown();
            brushStart = CurrentPointerPosition ();
        }

        public override GameObject CurrentRaycastObject()
        {
            return base.CurrentRaycastObject();
        }

        public override Vector2 CurrentPointerPosition()
        {
            return base.CurrentPointerPosition();
        }

        public override bool CheckRaycastTexture(GameObject raycastObject)
        {
            return base.CheckRaycastTexture(raycastObject);
        }
    }
}
