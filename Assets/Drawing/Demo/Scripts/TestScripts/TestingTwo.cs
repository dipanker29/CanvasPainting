using UnityEngine;
using System.Collections;

public class TestingTwo : MonoBehaviour {

    public TestingOne testing;
    public float referencesValue;
    public float referencesValueTwo;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Show();
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Showw();
        }
	}

    public void Callback (float value)
    {
        referencesValue = value;
    }

    void Show ()
    {
        testing.RefExample(ref referencesValue);
    }

    void Showw ()
    {
        testing.OutExample(out referencesValueTwo);
    }
}
