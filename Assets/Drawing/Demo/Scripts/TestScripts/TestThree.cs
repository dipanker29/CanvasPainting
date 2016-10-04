using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TestThree : MonoBehaviour {

    public Text positionText;
    public Camera mainCamera;

    public Image image;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Color32[] col = image.sprite.texture.GetPixels32();
            rotateSquare(col, 45f);
        }
            
	}

    void OnDrawGizmosSelected ()
    {
        if (positionText != null)
        {
//            Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.current, this.transform.position);
//            Debug.Log(RectTransformUtility.WorldToScreenPoint(null, this.transform.position));
            /*pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            pos.z = Mathf.Round(pos.z);*/
            positionText.text = RectTransformUtility.WorldToScreenPoint(null, this.transform.position).ToString();
        }
    }

    void rotateSquare (Color32[] arr, float phi){
        int x, y, i, j;
        float sn = Mathf.Sin(phi);
        float cs = Mathf.Cos(phi);
        Texture2D texture = image.sprite.texture;
        Color32[] arr2 = texture.GetPixels32();
        int W = texture.width;
        int H = texture.height;
        int xc = W/2;
        int yc = H/2;

        for (j=0; j<H; j++){
            for (i=0; i<W; i++){
                arr2[j*W+i] = new Color32(0,0,0,0);

                x = Mathf.RoundToInt(cs) * (i - xc) + Mathf.RoundToInt(sn) * (j - yc) + xc;
                y = Mathf.RoundToInt(-sn) * (i - xc) + Mathf.RoundToInt(cs) * (j - yc) + yc;

                if ((x > -1) && (x < W) && (y > -1) && (y < H))
                { 
                    arr2[j * W + i] = arr[y * W + x];
                }
            }
        }

        texture.SetPixels32(arr2);
        Sprite sprite = Sprite.Create (texture, new Rect(0,0,texture.width, texture.height), new Vector2(0.5f,0.5f));
        image.sprite = sprite;
    }

    public Texture2D rotateTexture(Texture2D image )
    {

        Texture2D target = new Texture2D(image.height, image.width, image.format, false);    //flip image width<>height, as we rotated the image, it might be a rect. not a square image

        Color32[] pixels = image.GetPixels32(0);
        pixels = rotateTextureGrid(pixels, image.width, image.height);
        target.SetPixels32(pixels);
        target.Apply();

        //flip image width<>height, as we rotated the image, it might be a rect. not a square image

        return target;
    }


    public Color32[] rotateTextureGrid(Color32[] tex, int wid, int hi)
    {
        Color32[] ret = new Color32[wid * hi];      //reminder we are flipping these in the target

        for (int y = 0; y < hi; y++)
        {
            for (int x = 0; x < wid; x++)
            {
                ret[(hi-1)-y + x * hi] = tex[x + y * wid];         //juggle the pixels around

            }
        }

        return ret;
    }
}
