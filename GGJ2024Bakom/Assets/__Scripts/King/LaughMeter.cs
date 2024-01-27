using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaughMeter : MonoBehaviour
{
    public Slider laughSlider;
    public float positiveIncrement = 0.5f;
    public float negativeIncrement = 0.5f;
    private Audience audience;

    public void PositiveEvent()
    {
        laughSlider.value += positiveIncrement;
        Debug.Log("positive");

        if(laughSlider.value > 0.5f)
        {
            //something
        }
    }

    public void NegativeEvent()
    {
        laughSlider.value -= negativeIncrement;
        Debug.Log("Negattive");

        if(laughSlider.value < 0.5f)
        {
            //audience starts throwing 
            audience.ThrowTomato();
        }   
    }

    public void HonkEvent()
    {
        laughSlider.value = 0.3f;
        Debug.Log("honk event");
    }
}
