using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Drawing
{
    /// <summary>
    /// Eraser class is use for changing the alpha color to 0 of a texture.
    /// </summary>
    public class Eraser : Pointer {

        public Image eraser;

        private Vector2 eraserStart;
        private Vector2 eraserEnd;
        private Vector2 eraserRemove;

        // Use this for initialization
        void Start () {
            
        }
        
        // Update is called once per frame
        void Update () {
            base.Update();

            if (eraser != null)
            {
                eraser.transform.position = Input.mousePosition;
            }

            if(isPointerDown)
            {
                if(CurrentRaycastObject() != null)
                {
                    if(CurrentGameObject == null)
                    {
                        ResetEraser();
                        CheckRaycastTexture(CurrentRaycastObject());
                        OnPointerDown();
                        return;
                    }
                    else if (CurrentGameObject != CurrentRaycastObject())
                    {
                        if(CheckRaycastTexture(CurrentRaycastObject()))
                        {
                            ResetEraser();
                            OnPointerDown();
                        }
                        else
                        {
                            return;
                        }
                    }

                    eraserEnd = CurrentPointerPosition ();

                    if (eraserStart == Vector2.zero) { return; }

                    if (eraser != null)
                    {
                        eraser.gameObject.SetActive(true);
                    }

                    base.ErasePaint(eraserEnd, eraserRemove);

                    eraserRemove = eraserEnd;
                }
                else
                {
                    ResetEraser();
                }
            }
            else
            {
                CurrentGameObject = null;
                ResetEraser();
                if (eraser != null && eraser.gameObject.activeInHierarchy)
                {
                    eraser.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Resets the eraser position.
        /// </summary>
        void ResetEraser ()
        {
            eraserStart = Vector2.zero;
            eraserEnd = eraserStart;
            eraserRemove = eraserEnd;
        }

        public override void OnPointerDown()
        {
            base.OnPointerDown();
            eraserStart = CurrentPointerPosition ();
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
