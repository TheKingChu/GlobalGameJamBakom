using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joke : MonoBehaviour
{
    public bool correctJoke = false, criticalHonk = false;
    private JokeController jester;
    private Button _myButton;
    // Start is called before the first frame update
    void Start()
    {
        jester = GameObject.Find("Jester").GetComponent<JokeController>();
        _myButton = GetComponent<Button>();
    }
    public void JokeValue()
    {
        if (correctJoke)
        {
            Debug.Log("Good Joke");
        }
        else
        {
            Debug.Log("Bad Joke");
            jester.maxTime -= 2f;
        }

        if (criticalHonk)
        {
            Debug.Log("Critical Honk!");
            jester.maxTime += 1f;
        }
        jester.NextJoke();

        if (EventSystem.current.currentSelectedGameObject == _myButton.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
