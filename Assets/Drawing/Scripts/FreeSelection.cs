using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Drawing;

public class FreeSelection : Pointer
{
    public Image eraser;

    private Vector2 eraserStart;
    private Vector2 eraserEnd;
    private Vector2 eraserRemove;

    // Use this for initialization
    void Start () {
        if (newImage.sprite == null)
        {
            Texture2D _fromImage = new Texture2D(fromImage.sprite.texture.width, fromImage.sprite.texture.height);
            _fromImage = Drawing.Util.TextureUtility.CopyTexture(fromImage.sprite.texture);

            Texture2D tex = new Texture2D(_fromImage.width, _fromImage.height);
            tex = Drawing.Util.TextureUtility.CopyTexture(_fromImage);
            Color[] colors = tex.GetPixels();

            for (int i=0; i < colors.Length; i++)
            {
                colors[i].a = 0;
            }

            tex.SetPixels(colors);
            tex.Apply();

            newImage.sprite = Drawing.Util.CanvasUtility.GetSprite(tex);
        }
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

                ErasePaint(eraserEnd, eraserRemove);

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

    public Image newImage;
    public Image fromImage;
    public void ErasePaint (Vector2 point1, Vector2 point2)
    {
        if (point2 == Vector2.zero)
        {
            point2 = point1;
        }

        PaintTexture.FreeSelection(point1, point2, (radius / objectScale), Color.red, (opacity / objectScale), imageTexture,
            newImage.sprite.texture, false);
        newImage.sprite.texture.Apply();
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
