using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestingOne : MonoBehaviour {

    public Slider slider;
    public Slider sliderTwo;

    public System.Action<float> SliderValueCallback;

	// Use this for initialization
	void Start () {

        slider.onValueChanged.AddListener(OnValueChanged);
        sliderTwo.onValueChanged.AddListener(OnValueChangedTwo);
        GameObject go = new GameObject("TESTING");
        TestingTwo testing = go.AddComponent<TestingTwo>();
        testing.testing = this;
        testing.referencesValue = valuessss;
        SliderValueCallback = testing.Callback;
	}

    void OnValueChanged (float value)
    {
        Debug.Log("VALUE " + value);
        valuessss = value;

        if (SliderValueCallback != null)
            SliderValueCallback(value);
    }

    void OnValueChangedTwo (float value)
    {
        Debug.Log("VALUE " + value);
    }

	// Update is called once per frame
	void Update () {
	
	}

    public float valuessss;

    public void RefExample (ref float value)
    {
        value = slider.value;
    }

    public void OutExample (out float value)
    {
        value = sliderTwo.value;
    }
}
