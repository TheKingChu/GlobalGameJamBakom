using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaughMeter : MonoBehaviour
{
    public Slider laughSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLaughMeter();
    }

    public void UpdateLaughMeter()
    {
        float sliderValue = laughSlider.value;
    }
}
