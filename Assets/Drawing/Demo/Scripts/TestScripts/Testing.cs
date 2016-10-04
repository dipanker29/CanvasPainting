using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using Drawing;

public class Testing : MonoBehaviour, /*IPointerClickHandler,*/ IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public float brushRadius = 5f;
    public float brushHadness = 5f;
    public Color brushColor = Color.green;

    private PointerEventData pointerEventData;
    private bool isMouseDown;
    private Image image;
    private Texture2D imageTexture;
    private Vector2 imagePosition;
    private Vector2 mousePosition;
    private Vector2 brushStart;
    private Vector2 brushEnd;
    private Vector2 brushPolish;
    private float screenRatio;                      //Usefull for different aspect ratio.

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        screenRatio = GetComponent<Image>().canvas.scaleFactor;

        if (image.sprite != null && image.sprite.texture != null)
        {
            SetTexture();
        }
	}
	
    void Update () {

        if (image == null)
            return;

        if (isMouseDown)
        {
            mousePosition = pointerEventData.pointerCurrentRaycast.screenPosition;
            brushEnd = GetTextureCordinate ();

            if (brushStart == Vector2.zero)
                return;

            Brush(brushEnd, brushPolish);

            brushPolish = brushEnd;
        }
    }

    private bool wasEntered = false;
    public void OnPointerExit (PointerEventData eventData)
    {
        Debug.Log("Exit "+eventData.pointerCurrentRaycast.gameObject.name);
        if (isMouseDown)
        {
            wasEntered = true;
            isMouseDown = false;
            brushStart = Vector2.zero;
            brushEnd = brushStart;
            brushPolish = brushEnd;
        }
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        Debug.Log("Enter "+wasEntered);
        if (wasEntered)
        {
            imagePosition = eventData.pointerCurrentRaycast.gameObject.transform.position;
            pointerEventData = eventData;
            brushStart = GetTextureCordinate();
            isMouseDown = true;
            wasEntered = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("On Pointer Down");
        if (CanSelectGameObject (eventData.pointerCurrentRaycast.gameObject))
        {
            imagePosition = eventData.pointerCurrentRaycast.gameObject.transform.position;
            pointerEventData = eventData;
            brushStart = GetTextureCordinate();
            isMouseDown = true;
            wasEntered = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("On Pointer Up");
        isMouseDown = false;
        brushStart = Vector2.zero;
        brushEnd = brushStart;
        brushPolish = brushEnd;
    }

    bool CanSelectGameObject (GameObject gameobject)
    {
        if (gameObject == null)
            return false;

        if (imageTexture == null)
            return false;
        
        if (image.gameObject != gameObject)
            return false;

        return true;
    }

    Vector2 GetTextureCordinate ()
    {
        Vector2 textureCordinates = Vector2.zero;

        float textureCordX = imagePosition.x - (GetTextureSize().x / 2f);
        float textureCordy = imagePosition.y - (GetTextureSize().y / 2f);

        float pointerTextureCordX = mousePosition.x - textureCordX;
        float pointerTextureCordY = mousePosition.y - textureCordy;

        textureCordinates.x = pointerTextureCordX / screenRatio;
        textureCordinates.y = pointerTextureCordY / screenRatio;

        return textureCordinates;
    }

    Vector2 GetTextureSize ()
    {
        Vector2 textureSize = Vector2.zero;

        float textureWidth = (float)imageTexture.width * screenRatio;
        float textureHeight = (float)imageTexture.height * screenRatio;

        textureSize.x = textureWidth;
        textureSize.y = textureHeight;

        return textureSize;
    }

    void Brush(Vector2 p1, Vector2 p2)
    {
        if (wasEntered)
            Debug.Log("BRUSH");
        
        if (p2 == Vector2.zero)
        {
            p2 = p1;
        }

        PaintTexture.Paint(p1, p2, brushRadius, brushColor, brushHadness, imageTexture);
        imageTexture.Apply();
    }

    void SetTexture ()
    {
        imageTexture = new Texture2D(image.sprite.texture.width, image.sprite.texture.height);
        imageTexture.SetPixels32(image.sprite.texture.GetPixels32());
        imageTexture.Apply();

        Sprite sprite = Sprite.Create (imageTexture, new Rect(0,0,imageTexture.width, imageTexture.height), new Vector2(0.5f,0.5f));
        image.sprite = sprite;
    }
}
