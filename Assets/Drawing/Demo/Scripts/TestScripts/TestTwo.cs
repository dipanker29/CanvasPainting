using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestTwo : MonoBehaviour
{

    public Image image;

    private Texture2D imageTexture;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        imageTexture = new Texture2D(image.sprite.texture.width, image.sprite.texture.height);
        imageTexture.SetPixels32(image.sprite.texture.GetPixels32());
        imageTexture.Apply();

        Sprite sprite = Sprite.Create (imageTexture, new Rect(0,0,imageTexture.width, imageTexture.height), new Vector2(0.5f,0.5f));
        image.sprite = sprite;
    }
    
    // Update is called once per frame
    void Update () {
    
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //sideImage.rectTransform.anchoredPosition = new Vector2(GetComponent<RectTransform>().rect.width/2f, GetComponent<RectTransform>().rect.height/2f);
            float size = transform.localScale.x;
            int newWidth = Mathf.RoundToInt((float)imageTexture.width * size);
            int newHeight = Mathf.RoundToInt((float)imageTexture.height * size);
            Vector3 newScale = new Vector3((float)newWidth, (float)newHeight);
//            image.rectTransform.sizeDelta = newScale;
//            image.rectTransform.localScale = Vector3.one;

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //sideImage.rectTransform.anchoredPosition = new Vector2(GetComponent<RectTransform>().rect.width/2f, GetComponent<RectTransform>().rect.height/2f);
            float size = transform.localScale.x;
            int newWidth = Mathf.RoundToInt((float)imageTexture.width * size);
            int newHeight = Mathf.RoundToInt((float)imageTexture.height * size);
            Vector3 newScale = new Vector3((float)newWidth, (float)newHeight);
//            image.rectTransform.sizeDelta = newScale;
//            image.rectTransform.localScale = Vector3.one;

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //sideImage.rectTransform.anchoredPosition = new Vector2(GetComponent<RectTransform>().rect.width/2f, GetComponent<RectTransform>().rect.height/2f);
            float size = transform.localScale.x;
            int newWidth = Mathf.RoundToInt((float)imageTexture.width * size);
            int newHeight = Mathf.RoundToInt((float)imageTexture.height * size);
            Vector3 newScale = new Vector3((float)newWidth, (float)newHeight);
//            image.rectTransform.sizeDelta = newScale;
//            image.rectTransform.localScale = Vector3.one;

        }
	}

    GameObject target;
    GameObject myOffscreenIndicator;
    Vector3 borderPos;
    void OnDrawGizmosSelectedZ ()
    {
        Gizmos.DrawSphere (Input.mousePosition, 5f);
        Debug.Log (Camera.current.WorldToScreenPoint (Input.mousePosition));

        //Get the targets position on screen into a Vector3
        Vector3 targetPos = Camera.current.WorldToScreenPoint (target.transform.position);
        //Get the middle of the screen into a Vector3
        Vector3 screenMiddle = new Vector3(Screen.width/2, Screen.height/2, 0); 
        //Compute the angle from screenMiddle to targetPos
        float tarAngle = (Mathf.Atan2(targetPos.x-screenMiddle.x,Screen.height-targetPos.y-screenMiddle.y) * Mathf.Rad2Deg)+90;
        if (tarAngle < 0) tarAngle +=360;

        if(targetPos.z < 0) {
            myOffscreenIndicator.GetComponent<RotatableGuiItem>().angle = -(tarAngle-90); //Quaternion.Euler(tarAngle,270,90);
            borderPos.x = (Screen.width/2) - Mathf.Cos((tarAngle+180) * Mathf.Deg2Rad) * 100.0f;
            borderPos.y = (Screen.height/2) + Mathf.Sin((tarAngle+180) * Mathf.Deg2Rad) * 100.0f;
        }
        else {
            myOffscreenIndicator.GetComponent<RotatableGuiItem>().angle = -(tarAngle+90); //Quaternion.Euler(-tarAngle,90,270);
            borderPos.x = (Screen.width/2) - Mathf.Cos((tarAngle) * Mathf.Deg2Rad) * 100.0f;
            borderPos.y = (Screen.height/2) + Mathf.Sin((tarAngle) * Mathf.Deg2Rad) * 100.0f;
        }

        myOffscreenIndicator.transform.position = borderPos;
    }

    class RotatableGuiItem 
    {
        public float angle;
    }

    public float radius = 10f;
    void OnDrawGizmosSelectedzz ()
    {
        Gizmos.color = Color.blue;
        Vector3 pos = Camera.current.WorldToScreenPoint(Input.mousePosition);
        Debug.Log(pos);
//        Gizmos.DrawSphere (Input.mousePosition, radius);
    }
}
