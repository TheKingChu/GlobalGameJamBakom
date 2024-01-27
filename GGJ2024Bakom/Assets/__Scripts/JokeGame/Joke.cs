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
        jester = GameObject.Find("JokeController").GetComponent<JokeController>();
        _myButton = GetComponent<Button>();
    }

    public void TellJoke()
    {
        jester.Joking(this.GetComponent<Joke>());
        if (EventSystem.current.currentSelectedGameObject == _myButton.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

    }
}
