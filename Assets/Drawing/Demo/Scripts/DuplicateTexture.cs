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

        Sprite sprite = image.sprite;

        Texture2D texture = new Texture2D(0, 0);
        texture.CopyTexture(sprite.texture);

        image.sprite.CreateSprite(texture);

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
