using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Drawing.Util;

public class DuplicateTexture : MonoBehaviour
{

    void Awake ()
    {
        if (GetComponent<Image>() == null)
            return;
        
        Image image = GetComponent<Image>();

        if (image.sprite == null)
            return;

        image.sprite = CanvasUtility.GetSprite(TextureUtility.CopyTexture(image.sprite.texture));
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
