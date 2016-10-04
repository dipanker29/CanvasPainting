using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Drawing;

public class TestOne : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public float brushRadius = 5f;
    public float brushHadness = 5f;
    public Color brushColor = Color.green;

    private float screenRatio;  
    private bool isBrushDown;
    private Texture2D imageTexture;
    private PointerEventData pointerEventData;

    private Vector2 brushPosition;
    private Vector2 imagePosition;
    private Vector2 brushStart;
    private Vector2 brushEnd;
    private Vector2 brushPolish;
    private GameObject currentObject;

    public Vector2 brushOffsets;
    public Image brush;

    private bool testBool;

    List<RaycastResult> raycastResults;
    PointerEventData mainPointerEventData;

    void Start ()
    {
        pointerEventData = new PointerEventData(EventSystem.current);
    }

    void Update ()
    {
        brush.transform.position = Input.mousePosition;

        if (isBrushDown)
        {
            PointerRaycast();

            if (currentObject != pointerEventData.pointerCurrentRaycast.gameObject)
            {
                if (pointerEventData.pointerCurrentRaycast.gameObject != null &&
                    CheckTexture (pointerEventData.pointerCurrentRaycast.gameObject))
                {
                    brushStart = Vector2.zero;
                    brushEnd = brushStart;
                    brushPolish = brushEnd;
                    PointerDown();
                }
                else
                {
                    return;
                }
            }

            brushPosition = pointerEventData.pointerCurrentRaycast.screenPosition;

            brushEnd = PaintTexture.GetTextureCordinate(imagePosition, brushPosition, screenRatio, imageTexture, currentObject.transform.eulerAngles.z);

            if (brushStart == Vector2.zero)
                return;

            Brush(brushEnd, brushPolish);

            brushPolish = brushEnd;
        }
    }


    public void OnPointerEnter (PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null) {
            Debug.Log("Enter "+eventData.pointerCurrentRaycast.gameObject.name);
        }

        if (isBrushDown)
        {
            if (pointerEventData.pointerCurrentRaycast.gameObject != null &&
                CheckTexture (pointerEventData.pointerCurrentRaycast.gameObject))
            {
                imagePosition = pointerEventData.pointerCurrentRaycast.gameObject.transform.position;
                brushStart = PaintTexture.GetTextureCordinate(imagePosition, brushPosition, screenRatio, imageTexture, currentObject.transform.eulerAngles.z);
            }
        }
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        Debug.Log("Exit");
        if (eventData.pointerCurrentRaycast.gameObject != null) {
            Debug.Log("Exit "+eventData.pointerCurrentRaycast.gameObject.name);
        }
    }

    public void OnPointerDown (PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null) {
            Debug.Log("Down "+eventData.pointerCurrentRaycast.gameObject.name);
        }

        mainPointerEventData = eventData;

        PointerRaycast();

        if (pointerEventData.pointerCurrentRaycast.gameObject != null &&
            CheckTexture (pointerEventData.pointerCurrentRaycast.gameObject))
        {
            PointerDown();
        }
    }

    public void OnPointerUp (PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null) {
            Debug.Log("Up "+eventData.pointerCurrentRaycast.gameObject.name);
        }

        isBrushDown = false;
        brushStart = Vector2.zero;
        brushEnd = brushStart;
        brushPolish = brushEnd;
    }

    void PointerRaycast ()
    {
        OffsetPointer();

        raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        if (raycastResults.Count > 0)
        {
            pointerEventData.pointerCurrentRaycast = raycastResults[0];
        }
    }

    void PointerDown ()
    {
        imagePosition = pointerEventData.pointerCurrentRaycast.gameObject.transform.position;
        brushStart = PaintTexture.GetTextureCordinate(imagePosition, brushPosition, screenRatio, imageTexture, currentObject.transform.eulerAngles.z);
        isBrushDown = true;
    }

    void OffsetPointer ()
    {
        Vector2 temp = mainPointerEventData.position;
        temp.x += brushOffsets.x;
        temp.y += brushOffsets.y;
        pointerEventData.position = temp;
    }

    void Brush(Vector2 p1, Vector2 p2)
    {
        if (p2 == Vector2.zero)
        {
            p2 = p1;
        }

        PaintTexture.Paint(p1, p2, brushRadius, brushColor, brushHadness, imageTexture, false);
        imageTexture.Apply();
    }

    bool CheckTexture (GameObject gameobject)
    {
        if (gameobject.GetComponent<Image> () != null)
        {
            Image image = gameobject.GetComponent<Image>();
            screenRatio = image.canvas.scaleFactor;

            try
            {
                image.sprite.texture.GetPixel(0,0);

                if (image.sprite != null && image.sprite.texture != null)
                {
                    imageTexture = image.sprite.texture;
                    currentObject = gameobject;
                    Debug.LogError("TRUE");
                    return true;
                }
            }
            catch (UnityException e)
            {
                if(e.Message.StartsWith("Texture '" + image.sprite.texture.name + "' is not readable"))
                {
                    Debug.LogError("Please enable read/write on texture [" + image.sprite.texture.name + "]");
                }
            }
        }
     
        Debug.LogError("FALSE");
        return false;
    }

    #if UNITY_EDITOR
    void OnGUI ()
    {
        if (isBrushDown)
        {
            float x = brushPosition.x - 5f;
            float y = (((float)Screen.height) - (brushPosition.y)) - 5f;
            GUI.Box(new Rect(x, y, 10f, 10f), "");
        }
    }
    /*private Image dotImage;
    private Image dotImageChild;
    public bool showDot = false;
    private bool isMouseDown;

    void OnGUI ()
    {
        if (showDot == false)
            return;
        
        if (dotImage == null)
        {
            GameObject dot = new GameObject("DotImage");
            dot.AddComponent<RectTransform>();
            dotImage = dot.AddComponent<Image>();
            dot.transform.SetParent(this.transform);
            dotImage.rectTransform.sizeDelta = new Vector2(20f,20f);
            dotImage.color = Color.blue;
            dotImage.raycastTarget = false;
    
            GameObject dotChild = Instantiate (dot);
            dotChild.transform.SetParent(dot.transform);
            dotImageChild = dotChild.GetComponent<Image>();
            dotImageChild.color = Color.blue;
            dotImageChild.raycastTarget = false;

            return;
        }

        if (dotImageChild.rectTransform.sizeDelta.x != brushRadius)
        {
            dotImageChild.rectTransform.sizeDelta = new Vector2((brushRadius * 2f), (brushRadius*2f));
        }

        dotImageChild.rectTransform.anchoredPosition = brushOffsets;

        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }

        if (isMouseDown)
        {
            dotImage.transform.position = Input.mousePosition;
        }
    }*/

    #endif
}
