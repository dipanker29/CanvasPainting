using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Drawing
{
    /// <summary>
    /// Brush class is use for painting the color in top of texture.
    /// </summary>
    public class Brush : Pointer
    {
        public Image brush;
        public Color color = Color.white;

        private Vector2 brushStart;
        private Vector2 brushEnd;
        private Vector2 brushPolish;

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

            if(isPointerDown)
            {
                if(CurrentRaycastObject() != null)
                {
                    if(CurrentGameObject == null)
                    {
                        ResetBrush();
                        CheckRaycastTexture(CurrentRaycastObject());
                        OnPointerDown();
                        return;
                    }
                    else if (CurrentGameObject != CurrentRaycastObject())
                    {
                        if(CheckRaycastTexture(CurrentRaycastObject()))
                        {
                            ResetBrush();
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

                    base.ApplyPaint(brushEnd, brushPolish, color);

                    brushPolish = brushEnd;
                }
                else
                {
                    ResetBrush();
                }
            }
            else
            {
                CurrentGameObject = null;
                ResetBrush();
                if (brush != null && brush.gameObject.activeInHierarchy)
                {
                    brush.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Resets the brush position.
        /// </summary>
        void ResetBrush ()
        {
            brushStart = Vector2.zero;
            brushEnd = brushStart;
            brushPolish = brushEnd;
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
