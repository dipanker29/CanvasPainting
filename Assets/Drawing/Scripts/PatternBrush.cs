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
        public float distance;

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
            }

            /*if(isPointerDown)
            {
                if(CurrentRaycastObject() != null)
                {
                    if(CurrentGameObject == null)
                    {
                        ResetPatternBrush();
                        CheckRaycastTexture(CurrentRaycastObject());
                        OnPointerDown();
                        return;
                    }
                    else if (CurrentGameObject != CurrentRaycastObject())
                    {
                        if(CheckRaycastTexture(CurrentRaycastObject()))
                        {
                            ResetPatternBrush();
                            OnPointerDown();
                        }
                        else
                        {
                            return;
                        }
                    }

                    brushEnd = CurrentPointerPosition ();

                    if (brushStart == Vector2.zero) { return; }

                    if (brush != null)
                    {
                        brush.gameObject.SetActive(true);
                    }

                    base.PatternPaint(brushEnd, brushPolish, patternImage);

                    brushPolish = brushEnd;
                }
                else
                {
                    ResetPatternBrush();
                }
            }
            else
            {
                CurrentGameObject = null;
                ResetPatternBrush();
                if (brush != null && brush.gameObject.activeInHierarchy)
                {
                    brush.gameObject.SetActive(false);
                }
            }*/
        }

        /// <summary>
        /// Resets the brush position.
        /// </summary>
        void ResetPatternBrush ()
        {
            brushStart = Vector2.zero;
            brushEnd = brushStart;
            brushPolish = brushEnd;
        }

        float PatternDistance ()
        {
            float width = (float)patternImage.width;
            
            return width / 2f;
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

            go.GetComponent<RectTransform>().ScreenPointToRectPosition(pointerPosition);

            Texture2D _texture = new Texture2D(0,0);
            _texture.CopyTexture(pattern);
            image.sprite.CreateSprite(_texture);
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
