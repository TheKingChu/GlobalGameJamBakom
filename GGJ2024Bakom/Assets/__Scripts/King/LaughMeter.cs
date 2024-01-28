using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaughMeter : MonoBehaviour
{
    public Slider laughSlider;
    public float positiveIncrement = 1f;
    public float negativeIncrement = 3f;
    private Audience audience;
    private MouseRotation mouseRotation;
    public GameObject confettiParticles;
    public GameObject winPanel, losePanel;


    private void Start()
    {
        mouseRotation = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseRotation>();
    }

    public void PositiveEvent()
    {
        laughSlider.value += positiveIncrement;
        Debug.Log("positive");

        if(laughSlider.value > 0.5f)
        {
            //confetti
            Instantiate(confettiParticles, transform.position, Quaternion.identity);
        }
        if(laughSlider.value >= 10)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //stop player movement
            mouseRotation.StopMouseMovement();
        }
    }

    public void NegativeEvent()
    {
        laughSlider.value -= negativeIncrement;
        Debug.Log("Negattive");

        if(laughSlider.value < 0.5f)
        {
            //audience starts throwing 
            //audience.ThrowTomato();
        }   
        if(laughSlider.value == -10)
        {
            losePanel.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //stop player movement
            mouseRotation.StopMouseMovement();
        }
    }

    public void HonkEvent()
    {
        laughSlider.value = 0.3f;
        Debug.Log("honk event");
    }
}
